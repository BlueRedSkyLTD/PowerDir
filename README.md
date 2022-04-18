# PowerDir

PowerDir is a PowerShell 7.2 Cmdlet as alternative to `Get-ChildItem`.

## Build Status

|  Build  |      Windows    | Linux | MacOS |
|:-------:|:---------------:|------:|:-----:|
| Debug   | [![Build Status](https://dev.azure.com/BlueRedSky/PowerDir/_apis/build/status/PowerDir%20CI?branchName=main&jobName=Windows&configuration=Windows%20Release)](https://dev.azure.com/BlueRedSky/PowerDir/_build/latest?definitionId=2&branchName=main) | [![Build Status](https://dev.azure.com/BlueRedSky/PowerDir/_apis/build/status/PowerDir%20CI?branchName=main&jobName=Linux&configuration=Linux%20Debug)](https://dev.azure.com/BlueRedSky/PowerDir/_build/latest?definitionId=2&branchName=main)   | [![Build Status](https://dev.azure.com/BlueRedSky/PowerDir/_apis/build/status/PowerDir%20CI?branchName=main&jobName=MacOS&configuration=MacOS%20Debug)](https://dev.azure.com/BlueRedSky/PowerDir/_build/latest?definitionId=2&branchName=main)   |
| Release | [![Build Status](https://dev.azure.com/BlueRedSky/PowerDir/_apis/build/status/PowerDir%20CI?branchName=main&jobName=Windows&configuration=Windows%20Release)](https://dev.azure.com/BlueRedSky/PowerDir/_build/latest?definitionId=2&branchName=main) | [![Build Status](https://dev.azure.com/BlueRedSky/PowerDir/_apis/build/status/PowerDir%20CI?branchName=main&jobName=Linux&configuration=Linux%20Release)](https://dev.azure.com/BlueRedSky/PowerDir/_build/latest?definitionId=2&branchName=main) | [![Build Status](https://dev.azure.com/BlueRedSky/PowerDir/_apis/build/status/PowerDir%20CI?branchName=main&jobName=MacOS&configuration=MacOS%20Release)](https://dev.azure.com/BlueRedSky/PowerDir/_build/latest?definitionId=2&branchName=main) |

At the moment only Windows is fully supported, but still compatible where .NET6.0 can be run.

| Code Coverage | Powershell Gallery |
|:-----------------:|:-------------:|
| [![codecov](https://codecov.io/gh/BlueRedSkyLTD/PowerDir/branch/main/graph/badge.svg?token=IYQC61BVWR)](https://codecov.io/gh/BlueRedSkyLTD/PowerDir) |  ![PSGallery Version](https://img.shields.io/powershellgallery/v/PowerDir.GetPowerDir.png?style=plastic&logo=powershell&label=PowerShell%20Gallery) |
 

## Usage

- [Help Get-PowerDir](./PowerDir/doc/Get-PowerDir.md)
- [Module Help](./PowerDir/doc/PowerDir.GetPowerDir.md)

### Suggested Aliases

Edit `$Profile` and add the following aliases
```powershell
function dd() { Get-PowerDir -d ld @args }
function dw() { Get-PowerDir -d w  @args }
function  l() { Get-PowerDir -d l  @args }
```

## Install

Install it from [Powershell Gallery](https://www.powershellgallery.com/packages/PowerDir.GetPowerDir)
```powershell
C:\ PS> Install-Module -Name PowerDir.GetPowerDir
```

## Generate CmdLet help

ref: https://docs.microsoft.com/en-us/powershell/utility-modules/platyps/create-help-using-platyps?view=ps-modules&viewFallbackFrom=powershell-7.2

## Feedback

Open an issue on github or send a message in Powershell gallery

## Contribution

## RoadMap / Changelog

- [x] v0.1.0: publishing test, basic functionalities almost complete
- [x] v0.2.0: basic functionalities
- [ ] v0.3.0: using escape codes
- [ ] v0.4.0: globbing search / advanced search patterns
