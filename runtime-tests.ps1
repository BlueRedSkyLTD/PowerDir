$tests = @("$HOME", "$HOME/", "$HOME/", "~", "~/", "~/*")

ForEach($t in $tests)
{
    Write-Host ""
    Write-Host "runnning with $t"
    Get-PowerDir $t -d w
    Write-Host ""
}
