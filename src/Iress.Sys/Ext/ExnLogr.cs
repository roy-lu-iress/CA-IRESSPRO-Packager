using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Iress.Sys.Ext
{
  public static class ExnLogr 
  {
    public static TraceSwitch AppTraceLevelCfg => new TraceSwitch("CfgTraceLevelSwitch", "Switch in config file:  <system.diagnostics><switches><!--0-off, 1-error, 2-warn, 3-info, 4-verbose. --><add name='CfgTraceLevelSwitch' value='3' /> ");

    public static string Log(this Exception ex, string? optl = null, [CallerMemberName] string? cmn = "", [CallerFilePath] string cfp = "", [CallerLineNumber] int cln = 0)
    {
      var msgForPopup = $"{ex?.InnerMessages()}\r\n{ex?.GetType().Name} at {cfp}({cln}): {cmn}() {optl}";

      Trace.WriteLine($"{DateTimeOffset.Now:HH:mm:ss.f} ██ {msgForPopup.Replace("\n", "  ").Replace("\r", "  ")}"); 

      traceStackTraceIfVerbose(ex);

      if (Debugger.IsAttached)
        Debugger.Break();

      return msgForPopup;
    }

    static void traceStackTraceIfVerbose(Exception? ex)
    {
      if (AppTraceLevelCfg.TraceVerbose)
      {
        var prevLv = Trace.IndentLevel;
        var prevSz = Trace.IndentSize;
        Trace.IndentLevel = 2;
        Trace.IndentSize = 2;
        Trace.WriteLineIf(AppTraceLevelCfg.TraceVerbose, ex?.StackTrace);
        Trace.IndentLevel = prevLv;
        Trace.IndentSize = prevSz;
      }
    }

    public static string InnerMessages(this Exception? ex, char delimiter = '\n')
    {
      var sb = new StringBuilder();
      while (ex != null)
      {
        sb.Append($" - {ex.Message}{delimiter}");
        ex = ex.InnerException;
      }

      return sb.ToString();
    }
    public static string InnermostMessage(this Exception ex)
    {
      while (ex != null)
      {
        if (ex.InnerException == null)
          return ex.Message;

        ex = ex.InnerException;
      }

      return "This is very-very odd.";
    }
  }
}