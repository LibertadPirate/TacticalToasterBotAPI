using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Linq;
using System.Reflection;
using TacticalToasterUNTARGH.Prepatches;
using UnityEngine;
namespace TacticalToasterUNTARGH.Patches;
[HarmonyPatch]
internal class BotAPISettingsPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
    typeof(GClass598).GetMethod("smethod_1", BindingFlags.Static | BindingFlags.Public);
    [PatchPrefix]
    private static bool Smethod1Prefix(BotDifficulty d, WildSpawnType role, bool external, ref BotSettingsComponents __result)
    {
        if (BotAPIEnums.BotAPITypesDict.ContainsKey((int)role))
        {
            if (Plugin.botText != null)
            {
                // Provide all required arguments for BotSettingsComponents.Create
                // You may need to adjust how pvp/pve are extracted from botText.text if needed
                __result = BotSettingsComponents.Create(Plugin.botText.text, Plugin.botText.text, d, role);
                return false;
            }
            else
            {
                Debug.LogError($"Failed to load custom bot settings text asset.");
            }
        }
        return true;
    }
}