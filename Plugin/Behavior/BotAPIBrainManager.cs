using DrakiaXYZ.BigBrain.Brains;
using EFT;
using System.Collections.Generic;
using TacticalToasterUNTARGH.Behavior.Layers;
namespace TacticalToasterUNTARGH.Behavior
{
    public static class BotAPIBrainManager
    {
        private static readonly string[] commonVanillaLayersToRemove = new string[]
        {
"Help",
"AdvAssaultTarget",
"Hit",
"Simple Target",
"Pmc",
"AssaultHaveEnemy",
"Assault Building",
"Enemy Building",
"PushAndSup",
"Pursuit",
        };
        public static void AddBotAPIBrainLayers()
        {
            var botBrainList = new List<string>() { "BotAPI" };
            var botTypes = BotAPIEnums.BotAPITypes.ConvertAll<WildSpawnType>(type => (WildSpawnType)type.wildSpawnType);
            var layers = new List<string>()
{
"Request",
"KnightFight",
"PmcBear",
"PmcUsec",
};
            layers.AddRange(commonVanillaLayersToRemove);
            BrainManager.RemoveLayers(layers, botBrainList, botTypes);
            BrainManager.AddCustomLayer(typeof(GoToCheckpointLayer), botBrainList, 4, botTypes);
            Plugin.LogSource.LogMessage("Removed common vanilla layers from BotAPI brains.");
        }
    }
}