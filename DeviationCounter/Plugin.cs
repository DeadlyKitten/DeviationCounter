using System.Linq;
using IPA;
using UnityEngine;
using UnityEngine.SceneManagement;
using CountersPlus.Custom;
using IPALogger = IPA.Logging.Logger;

namespace DeviationCounter
{
    public class Plugin : IBeatSaberPlugin
    {
        DeviationCounter _counter;

        public void Init(IPALogger logger)
        {
            Logger.logger = logger;
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
            CustomCounterCreator.Create(counter);
        }
    }
}
