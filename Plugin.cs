using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using StopWatchPluginAPI;
using UnityEngine;
using Utilla;

namespace GorillaStopWatch
{
    [BepInDependency("arielthemonke.gorillastats")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.ProjectName, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance { get; private set; }
        public List<IStopWatchPlugin> plugins { get; private set; } = new List<IStopWatchPlugin>();

        void Start()
        {
            GorillaTagger.OnPlayerSpawned(Init);

        }

        void Init()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            instance = this;
            LoadPlugins();
        }

        void LoadPlugins()
        {
            string pluginsPath = Path.Combine(Paths.PluginPath, "StopWatchPlugins");

            if (!Directory.Exists(pluginsPath))
                Directory.CreateDirectory(pluginsPath);

            foreach (var dll in Directory.GetFiles(pluginsPath, "*.dll"))
            {
                try
                {
                    var asm = Assembly.LoadFile(dll);
                    foreach (var type in asm.GetTypes())
                    {
                        if (typeof(IStopWatchPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            IStopWatchPlugin plugin = (IStopWatchPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugin);
                            plugin.OnAwake();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to load plugin from {dll}: {ex.Message}");
                }
            }
        }

        public void RunEvent(string eventName)
        {
            if (plugins == null || plugins.Count == 0)
            {
                Logger.LogWarning("No plugins loaded to handle the event.");
                return;
            }

            foreach (var plugin in plugins)
            {
                try
                {
                    switch (eventName)
                    {
                        case "Start":
                            plugin.OnStart();
                            break;
                        case "Stop":
                            plugin.OnStop();
                            break;
                        default:
                            Logger.LogWarning($"Unknown event: {eventName}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to run event for plugin {plugin.GetType().Name}: {ex.Message}");
                }
            }
        }
    }
}
