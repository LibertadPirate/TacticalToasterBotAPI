using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using TacticalToasterUNTARGH.Behavior.Brains;

namespace TacticalToasterUNTARGH.Patches
{
    public class BotBrainPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(StandartBotBrain), nameof(StandartBotBrain.Activate));
        }

        [PatchPostfix]
        private static void Postfix(StandartBotBrain __instance)
        {
            if (__instance.BotOwner_0.Profile.Info.Settings.Role.IsCustomBot())
            {
                __instance.BaseBrain = new BotAPIBrain(__instance.BotOwner_0, true);
            }
        }
    }
}