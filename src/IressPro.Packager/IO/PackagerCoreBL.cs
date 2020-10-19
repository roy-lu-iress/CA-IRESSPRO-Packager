using Iress.Sys.Helpers;
using Iress.WPF.Ext;
using IressPro.Packager.AppSettings;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IressPro.Packager.IO
{
  public class PackagerCoreBL
  {
    string? _cliRar_exe, _winRar_exe;
    readonly AppStg _appStg;

    public PackagerCoreBL(AppStg appStg)
    {
      _appStg = appStg;
      var dummy = IsWinRarInstalled;
    }

    public bool IsWinRarInstalled
    {
      get
      {
        if (_cliRar_exe != null)
          return true;

        tryFindWinRar();

        return _cliRar_exe != null;
      }
    }
    public static void LoadListBox(ListBox listbox, string folder, string pattern)
    {
      try
      {
        listbox?.Items.Clear();
        var dashIdx = pattern?.IndexOf('-', StringComparison.OrdinalIgnoreCase) ?? -1;
        var stargIdx = (folder?.Length ?? 0) + (dashIdx > -1 ? dashIdx : -2) + 2;

        Directory.GetDirectories(folder, pattern).OrderByDescending(r => r).ToList().ForEach(ver => listbox?.Items.Add(ver.Substring(stargIdx)));
      }
      catch (Exception ex) { ex.Pop($"Either make sure the folder {folder} is accessible ...\n\t... or adjust the setings to point to another folder in the above menu:  ° ° °"); }
    }

    public async Task CreateComboPackage(string?[] sourceFolders, string pkgFilenameExe, bool autoOpen, bool autoCopyToYDrive)
    {
      try
      {
        theSetup(pkgFilenameExe, out var folderEndings, out var stgDir, out var sfxDir);

        await ProcessExt.ExecuteAsync(_appStg.Robocopy, $"\"{_appStg.PackageProcessFolder.Trim(folderEndings)}\"  \"{stgDir.Trim(folderEndings)}\"  *.wsp *.reg  {pkgFilenameExe}   /XO /NJH /NJS").ConfigureAwait(false); // copy ..\PackageProcess\ to the staging root

        await copySetupExeBasedComboToStaging(sourceFolders, pkgFilenameExe, folderEndings, stgDir).ConfigureAwait(false);

        await generateAutoRunBatFileCombo(sourceFolders, Path.Combine(stgDir, _appStg.AutoRunFilename)).ConfigureAwait(false);

        await compress(pkgFilenameExe, autoOpen, autoCopyToYDrive, folderEndings, stgDir, sfxDir).ConfigureAwait(false);
      }
      catch (Exception ex) { ex.Pop($"\n\n{pkgFilenameExe}     \n"); }
    }
    public async Task CreateSelfPackage(string sourceFolder, string pkgFilenameExe, bool autoOpen, bool autoCopyToYDrive)
    {
      try
      {
        theSetup(pkgFilenameExe, out var folderEndings, out var stgDir, out var sfxDir);

        await copySelfToStaging(sourceFolder, stgDir, folderEndings).ConfigureAwait(false);

        await generateAutoRunBatFileSelf(Path.Combine(stgDir, _appStg.AutoRunFilename)).ConfigureAwait(false);

        await compress(pkgFilenameExe, autoOpen, autoCopyToYDrive, folderEndings, stgDir, sfxDir).ConfigureAwait(false);
      }
      catch (Exception ex) { ex.Pop($"\n\n{pkgFilenameExe}     \n"); }
    }

    void theSetup(string pkgFilenameExe, out char[] folderEndings, out string stgDir, out string sfxDir)
    {
      folderEndings = new[] { '\\', ' ' };
      var trgDir = Path.Combine(_appStg.StagingRoot, $"{DateTimeOffset.Now:yyyy-MM-dd}\\");
      var pkgNme = pkgFilenameExe.Substring(0, pkgFilenameExe.Length - 4);

      stgDir = Path.Combine(trgDir, pkgNme, "Staging");
      sfxDir = Path.Combine(trgDir, pkgNme, "SFX");

      FSHelper.ExistsOrCreated(sfxDir);
      FSHelper.ExistsOrCreated(stgDir);
    }
    async Task compress(string pkgFilenameExe, bool autoOpen, bool autoCopyToYDrive, char[] folderEndings, string stgDir, string sfxDir)
    {
      var rarStngsPathFile = await generateRarSettingsFile().ConfigureAwait(false);

      var pkgPathFilename_exe = Path.Combine(sfxDir, pkgFilenameExe);

      Directory.SetCurrentDirectory(stgDir); // looks like we need write permissions in the cur dir.
      Debug.WriteLine($"e> *** CurrentDirectory : {Directory.GetCurrentDirectory()}");

      if (_cliRar_exe != null)
        await ProcessExt.ExecuteAsync(_cliRar_exe, $"a -r -ep1 -sfx -z\"{rarStngsPathFile}\" \"{pkgPathFilename_exe}\" \"{stgDir}\\\"").ConfigureAwait(false);

      File.Delete(rarStngsPathFile);

      if (autoOpen)
      {
        await ProcessExt.ExecuteAsync("Explorer", $"\"{sfxDir}\"").ConfigureAwait(false);
        await ProcessExt.ExecuteAsync("Explorer", $"\"{_appStg.TargetBetaRetailFolderY}\"").ConfigureAwait(false);
        if (_winRar_exe != null)
          await ProcessExt.ExecuteAsync(_winRar_exe, $"\"{pkgPathFilename_exe}\"", 2500).ConfigureAwait(false);
      }

      if (autoCopyToYDrive) // copy to ultimate target Y-folder
      {
        await ProcessExt.ExecuteAsync(_appStg.Robocopy, $"\"{sfxDir.Trim(folderEndings)}\"  \"{_appStg.TargetBetaRetailFolderY.Trim(folderEndings)}\"  *.exe  /XO /NJH /NJS").ConfigureAwait(false);
      }
    }
    async Task<string> generateRarSettingsFile()
    {
      var rarStngsPathFile = Path.Combine(_appStg.StagingRoot, _appStg.rarStngsFilename);
      if (!File.Exists(rarStngsPathFile))
        await File.WriteAllTextAsync(rarStngsPathFile, _appStg.rarStngsContents).ConfigureAwait(false);
      if (!File.Exists(rarStngsPathFile))
        throw new FileNotFoundException(rarStngsPathFile);
      return rarStngsPathFile;
    }
    async Task copySetupExeBasedComboToStaging(string?[] sourceFolders, string pkgFilenameExe, char[] folderEndings, string stgDir)
    {
      await ProcessExt.ExecuteAsync(_appStg.Robocopy, $"\"{_appStg.PackageProcessFolder.Trim(folderEndings)}\"  \"{stgDir.Trim(folderEndings)}\"  *.wsp *.reg  {pkgFilenameExe}   /XO /NJH /NJS").ConfigureAwait(false); // copy ..\PackageProcess\ to the staging root

      var i = 0;
      foreach (var sourceFolder in sourceFolders)
      {
        if (sourceFolder != null)
        {
          var args = $"\"{sourceFolder.Trim(folderEndings)}\" \"{Path.Combine(stgDir, _appStg.StagingSubfolders[i])}\"  {wildcards(i)} /XO /NJH /NJS"; //todo: for non-SETUP.EXE installers - need more wits in the Autorun.bat area.
          await ProcessExt.ExecuteAsync(_appStg.Robocopy, args).ConfigureAwait(false);
        }
        i++;
      }
    }

    static string wildcards(int i)
    {
      switch (i)
      {
        case 4:
        case 5: return "*.*"; // for extra files to pack into the target root from the Reg and SelfContained listbox folder.
        default: return "setu?.exe";
      }
    }

    async Task copySelfToStaging(string sourceFolder, string targetStgDir, char[] folderEndings)
    {
      var args = $"\"{sourceFolder.Trim(folderEndings)}\" \"{targetStgDir.Trim(folderEndings)}\"  /XO /NJH /NJS";

      await ProcessExt.ExecuteAsync(_appStg.Robocopy, args).ConfigureAwait(false);
    }
    async Task generateAutoRunBatFileCombo(string?[] a, string autorunPathFileExt)
    {
      var autorunContent =
            a[2] == null && a[3] == null ? AppStg.BatCtx_IrsIos______ :
            a[2] != null && a[3] == null ? AppStg.BatCtx_IrsIosRtl___ :
            a[2] == null && a[3] != null ? AppStg.BatCtx_IrsIos___Ips :
            a[2] != null && a[3] != null ? AppStg.BatCtx_IrsIosRtlIps : AppStg.BatCtx_IrsIos______;

      await File.WriteAllTextAsync(autorunPathFileExt, autorunContent).ConfigureAwait(false);
    }
    async Task generateAutoRunBatFileSelf(string autorunPathFileExt) => await File.WriteAllTextAsync(autorunPathFileExt, AppStg.BatCtx_CIBC).ConfigureAwait(false);

    void tryFindWinRar()
    {
      for (int i = 33; i >= 0; i--)
      {
        _cliRar_exe =
          File.Exists(_appStg.rar64) ? _appStg.rar64 : File.Exists(_appStg.rar86) ? _appStg.rar86 :
          null;
        _winRar_exe =
          File.Exists(_appStg.wrr64) ? _appStg.wrr64 : File.Exists(_appStg.wrr86) ? _appStg.wrr86 :
          null;

        if (_cliRar_exe != null && _winRar_exe != null)
          return;

        SystemSounds.Hand.Play();

        var rv = MessageBox.Show("Please install WinRar from:\r\n\n" +
          @"    F:\ahmad\Laptop Move\Downloads\winrar-x64-560.exe " +
          $"\r\n       or from \r\n" +
          $"    https://www.rarlab.com/ \r\n\n" +
          $"and click OK to continue\r\n\n" +
          $"...or click Cancel to exit the app" +
          $"", $"Packager needs WinRar ... Try # {i}", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

        if (rv == MessageBoxResult.Cancel || i <= 1)
        {
          SystemSounds.Asterisk.Play();
          App.Current?.Shutdown();
          return;
        }
      }
    }
  }
}
