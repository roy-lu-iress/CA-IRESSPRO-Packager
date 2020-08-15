using Iress.WPF.Ext;
using IressPro.Packager.AppSettings;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IressPro.Packager.IO
{
  public static class ProcessExt
  {
    public static async Task ExecuteAsync(string exe, string args, int waitForExitMs = 600000, string workDir = AppStg.StagingRootConst)
    {
      var sw = Stopwatch.StartNew();
      try
      {
        var psi = new ProcessStartInfo { FileName = exe, Arguments = args, WorkingDirectory = workDir };

        //Debug.WriteLine($"f> *** Exec:  {psi.FileName}  {psi.Arguments}");
        var process = Process.Start(psi);
        await process.WaitForExitOrTimeoutAsync(waitForExitMs).ConfigureAwait(false); // 45 seconds to abort.
      }
      catch (Exception ex) { ex.Pop($"Exe: '{exe}'  Args: '{args}'  Timeout: {waitForExitMs} ms  WorkDir: '{workDir}'."); }

      await Task.Delay(100).ConfigureAwait(false);

      Trace.WriteLine($"{DateTimeOffset.Now:yy.MM.dd HH:mm:ss.f}  Took {sw.Elapsed:mm\\:ss\\.fff}  (timeout {waitForExitMs / 60000} min)  {exe}  {(sw.ElapsedMilliseconds < waitForExitMs ? "" : "▄▀▄▀▄▀▄▀▄▀▄▀ Timeout exceeded!!!")}");
    }

    public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
    {
      var tcs = new TaskCompletionSource<object>();
      if (process != null)
      {
        process.EnableRaisingEvents = true;
        process.Exited += (sender, args) => tcs.TrySetResult(tcs.Task);// ????????????????? null);
        if (cancellationToken != default)
          cancellationToken.Register(tcs.SetCanceled);
      }
      return tcs.Task;
    }

    public static Task WaitForExitOrTimeoutAsync(this Process process, int milliseconds, CancellationToken cancellationToken = default)
    {
      var tcs = new TaskCompletionSource<object>();
      if (process != null)
      {
        process.EnableRaisingEvents = true;
        process.Exited += (sender, args) => tcs.TrySetResult(tcs.Task);// ????????????????? null);
        if (cancellationToken != default)
          cancellationToken.Register(tcs.SetCanceled);
      }
      return Task.WhenAny(tcs.Task, Task.Delay(milliseconds));
    }
  }
}
