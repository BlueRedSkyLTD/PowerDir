---
external help file: PowerDir.dll-Help.xml
Module Name: PowerDir
online version:
schema: 2.0.0
---

# Get-PowerDir

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

```
Get-PowerDir [[-Path] <String>] [-Recursive] [-Level <Int32>] [-Display <DisplayOptions>] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Display
Display type (default: Object)

```yaml
Type: DisplayOptions
Parameter Sets: (All)
Aliases: d
Accepted values: o, Object, List, l, ListDetails, ld, Wide, w

Required: False
Position: Named
Default value: None
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
Default value: None
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
Default value: None
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
Default value: None
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
