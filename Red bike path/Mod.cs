using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Red_bike_path.Systems;

namespace Red_bike_path
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(Red_bike_path)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        private Setting m_Setting;
        
        // Statische Referenz für Zugriff aus Systemen
        public static Setting Settings { get; private set; }

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            GameManager.instance.localizationManager.AddSource("de-DE", new LocaleDE(m_Setting));

            AssetDatabase.global.LoadSettings(nameof(Red_bike_path), m_Setting, new Setting(this));
            
            // Setze statische Referenz für Systeme
            Settings = m_Setting;
            
            // Registriere die Systeme zum Ändern der Fahrradweg-Farben
            // Wir verwenden mehrere Ansätze für maximale Kompatibilität
            try
            {
                // System 1: Material-basiertes System (meistens am effektivsten)
                updateSystem.UpdateAt<MaterialColorSystem>(SystemUpdatePhase.Rendering);
                log.Info("MaterialColorSystem registered");
                
                // System 2: Entity-basiertes System für Overlay-Farben
                updateSystem.UpdateAt<BikePathColorSystem>(SystemUpdatePhase.Modification2);
                log.Info("BikePathColorSystem registered");
                
                // System 3: Prefab-Modifier System (läuft einmalig beim Start)
                updateSystem.UpdateAt<PrefabColorModifierSystem>(SystemUpdatePhase.PrefabUpdate);
                log.Info("PrefabColorModifierSystem registered");
            }
            catch (System.Exception e)
            {
                log.Error($"Failed to register systems: {e.Message}");
            }
            
            log.Info("Red bike path mod loaded - Fahrradwege werden jetzt rot dargestellt!");
            log.Info("Press F9 in-game to reapply colors if needed");
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
            
            // Clear static reference
            Settings = null;
            
            log.Info("Red bike path mod unloaded");
        }
    }
}
