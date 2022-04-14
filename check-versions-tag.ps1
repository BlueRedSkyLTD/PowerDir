###
# Unnamed Arguments:
# - $Args[0] = $(Build.SourceVersion)
# - $Args[0] eg => refs/tags/v0.1.0

###


### Due to azure pipelines checkout step
### it is not really simple to detect the tag on witch branch belongs too
### as the repo is in a detached head disconnected from all other branches


$ErrorActionPreference = "Stop"
echo $Args[0]
echo "git log -1"
git log -1 --pretty=%D

echo "git rev-parse Args[0]"
git rev-parse $Args[0]

echo "git show HEAD"
git show -s --pretty=%d HEAD

git log -1 --pretty=%D | Select-String -Pattern '^HEAD -> (.+),' | ForEach-Object {
    $branch = $_.Matches[0].Groups[1].Value
}
echo "Branch name: $branch"

if ($brach -ne "main") {
    echo "not main branch. Exit"
    # exit 1
}

### if doesn't found the branch will return null, that will generate an error and exit.
$tagBranch = git branch $branch --contains $Args[0]
if ($tagBranch | Select-String -Pattern 'main$') {}
else {
    $t = $tagBranch -join ', '
    echo "git tag not in main branch, but in $t"
    # exit 1
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
