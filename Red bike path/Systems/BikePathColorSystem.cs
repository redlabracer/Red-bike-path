using Game;
using Unity.Entities;
using UnityEngine;
using Colossal.Logging;

namespace Red_bike_path.Systems
{
    /// <summary>
    /// System zur Änderung der Fahrradweg-Overlay-Farben
    /// Ändert die Farbe von Grün (Standard) zu Rot
    /// </summary>
    public partial class BikePathColorSystem : GameSystemBase
    {
        private static ILog log = LogManager.GetLogger($"{nameof(Red_bike_path)}.{nameof(BikePathColorSystem)}");
        private bool m_ColorsApplied = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            log.Info("BikePathColorSystem created");
        }

        protected override void OnUpdate()
        {
            // Nur einmal beim Start ausführen
            if (!m_ColorsApplied)
            {
                ApplyBikePathColors();
                m_ColorsApplied = true;
            }
        }

        private void ApplyBikePathColors()
        {
            try
            {
                var settings = Mod.Settings;
                if (settings == null)
                {
                    log.Warn("Settings not available yet");
                    return;
                }

                // Hole die Farbe aus den Einstellungen (Standard: Rot)
                Color targetColor = settings.GetBikePathColor();
                float intensity = settings.ColorIntensity;
                
                log.Info($"Applying bike path color: R={targetColor.r}, G={targetColor.g}, B={targetColor.b}, Intensity={intensity}");
                
                // Die eigentliche Farbänderung erfolgt im MaterialColorSystem
                // Dieses System sorgt nur für die Initialisierung
            }
            catch (System.Exception ex)
            {
                log.Error($"Error applying bike path colors: {ex.Message}");
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            log.Info("BikePathColorSystem destroyed");
        }
    }
}
