using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Iress.Sys.Helpers
{
  public static class NativeMethods
  {
    [DllImport("user32.dll")] internal static extern int SendMessage(int hWnd, uint wMsg, uint wParam, uint lParam);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] internal static extern int SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)] internal static extern IntPtr SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, string lParam, uint fuFlags, uint uTimeout, IntPtr lpdwResult);
    [DllImport("user32.dll", EntryPoint = "SendMessageTimeoutW", SetLastError = true, CharSet = CharSet.Unicode)] internal static extern uint SendMessageTimeoutW(IntPtr hWnd, int Msg, int countOfChars, StringBuilder text, uint flags, uint uTImeoutj, uint result);

#if BeeperIsOn
    [System.Runtime.InteropServices.DllImport("kernel32.dll")] internal static extern bool Beep(int freq, int dur);
#else
    internal static int Beep(int freq, int dur) => freq + dur;
#endif



    #region Win32 API declarations to set and get window placement
    public static bool SetWindowPlacement_(IntPtr hWnd, [In] ref WindowPlacement lpwndpl) => SetWindowPlacement(hWnd, ref lpwndpl);
    public static bool GetWindowPlacement_(IntPtr hWnd, out WindowPlacement lpwndpl) => GetWindowPlacement(hWnd, out lpwndpl);
    
    [DllImport("user32.dll")] static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement lpwndpl);
    [DllImport("user32.dll")] static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement lpwndpl);

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPlacement
    {
      public int length;
      public int flags;
      public int showCmd;
      public Point minPosition;
      public Point maxPosition;
      public Rect normalPosition;

      public override bool Equals(object? obj) => obj is WindowPlacement placement && length == placement.length && flags == placement.flags && showCmd == placement.showCmd && EqualityComparer<Point>.Default.Equals(minPosition, placement.minPosition) && EqualityComparer<Point>.Default.Equals(maxPosition, placement.maxPosition) && EqualityComparer<Rect>.Default.Equals(normalPosition, placement.normalPosition);

      public static bool operator ==(WindowPlacement left, WindowPlacement right) => left.Equals(right);

      public static bool operator !=(WindowPlacement left, WindowPlacement right) => !(left == right);
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
      internal int X, Y; internal Point(int x, int y) { X = x; Y = y; }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
      public int Left, Top, Right, Bottom;
      internal Rect(int left, int top, int right, int bottom) { Left = left; Top = top; Right = right; Bottom = bottom; }
    } // RECT structure required by WINDOWPLACEMENT structure

    //[Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WPContainer
    {
      public WindowPlacement WindowPlacement { get; set; }
      public double Zb { get; set; }
      public string Thm { get; set; }
    }
    #endregion
  }
}
