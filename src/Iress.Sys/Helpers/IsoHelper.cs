namespace Iress.Sys.Helpers
{
  public static class IsoHelper
  {
    public static string GetSetFilename<T>(string filenameONLY, string ext) => $"{(string.IsNullOrEmpty(filenameONLY) ? typeof(T).Name : filenameONLY)}.{ext}";
  }
}
