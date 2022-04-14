### Need to import Powerdir module
### Need to import platyps

$ErrorActionPreference = "Stop"

$parameters = @{
    Path = ".\doc\"
    RefreshModulePage = $true
    AlphabeticParamsOrder = $true
    UpdateInputOutput = $true
    ExcludeDontShow = $true
 #   LogPath = ".\doc\"
 #   Encoding = [System.Text.Encoding]::UTF8
}

Update-MarkdownHelpModule @parameters @args
