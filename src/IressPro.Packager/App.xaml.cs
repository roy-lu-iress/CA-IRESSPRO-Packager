using Iress.Sys.Helpers;
using Iress.WPF.Helpers;
using IressPro.Packager.AppSettings;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace IressPro.Packager
{
  public partial class App : Application
  {
    public static readonly DateTimeOffset Started = DateTimeOffset.Now;

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      Current.DispatcherUnhandledException += UnhandledExceptionHndlr.OnCurrentDispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox)?.SelectAll(); }));

      new MainWindowIP(AppStg.Instance).Show();
      Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f} +{(DateTimeOffset.Now - Started):mm\\:ss\\.ff}    {VerHelper.CurVerStr("Core3")}  ");
    }

    protected override void OnExit(ExitEventArgs e) => Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f} +{(DateTimeOffset.Now - Started):mm\\:ss\\.ff}    OnExit  ().");
  }
}