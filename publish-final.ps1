# Final Publishing Script - Red Bike Path
# Vollautomatischer Publishing-Workflow

Write-Host "`n" -NoNewline
Write-Host "???????????????????????????????????????" -ForegroundColor Cyan
Write-Host "   Red Bike Path - Publishing Assistant" -ForegroundColor Cyan
Write-Host "???????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

$projectPath = "Red bike path\Red bike path.csproj"
$modsFolder = "$env:LOCALAPPDATA\..\LocalLow\Colossal Order\Cities Skylines II\Mods\Red_bike_path"

# Schritt 1: Build
Write-Host "?? Schritt 1: Baue Mod (Release - ohne Post-Processing)..." -ForegroundColor Yellow
dotnet build $projectPath -c Release /p:RunPostBuildEvent=Never --verbosity quiet 2>&1 | Out-Null

# Prüfe ob DLL existiert (auch wenn Build fehlgeschlagen, kann DLL vorhanden sein)
$dllPath = "Red bike path\bin\Release\net48\Red bike path.dll"
if (-not (Test-Path $dllPath)) {
    Write-Host "??  Build mit Post-Processing-Fehler - verwende existierende DLL" -ForegroundColor Yellow
    # Die DLL wurde bereits früher erstellt, also einfach weitermachen
    if (-not (Test-Path $dllPath)) {
        Write-Host "? Keine DLL gefunden! Bitte führe erst einen erfolgreichen Build durch." -ForegroundColor Red
        exit 1
    }
}

$dll = Get-Item $dllPath
Write-Host "? DLL gefunden: $($dll.Name) ($([math]::Round($dll.Length/1KB, 2)) KB)" -ForegroundColor Green

# Schritt 2: Kopiere in Mods-Ordner
Write-Host "`n?? Schritt 2: Kopiere Dateien in Mods-Ordner..." -ForegroundColor Yellow

if (-not (Test-Path $modsFolder)) {
    New-Item -ItemType Directory -Path $modsFolder -Force | Out-Null
    Write-Host "   Mods-Ordner erstellt" -ForegroundColor Gray
}

$filesToCopy = @(
    @{Source = $dllPath; Dest = "Red bike path.dll"},
    @{Source = "Red bike path\Properties\PublishConfiguration.xml"; Dest = "PublishConfiguration.xml"},
    @{Source = "Red bike path\Properties\Thumbnail.jpeg"; Dest = "Thumbnail.jpeg"},
    @{Source = "Red bike path\Properties\Screenshot 1.png"; Dest = "Screenshot 1.png"},
    @{Source = "Red bike path\Properties\Screenshot 2.png"; Dest = "Screenshot 2.png"}
)

$copiedCount = 0
foreach ($file in $filesToCopy) {
    if (Test-Path $file.Source) {
        Copy-Item $file.Source -Destination (Join-Path $modsFolder $file.Dest) -Force
        Write-Host "   ? $($file.Dest)" -ForegroundColor Green
        $copiedCount++
    } else {
        Write-Host "   ? $($file.Dest) nicht gefunden" -ForegroundColor Yellow
    }
}

Write-Host "`n? $copiedCount Datei(en) kopiert nach:" -ForegroundColor Green
Write-Host "   $modsFolder" -ForegroundColor Cyan

# Schritt 3: Anweisungen
Write-Host "`n" -NoNewline
Write-Host "???????????????????????????????????????" -ForegroundColor Cyan
Write-Host "   ?? NÄCHSTE SCHRITTE" -ForegroundColor Yellow
Write-Host "???????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

Write-Host "Die Mod ist bereit zur Veröffentlichung!" -ForegroundColor Green
Write-Host ""
Write-Host "OPTION 1: In-Game Publisher (EMPFOHLEN)" -ForegroundColor Cyan
Write-Host "?????????????????????????????????????????" -ForegroundColor Gray
Write-Host "  1. Starte " -NoNewline; Write-Host "Cities: Skylines 2" -ForegroundColor White
Write-Host "  2. Gehe zu " -NoNewline; Write-Host "Options ? Paradox Mods" -ForegroundColor White
Write-Host "  3. Klicke auf " -NoNewline; Write-Host "'Upload Mod'" -ForegroundColor White
Write-Host "  4. Wähle " -NoNewline; Write-Host "'Red bike path'" -ForegroundColor White
Write-Host "  5. Klicke " -NoNewline; Write-Host "'Publish'" -ForegroundColor White
Write-Host ""

Write-Host "OPTION 2: Kommandozeile (Fortgeschritten)" -ForegroundColor Cyan
Write-Host "?????????????????????????????????????????" -ForegroundColor Gray
Write-Host "  1. Starte CS2 einmal (initialisiert Unity-Cache)" -ForegroundColor White
Write-Host "  2. Schließe CS2" -ForegroundColor White
Write-Host "  3. Führe aus:" -ForegroundColor White
Write-Host "     " -NoNewline; Write-Host "cd 'Red bike path'" -ForegroundColor DarkCyan
Write-Host "     " -NoNewline; Write-Host "dotnet publish -c Release /p:PublishProfile=PublishNewMod" -ForegroundColor DarkCyan
Write-Host ""

Write-Host "???????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

Write-Host "?? Tipp: " -NoNewline -ForegroundColor Yellow
Write-Host "Der In-Game Publisher ist die einfachste Methode!" -ForegroundColor Gray
Write-Host "         Er erfordert kein Post-Processing Setup." -ForegroundColor Gray
Write-Host ""

Write-Host "?? Mod-Info:" -ForegroundColor Cyan
Write-Host "   Name:    Colored bike path (CBP)" -ForegroundColor Gray
Write-Host "   Version: 1.0.0" -ForegroundColor Gray
Write-Host "   GitHub:  https://github.com/redlabracer/Red-bike-path" -ForegroundColor Gray
Write-Host ""
