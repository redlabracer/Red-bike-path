# Copy CodeGen DLLs for Cities: Skylines 2 Post-Processing
Write-Host "`n?? Kopiere CodeGen-DLLs...`n" -ForegroundColor Cyan

$scriptAssemblies = "$env:LOCALAPPDATA\..\LocalLow\Colossal Order\Cities Skylines II\.cache\Modding\UnityModsProject\Library\ScriptAssemblies"
$packageCache = "$env:LOCALAPPDATA\..\LocalLow\Colossal Order\Cities Skylines II\.cache\Modding\UnityModsProject\Library\PackageCache"

# Entities CodeGen
$entitiesPath = Join-Path $packageCache "com.unity.entities@1.3.10\Unity.Entities.CodeGen\Unity.Entities.CodeGen.dll"
if (Test-Path $entitiesPath) {
    Copy-Item $entitiesPath -Destination $scriptAssemblies -Force
    Write-Host "? Unity.Entities.CodeGen.dll kopiert" -ForegroundColor Green
} else {
    Write-Host "? Unity.Entities.CodeGen.dll nicht gefunden" -ForegroundColor Red
}

# Burst CodeGen  
$burstPath = Join-Path $packageCache "com.unity.burst@1.8.21\Unity.Burst.CodeGen\Unity.Burst.CodeGen.dll"
if (Test-Path $burstPath) {
    Copy-Item $burstPath -Destination $scriptAssemblies -Force
    Write-Host "? Unity.Burst.CodeGen.dll kopiert" -ForegroundColor Green
} else {
    Write-Host "? Unity.Burst.CodeGen.dll nicht gefunden" -ForegroundColor Red
}

# Collections CodeGen
$collectionsPath = Join-Path $packageCache "com.unity.collections@2.5.3\Unity.Collections.CodeGen\Unity.Collections.CodeGen.dll"
if (Test-Path $collectionsPath) {
    Copy-Item $collectionsPath -Destination $scriptAssemblies -Force
    Write-Host "? Unity.Collections.CodeGen.dll kopiert" -ForegroundColor Green
} else {
    Write-Host "? Unity.Collections.CodeGen.dll nicht gefunden" -ForegroundColor Red
}

Write-Host "`n? CodeGen-DLLs Setup abgeschlossen!`n" -ForegroundColor Green

# Zeige alle Dateien im ScriptAssemblies
Write-Host "?? Dateien in ScriptAssemblies:" -ForegroundColor Cyan
Get-ChildItem $scriptAssemblies -Filter "*.dll" | ForEach-Object {
    Write-Host "   • $($_.Name)" -ForegroundColor Gray
}
Write-Host ""
