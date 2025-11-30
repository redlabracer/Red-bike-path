# Setup Unity Cache für Cities: Skylines 2 Modding
# Dieses Skript bereitet den Unity-Cache vor, der für Post-Processing benötigt wird

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Unity Cache Setup für CS2 Modding" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

$cacheDir = "$env:LOCALAPPDATA\..\LocalLow\Colossal Order\Cities Skylines II\.cache\Modding\UnityModsProject\Library\ScriptAssemblies"
$gameDir = "D:\SteamLibrary\steamapps\common\Cities Skylines II\Cities2_Data\Managed"

Write-Host "?? Prüfe Unity-Cache..." -ForegroundColor Yellow

if (-not (Test-Path $cacheDir)) {
    Write-Host "? Cache-Verzeichnis existiert nicht!" -ForegroundColor Red
    Write-Host "   Erstelle Verzeichnis..." -ForegroundColor Gray
    New-Item -ItemType Directory -Path $cacheDir -Force | Out-Null
}

$cacheFiles = Get-ChildItem $cacheDir -Filter "*.dll" -ErrorAction SilentlyContinue
if ($cacheFiles.Count -eq 0) {
    Write-Host "??  Unity-Cache ist leer!" -ForegroundColor Yellow
    Write-Host "`n?? Der Unity-Cache wird beim ersten Start von Cities: Skylines 2 erstellt." -ForegroundColor Cyan
    Write-Host "`n? Empfohlene Lösung:" -ForegroundColor Green
    Write-Host "   1. Starte Cities: Skylines 2" -ForegroundColor Gray
    Write-Host "   2. Warte bis das Hauptmenü geladen ist" -ForegroundColor Gray
    Write-Host "   3. Schließe das Spiel wieder" -ForegroundColor Gray
    Write-Host "   4. Führe dann erneut dotnet publish aus" -ForegroundColor Gray
    
    Write-Host "`n?? Alternative: Kopiere benötigte Dateien aus dem Spiel-Verzeichnis" -ForegroundColor Yellow
    Write-Host "   Quelle: $gameDir" -ForegroundColor Gray
    Write-Host "   Ziel:   $cacheDir" -ForegroundColor Gray
    
    # Versuche Dateien zu kopieren
    Write-Host "`n?? Versuche fehlende Dateien zu kopieren..." -ForegroundColor Yellow
    
    $filesToCopy = @(
        "Unity.Entities.dll",
        "Unity.Burst.dll",
        "Unity.Collections.dll"
    )
    
    $copiedFiles = 0
    foreach ($file in $filesToCopy) {
        $sourcePath = Join-Path $gameDir $file
        if (Test-Path $sourcePath) {
            try {
                Copy-Item $sourcePath -Destination $cacheDir -Force
                Write-Host "   ? Kopiert: $file" -ForegroundColor Green
                $copiedFiles++
            } catch {
                Write-Host "   ? Fehler beim Kopieren: $file" -ForegroundColor Red
            }
        } else {
            Write-Host "   ? Nicht gefunden: $file" -ForegroundColor Yellow
        }
    }
    
    if ($copiedFiles -gt 0) {
        Write-Host "`n? $copiedFiles Datei(en) erfolgreich kopiert!" -ForegroundColor Green
        Write-Host "   Versuche jetzt erneut: dotnet publish`n" -ForegroundColor Cyan
    } else {
        Write-Host "`n??  Keine Dateien kopiert. Bitte starte das Spiel einmal.`n" -ForegroundColor Yellow
    }
    
} else {
    Write-Host "? Unity-Cache ist bereits vorhanden!" -ForegroundColor Green
    Write-Host "`n?? Gefundene Cache-Dateien:" -ForegroundColor Cyan
    $cacheFiles | ForEach-Object {
        Write-Host "   • $($_.Name)" -ForegroundColor Gray
    }
    Write-Host "`n? Du kannst jetzt dotnet publish ausführen!`n" -ForegroundColor Green
}

Write-Host "========================================`n" -ForegroundColor Cyan
