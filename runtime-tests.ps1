$ErrorActionPreference = "Stop"
$tests = @("$HOME", "$HOME/", "$HOME/", "~", "~/", "~/*")

### Debug
### If these are passing, must be PowerDir
ForEach($t in $tests)
{
    Write-Host ""
    Write-Host "Debug runnning with $t"
    Get-ChildItem $t
    Write-Host ""
}

### Run-time Tests
ForEach($t in $tests)
{
    Write-Host ""
    Write-Host "runnning with $t"
    Get-PowerDir $t -d w -Debug -Verbose
    Write-Host ""
}
