
using System;
using System.IO;

namespace Iress.Sys.Helpers
{
  public static class Downloads // Core 3
  {
    public static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"); // Jan 2020: use Computer\HKEY_CURRENT_USER\Environment   'cause Asus2 is on D:

    public static string Folder(string subFolder) => Path.Combine(Root, subFolder);
  }
}