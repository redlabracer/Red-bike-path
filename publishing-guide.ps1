# Red Bike Path - Complete Publishing Guide
# Führt durch den kompletten Veröffentlichungsprozess

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Red Bike Path - Publishing Guide" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "?? Ziel: Mod auf Paradox Mods veröffentlichen`n" -ForegroundColor Yellow

$cacheDir = "$env:LOCALAPPDATA\..\LocalLow\Colossal Order\Cities Skylines II\.cache\Modding\UnityModsProject\Library\ScriptAssemblies"
$cacheFiles = Get-ChildItem $cacheDir -Filter "*CodeGen.dll" -ErrorAction SilentlyContinue

if ($cacheFiles.Count -lt 3) {
    Write-Host "??  Unity-Cache ist nicht vollständig!" -ForegroundColor Yellow
    Write-Host "`n?? Erforderliche Schritte:" -ForegroundColor Cyan
    Write-Host "`n   SCHRITT 1: Unity-Cache initialisieren" -ForegroundColor Green
    Write-Host "   ?????????????????????????????????????" -ForegroundColor Gray
    Write-Host "   1. Starte Cities: Skylines 2" -ForegroundColor White
    Write-Host "   2. Warte bis das Hauptmenü vollständig geladen ist" -ForegroundColor White
    Write-Host "   3. Schließe das Spiel wieder" -ForegroundColor White
    Write-Host "   4. Führe dieses Skript erneut aus" -ForegroundColor White
    
    Write-Host "`n   ?? ODER: Nutze den in-game Publisher" -ForegroundColor Yellow
    Write-Host "   ?????????????????????????????????????" -ForegroundColor Gray
    Write-Host "   1. Öffne Cities: Skylines 2" -ForegroundColor White
    Write-Host "   2. Gehe zu: Options ? Paradox Mods" -ForegroundColor White
    Write-Host "   3. Klicke auf: 'Upload Mod' oder 'Publish Mod'" -ForegroundColor White
    Write-Host "   4. Wähle 'Red bike path' aus der Liste" -ForegroundColor White
    Write-Host "   5. Klicke auf 'Publish'" -ForegroundColor White
    
    Write-Host "`n========================================`n" -ForegroundColor Cyan
    
    Write-Host "??  Technische Info:" -ForegroundColor Cyan
    Write-Host "   Das Post-Processing benötigt Unity CodeGen-Dateien." -ForegroundColor Gray
    Write-Host "   Diese werden beim ersten Start von CS2 erstellt." -ForegroundColor Gray
    Write-Host "`n   Aktuell im Cache:" -ForegroundColor Gray
    Get-ChildItem $cacheDir -Filter "*.dll" -ErrorAction SilentlyContinue | ForEach-Object {
        Write-Host "   • $($_.Name)" -ForegroundColor DarkGray
    }
    Write-Host ""
    
} else {
    Write-Host "? Unity-Cache ist vollständig!" -ForegroundColor Green
    Write-Host "`n?? Gefundene CodeGen-Dateien:" -ForegroundColor Cyan
    $cacheFiles | ForEach-Object {
        Write-Host "   ? $($_.Name)" -ForegroundColor Green
    }
    
    Write-Host "`n?? Du kannst jetzt veröffentlichen:" -ForegroundColor Yellow
    Write-Host "`n   OPTION 1: dotnet publish (Kommandozeile)" -ForegroundColor Cyan
    Write-Host "   ?????????????????????????????????????" -ForegroundColor Gray
    Write-Host "   cd `"Red bike path`"" -ForegroundColor White
    Write-Host "   dotnet publish -c Release /p:PublishProfile=PublishNewMod" -ForegroundColor White
    
    Write-Host "`n   OPTION 2: Visual Studio" -ForegroundColor Cyan
    Write-Host "   ?????????????????????????????????????" -ForegroundColor Gray
    Write-Host "   1. Rechtsklick auf Projekt" -ForegroundColor White
    Write-Host "   2. Publish ? PublishNewMod" -ForegroundColor White
    
    Write-Host "`n   OPTION 3: Im Spiel" -ForegroundColor Cyan
    Write-Host "   ?????????????????????????????????????" -ForegroundColor Gray
    Write-Host "   1. Starte Cities: Skylines 2" -ForegroundColor White
    Write-Host "   2. Options ? Paradox Mods ? Upload Mod" -ForegroundColor White
    
    Write-Host "`n========================================`n" -ForegroundColor Cyan
}
