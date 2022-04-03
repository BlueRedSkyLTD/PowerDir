# PowerDir

PowerDir is a PowerShell 7.2 Cmdlet as alternative to `Get-ChildItem`.

## Usage

- [Help Get-PowerDir](./PowerDir/doc/Get-PowerDir.md)
- [Module Help](./PowerDir/doc/PowerDir.GetPowerDir.md)

## Themes

Improvements with escape sequence to support more colors, eg:

```powershell
PS C:\> "`e[38;5;100mSample\`e[0m"
```

Note:
```
ESC[ char is e`
```

256-colors
```
ESC[38;5;⟨n⟩m Select foreground color
ESC[48;5;⟨n⟩m Select background color
  0-  7:  standard colors (as in ESC [ 30–37 m)
  8- 15:  high intensity colors (as in ESC [ 90–97 m)
 16-231:  6 × 6 × 6 cube (216 colors): 16 + 36 × r + 6 × g + b (0 ≤ r, g, b ≤ 5)
232-255:  grayscale from black to white in 24 steps
```

24-bits:

```
ESC[38;2;⟨r⟩;⟨g⟩;⟨b⟩mSelect RGB foreground color
ESC[ 48;2;⟨r⟩;⟨g⟩;⟨b⟩ m Select RGB background color
```

the sequence must end with:

```
ESC[0m
```

eg 24-bits
```powershell
"`e[38;2;255;128;64mRGB Text`e[0m"
```

more info on VT100 ansi terminal:
- https://en.wikipedia.org/wiki/ANSI_escape_code
- https://ss64.com/ps/syntax-esc.html
- https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_special_characters?view=powershell-7.2
- https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
- - https://www2.ccs.neu.edu/research/gpc/VonaUtils/vona/terminal/vtansi.htm

it is possible to underline text, bold, etc and so on too.

## Generate CmdLet help

ref: https://docs.microsoft.com/en-us/powershell/utility-modules/platyps/create-help-using-platyps?view=ps-modules&viewFallbackFrom=powershell-7.2
