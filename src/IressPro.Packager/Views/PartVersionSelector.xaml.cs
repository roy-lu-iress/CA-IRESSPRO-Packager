using Iress.WPF.Ext;
using IressPro.Packager.AppSettings;
using IressPro.Packager.IO;
using IressPro.Packager.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IressPro.Packager
{
  public partial class PartVersionSelector : UserControl
  {
    readonly PackagerCoreBL _packager;
    readonly AppStg _appStg;
    bool _isLoaded;

    public PartVersionSelector() : this(new PackagerCoreBL(AppStg.Instance), AppStg.Instance) { }
    public PartVersionSelector(PackagerCoreBL pkgr, AppStg appStg)
    {
      InitializeComponent();
      _appStg = appStg;
      _packager = pkgr;

      Loaded += (s, e) =>
      {
        for (var i = 9; i > 0 && (!(
          Directory.Exists(_appStg.IrsSrcFolder) &&
          Directory.Exists(_appStg.IosSrcFolder) &&
          Directory.Exists(_appStg.RtlSrcFolder) &&
          Directory.Exists(_appStg.IpsSrcFolder) &&
          Directory.Exists(_appStg.RegSrcFolder) &&
          Directory.Exists(_appStg.SlfSrcFolder))); i--)
        {
          Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f}  onShowSettings()");
          var se = new SettingsEditor(_appStg, $"Only  {i}  attempts left", new Defaults()) { Owner = WpfUtils.FindParentWindow(this) };
          se.ShowDialog();
        }

        PackagerCoreBL.LoadListBox(lbx1, _appStg.IrsSrcFolder, _appStg.IrsPattern);
        PackagerCoreBL.LoadListBox(lbx2, _appStg.IosSrcFolder, _appStg.IosPattern);
        PackagerCoreBL.LoadListBox(lbx3, _appStg.RtlSrcFolder, _appStg.RtlPattern);
        PackagerCoreBL.LoadListBox(lbx4, _appStg.IpsSrcFolder, _appStg.IpsPattern);
        PackagerCoreBL.LoadListBox(lbx5, _appStg.RegSrcFolder, _appStg.RegPattern);
        PackagerCoreBL.LoadListBox(lbxZ, _appStg.SlfSrcFolder, _appStg.SlfPattern);

        chk1.IsChecked = _appStg.IsChecked1;
        chk2.IsChecked = _appStg.IsChecked2;
        chk3.IsChecked = _appStg.IsChecked3;
        chk4.IsChecked = _appStg.IsChecked4;
        chk5.IsChecked = _appStg.IsChecked5;
        chkZ.IsChecked = _appStg.IsCheckedZ;

        lbx1.SelectedIndex = _appStg.LbxSelectedIndex1; //lbx3.SelectedIndex = lbx4.SelectedIndex = lbx5.SelectedIndex = lbxZ.SelectedIndex = -1;
        lbx2.SelectedIndex = _appStg.LbxSelectedIndex2;
        lbx3.SelectedIndex = _appStg.LbxSelectedIndex3;
        lbx4.SelectedIndex = _appStg.LbxSelectedIndex4;
        lbx5.SelectedIndex = _appStg.LbxSelectedIndex5;
        lbxZ.SelectedIndex = _appStg.LbxSelectedIndexZ;

#if DEBUG
        pnlUnderC.Visibility = Visibility.Collapsed;
#elif LOG_FILE_PER_SESSION
#else
        //pnlUnderC.Visibility = Visibility.Visible;
#endif
        _isLoaded = true;
      };

    }

    void onChkChanged(object s, RoutedEventArgs e) => recalclulate();
    void onSelectionChanged(object s, SelectionChangedEventArgs e) => recalclulate();
    async void onCreatePkg(object s, RoutedEventArgs e)
    {
      root.IsEnabled = false;
      try
      {
        var sw = Stopwatch.StartNew();
        Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f}  onCreatePkg({tbkPkgFilename.Text}) ");

        _appStg.LbxSelectedIndex1 = lbx1.SelectedIndex;
        _appStg.LbxSelectedIndex2 = lbx2.SelectedIndex;
        _appStg.LbxSelectedIndex3 = lbx3.SelectedIndex;
        _appStg.LbxSelectedIndex4 = lbx4.SelectedIndex;
        _appStg.LbxSelectedIndex5 = lbx5.SelectedIndex;
        _appStg.LbxSelectedIndexZ = lbxZ.SelectedIndex;
        _appStg.IsChecked1 = chk1.IsChecked == true;
        _appStg.IsChecked2 = chk2.IsChecked == true;
        _appStg.IsChecked3 = chk3.IsChecked == true;
        _appStg.IsChecked4 = chk4.IsChecked == true;
        _appStg.IsChecked5 = chk5.IsChecked == true;
        _appStg.IsCheckedZ = chkZ.IsChecked == true;

        _appStg.Save();

        //await Task.Delay(999).ConfigureAwait(false);

        if (chkZ.IsChecked == true)
        {
          await _packager.CreateSelfPackage(SlfSrcFolder, tbkPkgFilename.Text, _appStg.DisconnectedMode, _appStg.IsParallelExectn); // The calling thread cannot access this object because a different thread owns it.            .ConfigureAwait(false);
        }
        else
        {
          await _packager.CreateComboPackage(
            new[] {
              chk1.IsChecked != true ? null : IrsSrcFolder,
              chk2.IsChecked != true ? null : IosSrcFolder,
              chk3.IsChecked != true ? null : RtlSrcFolder,
              chk4.IsChecked != true ? null : IpsSrcFolder,
              chk5.IsChecked != true ? null : RegSrcFolder},
            tbkPkgFilename.Text, _appStg.DisconnectedMode, _appStg.IsParallelExectn); // thread access violation for root.Enbled: .ConfigureAwait(false); 
        }

        Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f}  onCreatePkg(...) took  {sw.Elapsed:mm\\:ss}  altogether  ■ ■ ■");
      }
      catch (Exception ex) { ex.Pop(); }
      finally { root.IsEnabled = true; }
    }

    void recalclulate()
    {
      if (!_isLoaded || lbx1 == null) return;
      try
      {
#if !BEFORE_Jan15
        if (chkZ.IsChecked == true) { chk1.IsChecked = chk2.IsChecked = chk3.IsChecked = chk4.IsChecked = false; } else { chk1.IsChecked = chk2.IsChecked = true; }
#else
#endif

        lbx1.IsEnabled = chk1.IsChecked == true;
        lbx2.IsEnabled = chk2.IsChecked == true;
        lbx3.IsEnabled = chk3.IsChecked == true;
        lbx4.IsEnabled = chk4.IsChecked == true;
        lbx5.IsEnabled = chk5.IsChecked == true;

        if (!lbx1.IsEnabled) lbx1.SelectedIndex = -1;
        if (!lbx2.IsEnabled) lbx2.SelectedIndex = -1;
        if (!lbx3.IsEnabled) lbx3.SelectedIndex = -1;
        if (!lbx4.IsEnabled) lbx4.SelectedIndex = -1;
        if (!lbx5.IsEnabled) lbx5.SelectedIndex = -1;

        var a1 = chk1.IsChecked != true ? "" : $"IressPro{lbx1?.SelectedItem}-";
        var a2 = chk2.IsChecked != true ? "" : $"IOS{lbx2?.SelectedItem}-";
        var a3 = chk3.IsChecked != true ? "" : $"Retail{lbx3?.SelectedItem}-";
        var a4 = chk4.IsChecked != true ? "" : $"IPS{lbx4?.SelectedItem}-";
        var a5 = chk5.IsChecked != true ? "" : $"Regist{lbx5?.SelectedItem}";
        var aZ = chkZ.IsChecked != true ? "" : $"{lbxZ?.SelectedItem}";

#if !BEFORE_Jan15
        tbkPkgFilename.Text = (chkZ.IsChecked == true ? $"{aZ}".Trim(new[] { '-' }) : $"{a1}{a2}{a3}{a4}{a5}".Trim(new[] { '-' })).Replace(" ", "_", StringComparison.Ordinal).Replace(".", "_", StringComparison.Ordinal) + ".exe";
#else
        tbkPkgFilename.Text = $"{a1}{a2}{a3}{a4}{a5}".Trim(new[] { '-' }) + ".exe";
#endif
        const string installer = "Setup.exe";
        tbkInfo.Text = "";
        if (chk1.IsChecked == true && lbx1?.SelectedIndex >= 0) { if (!File.Exists(Path.Combine(IrsSrcFolder, installer))) tbkInfo.Text += $"Warning: No  '{installer}'  in  '{IrsSrcFolder}' \n"; }
        if (chk2.IsChecked == true && lbx2?.SelectedIndex >= 0) { if (!File.Exists(Path.Combine(IosSrcFolder, installer))) tbkInfo.Text += $"Warning: No  '{installer}'  in  '{IosSrcFolder}' \n"; }
        if (chk3.IsChecked == true && lbx3?.SelectedIndex >= 0) { if (!File.Exists(Path.Combine(RtlSrcFolder, installer))) tbkInfo.Text += $"Warning: No  '{installer}'  in  '{RtlSrcFolder}' \n"; }
        if (chk4.IsChecked == true && lbx4?.SelectedIndex >= 0) { if (!File.Exists(Path.Combine(IpsSrcFolder, installer))) tbkInfo.Text += $"Warning: No  '{installer}'  in  '{IpsSrcFolder}' \n"; }
        if (chk5.IsChecked == true && lbx5?.SelectedIndex >= 0) { if (!File.Exists(Path.Combine(RegSrcFolder, installer))) tbkInfo.Text += $"Warning: No  '{installer}'  in  '{RegSrcFolder}' \n"; }
        if (chkZ.IsChecked == true && lbxZ?.SelectedIndex >= 0) { if (!File.Exists(Path.Combine(SlfSrcFolder, installer))) tbkInfo.Text += $"Warning: No  '{installer}'  in  '{SlfSrcFolder}' \n"; }

        btnCreatePkg.IsEnabled = (lbx1?.SelectedIndex >= 0 && lbx2?.SelectedIndex >= 0) || lbxZ?.SelectedIndex >= 0;

        tbkInfo.Text = btnCreatePkg.IsEnabled ?
          tbkInfo.Text.Trim(new char[] { '\n' }) :
          "^^ Please select versions to package from above !!!";
      }
      catch (Exception ex) { ex.Pop(); }
      finally { Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f}  recalclulate => {tbkPkgFilename.Text}) "); }
    }

    string IrsSrcFolder => Path.Combine(_appStg.IrsSrcFolder, $"{_appStg.IrsPattern.Substring(0, 08)}{lbx1.SelectedItem}");
    string IosSrcFolder => Path.Combine(_appStg.IosSrcFolder, $"{_appStg.IosPattern.Substring(0, 11)}{lbx2.SelectedItem}");
    string RtlSrcFolder => Path.Combine(_appStg.RtlSrcFolder, $"{_appStg.RtlPattern.Substring(0, 18)}{lbx3.SelectedItem}");
    string IpsSrcFolder => Path.Combine(_appStg.IpsSrcFolder, $"{_appStg.IpsPattern.Substring(0, 06)}{lbx4.SelectedItem}");
    string RegSrcFolder => Path.Combine(_appStg.RegSrcFolder, $"{_appStg.RegPattern.Substring(0, 00)}{lbx5.SelectedItem}");
    string SlfSrcFolder => Path.Combine(_appStg.SlfSrcFolder, $"{_appStg.SlfPattern.Substring(0, 00)}{lbxZ.SelectedItem}");

    public string PkgFilenameExe => tbkPkgFilename.Text;

    async void onLoaded(object sender, RoutedEventArgs e)
    {
      await Task.Delay(1999);
      System.Media.SystemSounds.Beep.Play();
      recalclulate();
    }
  }
}
