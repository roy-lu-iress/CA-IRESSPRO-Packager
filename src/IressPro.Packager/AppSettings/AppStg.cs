namespace IressPro.Packager.AppSettings
{
  public partial class AppStg
  {
    public const string StagingRootConst = @"C:\IressProSetup\";

    public string Robocopy { get; set; } = "robocopy.exe";
    public string rar64 { get; set; } = @"C:\Program Files\WinRAR\Rar.exe";
    public string wrr64 { get; set; } = @"C:\Program Files\WinRAR\WinRAR.exe";
    public string rar86 { get; set; } = @"C:\Program Files (x86)\WinRAR\Rar.exe";
    public string wrr86 { get; set; } = @"C:\Program Files (x86)\WinRAR\WinRAR.exe";
    public string rarStngsFilename { get; set; } = "CreateSfxWithAutoexec.Settings.txt";
    public string rarStngsContents { get; set; } = "Overwrite=1\r\nPath=IRESS2\r\nTempMode\r\nSetup=IressCanadaInstall.bat";
    public string AutoRunFilename { get; set; } = "IressCanadaInstall.bat";

    public string StagingRoot { get; set; } = StagingRootConst;

#if DEBUG
    public string PackageProcessFolder { get; set; } = @"C:\Packager.Sources\PackageProcess\";
#else
    public string PackageProcessFolder { get; set; } = @"Y:\Production Packages\Client\IressCanada\PackageProcess\";
#endif
    public string TargetBetaRetailFolderY { get; set; } = @"Y:\Production Packages\Client\IressCanada\Beta\Retail\19.2\";
    public string TargetFinalIressFolderY { get; set; } = @"Y:\Production Packages\Client\IressCanada\Final\Iress\19.2\";
#if BEFORE_Jan15
    public  string[] StagingSubfolders { get; set; } = { "IRESS Pro", "IOS Plus", "IOS Plus Retail", "IPS", "CIBC" };
#else
    public string[] StagingSubfolders { get; set; } = { "IRESS Pro", "IOS Plus", "IOS Plus Retail", "IPS", "", "" };
#endif
    public string IrsSrcFolder { get; set; } = @"Y:\Release Candidates\Client\IRESS\";
    public string IosSrcFolder { get; set; } = @"Y:\Release Candidates\IOS Plus\Client\";
    public string RtlSrcFolder { get; set; } = @"Y:\Release Candidates\IOS Plus Retail\Client\Neo\";
    public string IpsSrcFolder { get; set; } = @"Y:\Release Candidates\Source\IPS\";
    public string AuxSrcFolder { get; set; } = @"F:\Jagriti\Panther\Versions\Registries\";
    public string SlfSrcFolder { get; set; } = @"F:\Jagriti\Panther\Versions\Self Contained\";

    public string IrsPattern { get; set; } = @"IRESS - ??.*";
    public string IosPattern { get; set; } = @"IOS Plus - ??.*";
    public string RtlPattern { get; set; } = @"IOS Plus Retail - ??.*";
    public string IpsPattern { get; set; } = @"IPS - *";
    public string SlfPattern { get; set; } = @"*";
    public string AuxPattern { get; set; } = @"*";

    public string VersionDelimeter { get; set; } = @" - ";
    public bool DisconnectedMode { get; set; }
    public bool IsParallelExectn { get; set; }
    public bool? AmendIfExistsJ1a { get; set; } = false;

    public string[] VersionPatterns { get; set; } = { "??.*", "??.*", "??.*", "*" };

    public int LbxSelectedIndex1 { get; set; }
    public int LbxSelectedIndex2 { get; set; }
    public int LbxSelectedIndex3 { get; set; }
    public int LbxSelectedIndex4 { get; set; }
    public int LbxSelectedIndex5 { get; set; }
    public int LbxSelectedIndexZ { get; set; }

    public bool IsChecked1 { get; set; }
    public bool IsChecked2 { get; set; }
    public bool IsChecked3 { get; set; }
    public bool IsChecked4 { get; set; }
    public bool IsChecked5 { get; set; }
    public bool IsCheckedZ { get; set; }
  }
}
