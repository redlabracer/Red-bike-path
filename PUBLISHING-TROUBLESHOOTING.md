# Publishing Troubleshooting Guide

## Problem: ModPostProcessor Fehler beim Veröffentlichen

### Symptom
```
Fehler bei der Veröffentlichung.
Could not find file 'Unity.Entities.CodeGen.dll'
```

### Ursache
Das Cities: Skylines 2 ModPostProcessor-Tool benötigt Unity CodeGen-Dateien, die beim ersten Start des Spiels erstellt werden. Wenn diese Dateien fehlen, schlägt die Veröffentlichung fehl.

### ? Lösung 1: Unity-Cache initialisieren (EMPFOHLEN)

**Führe das Skript aus:**
```powershell
.\publishing-guide.ps1
```

Das Skript zeigt dir den Status deines Unity-Cache und gibt Anweisungen.

**Manuelle Schritte:**
1. **Starte Cities: Skylines 2**
2. **Warte**, bis das Hauptmenü vollständig geladen ist (ca. 30-60 Sekunden)
3. **Schließe das Spiel** wieder
4. Der Unity-Cache ist nun initialisiert

**Veröffentliche dann:**
```powershell
cd "Red bike path"
dotnet publish -c Release /p:PublishProfile=PublishNewMod
```

### ? Lösung 2: In-Game Publisher (EINFACHST)

Umgehe das Problem komplett, indem du den integrierten Publisher verwendest:

1. **Starte Cities: Skylines 2**
2. **Gehe zu:** `Options ? Paradox Mods`
3. **Klicke auf:** `Upload Mod` oder `Publish Mod`
4. **Wähle:** `Red bike path` aus der Liste
5. **Klicke:** `Publish`
6. **Fertig!** Die Mod wird direkt hochgeladen

### ? Lösung 3: Unity-Dateien manuell kopieren

Wenn der Cache nicht automatisch erstellt wird:

```powershell
.\setup-unity-cache.ps1
```

Das Skript kopiert die benötigten Dateien automatisch aus dem Spiel-Verzeichnis.

## Dateien-Übersicht

### Benötigte Unity-Dateien im Cache
Pfad: `%LocalAppData%\..\LocalLow\Colossal Order\Cities Skylines II\.cache\Modding\UnityModsProject\Library\ScriptAssemblies\`

Erforderliche Dateien:
- ? `Unity.Entities.dll`
- ? `Unity.Burst.dll`
- ? `Unity.Collections.dll`
- ? `Unity.Entities.CodeGen.dll` (wird beim Spielstart erstellt)
- ? `Unity.Burst.CodeGen.dll` (wird beim Spielstart erstellt)
- ? `Unity.Collections.CodeGen.dll` (wird beim Spielstart erstellt)

### Cache-Status prüfen

```powershell
# Zeige alle Dateien im Unity-Cache
Get-ChildItem "$env:LOCALAPPDATA\..\LocalLow\Colossal Order\Cities Skylines II\.cache\Modding\UnityModsProject\Library\ScriptAssemblies" -Filter "*.dll"
```

## Häufige Fehler

### Fehler: "Could not find file 'Unity.Entities.CodeGen.dll'"
**Lösung:** Starte CS2 einmal, um den Cache zu initialisieren

### Fehler: "Fehler bei der Veröffentlichung" (keine Details)
**Lösung:** Nutze den In-Game Publisher (Lösung 2)

### Fehler: "ModPostProcessor.exe ... wurde mit dem Code -1 beendet"
**Lösung:** Unity-Cache ist leer - siehe Lösung 1

## Warum ist Post-Processing erforderlich?

Das ModPostProcessor-Tool:
1. Führt IL-Code-Transformationen durch (Burst Compiler)
2. Generiert ECS-Code (Entity Component System)
3. Optimiert den Code für Unity
4. Fügt Mod-Metadaten hinzu

**Für Paradox Mods Publishing ist dies zwingend erforderlich!**

## Alternative: Entwicklungs-Build ohne Post-Processing

Für lokales Testing kannst du das Post-Processing überspringen:

```powershell
dotnet build -c Debug
```

Aber: **Dies funktioniert NICHT für Paradox Mods Publishing!**

## Hilfreiche Skripte

| Skript | Beschreibung |
|--------|-------------|
| `publishing-guide.ps1` | Zeigt Publishing-Status und Anweisungen |
| `setup-unity-cache.ps1` | Kopiert Unity-Dateien in den Cache |
| `copy-codegen-dlls.ps1` | Kopiert CodeGen-DLLs (experimentell) |
| `publish.ps1` | Vereinfachter Publish-Befehl |

## Support

**Problem gelöst?** ??

**Immer noch Probleme?**
- Öffne ein Issue auf GitHub
- Stelle sicher, dass CS2 richtig installiert ist
- Prüfe, ob die Modding Toolchain korrekt konfiguriert ist
