---
external help file: PowerDir.dll-Help.xml
Module Name: PowerDir.GetPowerDir
online version:
schema: 2.0.0
---

# Get-PowerDir

## SYNOPSIS
An alternative Get-ChildItem.

## SYNTAX

```
Get-PowerDir [-About] [[-Path] <String>] [-Recursive] [-NoColor] [-Level <Int32>] [-Display <DisplayOptions>]
 [<CommonParameters>]
```

## DESCRIPTION
Get-PowerDir is used to display files and directories.
Search for them in a user-friendly way and supporting colors.

## EXAMPLES

### Example 1
```powershell
PS C:\> d
```

get all items in the current path folder.

### Example 2
```powershell
PS C:\> d -d l
```

get all items as list in the current path folder.

### Example 3
```powershell
PS C:\> d -d dl
```

get all items as detailed list in the current path folder.

### Example 4
```powershell
PS C:\> d -d w
```

get all items in a wide 4 columns table in the current path folder.

### Example 5
```powershell
PS C:\> d -r -l 2 *.exe
```

get all items ending with `.exe` recursively with a depth level of 2.

### Example 6
```powershell
PS C:\> Get-PowerDir | ft
```

get all items in the current directory in `Format-table` mode.

### Example 7
```powershell
PS C:\> d \`$RecycleBin
```

Display content in the Recycle Bin in Windows System.
Must use the backtick char before `$`to escape it.

### Example 8
```powershell
PS C:\> notepad $Profile

function dd() { Get-PowerDir -d ld @args }
function dw() { Get-PowerDir -d w  @args }
function  l() { Get-PowerDir -d l  @args }
```

Edit your profile and add the following aliases

## PARAMETERS

### -About
About GetPowerDir information

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Display
Display type (default: Object)

```yaml
Type: DisplayOptions
Parameter Sets: (All)
Aliases: d
Accepted values: o, Object, List, l, ListDetails, ld, Wide, w

Required: False
Position: Named
Default value: Object
Accept pipeline input: False
Accept wildcard characters: False
```

### -Level
Max Recursion Depth (default: int.MaxValue)

```yaml
Type: Int32
Parameter Sets: (All)
Aliases: l

Required: False
Position: Named
Default value: int.MaxValue
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoColor
Disable colors (default: no)

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: n

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
Path to search.
Accepting wildcards (default: *)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: *
Accept pipeline input: False
Accept wildcard characters: True
```

### -Recursive
Search Recursively (default: No)

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: r

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### PowerDir.GetPowerDirInfo
## NOTES

## RELATED LINKS
