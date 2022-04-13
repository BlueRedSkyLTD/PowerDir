$ErrorActionPreference = "Stop"

$v1 = Select-String -Path .\PowerDir\PowerDir.GetPowerDir.psd1 -Pattern "^ModuleVersion = '(\d).(\d).(\d)'$" | ForEach-Object {
    $major, $minor, $patch = $_.Matches[0].Groups[1..3].Value
    [PSCustomObject] @{
      Major = $major
      Minor = $minor
      Patch = $patch
    }
}

$v2 = Select-String -Path .\PowerDir\PowerDir.csproj -Pattern "<Version>(\d).(\d).(\d)" | ForEach-Object {
    $major,$minor,$patch = $_.Matches[0].Groups[1..3].Value
    [PSCustomObject] @{
        Major = $major
        Minor = $minor
        Patch = $patch
    }
}

if ($v1.Major -eq $v2.Major -and $v1.Minor -eq $v2.Minor -and $v1.Patch -eq $v2.Patch) {}
else {
    echo "Versions mismatching"
    echo "psd1 Manifest file"
    $v1
    echo "C# project version"
    $v2
    exit 1
}

$v3 = git describe --exact-match $(Build.SourceVersion)
$v3 | Select-String -Pattern '^v(\d).(\d).(\d)$' | ForEach-Object {
    $major,$minor,$patch = $_.Matches[0].Groups[1..3].Value
    [PSCustomObject] @{
      Major = $major
      Minor =$minor
      Patch=$patch
    }
}

if ($v1.Major -eq $v3.Major -and $v1.Minor -eq $v3.Minor -and $v1.Patch -eq $v3.Patch) {}
else {
    echo "Versions mismatching"
    echo "psd1 Manifest file"
    $v1
    echo "git tag"
    $v3
    exit 1
}
