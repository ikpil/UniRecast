# DotRecast -> Runtime sync script (PowerShell version of pull_dotrecast.sh)

param(
    [Parameter(Mandatory = $false, Position = 0)]
    [string]$DotRecastSrc = $args[0]
)

# 1) Argument check (same semantics as bash script)
if ([string]::IsNullOrWhiteSpace($DotRecastSrc)) {
    Write-Host "Usage: $($MyInvocation.MyCommand.Name) <DotRecast-Source-Path>" -ForegroundColor Yellow
    Write-Host "Example: $($MyInvocation.MyCommand.Name) ..\DotRecast" -ForegroundColor Yellow
    exit 1
}

# 2) Source directory check
if (-not (Test-Path -Path $DotRecastSrc -PathType Container)) {
    Write-Host "Error: Directory '$DotRecastSrc' not found." -ForegroundColor Red
    exit 1
}

# 3) Module list (must match pull_dotrecast.sh)
$Modules = @(
    "DotRecast.Core"
    "DotRecast.Detour"
    "DotRecast.Detour.Crowd"
    "DotRecast.Detour.Dynamic"
    "DotRecast.Detour.Extras"
    "DotRecast.Detour.TileCache"
    "DotRecast.Recast"
    "DotRecast.Recast.Toolset"
)

# 4) Destination root (same as ./Runtime)
$DestRoot = Join-Path (Get-Location) "Runtime"

Write-Host "Starting sync from '$DotRecastSrc' to '$DestRoot'..." -ForegroundColor Cyan
Write-Host ""

foreach ($Module in $Modules) {
    # 5) Decide source path: <src>/src/Module or <src>/Module
    $SrcPathUnderSrc = Join-Path $DotRecastSrc ("src\" + $Module)
    $SrcPathAtRoot   = Join-Path $DotRecastSrc $Module
    $SrcPath         = $null

    if (Test-Path -Path $SrcPathUnderSrc -PathType Container) {
        $SrcPath = $SrcPathUnderSrc
    }
    elseif (Test-Path -Path $SrcPathAtRoot -PathType Container) {
        $SrcPath = $SrcPathAtRoot
    }
    else {
        Write-Host "Warning: Module '$Module' not found in source path. Skipping." -ForegroundColor Yellow
        Write-Host ""
        continue
    }

    # Destination path: Runtime/Module
    $DestPath = Join-Path $DestRoot $Module

    Write-Host "Syncing $Module..." -ForegroundColor Green
    Write-Host "  From: $SrcPath" -ForegroundColor DarkGray
    Write-Host "  To  : $DestPath" -ForegroundColor DarkGray

    # Ensure destination directory exists
    if (-not (Test-Path -Path $DestPath -PathType Container)) {
        New-Item -ItemType Directory -Path $DestPath -Force | Out-Null
    }

    # 6) Use robocopy to emulate rsync -av --delete with excludes
    #    - /MIR : mirror directory tree (equivalent to -a + --delete)
    #    - /XD  : exclude directories (bin, obj)
    #    - /XF  : exclude files (*.meta, *.asmdef, *.csproj, .*)
    #    robocopy exit codes 0–7 are success/info, >=8 is failure.

    & robocopy "$SrcPath" "$DestPath" /MIR `
        /XD bin obj `
        /XF *.meta *.asmdef *.csproj .* `
        /R:3 /W:1

    $code = $LASTEXITCODE
    if ($code -ge 8) {
        Write-Host "  Error: robocopy failed for $Module (code $code)." -ForegroundColor Red
    }
    else {
        Write-Host "  Completed $Module (robocopy code $code)." -ForegroundColor Cyan
    }

    Write-Host ""
}

Write-Host "Sync completed." -ForegroundColor Cyan
