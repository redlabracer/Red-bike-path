# Red Bike Path - Build und Install Script
# Dieses Script baut die Mod und kopiert sie in den Cities: Skylines 2 Mods-Ordner

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Red Bike Path - Build & Install" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Build das Projekt
Write-Host "?? Baue Projekt..." -ForegroundColor Yellow
dotnet build "Red bike path\Red bike path.csproj" --verbosity minimal 2>&1 | Out-Null

# Prüfe ob DLL erstellt wurde
$dllPath = "Red bike path\bin\Debug\net48\Red bike path.dll"
if (-not (Test-Path $dllPath)) {
    Write-Host "? Build fehlgeschlagen - DLL nicht gefunden!" -ForegroundColor Red
    exit 1
}

$dll = Get-Item $dllPath
Write-Host "? Build erfolgreich!" -ForegroundColor Green
Write-Host "   Datei: $($dll.Name)" -ForegroundColor Gray
Write-Host "   Größe: $($dll.Length) Bytes" -ForegroundColor Gray
Write-Host "   Zeit:  $($dll.LastWriteTime.ToString('HH:mm:ss'))`n" -ForegroundColor Gray

# Mods-Ordner vorbereiten
$modsBase = "$env:LOCALAPPDATA\..\LocalLow\Colossal Order\Cities Skylines II\Mods"
$modFolder = Join-Path $modsBase "Red_bike_path"

Write-Host "?? Bereite Mod-Ordner vor..." -ForegroundColor Yellow

# Entferne alte Datei falls vorhanden (Fehlerfall)
$oldFile = Join-Path $modsBase "Red_bike_path"
if ((Test-Path $oldFile) -and -not (Get-Item $oldFile).PSIsContainer) {
    Remove-Item $oldFile -Force
    Write-Host "   Alte Datei entfernt" -ForegroundColor Gray
}

# Erstelle Ordner
New-Item -ItemType Directory -Force -Path $modFolder | Out-Null
Write-Host "   Ordner: $modFolder" -ForegroundColor Gray

# Kopiere Dateien
Write-Host "`n?? Kopiere Mod-Dateien..." -ForegroundColor Yellow
Copy-Item "Red bike path\bin\Debug\net48\Red bike path.dll" -Destination $modFolder -Force
Copy-Item "Red bike path\bin\Debug\net48\Red bike path.pdb" -Destination $modFolder -Force -ErrorAction SilentlyContinue

# Zeige installierte Dateien
Write-Host "? Installation erfolgreich!`n" -ForegroundColor Green
Write-Host "Installierte Dateien:" -ForegroundColor Cyan
Get-ChildItem $modFolder | ForEach-Object {
    Write-Host "   • $($_.Name) ($($_.Length) Bytes)" -ForegroundColor Gray
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "? Mod ist bereit für Cities: Skylines 2!" -ForegroundColor Green
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "?? Nächste Schritte:" -ForegroundColor Yellow
Write-Host "   1. Starte Cities: Skylines 2" -ForegroundColor Gray
Write-Host "   2. Aktiviere die Mod im Mod-Manager" -ForegroundColor Gray
Write-Host "   3. Passe die Einstellungen an`n" -ForegroundColor Gray
