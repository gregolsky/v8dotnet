param(
    $Target="",
    [switch]$WinX64,
    [switch]$WinX86,
    [switch]$LinuxX64,
    [switch]$MacOs,
    [switch]$Osx,
    [switch]$Rpi,
    [switch]$LinuxArm64,
    [switch]$DontRebuildStudio,
    [switch]$DontBuildStudio,
    [switch]$JustStudio,
    [switch]$JustNuget,
    [switch]$Debug,
    [switch]$NoBundling,
    [switch]$DryRunVersionBump = $false,
    [switch]$DryRunSign = $false,
    [switch]$Help)

$ErrorActionPreference = "Stop"
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

. '.\scripts\env.ps1'
. '.\scripts\checkLastExitCode.ps1'
. '.\scripts\checkPrerequisites.ps1'
. '.\scripts\restore.ps1'
. '.\scripts\clean.ps1'
. '.\scripts\archive.ps1'
. '.\scripts\package.ps1'
. '.\scripts\buildProjects.ps1'
. '.\scripts\getScriptDirectory.ps1'
. '.\scripts\copyAssets.ps1'
. '.\scripts\validateAssembly.ps1'
. '.\scripts\validateRuntimeConfig.ps1'
#. '.\scripts\version.ps1'
. '.\scripts\updateSourceWithBuildInfo.ps1'
. '.\scripts\nuget.ps1'
. '.\scripts\target.ps1'
. '.\scripts\help.ps1'
. '.\scripts\sign.ps1'
#. '.\scripts\docker.ps1'
#. '.\scripts\schemaInfo.ps1'
. '.\scripts\runtime.ps1'
. '.\scripts\Get-DestinationFilePath.ps1'
. '.\scripts\Copy-FileHash.ps1'
. '.\scripts\exec.ps1'

if ($Help) {
    Help
}

if ($Osx) {
    $MacOs = $true
}

CheckPrerequisites

if ($Debug.IsPresent) {
    $BUILD_TYPE = "Debug"
} else {
    $BUILD_TYPE = "Release"
}

$env:PROJECT_DIR = $PROJECT_DIR = Get-ScriptDirectory
$env:RELEASE_DIR = $RELEASE_DIR = [io.path]::combine($PROJECT_DIR, "artifacts")
$OUT_DIR = [io.path]::combine($PROJECT_DIR, "artifacts")

$V8DOTNET_SRC_DIR = [io.path]::combine($PROJECT_DIR, "Source", "V8.Net")
$V8DOTNET_PROJ_PATH = [io.path]::combine($V8DOTNET_SRC_DIR, "V8.Net-Standard.csproj")
$V8DOTNET_OUT_DIR = [io.path]::combine($V8DOTNET_SRC_DIR, "bin", $BUILD_TYPE)

$V8NET_PROXY_SRC_DIR = [io.path]::combine($PROJECT_DIR, "Source", "V8.NET-Proxy")
#$V8NET_PROXY_PROJ_PATH = [io.path]::combine($V8NET_PROXY_SRC_DIR, "V8.Net-Proxy-x64-Standard.csproj")
$V8NET_PROXY_OUT_DIR = [io.path]::combine($V8NET_PROXY_SRC_DIR, "bin", $BUILD_TYPE)

$targets = GetBuildTargets $Target

if ($targets.Count -eq 0) {
    write-host "No targets specified."
    exit 0;
} else {
    Write-Host -ForegroundColor Magenta "Build targets: $($targets.Name)"
}

New-Item -Path $RELEASE_DIR -Type Directory -Force
CleanFiles $RELEASE_DIR
CleanSrcDirs $V8DOTNET_SRC_DIR 

InitGlobals $Debug $NoBundling

foreach ($target in $targets) {
    BuildV8NetProxy $V8NET_PROXY_SRC_DIR $V8NET_PROXY_OUT_DIR $BUILD_TYPE $TARGET_PLATFORM
}



