namespace IressPro.Packager.AppSettings
{
  public partial class AppStg
  {
    public const string BatCtx_IrsIosRtl___ = @"
@echo off

rem ****
rem ****  This script will run the three installation packages that make up the 
rem ****  Iress Canada client application with Retail, in silent mode.
rem ****

rem ****
rem ****  Update the following three variables with each new version.
rem ****

set DFS_IRESSPACKAGE_DIR=IRESS Pro
set DFS_IRESSCANADAPACKAGE_DIR=IOS Plus
set DFS_IRESSRETAILPACKAGE_DIR=IOS Plus Retail


rem ****
rem ****  Change this goto statement to the type of installation desired.
rem ****

echo.
echo Installing IRESS Canada Client Packages with Retail.
echo.

for /R %%R in (*.reg) do REGEDIT.EXE /S %%R

Set DFS_DOFMT=FALSE

echo Installing main IRESS Client Application...

goto PerUserInstall



rem **** PER MACHINE INSTALL - NOT USED ***
:PerMachineInstall

set DFS_APPDIR=%ALLUSERSPROFILE%\Application Data\Iress
cd ""%DFS_IRESSPACKAGE_DIR%""
Setup.exe /passive MACHINE_INSTALL=YES SOCKETADDRESS=MACHINE:36865 SECONDARY_SOCKETADDRESS=MACHINE:36865 LOGIN_COMPANYNAME=COMPANY USERDIR=\""%ALLUSERSPROFILE%\Documents\IRESS\"" APPDIR=\""%ALLUSERSPROFILE%\Application Data\IRESS\""
cd ..

goto CanadaPackage



rem **** PER USER INSTALL ***2
:PerUserInstall

rem ****
rem **** Backup UserDir directory
rem ****

FOR /F ""tokens=2,*"" %%A IN ('reg.exe query ""HKCU\Software\DFS\IRESS\File"" /v ""UserDir""') DO set ""USERDIR=%%B""
if not defined USERDIR goto BackupWorkDir

if not exist ""%USERDIR%_BACKUP"" mkdir ""%USERDIR%_BACKUP""
copy /v /y ""%USERDIR%\*.*"" ""%USERDIR%_BACKUP\""



rem ****
rem **** Backup WorkDir directory
rem ****

:BackupWorkDir

FOR /F ""tokens=2,*"" %%A IN ('reg.exe query ""HKCU\Software\DFS\IRESS\File"" /v ""WorkDir""') DO set ""WORKDIR=%%B""
if not defined WORKDIR goto Next

if not exist ""%WORKDIR%_BACKUP"" mkdir ""%WORKDIR%_BACKUP""
copy /v /y ""%WORKDIR%\*.*"" ""%WORKDIR%_BACKUP\""



:Next

set DFS_APPDIR=%APPDATA%\Iress
cd ""%DFS_IRESSPACKAGE_DIR%""
if not exist ""%DFS_APPDIR%\CMD32.FMT"" SET DFS_DOFMT=TRUE
Setup.exe /passive SOCKETADDRESS=MACHINE:36865 SECONDARY_SOCKETADDRESS=MACHINE:36865 LOGIN_COMPANYNAME=COMPANY USERDIR=\""%USERPROFILE%\Documents\IRESS\"" APPDIR=\""%APPDATA%\IRESS\""
cd ..

if exist ""%ProgramFiles(x86)%"" (GOTO ProgramFilesx86) else (GOTO ProgramFiles)



:ProgramFilesx86

rem copy /v /y ""(CA) BASIC - 1280 X 1024.wsp"" ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) Basic - 1920 X 1080.wsp"" ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) default - 1280 X 1024.wsp"" ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) default - 1920 X 1080.wsp"" ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) Market Data - 1280 X 1024.wsp"" ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) Market Data - 1920 X 1080.wsp"" ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) RETAIL OMS.wsp"" ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS\DefaultProfile\UserData""

copy /v /y *.wsp  ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
copy /v /y *.xlsm ""%ProgramFiles(x86)%\IRESS Market Technology\IRESS""

goto CanadaPackage



:ProgramFiles

rem copy /v /y ""(CA) BASIC - 1280 X 1024.wsp"" ""%ProgramFiles%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) Basic - 1920 X 1080.wsp"" ""%ProgramFiles%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) default - 1280 X 1024.wsp"" ""%ProgramFiles%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) default - 1920 X 1080.wsp"" ""%ProgramFiles%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) Market Data - 1280 X 1024.wsp"" ""%ProgramFiles%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) Market Data - 1920 X 1080.wsp"" ""%ProgramFiles%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
rem copy /v /y ""(CA) RETAIL OMS.wsp"" ""%ProgramFiles%\IRESS Market Technology\IRESS\DefaultProfile\UserData""

copy /v /y *.wsp  ""%ProgramFiles%\IRESS Market Technology\IRESS\DefaultProfile\UserData""
copy /v /y *.xlsm ""%ProgramFiles%\IRESS Market Technology\IRESS""

goto CanadaPackage



rem *** IOS Plus Package (for both installation types)
:CanadaPackage

echo Installing IOS Plus Client Plugin...

if %DFS_DOFMT%==FALSE GOTO CanadaPackageInstall


rem Delete format files here
del ""%DFS_APPDIR%\CMD32.FMT""
del ""%DFS_APPDIR%\FLD32.FMT""
del ""%DFS_APPDIR%\FLD32CMD.FMT""

:CanadaPackageInstall
cd ""%DFS_IRESSCANADAPACKAGE_DIR%""
Setup.exe /S /V/qb
cd ..

:RetailInstall
echo Installing IOS Plus Retail Client Plugin...
cd ""%DFS_IRESSRETAILPACKAGE_DIR%""
Setup.exe /S /V/qb+
cd ..

goto end



:end
rem Disable IRESS Manifest (enables Windows Common Controls 5.8x)
ren ""C:\Program Files\IRESS KTG\IRESS\IRESS.EXE.MANIFEST"" IRESS.EXE.MANIFEST.BAK
ren ""C:\Program Files\IRESS Market Technology\IRESS\IRESS.EXE.MANIFEST"" IRESS.EXE.MANIFEST.BAK

rem Move any IRESS install packages from the desktop to the root directory
%HOMEDRIVE%
cd ""%HOMEPATH%""
cd ..
FOR /D %%i IN (*) DO copy /v /y ""%%i\Desktop\Felix2*.exe"" ""%HOMEDRIVE%\""
FOR /D %%i IN (*) DO del /f /q ""%%i\Desktop\Felix2*.exe""

echo Installation Completed!
set DFS_APPDIR=
set DFS_DOFMT=";
  }
}
