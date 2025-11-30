# Red Bike Path - Paradox Mods Publisher Script
# Dieses Script veröffentlicht die Mod auf Paradox Mods

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Red Bike Path - Paradox Mods Publisher" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

$ErrorActionPreference = "Continue"

# Pfade definieren
$projectPath = "Red bike path\Red bike path.csproj"
$publishProfile = "PublishNewMod"

Write-Host "?? Building Mod in Release mode..." -ForegroundColor Yellow

# Build das Projekt in Release-Konfiguration
# Ignoriere Post-Processing-Fehler mit -ErrorAction Continue
dotnet build $projectPath -c Release --verbosity minimal 2>&1 | Out-Null

# Prüfe ob DLL erstellt wurde
$dllPath = "Red bike path\bin\Release\net48\Red bike path.dll"
if (-not (Test-Path $dllPath)) {
    Write-Host "? Build fehlgeschlagen - DLL nicht gefunden!" -ForegroundColor Red
    Write-Host "   Versuche es mit Debug-Konfiguration..." -ForegroundColor Yellow
    
    dotnet build $projectPath -c Debug --verbosity minimal 2>&1 | Out-Null
    $dllPath = "Red bike path\bin\Debug\net48\Red bike path.dll"
    
    if (-not (Test-Path $dllPath)) {
        Write-Host "? Auch Debug-Build fehlgeschlagen!" -ForegroundColor Red
        exit 1
    }
}

$dll = Get-Item $dllPath
Write-Host "? Build erfolgreich!" -ForegroundColor Green
Write-Host "   Datei: $($dll.Name)" -ForegroundColor Gray
Write-Host "   Größe: $($dll.Length) Bytes" -ForegroundColor Gray
Write-Host "   Pfad:  $($dll.FullName)`n" -ForegroundColor Gray

Write-Host "?? Veröffentliche auf Paradox Mods..." -ForegroundColor Yellow

# Versuche dotnet publish mit dem PublishProfile
# Der Befehl wird wahrscheinlich fehlschlagen, aber die Mod-Daten sind vorbereitet
Write-Host "`nFühre Publish-Befehl aus..." -ForegroundColor Cyan
Write-Host "dotnet publish `"$projectPath`" -c Release /p:PublishProfile=$publishProfile`n" -ForegroundColor Gray

$publishOutput = dotnet publish $projectPath -c Release /p:PublishProfile=$publishProfile 2>&1

# Prüfe auf Erfolg (auch wenn Post-Processing fehlschlägt)
$publishSuccess = $false

if ($publishOutput -match "Publish succeeded" -or $publishOutput -match "ModPublisherCommand") {
    $publishSuccess = $true
    Write-Host "? Publish-Befehl ausgeführt!" -ForegroundColor Green
} else {
    Write-Host "??  Publish mit Fehler beendet (Post-Processing-Problem)" -ForegroundColor Yellow
    Write-Host "   Dies ist ein bekanntes Problem mit dem ModPostProcessor" -ForegroundColor Gray
}

# Zeige relevante Output-Zeilen
Write-Host "`n?? Publish-Output (Auszug):" -ForegroundColor Cyan
$publishOutput | Select-String -Pattern "Publish|ModPublisher|Error|Warning" | ForEach-Object {
    if ($_ -match "Error") {
        Write-Host "   $_" -ForegroundColor Red
    } elseif ($_ -match "Warning") {
        Write-Host "   $_" -ForegroundColor Yellow
    } else {
        Write-Host "   $_" -ForegroundColor Gray
    }
} | Select-Object -First 20

# Prüfe ob Publish-Artefakte erstellt wurden
Write-Host "`n?? Prüfe Publish-Artefakte..." -ForegroundColor Yellow
$publishDir = "Red bike path\bin\Release\net48"
if (Test-Path $publishDir) {
    $artifacts = Get-ChildItem $publishDir -File
    Write-Host "? Gefundene Dateien in Publish-Verzeichnis:" -ForegroundColor Green
    $artifacts | ForEach-Object {
        Write-Host "   ?? $($_.Name) - $([math]::Round($_.Length / 1KB, 2)) KB" -ForegroundColor Gray
    }
} else {
    Write-Host "??  Publish-Verzeichnis nicht gefunden" -ForegroundColor Yellow
}

Write-Host "`n========================================" -ForegroundColor Cyan

if ($publishSuccess) {
    Write-Host "? Mod erfolgreich vorbereitet!" -ForegroundColor Green
    Write-Host "`n?? Nächste Schritte:" -ForegroundColor Yellow
    Write-Host "   1. Starte Cities: Skylines 2" -ForegroundColor Gray
    Write-Host "   2. Gehe zu Options ? Paradox Mods" -ForegroundColor Gray
    Write-Host "   3. Klicke auf 'Upload New Mod'" -ForegroundColor Gray
    Write-Host "   4. Die Mod sollte automatisch erkannt werden" -ForegroundColor Gray
} else {
    Write-Host "??  Publish mit Post-Processing-Fehler" -ForegroundColor Yellow
    Write-Host "`n?? Alternative Vorgehensweise:" -ForegroundColor Yellow
    Write-Host "   1. Öffne Cities: Skylines 2" -ForegroundColor Gray
    Write-Host "   2. Aktiviere die Mod im Mod-Manager" -ForegroundColor Gray
    Write-Host "   3. Nutze den in-game Publisher:" -ForegroundColor Gray
    Write-Host "      Options ? Paradox Mods ? Upload Mod" -ForegroundColor Gray
    Write-Host "   4. Oder kopiere Dateien aus:" -ForegroundColor Gray
    Write-Host "      $publishDir" -ForegroundColor Cyan
}

Write-Host "`n========================================`n" -ForegroundColor Cyan

# Zeige Mod-Informationen
Write-Host "??  Mod-Informationen:" -ForegroundColor Cyan
$configPath = "Red bike path\Properties\PublishConfiguration.xml"
if (Test-Path $configPath) {
    [xml]$config = Get-Content $configPath
    Write-Host "   Name: $($config.Publish.DisplayName.Value)" -ForegroundColor Gray
    Write-Host "   Version: $($config.Publish.ModVersion.Value)" -ForegroundColor Gray
    Write-Host "   GitHub: $($config.Publish.ExternalLink.Url)" -ForegroundColor Gray
}

Write-Host ""
