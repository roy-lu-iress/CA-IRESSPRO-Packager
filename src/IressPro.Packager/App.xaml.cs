using Iress.Sys.Helpers;
using Iress.WPF.Helpers;
using IressPro.Packager.AppSettings;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
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

      const string suffix = "Pkg.";

#if LogErrors
      var logFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{suffix}Logs";
      if (!Directory.Exists(logFolder))
        Directory.CreateDirectory(logFolder);

      Trace.Listeners.Add(new TextWriterTraceListener(@$"{logFolder}\ErrorLog.{DateTime.Now:yyyy-MM-dd.HHmm}.txt"));
#endif

      Current.DispatcherUnhandledException += UnhandledExceptionHndlr.OnCurrentDispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox)?.SelectAll(); }));

#if false
      var builder = new ConfigurationBuilder();
      builder
        .SetBasePath(AppContext.BaseDirectory)
      //.SetBasePath(Directory.GetCurrentDirectory()) //nogo: lands in ~C:\Windows\SystemWOW\...
      //.AddJsonFile($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\appsettings.{suffix}json", optional: false, reloadOnChange: true)
      //.AddJsonFile($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\appsettings.{suffix}{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{suffix}json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{suffix}{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

      var cfg = builder.Build();
#else
      var cfg = ConfigHelper.InitConfig(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), $"appsettings.{suffix}json"), ConstDefaults._fallback);
#endif
      var defaults = cfg.GetSection("Defaults").Get<Defaults>();

#if AltLoggerIsUp
      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(cfg)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();
      Log.Logger.Information($"────   App starting   {new string('─', 128)}\n".Substring(0, 140));
#endif

      new MainWindowIP(AppStg.Instance, defaults).Show();
      Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f} +{(DateTimeOffset.Now - Started):mm\\:ss\\.ff}    {VerHelper.CurVerStr("Core3")}  ");
    }

    protected override void OnExit(ExitEventArgs e) => Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f} +{(DateTimeOffset.Now - Started):mm\\:ss\\.ff}    OnExit  ().");
   
  }
}