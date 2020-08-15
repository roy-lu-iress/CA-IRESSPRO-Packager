using Iress.Sys.Helpers;
using IressPro.Packager.AppSettings;
using IressPro.Packager.IO;
using IressPro.Packager.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace IressPro.Packager
{
  public partial class MainWindowIP : Iress.WPF.Base.WindowBase
  {
    readonly AppStg _appStg;
    bool _isLoaded = false;

    public MainWindowIP(AppStg appStg)
    {
      InitializeComponent();
      _appStg = appStg;

      Loaded += /*async */(s, e) =>
      {
        tbkVer.Text = VerHelper.CurVerStr("Core3");        //await Task.Delay(750).ConfigureAwait(false);

        mnuAutoOpenResults2.IsChecked = mnuAutoOpenResults1.IsChecked = _appStg.DisconnectedMode;
        mnuAutoCopyToDrivY2.IsChecked = mnuAutoCopyToDrivY1.IsChecked = _appStg.IsParallelExectn;

        _isLoaded = true;
      };
      themeSelector1.ApplyTheme = ApplyTheme;
    }

    void onExit(object s, RoutedEventArgs e)
    {
      Close();
      App.Current.Shutdown();
    }
    void onShowSettings(object s, RoutedEventArgs e)
    {
      Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f}  onShowSettings()");
      var se = new SettingsEditor(_appStg, " ") { Owner = this };
      se.ShowDialog();
    }
    void onChkd1(object sender, RoutedEventArgs e)
    {
      if (!_isLoaded) return;
      _appStg.DisconnectedMode = mnuAutoOpenResults2.IsChecked = mnuAutoOpenResults1.IsChecked == true;
      _appStg.IsParallelExectn = mnuAutoCopyToDrivY2.IsChecked = mnuAutoCopyToDrivY1.IsChecked == true;
      _appStg.Save();
    }
    void onChkd2(object sender, RoutedEventArgs e)
    {
      if (!_isLoaded) return;
      _appStg.DisconnectedMode = mnuAutoOpenResults1.IsChecked = mnuAutoOpenResults2.IsChecked == true;
      _appStg.IsParallelExectn = mnuAutoCopyToDrivY1.IsChecked = mnuAutoCopyToDrivY2.IsChecked == true;
      _appStg.Save();
    }

    async void onExplorerTargetFolders(object sender, RoutedEventArgs e)
    {
      var folderEndings = new[] { '\\', ' ' };
      var trgDir = Path.Combine(_appStg.StagingRoot, $"{DateTimeOffset.Now:yyyy-MM-dd}\\");

      var pkgFilenameExe = pvs.PkgFilenameExe;

      var pkgNme = pkgFilenameExe.Substring(0, pkgFilenameExe.Length - 4);
      var sfxDir = Path.Combine(trgDir, pkgNme, "SFX");

      FSHelper.ExistsOrCreated(sfxDir);

      await ProcessExt.ExecuteAsync("Explorer", $"\"{sfxDir}\"").ConfigureAwait(false);
      await ProcessExt.ExecuteAsync("Explorer", $"\"{_appStg.TargetBetaRetailFolderY}\"").ConfigureAwait(false);
    }
  }
}
