using Iress.Sys.Helpers;
using System.Reflection;

namespace IressPro.Packager.AppSettings
{
  public partial class AppStg
  {
    public void Save()
    {
      if (_instance == null) return; //tu: ignore autosaves before fully rehydrated from the [iso] store.

      JsonIsoFileSerializer.Save<AppStg>(_instance);
    }

    #region Singletone
    public static AppStg Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (syncRoot)
          {
            if (_instance == null)
              _instance = JsonIsoFileSerializer.Load<AppStg>() as AppStg;

            if (_instance == null)
              _instance = new AppStg();
          }
        }

        return _instance;
      }
    }


    public AppStg() { }
    static volatile AppStg? _instance;
    static readonly object syncRoot = new object(); // multithreading support
    #endregion
  }

}
