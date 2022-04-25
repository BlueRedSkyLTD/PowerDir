### @link: https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_signing?view=powershell-7.2#methods-of-signing-scripts

## Generate a Certificate
# $params = @{
#     Subject = 'CN=PowerShell Code Signing Cert'
#     Type = 'CodeSigning'
#     CertStoreLocation = 'Cert:\CurrentUser\My'
#     HashAlgorithm = 'sha256'
# }
# $cert = New-SelfSignedCertificate @params

## Signs a file
param([string] $file="PowerDir.GetPowerDir.format.ps1xml")
$cert = @(Get-ChildItem cert:\CurrentUser\My -codesigning)[0]
Set-AuthenticodeSignature $file $cert
