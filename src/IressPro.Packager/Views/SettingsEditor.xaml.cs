using Iress.Sys.Helpers;
using IressPro.Packager.AppSettings;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IressPro.Packager.Views
{
  public partial class SettingsEditor : Window
  {
    readonly AppStg _appStg;
    readonly Defaults _defaults;

    public SettingsEditor(AppStg appStg, string info, Defaults defaults)
    {
      InitializeComponent();

      _appStg = appStg;
      _defaults = defaults;

      MouseLeftButtonDown += (s, e) => DragMove();

      tbkInfo.Text = info;
      tbkVer.Text = VerHelper.CurVerStr("Core3");

      tbxStagingRoot.Text           /**/ = _appStg.StagingRoot;
      tbxPkgPrcsFolder.Text         /**/ = _appStg.PackageProcessFolder;
      tbxVersionDelimeter.Text      /**/ = _appStg.VersionDelimeter;
      cbxAutoOpenResults.IsChecked  /**/ = _appStg.DisconnectedMode;
      cbxAutoCopyToDrivY.IsChecked  /**/ = _appStg.IsParallelExectn;

      tbxIrsSrcFolder.Text           /**/ = _appStg.IrsSrcFolder;
      tbxIosSrcFolder.Text           /**/ = _appStg.IosSrcFolder;
      tbxRtlSrcFolder.Text           /**/ = _appStg.RtlSrcFolder;
      tbxIpsSrcFolder.Text           /**/ = _appStg.IpsSrcFolder;
      tbxAuxSrcFolder.Text           /**/ = _appStg.AuxSrcFolder;
      tbxSlfSrcFolder.Text           /**/ = _appStg.SlfSrcFolder;
    }

    void onSaveAndClose(object sender, RoutedEventArgs e)
    {
      _appStg.StagingRoot           /**/ = tbxStagingRoot.Text                   /**/ ;
      _appStg.PackageProcessFolder  /**/ = tbxPkgPrcsFolder.Text                 /**/ ;
      _appStg.VersionDelimeter      /**/ = tbxVersionDelimeter.Text              /**/ ;
      _appStg.DisconnectedMode      /**/ = cbxAutoOpenResults.IsChecked == true  /**/ ;
      _appStg.IsParallelExectn      /**/ = cbxAutoCopyToDrivY.IsChecked == true  /**/ ;

      _appStg.IrsSrcFolder           /**/ = tbxIrsSrcFolder.Text                   /**/ ;
      _appStg.IosSrcFolder           /**/ = tbxIosSrcFolder.Text                   /**/ ;
      _appStg.RtlSrcFolder           /**/ = tbxRtlSrcFolder.Text                   /**/ ;
      _appStg.IpsSrcFolder           /**/ = tbxIpsSrcFolder.Text                   /**/ ;
      _appStg.AuxSrcFolder           /**/ = tbxAuxSrcFolder.Text                   /**/ ;
      _appStg.SlfSrcFolder           /**/ = tbxSlfSrcFolder.Text                   /**/ ;

      _appStg.Save();

      Process.Start(new ProcessStartInfo { FileName = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe", StringComparison.OrdinalIgnoreCase) }); // temp poor man's reload.
      App.Current.Shutdown();

      Close();
    }
    void onTextChanged(object s, TextChangedEventArgs e) => chackExistanceAdjustUI(s);
    void onCreateDir(object s, MouseButtonEventArgs e)
    {
      try
      {
        Directory.CreateDirectory(((TextBox)s).Text);
        chackExistanceAdjustUI(s);
        tbkInfo.Text = $"Folder  '{((TextBox)s).Text}'  has been successfully created.";
      }
      catch (Exception ex) { tbkInfo.Text = $"{ex.Message}"; }
    }

    void chackExistanceAdjustUI(object s)
    {
      tbkInfo.Foreground = new SolidColorBrush(Directory.Exists(((TextBox)s).Text) ? Colors.Green : Colors.OrangeRed);
      ((TextBox)s).BorderBrush = new SolidColorBrush(Directory.Exists(((TextBox)s).Text) ? Colors.Transparent : Colors.OrangeRed);
      ((TextBox)s).ToolTip = Directory.Exists(((TextBox)s).Text) ? "" : "Folder does not exist.\nDoubleclick to create";
    }

    void onSetDefaults(object s, RoutedEventArgs e)
    {
      SourceFolders? src;
      switch (((MenuItem)s).Tag)
      {
        case "AUS": src = _defaults?.AUS; break;
        case "CAN": src = _defaults?.CAN; break;
        case "NZL": src = _defaults?.NZL; break;
        case "SGP": src = _defaults?.SGP; break;
        case "RSA": src = _defaults?.RSA; break;
        case "GBR": src = _defaults?.GBR; break;
        case "exp": src = _defaults?.EXP; break;
        case "DEV": src = _defaults?.DEV; break;
        default: src = _defaults?.CAN; MessageBox.Show($"This feature is currently under construction:\n\n{((MenuItem)s).Tag}\n\nUse on your own risk", "Hello!", MessageBoxButton.OK, MessageBoxImage.Information); break;
      }

      tbxIrsSrcFolder.Text = src?.IrsSrcFolder;
      tbxIosSrcFolder.Text = src?.IosSrcFolder;
      tbxRtlSrcFolder.Text = src?.RtlSrcFolder;
      tbxIpsSrcFolder.Text = src?.IpsSrcFolder;
      tbxAuxSrcFolder.Text = src?.AuxSrcFolder;
      tbxSlfSrcFolder.Text = src?.SlfSrcFolder;
    }
  }
}
