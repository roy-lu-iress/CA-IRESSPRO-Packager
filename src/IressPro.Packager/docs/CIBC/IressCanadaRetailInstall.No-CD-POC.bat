@echo off
echo Installing IRESS Canada Client Packages with Retail.
echo.
REGEDIT.EXE /S SetLocaleCanada.reg
REGEDIT.EXE /S Filters.reg
REGEDIT.EXE /S OrderPadFilters.reg
REGEDIT.EXE /S PortfolioFilters.reg
REGEDIT.EXE /S RegTimeZone.reg
Set DFS_DOFMT=FALSE
echo Installing main IRESS Client Application...
echo    Backup UserDir directory
FOR /F ""tokens=2,*"" %%A IN ('reg.exe query ""HKCU\Software\DFS\IRESS\File"" /v ""UserDir""') DO set ""USERDIR=%%B""
if defined USERDIR (
  if not exist ""%USERDIR%_BACKUP"" mkdir ""%USERDIR%_BACKUP""
  copy /v /y ""%USERDIR%\*.*"" ""%USERDIR%_BACKUP\""
)
echo    Backup WorkDir directory
FOR /F ""tokens=2,*"" %%A IN ('reg.exe query ""HKCU\Software\DFS\IRESS\File"" /v ""WorkDir""') DO set ""WORKDIR=%%B""
if defined WORKDIR (
  if not exist ""%WORKDIR%_BACKUP"" mkdir ""%WORKDIR%_BACKUP""
  copy /v /y ""%WORKDIR%\*.*"" ""%WORKDIR%_BACKUP\""
)

set DFS_APPDIR=%APPDATA%\Iress
if not exist ""%DFS_APPDIR%\CMD32.FMT"" SET DFS_DOFMT=TRUE
Setup.exe /passive SOCKETADDRESS=MACHINE:36865 SECONDARY_SOCKETADDRESS=MACHINE:36865 LOGIN_COMPANYNAME=COMPANY USERDIR=\""%USERPROFILE%\Documents\IRESS\"" APPDIR=\""%APPDATA%\IRESS\""

if exist ""%ProgramFiles(x86)%"" (
  copy /v /y ""Sample_IOS_Plus_*.xlsm"" ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS""
) else (
  copy /v /y ""Sample_IOS_Plus_*.xlsm"" ""%ProgramFiles%\IRESS Market Technology\IRESS""
)

echo Installing IOS Plus Client Plugin...
if not %DFS_DOFMT%==FALSE (
  echo      Delete format files here
  del ""%DFS_APPDIR%\CMD32.FMT""
  del ""%DFS_APPDIR%\FLD32.FMT""
  del ""%DFS_APPDIR%\FLD32CMD.FMT""
)
msiexec /i IOSPlus.msi  /qb /norestart

echo Installing IOS Plus Retail Client Plugin...
msiexec /i IOSPlusRetail.msi /qb+ /norestart

echo Disable IRESS Manifest (enables Windows Common Controls 5.8x)
ren ""C:\Program Files\IRESS KTG\IRESS\IRESS.EXE.MANIFEST"" IRESS.EXE.MANIFEST.BAK
ren ""C:\Program Files\IRESS Market Technology\IRESS\IRESS.EXE.MANIFEST"" IRESS.EXE.MANIFEST.BAK

echo  Move any IRESS install packages from the desktop to the root directory
%HOMEDRIVE%
cd ""%HOMEPATH%""
cd ..
FOR /D %%i IN (*) DO move /v /y ""%%i\Desktop\Felix2*.exe"" ""%HOMEDRIVE%\""

echo Installation Completed!
set DFS_APPDIR=
set DFS_DOFMT=
