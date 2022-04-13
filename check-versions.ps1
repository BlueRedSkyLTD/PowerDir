###
# It checks the project version that are matching
# and the version is not already published
###

$ErrorActionPreference = "Stop"

$Result = 0

$v1 = Select-String -Path .\PowerDir\PowerDir.GetPowerDir.psd1 -Pattern "^ModuleVersion = '(\d).(\d).(\d)'$" | ForEach-Object {
    $major, $minor, $patch = $_.Matches[0].Groups[1..3].Value
    [Version] "$major.$minor.$patch"
}

$v2 = Select-String -Path .\PowerDir\PowerDir.csproj -Pattern "<Version>(\d).(\d).(\d)" | ForEach-Object {
    $major,$minor,$patch = $_.Matches[0].Groups[1..3].Value
    [Version] "$major.$minor.$patch"
}

echo "checking version Manifest <-> Project ..."
if ($v1 -ne $v2) {
    echo "Versions mismatching"
    echo "psd1 Manifest file"
    $v1
    echo "C# project version"
    $v2
    $Result++
}

echo "check published modules..."
if (Find-Module -name PowerDir.GetPowerDir -RequiredVersion $v1.toString() -ErrorAction SilentlyContinue) {
    echo "version already existing"
    $Result++
}

echo "done."
exit $Result
