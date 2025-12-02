using Game;
using Game.Rendering;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using Colossal.Logging;

namespace Red_bike_path.Systems
{
    /// <summary>
    /// System zur Änderung der Fahrradweg-Material-Farben.
    /// 
    /// Version 2.1.0 Fix:
    /// - Verhindert, dass Fahrzeuge und Gebäude eingefärbt werden
    /// - Verwendet spezifischere Material-Keywords (bikepath, bikelane, cycleway statt nur "bike")
    /// - Schließt explizit Fahrzeug-, Gebäude- und UI-Materialien aus
    /// - Prüft Shader-Namen, um nur Terrain/Path-Materialien zu färben
    /// 
    /// Version 2.2.0 Fix:
    /// - Fixed color blending issue where colors were multiplied instead of replaced
    /// - Now uses Color.Lerp for proper color blending, allowing bright colors like white and cyan to display correctly
    /// - ColorIntensity now controls the blend between white and the target color
    /// 
    /// Version 2.3.0 Fix:
    /// - Added null checks to prevent NullReferenceException in logging
    /// - Added safe shader name access with null checks
    /// - Removed special Unicode characters from log messages
    /// - Added defensive logging with try-catch blocks
    /// 
    /// Version 2.4.0 Fix:
    /// - Disabled all logging due to Colossal logging framework instability
    /// - System now operates silently to prevent crashes
    /// </summary>
    public partial class MaterialColorSystem : GameSystemBase
    {
        private ILog m_Log;
        private bool m_MaterialsUpdated = false;
        private float m_UpdateTimer = 0f;
        private const float UPDATE_INTERVAL = 2f;
        private const bool ENABLE_LOGGING = false; // Disabled due to logging framework issues

        protected override void OnCreate()
        {
            base.OnCreate();
            if (ENABLE_LOGGING)
            {
                try
                {
                    m_Log = LogManager.GetLogger($"{nameof(Red_bike_path)}.{nameof(MaterialColorSystem)}");
                }
                catch
                {
                    m_Log = null;
                }
            }
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

        private void SafeLog(string message)
        {
            if (!ENABLE_LOGGING) return;
            
            try
            {
                if (m_Log != null && !string.IsNullOrEmpty(message))
                {
                    m_Log.Info(message);
                }
            }
            catch
            {
                // Silently fail if logging crashes
            }
        }

        private void SafeLogError(string message)
        {
            if (!ENABLE_LOGGING) return;
            
            try
            {
                if (m_Log != null && !string.IsNullOrEmpty(message))
                {
                    m_Log.Error(message);
                }
            }
            catch
            {
                // Silently fail if logging crashes
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
                
                // Use Lerp to blend from white to the target color based on intensity
                // This replaces the color instead of multiplying it with the existing green
                Color finalColor = Color.Lerp(Color.white, targetColor, intensity);
                
                var allMaterials = Resources.FindObjectsOfTypeAll<Material>();
                int updatedCount = 0;
                int skippedCount = 0;
                int totalChecked = 0;

                // Beim ersten Durchlauf alle relevanten Materialien loggen
                if (!m_MaterialsUpdated)
                {
                    SafeLog("=== Scanning for bike path materials ===");
                }

                foreach (var material in allMaterials)
                {
                    if (material == null || material.name == null)
                        continue;

                    string matName = material.name.ToLower();
                    
                    // FIRST: Check if this should be excluded (vehicles, buildings, etc.)
                    if (ShouldExcludeMaterial(matName, material))
                    {
                        continue;
                    }
                    
                    if (IsBikePathMaterial(matName))
                    {
                        totalChecked++;
                        
                        // Log alle gefundenen Materialien beim ersten Scan
                        if (!m_MaterialsUpdated)
                        {
                            try
                            {
                                string shaderName = material.shader != null ? material.shader.name : "null";
                                SafeLog("Found bike material: " + material.name + " (Shader: " + shaderName + ")");
                            }
                            catch
                            {
                                // Skip logging if it fails
                            }
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
                                try
                                {
                                    SafeLog("Skipped: " + material.name + " - Reason: " + skipReason);
                                }
                                catch
                                {
                                    // Skip logging if it fails
                                }
                            }
                            skippedCount++;
                            continue;
                        }

                        bool updated = false;
                        if (material.HasProperty("_BaseColor"))
                        {
                            material.SetColor("_BaseColor", finalColor);
                            updated = true;
                        }
                        if (material.HasProperty("_Color"))
                        {
                            material.SetColor("_Color", finalColor);
                            updated = true;
                        }
                        if (material.HasProperty("_MainColor"))
                        {
                            material.SetColor("_MainColor", finalColor);
                            updated = true;
                        }
                        if (material.HasProperty("_TintColor"))
                        {
                            material.SetColor("_TintColor", finalColor);
                            updated = true;
                        }
                        if (material.HasProperty("_EmissionColor"))
                        {
                            material.SetColor("_EmissionColor", finalColor * 0.5f);
                            updated = true;
                        }
                        
                        if (updated)
                        {
                            if (!m_MaterialsUpdated)
                            {
                                try
                                {
                                    SafeLog("Updated: " + material.name);
                                }
                                catch
                                {
                                    // Skip logging if it fails
                                }
                            }
                            updatedCount++;
                        }
                    }
                }

                SafeLog("Material scan complete: Checked " + totalChecked + ", Updated " + updatedCount + ", Skipped " + skippedCount);
                
                if (updatedCount > 0 || skippedCount > 0)
                {
                    m_MaterialsUpdated = true;
                }
            }
            catch (System.Exception ex)
            {
                SafeLogError("Error updating bike path materials: " + ex.Message);
            }
        }

        private bool ShouldExcludeMaterial(string materialName, Material material)
        {
            // Exclude vehicle materials
            string[] vehicleKeywords = new[]
            {
                "vehicle",
                "car",
                "truck",
                "bus",
                "van",
                "automobile",
                "motor",
                "wheel",
                "tire",
                "tyre",
                "chassis",
                "body_",
                "paint_",
                "_lod",
                "trailer"
            };
            
            // Exclude building materials
            string[] buildingKeywords = new[]
            {
                "building",
                "house",
                "wall",
                "roof",
                "window",
                "door",
                "floor",
                "facade",
                "interior",
                "prop",
                "sign",
                "billboard",
                "fence"
            };
            
            // Exclude UI and other non-terrain materials
            string[] otherExclusions = new[]
            {
                "ui_",
                "_ui",
                "icon",
                "cursor",
                "button",
                "menu",
                "particle",
                "effect",
                "water",
                "tree",
                "vegetation"
            };
            
            foreach (var keyword in vehicleKeywords)
            {
                if (materialName.Contains(keyword))
                {
                    return true;
                }
            }
            
            foreach (var keyword in buildingKeywords)
            {
                if (materialName.Contains(keyword))
                {
                    return true;
                }
            }
            
            foreach (var keyword in otherExclusions)
            {
                if (materialName.Contains(keyword))
                {
                    return true;
                }
            }
            
            // Check shader names - only allow terrain/ground/path related shaders
            if (material != null && material.shader != null)
            {
                try
                {
                    string shaderName = material.shader.name.ToLower();
                    
                    // Exclude non-terrain shaders
                    if (shaderName.Contains("standard") && !shaderName.Contains("terrain") && !shaderName.Contains("ground"))
                    {
                        // Standard shader but not for terrain - likely a vehicle/building
                        return true;
                    }
                    
                    if (shaderName.Contains("vehicle") || shaderName.Contains("building") || shaderName.Contains("prop"))
                    {
                        return true;
                    }
                }
                catch
                {
                    // If shader name access fails, exclude to be safe
                    return true;
                }
            }
            
            return false;
        }

        private bool IsBikePathMaterial(string materialName)
        {
            // More specific keywords that are actually used for bike path materials in Cities Skylines 2
            // These should be specific to NETWORK/PATH materials, not generic "bike" references
            string[] bikePathKeywords = new[]
            {
                "bikepath",
                "bike_path",
                "bike-path",
                "bikelane",
                "bike_lane",
                "bike-lane",
                "cycleway",
                "cycle_way",
                "cycle-way",
                "cyclelane",
                "cycle_lane",
                "lane_marking_bike",
                "lane_bike",
                "marking_bike",
                "network_bike",
                "path_bike",
                "surface_bike"
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
            
            // Wenn NUR pedestrian Keywords (kein bike): überspringe auch
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
            SafeLog("MaterialColorSystem destroyed");
        }
    }
}
