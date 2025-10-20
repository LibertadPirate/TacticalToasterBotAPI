using BepInEx.Logging;
using Comfort.Common;
using EFT;
using EFT.InputSystem;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DrakiaXYZ.BigBrain;
using TacticalToasterUNTARGH.Behavior;
using TacticalToasterUNTARGH.Behavior.Brains;
using static AITaskManager;
namespace TacticalToasterUNTARGH.Patches
{
    public class BotAPIRolePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(StandartBotBrain), nameof(StandartBotBrain.Activate));
        }
        private static BaseBrain GetBotAPIBrain(BotOwner botOwner)
        {
            return new BotAPIBrain(botOwner, false);
        }
        private static Dictionary<BotLogicDecision, BotNodeAbstractClass> CreateAllNodes(BotOwner botOwner)
        {
            // Use the existing ActionsList method from BotActionNodesClass
            return BotActionNodesClass.ActionsList(botOwner);
        }

        private static AICoreAgentClass<BotLogicDecision> GetBotAPIAgent(BotOwner botOwner, BaseBrain brain, StandartBotBrain __instance)
        {
            var name = botOwner.name + " " + botOwner.Profile.Info.Settings.Role.ToString();
            // Replace BotActionNodesClass.CreateAllNodes with the new local CreateAllNodes method
            return new AICoreAgentClass<BotLogicDecision>(botOwner.BotsController.AICoreController, brain, CreateAllNodes(botOwner), botOwner.gameObject, name, new Func<BotLogicDecision, BotNodeAbstractClass>(__instance.method_0));
        }
        [PatchPostfix]
        [HarmonyPriority(Priority.First)] // Make sure this runs after BigBrain so we can override it
        public static void PatchPostfix(StandartBotBrain __instance, BotOwner ___botOwner_0)
        {
            if (BotAPIEnums.BotAPITypesDict.ContainsKey((int)___botOwner_0.Profile.Info.Settings.Role))
            {
                Plugin.LogSource.LogMessage("Inserting our BotAPI brain. How exciting!");
                __instance.BaseBrain = GetBotAPIBrain(___botOwner_0);
                __instance.Agent = GetBotAPIAgent(___botOwner_0, __instance.BaseBrain, __instance);
            }
        }
    }
}