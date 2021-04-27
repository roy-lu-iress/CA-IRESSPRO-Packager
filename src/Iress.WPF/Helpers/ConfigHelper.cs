using Iress.WPF.Ext;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;

namespace Iress.WPF.Helpers
{
  public class ConfigHelper //todo: Copy to the main Shared resourse and reuse for 'new ConfigurationBuilder()'.
  {
    const string _defaultValues = @"{{
      ""WhereAmI"": ""{0}"",
      ""LogFolder"": ""\\\\bbsfile01\\Public\\AlexPi\\Misc\\Logs\\PermMgr.DFLT..txt"",
      ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
      ""SqlConStr"": ""Server=mtDEVsqldb;Database=Inventory;Trusted_Connection=True;"",
      ""AppSettings"": {{
        ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
        ""RmsDbConStr"": ""Server={{0}};Database={{1}};Trusted_Connection=True;"",
        ""KeyVaultURL"": ""<moved to a safer place>""
      }}
}}";

    class WhatIsThatForType { public string MyProperty { get; set; } = "<Default Value of Nothing SPecial>"; }
    public static IConfigurationRoot InitConfig(string appsettingsFile, string defaultValues = _defaultValues)
    {
      try
      {
        for (var i = 0; i < 3; i++)
          try
          {
            return new ConfigurationBuilder()
              .SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile(appsettingsFile)
              .AddUserSecrets<WhatIsThatForType>()
              .Build();
          }
          catch (InvalidOperationException ex)
          {
            ex.Pop("Disaster ...");
          }
          catch (FileNotFoundException ex)
          {
            if (!tryCreateDefaultFile(appsettingsFile, defaultValues))
              ex.Pop("The default values (instead of appSettings') will be used  ...maybe, when implemented");
          }
          catch (FormatException ex)
          {
            new Process { StartInfo = new ProcessStartInfo("Notepad.exe", $"\"{appsettingsFile}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start();
            ex.Pop($"Try to edit the errors out from {appsettingsFile}");
          }
          catch (Exception ex)
          {
            if (!tryCreateDefaultFile(appsettingsFile, defaultValues))
              ex.Pop("██  ██  ██  Take a look!");
          }

        throw new Exception($"Unable to create default  {appsettingsFile}  file");
      }
      catch (Exception ex)
      {
        ex.Pop("The default values will be used  ...maybe");
      }

      throw new Exception($"Unable to create default  {appsettingsFile}  file");
    }

    static bool tryCreateDefaultFile(string appsettingsFile, string defaultValues)
    {
      try
      {
        if (!File.Exists(appsettingsFile))
          File.WriteAllText(appsettingsFile, string.Format(defaultValues, appsettingsFile.Replace(@"\", @"\\")));

        return true;
      }
      catch (Exception ex)
      {
        ex.Pop();
        return false;
      }
    }
  }
}
