###
# Unnamed Arguments:
# - $Args[0] = $(Build.SourceVersion)
# - $Args[0] eg => refs/tags/v0.1.0

###

$ErrorActionPreference = "Stop"
echo $Args[0]

### if doesn't found the branch will return null, that will generate an error and exit.
$tagBranch = git branch --contains $Args[0]
if ($tagBranch | Select-String -Pattern 'main$') {}
else {
    echo "git tag not in main branch, but in $tagBranch"
    exit 1
}

### TODO: instead of [PSCustomOjbect] use [Version]
$v1 = Select-String -Path .\PowerDir\PowerDir.GetPowerDir.psd1 -Pattern "^ModuleVersion = '(\d).(\d).(\d)'$" | ForEach-Object {
    $major, $minor, $patch = $_.Matches[0].Groups[1..3].Value
    [Version] "$major.$minor.$patch"
    # [PSCustomObject] @{
    #   Major = $major
    #   Minor = $minor
    #   Patch = $patch
    # }
}


echo "checking version Manifest <-> git tag ..."
$tag = git describe --tags --exact-match $Args[0]
$v3 = $tag | Select-String -Pattern '^v(\d).(\d).(\d)$' | ForEach-Object {
    $major,$minor,$patch = $_.Matches[0].Groups[1..3].Value
    [Version] "$major.$minor.$patch"
}

if ($v1 -ne $v3) {
    echo "Versions mismatching"
    echo "psd1 Manifest file"
    $v1
    echo "git tag"
    $v3
    exit 1
}

echo "OK"
