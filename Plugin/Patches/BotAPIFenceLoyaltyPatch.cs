using Comfort.Common;
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
    public class BotAPIFenceLoyaltyPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() =>
        AccessTools.Method(typeof(BotGroupWarnData), nameof(BotGroupWarnData.method_9));
        [PatchPostfix]
        public static void PatchPostfix(ref bool __result, BotGroupWarnData __instance, Player enemyInfo)
        {
            var role = __instance.Boss.Profile.Info.Settings.Role;
            if (role.IsCustomBot())
            {
                __result = false;
            }
        }
    }
}