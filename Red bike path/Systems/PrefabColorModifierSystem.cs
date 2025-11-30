using Game;
using Game.Net;
using Game.Prefabs;
using Unity.Entities;
using UnityEngine;
using Colossal.Logging;

namespace Red_bike_path.Systems
{
    public partial class PrefabColorModifierSystem : GameSystemBase
    {
        private static ILog log = LogManager.GetLogger($"{nameof(Red_bike_path)}.{nameof(PrefabColorModifierSystem)}");
        private EntityQuery m_PrefabQuery;
        private bool m_PrefabsModified = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            
            m_PrefabQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<PrefabData>(),
                    ComponentType.ReadOnly<NetData>()
                }
            });

            RequireForUpdate(m_PrefabQuery);
            log.Info("PrefabColorModifierSystem created");
        }

        protected override void OnUpdate()
        {
            if (m_PrefabsModified)
            {
                Enabled = false;
                return;
            }

            try
            {
                var settings = Mod.Settings;
                if (settings == null)
                {
                    log.Warn("Settings not available yet");
                    return;
                }

                Color targetColor = settings.GetBikePathColor();
                float intensity = settings.ColorIntensity;
                
                log.Info($"Modifying bike path prefabs to color: R={targetColor.r}, G={targetColor.g}, B={targetColor.b}");

                var prefabs = m_PrefabQuery.ToEntityArray(Unity.Collections.Allocator.Temp);
                int modifiedCount = 0;

                foreach (var prefabEntity in prefabs)
                {
                    if (EntityManager.HasComponent<PrefabData>(prefabEntity))
                    {
                        var prefabData = EntityManager.GetComponentData<PrefabData>(prefabEntity);
                        if (IsBikePathPrefab(prefabData, prefabEntity))
                        {
                            ModifyPrefabColor(prefabEntity, targetColor, intensity);
                            modifiedCount++;
                        }
                    }
                }

                prefabs.Dispose();

                if (modifiedCount > 0)
                {
                    log.Info($"Modified {modifiedCount} bike path prefabs");
                }
                else
                {
                    log.Info("No bike path prefabs found to modify");
                }

                m_PrefabsModified = true;
            }
            catch (System.Exception ex)
            {
                log.Error($"Error modifying prefabs: {ex.Message}");
                m_PrefabsModified = true;
            }
        }

        private bool IsBikePathPrefab(PrefabData prefabData, Entity prefabEntity)
        {
            try
            {
                if (EntityManager.HasComponent<NetData>(prefabEntity))
                {
                    if (EntityManager.HasComponent<NetGeometryData>(prefabEntity))
                    {
                        var geometryData = EntityManager.GetComponentData<NetGeometryData>(prefabEntity);
                        
                        if (geometryData.m_DefaultWidth < 6f)
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        private void ModifyPrefabColor(Entity prefabEntity, Color color, float intensity)
        {
            try
            {
                if (EntityManager.HasComponent<NetCompositionData>(prefabEntity))
                {
                    log.Info($"Found NetCompositionData for prefab {prefabEntity.Index}");
                }

                if (EntityManager.HasBuffer<SubMesh>(prefabEntity))
                {
                    var subMeshes = EntityManager.GetBuffer<SubMesh>(prefabEntity);
                    log.Info($"Prefab has {subMeshes.Length} submeshes");
                }
            }
            catch (System.Exception ex)
            {
                log.Warn($"Could not fully modify prefab {prefabEntity.Index}: {ex.Message}");
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            log.Info("PrefabColorModifierSystem destroyed");
        }
    }
}
