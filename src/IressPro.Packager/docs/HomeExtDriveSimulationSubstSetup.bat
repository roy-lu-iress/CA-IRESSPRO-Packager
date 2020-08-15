if exist F:\Jagriti (
  %OneDrive%\Public\bin\say "Already there."
) else (
  subst F: C:\temp\Iress\F
  subst Y: C:\temp\Iress\Y
  echo all done
  %OneDrive%\Public\bin\say "Done."
)

