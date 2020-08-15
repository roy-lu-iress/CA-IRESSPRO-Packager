using Iress.Sys.Helpers;
using IressPro.Packager.AppSettings;
using System;
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

    public SettingsEditor(AppStg appStg, string v)
    {
      InitializeComponent();

      _appStg = appStg;

      MouseLeftButtonDown += (s, e) => DragMove();

      tbkInfo.Text = v;
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
  }
}
