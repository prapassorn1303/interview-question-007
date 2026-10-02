# Select a workspace-local .NET 8 SDK for this PowerShell session.
# If needed, obtain it using Microsoft's official installation script.
$ErrorActionPreference = 'Stop'
$sdkDirectory = Join-Path $PSScriptRoot 'work/dotnet'
$sdkExecutable = Join-Path $sdkDirectory 'dotnet.exe'
$hasSdk = (Test-Path $sdkExecutable) -and
    ((& $sdkExecutable --list-sdks) -match '^8\.0\.')
if (!$hasSdk) {
    New-Item -ItemType Directory -Force -Path (Join-Path $PSScriptRoot 'work') | Out-Null
    $installerPath = Join-Path $PSScriptRoot 'work/dotnet-install.ps1'
    Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile $installerPath
    & $installerPath -Channel '8.0' -InstallDir $sdkDirectory -NoPath
    if (!(Test-Path $sdkExecutable)) { throw '.NET 8 SDK installation failed.' }
}
$env:DOTNET_ROOT = $sdkDirectory
$env:DOTNET_ROOT_X64 = $sdkDirectory
$env:PATH = $sdkDirectory + [IO.Path]::PathSeparator + $env:PATH
Write-Host "Using .NET SDK $(& $sdkExecutable --version) from $sdkDirectory"
