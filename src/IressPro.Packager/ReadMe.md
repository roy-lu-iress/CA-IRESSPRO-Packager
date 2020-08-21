#Misc Publish-related Notes
Tags: .net core publish deploy package cert

For publish/deployment options of .NET Core apps: MSIX is ClickOnce replacement: 
  https://youtu.be/ZysDhWTZoO0?t=835 (MsVS channel, episode "Modernizing Desktop Apps Part 1" - Feb 28, 2019)
  https://youtu.be/uc3tC6_mEvk?t=1623 (IAmTimCorey - .NET Core 3.0 Desktop Apps - Creating and Publishing WPF and WinForm Apps in .NET Core)

For Cer-Pub Installer woes:
  See steps in OneNote:  https://onedrive.live.com/redir?resid=869AFB15787C9269%21427703&page=Edit&wd=target%28J0.one%7Ca249a707-5a8d-4d3a-ac4b-06ee1a08d9c1%2FCert%20Pub%20%E2%80%A6%7Cb592a03a-f306-4f68-b136-f42adfb2c106%2F%29

In addition to CLI, one can use these Publish* entries to achieve single file 
    <RuntimeIdentifiers>win-x86;win-x64</RuntimeIdentifiers>
    <PublishSingleFile>true</PublishSingleFile>
    <PublishTrimmed>true</PublishTrimmed>

A fix for   Assets file 'project.assets.json' doesn't have a target for '.NETCoreApp,Version=v3.0'. Ensure that restore has run and that you have included 'netcoreapp3.0' in the TargetFrameworks for your project.
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <UseWPF>true</UseWPF>
    <RuntimeIdentifiers>win-x86;win-x64</RuntimeIdentifiers>    <======================================
  </PropertyGroup>
  
#May 19, 2020 - MSIX only
  Let's try to publish the stuff from the VS SLN and make staff use proper stuff by going to file:///F:/[your ID]/Installers/MSIX/IressPro.Packager/index.html
  ...and use other means as a fall-back only!!!
  
#Jun 05, 2020
  Increased timeouts to 10 min.
  Added timeout to loggings.
  Published to the installer only (see May 19 comment).

#Aug 15, 2020
  Moved to a new home: https://github.com/oneiress/iresspro-packager

#Aug 16, 2020
  Removed redundant bits; added optional rudimentary logging.

#Aug 17, 2020 - Scheduling for Australia, Singapore, Canada. 
▓▓▓ signifies an overlap of waking hours (assuming people sleep between 23:00 and 7:00)

AUS   1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24
AUS SSSSSSSSSSSSSSSSSS---      ▓▓▓▓▓▓                        ▓▓▓▓▓▓▓▓▓▓▓▓---SSSSSSSSSSSSSSSSSS---      ▓▓▓▓▓▓                        ▓▓▓▓▓▓▓▓▓▓▓▓---
SGP  23 24  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21 22
SGP    ---SSSSSSSSSSSSSSSSSS---▒▒▒▓▓▓                        ▓▓▓▓▓▓▓▓▓▓▓▓      ---SSSSSSSSSSSSSSSSSS---▒▒▒▓▓▓                        ▓▓▓▓▓▓▓▓▓▓▓▓
CAN  13 14 15 16 17 18 19 20 21 22 23 24  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24  1  2  3  4  5  6  7  8  9 10 11 12
CAN                            ▓▓▓▓▓▓---SSSSSSSSSSSSSSSSSS---▒▒▒▓▓▓▓▓▓▓▓▓                              ▓▓▓▓▓▓---SSSSSSSSSSSSSSSSSS---▒▒▒▓▓▓▓▓▓▓▓▓
