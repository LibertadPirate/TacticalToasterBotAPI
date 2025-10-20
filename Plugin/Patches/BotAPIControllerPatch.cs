using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TacticalToasterUNTARGH.Prepatches;
namespace TacticalToasterUNTARGH.Patches
{
    [HarmonyPatch]
    public class BotAPIControllerPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() =>
        AccessTools.Method(typeof(BotSettingsRepoClass), nameof(BotSettingsRepoClass.Init));
        static bool hasRun = false;
        [PatchPostfix]
        public static void PatchPostfix()
        {
            if (hasRun) return;
            var newList = BotAPIEnums.BotAPITypes.ConvertAll<WildSpawnType>(type => (WildSpawnType)type.wildSpawnType);
            hasRun = true;
            BotSettingsRepoClass.smethod_0(newList);
        }
    }
}