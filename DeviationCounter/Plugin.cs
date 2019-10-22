using System.Linq;
using IPA;
using UnityEngine;
using UnityEngine.SceneManagement;
using CountersPlus.Custom;
using IPALogger = IPA.Logging.Logger;
using Harmony;
using System;
using System.Reflection;

namespace DeviationCounter
{
    public class Plugin : IBeatSaberPlugin
    {
        DeviationCounter _counter;
        BS_Utils.Utilities.Config _config;

        internal static bool useMaxDeviation;
        internal static int maxDeviation;

        public void Init(IPALogger logger)
        {
            Logger.logger = logger;
            _config = new BS_Utils.Utilities.Config("Deviation Counter");
            LoadConfig();
        }

        public void OnApplicationStart()
        {
            Logger.Log("Checking for Counters+");
            if (IPA.Loader.PluginManager.AllPlugins.Any(x => x.Metadata.Id == "Counters+"))
            {
                Logger.Log("Counters+ is installed");
                AddCustomCounter();
            }
            else
                Logger.Log("Counters+ not installed");

            if (useMaxDeviation)
            {
                Logger.Log("Applying Harmony Patches", Logger.LogLevel.Notice);
                try
                {
                    var harmony = HarmonyInstance.Create("com.steven.beatsaber.deviation.counter");
                    harmony.PatchAll(Assembly.GetExecutingAssembly());

                    Logger.Log("Patched successfully");
                }
                catch (Exception e)
                {
                    Logger.Log($"{e.Message}\n{e.StackTrace}",
                        Logger.LogLevel.Error,
                        "This plugin requires Harmony. Make sure you installed the plugin " +
                        "properly, as the Harmony DLL should have been installed with it."
                    );
                }
            }
        }

        public void OnApplicationQuit() { }

        public void OnFixedUpdate() { }

        public void OnUpdate() { }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "GameCore")
            {
                _counter = new GameObject("Deviation Counter").AddComponent<DeviationCounter>();
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) { }

        public void OnSceneUnloaded(Scene scene) { }

        private void AddCustomCounter()
        {
            Logger.Log("Creating Custom Counter");
            CustomCounter counter = new CustomCounter
            {
                SectionName = "deviationCounter",
                Name = "Deviation Counter",
                BSIPAMod = this,
                Counter = "Deviation Counter",
            };

            CustomConfigModel defaults = new CustomConfigModel(counter.Name)
            {
                Enabled = true,
                Position = CountersPlus.Config.ICounterPositions.BelowCombo,
                Distance = 1
            };

            CustomCounterCreator.Create(counter, defaults);
        }

        private void LoadConfig()
        {
            useMaxDeviation = _config.GetBool("Settings", "Use Max Deviation", false, true);
            maxDeviation = _config.GetInt("Settings", "Max Deviation", 15, true);
        }
    }
}
