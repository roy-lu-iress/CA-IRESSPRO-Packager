namespace IressPro.Packager.AppSettings
{
  public class SourceFolders
  {
    public string AuxSrcFolder { get; set; } = @"C:\CompiledBinaries\Registries\";
    public string IosSrcFolder { get; set; } = @"C:\CompiledBinaries\IOS\";
    public string IpsSrcFolder { get; set; } = @"C:\CompiledBinaries\IPS\";
    public string IrsSrcFolder { get; set; } = @"C:\CompiledBinaries\IRESS\";
    public string RtlSrcFolder { get; set; } = @"C:\CompiledBinaries\Retail\";
    public string SlfSrcFolder { get; set; } = @"C:\CompiledBinaries\SelfContained\";

    public override string ToString() =>
      $"{AuxSrcFolder} \n" +
      $"{IosSrcFolder} \n" +
      $"{IpsSrcFolder} \n" +
      $"{IrsSrcFolder} \n" +
      $"{RtlSrcFolder} \n" +
      $"{SlfSrcFolder} \n";
  }
}
