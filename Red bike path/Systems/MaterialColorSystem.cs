using Game;
using Game.Rendering;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using Colossal.Logging;

namespace Red_bike_path.Systems
{
    public partial class MaterialColorSystem : GameSystemBase
    {
        private static ILog log = LogManager.GetLogger($"{nameof(Red_bike_path)}.{nameof(MaterialColorSystem)}");
        private bool m_MaterialsUpdated = false;
        private float m_UpdateTimer = 0f;
        private const float UPDATE_INTERVAL = 2f;

        protected override void OnCreate()
        {
            base.OnCreate();
            log.Info("MaterialColorSystem created");
        }

        protected override void OnUpdate()
        {
            m_UpdateTimer += UnityEngine.Time.deltaTime;
            
            if (!m_MaterialsUpdated || m_UpdateTimer >= UPDATE_INTERVAL)
            {
                m_UpdateTimer = 0f;
                UpdateBikePathMaterials();
            }
        }

        private void UpdateBikePathMaterials()
        {
            try
            {
                var settings = Mod.Settings;
                if (settings == null)
                {
                    return;
                }

                Color targetColor = settings.GetBikePathColor();
                float intensity = settings.ColorIntensity;
                
                var allMaterials = Resources.FindObjectsOfTypeAll<Material>();
                int updatedCount = 0;
                int skippedCount = 0;
                int totalChecked = 0;

                // Beim ersten Durchlauf alle relevanten Materialien loggen
                if (!m_MaterialsUpdated)
                {
                    log.Info("=== Scanning for bike path materials ===");
                }

                foreach (var material in allMaterials)
                {
                    if (material == null || material.name == null)
                        continue;

                    string matName = material.name.ToLower();
                    
                    if (IsBikePathMaterial(matName))
                    {
                        totalChecked++;
                        
                        // Log alle gefundenen Materialien beim ersten Scan
                        if (!m_MaterialsUpdated)
                        {
                            log.Info($"Found bike material: {material.name}");
                        }
                        
                        // Prüfe ob es übersprungen werden soll
                        bool shouldSkip = false;
                        string skipReason = "";
                        
                        if (settings.OnlyBikeLanesNotPedestrian && IsSharedPathMaterial(matName))
                        {
                            shouldSkip = true;
                            skipReason = "pedestrian/shared path (OnlyBikeLanes=true)";
                        }
                        else if (!settings.ColorMixedPaths && IsSharedPathMaterial(matName))
                        {
                            shouldSkip = true;
                            skipReason = "mixed path (ColorMixedPaths=false)";
                        }
                        
                        if (shouldSkip)
                        {
                            if (!m_MaterialsUpdated)
                            {
                                log.Info($"? Skipped: {material.name} - Reason: {skipReason}");
                            }
                            skippedCount++;
                            continue;
                        }

                        bool updated = false;
                        if (material.HasProperty("_BaseColor"))
                        {
                            material.SetColor("_BaseColor", targetColor * intensity);
                            updated = true;
                        }
                        if (material.HasProperty("_Color"))
                        {
                            material.SetColor("_Color", targetColor * intensity);
                            updated = true;
                        }
                        if (material.HasProperty("_MainColor"))
                        {
                            material.SetColor("_MainColor", targetColor * intensity);
                            updated = true;
                        }
                        if (material.HasProperty("_TintColor"))
                        {
                            material.SetColor("_TintColor", targetColor * intensity);
                            updated = true;
                        }
                        if (material.HasProperty("_EmissionColor"))
                        {
                            material.SetColor("_EmissionColor", targetColor * 0.5f * intensity);
                            updated = true;
                        }
                        
                        if (updated)
                        {
                            if (!m_MaterialsUpdated)
                            {
                                log.Info($"? Updated: {material.name}");
                            }
                            updatedCount++;
                        }
                    }
                }

                log.Info($"Material scan complete: Checked {totalChecked}, Updated {updatedCount}, Skipped {skippedCount}");
                
                if (updatedCount > 0 || skippedCount > 0)
                {
                    m_MaterialsUpdated = true;
                }
            }
            catch (System.Exception ex)
            {
                log.Error($"Error updating bike path materials: {ex.Message}");
            }
        }

        private bool IsBikePathMaterial(string materialName)
        {
            // Liste von Schlüsselwörtern die auf Fahrradweg-Materialien hinweisen
            string[] bikePathKeywords = new[]
            {
                "bike",
                "bicycle",
                "cycle",
                "bikepath",
                "cycleway",
                "lane_marking_bike",
                "bikelane"
            };

            foreach (var keyword in bikePathKeywords)
            {
                if (materialName.Contains(keyword))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSharedPathMaterial(string materialName)
        {
            // NEUE STRATEGIE mit separater Kontrolle für gemischte Wege
            
            bool hasBikeKeyword = materialName.Contains("bike") || 
                                  materialName.Contains("bicycle") || 
                                  materialName.Contains("cycle");
            
            bool hasPedestrianKeyword = materialName.Contains("pedestrian") ||
                                       materialName.Contains("sidewalk") ||
                                       materialName.Contains("footpath") ||
                                       materialName.Contains("pavement") ||
                                       materialName.Contains("walkway") ||
                                       materialName.Contains("_ped_");
            
            bool hasMixedKeyword = materialName.Contains("mixed") ||
                                  materialName.Contains("shared") ||
                                  materialName.Contains("divided");
            
            // Wenn SOWOHL bike ALS AUCH (pedestrian ODER mixed) im Namen:
            // Das ist ein gemischter Weg
            bool isMixedPath = hasBikeKeyword && (hasPedestrianKeyword || hasMixedKeyword);
            
            if (isMixedPath)
            {
                var settings = Mod.Settings;
                // Wenn gemischte Wege erlaubt sind, NICHT überspringen
                if (settings != null && settings.ColorMixedPaths)
                {
                    return false; // NICHT überspringen, färben!
                }
                // Sonst überspringen
                return true; // Ja, überspringen
            }
            
            // Wenn NUR pedestrian Keywords (kein bike): Überspringe auch
            if (!hasBikeKeyword && hasPedestrianKeyword)
            {
                return true; // Ja, nur Fußgänger, überspringe
            }
            
            // Wenn NUR bike Keywords: NICHT überspringen, färben!
            return false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            log.Info("MaterialColorSystem destroyed");
        }
    }
}
