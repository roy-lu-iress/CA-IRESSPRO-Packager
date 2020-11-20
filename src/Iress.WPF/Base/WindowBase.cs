using Microsoft.Extensions.Logging;
using Iress.Sys.Ext;
using Iress.Sys.Helpers;
using Iress.WPF.Ext;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Iress.WPF.Base
{
  public partial class WindowBase : Window
  {
    const double _defaultZoomV = 1.25;
    const string _defaultTheme = "No Theme";
    const int _swShowNormal = 1, _swShowMinimized = 2, _margin = 16;

    protected bool IgnoreEscape { get; set; } = false;
    protected bool IgnoreWindowPlacement { get; set; } = false;
    string _isoFilenameONLY => $"{GetType().Name}.xml";

    readonly ILogger<WindowBase> _logger;
    public WindowBase() : this(new LoggerFactory().CreateLogger<WindowBase>()) { }
    public WindowBase(ILogger<WindowBase> logger)
    {
      _logger = logger;

      MouseLeftButtonDown += (s, e) => DragMove();  //jan23: using bad code to catch/recreate/replace it with the commented section below .. no luck yet, at least on Core 3.1 (Mar2020)

      MouseWheel += (s, e) => { if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) return; ZV += (e.Delta * .001); e.Handled = true; Debug.WriteLine(Title = $">>ZV:{ZV}"); }; //tu:

      KeyUp += (s, e) =>
      {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
          switch (e.Key)
          {
            default: break;
            case Key.OemMinus:  /**/ ZV /= 1.1; break;
            case Key.OemPlus:   /**/ ZV *= 1.1; break;
            case Key.D0:        /**/ ZV = 1; break;
          }
        else
          switch (e.Key)
          {
            default: break;
            case Key.Escape:
              if (!IgnoreEscape) Close();
              base.OnKeyUp(e); break;
          }
      };
    }

    public static readonly DependencyProperty ZVProperty = DependencyProperty.Register("ZV", typeof(double), typeof(WindowBase), new PropertyMetadata(_defaultZoomV)); public double ZV { get => (double)GetValue(ZVProperty); set => SetValue(ZVProperty, value); }
    public static readonly DependencyProperty ThmProperty = DependencyProperty.Register("Thm", typeof(string), typeof(WindowBase), new PropertyMetadata(_defaultTheme)); public string Thm { get => (string)GetValue(ThmProperty); set => SetValue(ThmProperty, value); }

    protected void ApplyTheme(string themeShade)
    {
      const string prefix = "/Iress.WPF;component/ColorScheme/Theme.Color.";

      try
      {
        if (_defaultTheme.Equals(themeShade, StringComparison.Ordinal) || Thm.Equals(themeShade, StringComparison.Ordinal))
        {
          return;
        }

        var suri = $"{prefix}{themeShade}.xaml";
        if (Application.LoadComponent(new Uri(suri, UriKind.RelativeOrAbsolute)) is ResourceDictionary dict)
        {
          ResourceDictionary rd;
          while ((rd = Application.Current.Resources.MergedDictionaries.First(r => ((System.Windows.Markup.IUriContext)r)?.BaseUri?.AbsolutePath?.Contains(prefix, StringComparison.OrdinalIgnoreCase) == true)) != null)
            Application.Current.Resources.MergedDictionaries.Remove(rd);

          Application.Current.Resources.MergedDictionaries.Add(dict);
        }
       
        Thm = themeShade;
      }
      catch (Exception ex) { _logger.LogError(ex, $""); ex.Pop(); throw; }
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);

      if (IgnoreWindowPlacement) return;

      NativeMethods.WindowPlacement winPlcmnt;
      try
      {
        try
        {
          var wpc = XmlIsoFileSerializer.Load<NativeMethods.WPContainer>(_isoFilenameONLY);
          ZV = wpc.Zb == 0 ? 1 : wpc.Zb;
          winPlcmnt = wpc.WindowPlacement;

          ApplyTheme(string.IsNullOrEmpty(wpc.Thm) ? _defaultTheme : wpc.Thm); // -- for Mail.sln - causes the error: Cannot find resource named 'WindowStyle_Aav0'. Resource names are case sensitive 
        }
        catch (InvalidOperationException ex)
        {
          ex.Log();
          ZV = 1d;
          winPlcmnt = XmlIsoFileSerializer.Load<NativeMethods.WindowPlacement>(_isoFilenameONLY);
        }
        catch (Exception ex) { ex.Log(); throw; }

        winPlcmnt.length = Marshal.SizeOf(typeof(NativeMethods.WindowPlacement));
        winPlcmnt.flags = 0;
        winPlcmnt.showCmd = (winPlcmnt.showCmd == _swShowMinimized ? _swShowNormal : winPlcmnt.showCmd);

        if (winPlcmnt.normalPosition.Bottom == 0 && winPlcmnt.normalPosition.Top == 0 && winPlcmnt.normalPosition.Left == 0 && winPlcmnt.normalPosition.Right == 0)
        {
          Trace.WriteLine($"1st time: Window Positions - all zeros! {SystemParameters.WorkArea.Width}x{SystemParameters.WorkArea.Height} is this the screen dims? Feb2020");

          winPlcmnt.normalPosition.Left = _margin;
          winPlcmnt.normalPosition.Right = winPlcmnt.normalPosition.Left + (int)ActualWidth;
          winPlcmnt.normalPosition.Top = _margin;

          if (winPlcmnt.normalPosition.Right > SystemParameters.WorkArea.Width)
          {
            winPlcmnt.normalPosition.Left = _margin;
            winPlcmnt.normalPosition.Top += (_margin + (int)ActualHeight);
          }

          winPlcmnt.normalPosition.Bottom = winPlcmnt.normalPosition.Top + (int)ActualHeight;
        }
        else
          Trace.WriteLine($"Window Positions  {_isoFilenameONLY}  not all zeros! {SystemParameters.WorkArea.Width}x{SystemParameters.WorkArea.Height} is this the screen dims? Jun2020 {winPlcmnt.normalPosition.Bottom}|{winPlcmnt.normalPosition.Top}x{winPlcmnt.normalPosition.Left}-{winPlcmnt.normalPosition.Right}");


        NativeMethods.SetWindowPlacement_(new WindowInteropHelper(this).Handle, ref winPlcmnt); //Note: if window was closed on a monitor that is now disconnected from the computer, SetWindowPlacement will place the window onto a visible monitor.
      }
      catch (InvalidOperationException ex) { ex.Log(); }
      catch (Exception ex) { ex.Log(); throw; }
    }
    protected override void OnClosing(CancelEventArgs e) // WARNING - Not fired when Application.SessionEnding is fired
    {
      base.OnClosing(e);

      NativeMethods.GetWindowPlacement_(new WindowInteropHelper(this).Handle, out var wp);
      XmlIsoFileSerializer.Save(new NativeMethods.WPContainer { WindowPlacement = wp, Zb = ZV, Thm = Thm }, _isoFilenameONLY);
    }
  }
}