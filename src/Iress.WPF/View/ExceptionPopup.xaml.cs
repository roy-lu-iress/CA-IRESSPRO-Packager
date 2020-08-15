﻿using Iress.Sys.Ext;
using System;
using System.Windows;

namespace Iress.WPF.View
{
  public partial class ExceptionPopup : Iress.WPF.Base.WindowBase
  {
    public ExceptionPopup() => InitializeComponent();

    public ExceptionPopup(Exception ex, string optl, string cmn, string cfp, int cln) : this()
    {
      Loaded += (s, e) =>
      {
        ExType.Text = ex?.GetType().Name;
        callerFL.Text = $"{cfp} ({cln}): ";
        methodNm.Text = $"{cmn}()";
        optnlMsg.Text = optl;
        innrMsgs.Text = $"{ex.InnerMessages()}";
      };
    }
    void onClose(object s, RoutedEventArgs e) => Close(); // close popup and continue app execution
    void onShutdown(object s, RoutedEventArgs e) => Application.Current.Shutdown();
    void T4_MouseDown(object s, System.Windows.Input.MouseButtonEventArgs e) => onCopyClose(s, e);
    void onCopyClose(object s, RoutedEventArgs e)
    {
      Clipboard.SetText($"{callerFL.Text}\r\n{methodNm.Text}\r\n{optnlMsg.Text}\r\n{innrMsgs.Text}");
      onClose(s, e);
    }
  }
}