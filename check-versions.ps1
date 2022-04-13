###
# Unnamed Arguments:
# - $Args[0] = $(Build.SourceVersion)
# - $Args[0] eg => refs/tags/v0.1.0

###

$ErrorActionPreference = "Stop"
echo $Args[0]

### if doesn't found the branch will return null, that will generate an error and exit.
$tagBranch = git branch --contains $Args[0]
if ($tagBranch | Select-String -Pattern 'line$') {}
else {
    echo "git tag not in main branch, but in $tagBranch"
    exit 1
}

### TODO: instead of [PSCustomOjbect] use [Version]
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

echo "checking version Manifest <-> Project ..."
if ($v1.Major -eq $v2.Major -and $v1.Minor -eq $v2.Minor -and $v1.Patch -eq $v2.Patch) {}
else {
    echo "Versions mismatching"
    echo "psd1 Manifest file"
    $v1
    echo "C# project version"
    $v2
    exit 1
}

echo "checking version Manifest <-> git tag ..."
$tag = git describe --tags --exact-match $Args[0]
$v3 = $tag | Select-String -Pattern '^v(\d).(\d).(\d)$' | ForEach-Object {
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

echo "check published modules..."
if (Find-Module -name PowerDir.GetPowerDir -RequiredVersion $tag.Substring(1) -ErrorAction SilentlyContinue) {
    echo "version already existing, exit!"
    exit 1
}

echo "version not found OK!"

# $f = Find-Module -name PowerDir.GetPowerDir | Select Version
# $v4 = [version]$f.Version
# if ($v1.Major -eq $v3.Major -and $v1.Minor -eq $v3.Minor -and $v1.Patch -eq $v3.Patch) {}
# else {
#     echo "Versions mismatching"
#     echo "psd1 Manifest file"
#     $v1
#     echo "git tag"
#     $v3
#     exit 1
# }

echo "OK"
