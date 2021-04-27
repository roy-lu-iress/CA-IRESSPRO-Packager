using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IressPro.Packager
{
internal  class ConstDefaults
  {
    internal const string _fallback = @"{{
  ""WhereAmI"": ""{0}"",
  ""Defaults"": {{
    ""AUS"": {{
      ""RegSrcFolder"": ""Y:\\iress\\client\\Packager\\Registries"",
      ""IosSrcFolder"": ""Y:\\IOS Plus\\Client\\Neo\\"",
      ""IpsSrcFolder"": ""Y:\\Ips\\client\\Neo\\"",
      ""IrsSrcFolder"": ""Y:\\iress\\client\\Neo\\"",
      ""RtlSrcFolder"": ""Y:\\IOS Plus Retail\\Client\\Neo\\"",
      ""SlfSrcFolder"": ""Y:\\iress\\client\\Packager\\Self Contained\\""
    }},
    ""CAN"": {{
      ""RegSrcFolder"": ""F:\\Jagriti\\Panther\\Versions\\Registries\\"", 
      ""IosSrcFolder"": ""Y:\\Release Candidates\\IOS Plus\\Client\\"", 
      ""IpsSrcFolder"": ""Y:\\Release Candidates\\Source\\IPS\\"", 
      ""IrsSrcFolder"": ""Y:\\Release Candidates\\Client\\IRESS\\"", 
      ""RtlSrcFolder"": ""Y:\\Release Candidates\\IOS Plus Retail\\Client\\Neo\\"", 
      ""SlfSrcFolder"": ""F:\\Jagriti\\Panther\\Versions\\Self Contained\\"" 
    }},
    ""NZL"": {{
      ""RegSrcFolder"": ""C:\\CompiledBinaries\\Registries\\"",
      ""IosSrcFolder"": ""C:\\CompiledBinaries\\IOS\\"",
      ""IpsSrcFolder"": ""C:\\CompiledBinaries\\IPS\\"",
      ""IrsSrcFolder"": ""C:\\CompiledBinaries\\IRESS\\"",
      ""RtlSrcFolder"": ""C:\\CompiledBinaries\\Retail\\"",
      ""SlfSrcFolder"": ""C:\\CompiledBinaries\\SelfContained\\""
    }},
    ""SGP"": {{
      ""RegSrcFolder"": ""Y:\\iress\\client\\Packager\\Registries"",
      ""IosSrcFolder"": ""Y:\\IOS Plus\\Client\\Neo\\"",
      ""IpsSrcFolder"": ""Y:\\Ips\\client\\Neo\\"",
      ""IrsSrcFolder"": ""Y:\\iress\\client\\Neo\\"",
      ""RtlSrcFolder"": ""Y:\\IOS Plus Retail\\Client\\Neo\\"",
      ""SlfSrcFolder"": ""Y:\\iress\\client\\Packager\\Self Contained\\""
    }},
    ""RSA"": {{
      ""RegSrcFolder"": ""C:\\CompiledBinaries\\Registries\\"",
      ""IosSrcFolder"": ""C:\\CompiledBinaries\\IOS\\"",
      ""IpsSrcFolder"": ""C:\\CompiledBinaries\\IPS\\"",
      ""IrsSrcFolder"": ""C:\\CompiledBinaries\\IRESS\\"",
      ""RtlSrcFolder"": ""C:\\CompiledBinaries\\Retail\\"",
      ""SlfSrcFolder"": ""C:\\CompiledBinaries\\SelfContained\\""
    }},
    ""GBR"": {{
      ""RegSrcFolder"": ""C:\\CompiledBinaries\\Registries\\"",
      ""IosSrcFolder"": ""C:\\CompiledBinaries\\IOS\\"",
      ""IpsSrcFolder"": ""C:\\CompiledBinaries\\IPS\\"",
      ""IrsSrcFolder"": ""C:\\CompiledBinaries\\IRESS\\"",
      ""RtlSrcFolder"": ""C:\\CompiledBinaries\\Retail\\"",
      ""SlfSrcFolder"": ""C:\\CompiledBinaries\\SelfContained\\""
    }},
    ""EXP"": {{
      ""RegSrcFolder"": ""F:\\Jagriti\\Panther\\Versions\\Registries\\"",
      ""IosSrcFolder"": ""F:\\Jagriti\\Panther\\Versions\\Self Contained\\20.1.3\\2\\"",
      ""IpsSrcFolder"": ""F:\\Jagriti\\Panther\\Versions\\Self Contained\\20.1.3\\4\\"",
      ""IrsSrcFolder"": ""F:\\Jagriti\\Panther\\Versions\\Self Contained\\20.1.3\\1\\"",
      ""RtlSrcFolder"": ""F:\\Jagriti\\Panther\\Versions\\Self Contained\\20.1.3\\3\\"",
      ""SlfSrcFolder"": ""F:\\Jagriti\\Panther\\Versions\\Self Contained\\20.1.3\\5\\""
    }},
    ""DEV"": {{
      ""RegSrcFolder"": ""C:\\Packager.Sources\\Reg\\"",
      ""IosSrcFolder"": ""C:\\Packager.Sources\\Ios\\"",
      ""IpsSrcFolder"": ""C:\\Packager.Sources\\Ips\\"",
      ""IrsSrcFolder"": ""C:\\Packager.Sources\\Irs\\"",
      ""RtlSrcFolder"": ""C:\\Packager.Sources\\Rtl\\"",
      ""SlfSrcFolder"": ""C:\\Packager.Sources\\Slf\\""
    }}
  }},
  ""Serilog"": {{
    ""Using"": [],
    ""MinimumLevel"": ""Information"",
    ""Override"": {{
      ""Microsoft"": ""Warning"",
      ""System"": ""Warning""
    }},
    ""Enrich"": [ ""FromLogContext"" ], 
    ""WriteTo"": [
      {{
        ""Name"": ""Debug"",
        ""Args"": {{
          ""restrictedToMinimumLevel"": ""Verbose"" 
        }}
      }},
      {{
        ""Name"": ""File"",
        ""Args"": {{
          ""path"": ""log\\Log.Daily-.txt"", 
          ""rollingInterval"": ""Day"",
          ""outputTemplate"": ""{{Timestamp:HH:mm:ss.f}} {{Level:w3}} {{Message:j}}\t{{Properties}}\t{{NewLine:1}}{{Exception:1}}""
        }}
      }},
      {{
        ""Name"": ""File"",
        ""Args"": {{
          ""path"": ""log\\Log.100kMax.json"", 
          ""formatter"": ""Serilog.Formatting.Json.JsonFormatter, Serilog"",
          ""rollOnFileSizeLimit"": true,
          ""fileSizeLimitBytes"": 100000
        }}
      }}
    ]
  }}
}}";
  }
}
