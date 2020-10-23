using Iress.Sys.Ext;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Iress.Sys.Helpers
{
  public static class IsoConst
  {
    public const IsolatedStorageScope
      ULocA = (IsolatedStorageScope)(0 | 1 | 04), // C:\Users\[user]\AppData\ Local  \IsolatedStorage\...\AssemFiles\   
      ULocF = (IsolatedStorageScope)(1 | 2 | 04), // C:\Users\[user]\AppData\ Local  \IsolatedStorage\...\Files\        
      URoaA = (IsolatedStorageScope)(1 | 4 | 08), // C:\Users\[user]\AppData\Roaming \IsolatedStorage\...\AssemFiles\   
      PdFls = (IsolatedStorageScope)(2 | 4 | 16), // C:\ProgramData                  \IsolatedStorage\...\Files\                          
      PdAsm = (IsolatedStorageScope)(0 | 4 | 16); // C:\ProgramData                  \IsolatedStorage\...\AssemFiles\                     
  }

  public static class JsonIsoFileSerializer
  {
    public static void Save<T>(T o, string filenameONLY = "", IsolatedStorageScope iss = IsoConst.PdFls)
    {
      try
      {
        var isoStore = IsolatedStorageFile.GetStore(iss, null, null);

        using var isoStream = new IsolatedStorageFileStream(IsoHelper.GetSetFilename<T>(filenameONLY, "json"), FileMode.Create, isoStore);
        using var streamWriter = new StreamWriter(isoStream);
        new DataContractJsonSerializer(typeof(T)).WriteObject(streamWriter.BaseStream, o); // new XmlSerializer(o.GetType()).Serialize(streamWriter, o);
        streamWriter.Close();
      }
      catch (Exception ex) { ex.Log(); throw; }
    }

    public static T Load<T>(string filenameONLY = "", IsolatedStorageScope iss = IsoConst.PdFls) where T : new()
    {
      try
      {
        var isoStore = IsolatedStorageFile.GetStore(iss, null, null);

        if (isoStore.FileExists(IsoHelper.GetSetFilename<T>(filenameONLY, "json")))
          using (var isoStream = new IsolatedStorageFileStream(IsoHelper.GetSetFilename<T>(filenameONLY, "json"), FileMode.Open, FileAccess.Read, FileShare.Read, isoStore))
          {
            using var streamReader = new StreamReader(isoStream);
            var o = (T)(new DataContractJsonSerializer(typeof(T)).ReadObject(streamReader.BaseStream)); // var o = (T)(new XmlSerializer(typeof(T)).Deserialize(streamReader));
            streamReader.Close();
            return o;
          }
      }
      catch (InvalidOperationException /**/ ex) { if (ex.HResult != -2146233079) { ex.Log(); throw; } }  // "Root element is missing." ==> create new at the bottom
      catch (SerializationException    /**/ ex) { if (ex.HResult != -2146233076) { ex.Log(); throw; } }  // "There was an error deserializing the object of type Iress.SS.Logic.AppSettings. End element 'LastSave' from namespace '' expected. Found element 'DateTimeOffset' from namespace ''."	string
      catch (Exception                 /**/ ex) { ex.Log(typeof(T).Name); throw; }

      return (T)(Activator.CreateInstance(typeof(T)) ?? new T());
    }
  }
  public static class XmlIsoFileSerializer
  {
    public static void Save<T>(T o, string filenameONLY = "", IsolatedStorageScope iss = IsoConst.PdFls)
    {
      try
      {
        var isoStore = IsolatedStorageFile.GetStore(iss, null, null);

        using var isoStream = new IsolatedStorageFileStream(IsoHelper.GetSetFilename<T>(filenameONLY, "xml"), FileMode.Create, isoStore);
        //IsoHelper.DevDbgLookup(isoStore, isoStream);

        using var streamWriter = new StreamWriter(isoStream);
        new XmlSerializer(o?.GetType()).Serialize(streamWriter, o);
        streamWriter.Close();
      }
      catch (Exception ex) { ex.Log(); throw; }
    }

    public static T Load<T>(string filenameONLY = "", IsolatedStorageScope iss = IsoConst.PdFls) where T : new()
    {
      try
      {
        var isoStore = IsolatedStorageFile.GetStore(iss, null, null);


        if (isoStore.FileExists(IsoHelper.GetSetFilename<T>(filenameONLY, "xml")))
          using (var stream = new IsolatedStorageFileStream(IsoHelper.GetSetFilename<T>(filenameONLY, "xml"), FileMode.Open, FileAccess.Read, FileShare.Read, isoStore))
          {
            using var streamReader = XmlReader.Create(stream);
            var o = (T)(new XmlSerializer(typeof(T)).Deserialize(streamReader));
            streamReader.Close();
            return o;
          }
      }
      catch (InvalidOperationException ex) { if (ex.HResult != -2146233079) ex.Log(); throw; } // "Root element is missing." ==> create new at the bottom
      catch (Exception ex) { ex.Log(); throw; }

      return (T)(Activator.CreateInstance(typeof(T)) ?? new T());
    }
  }
}