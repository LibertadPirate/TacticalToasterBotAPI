using BepInEx.Logging;
using EFT;
using Mono.Cecil;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
namespace TacticalToasterUNTARGH.Prepatches;
public static class WildSpawnTypePatch
{
    public static IEnumerable<string> TargetDLLs { get; } = new[] { "Assembly-CSharp.dll" };
    public static List<int> suitableList;
    public static void Patch(ref AssemblyDefinition assembly)
    {
        if (!ShouldPatchAssembly())
        {
            Logger.CreateLogSource("UNTAR Go Home Prepatch")
            .LogWarning("UNTAR Go Home plugin not detected, not patching assembly. Make sure you have installed or uninstalled the mod correctly.");
            return;
        }
        suitableList = new List<int>();
        string dbTypesPath = GetDbTypesPath();
        if (string.IsNullOrEmpty(dbTypesPath) || !Directory.Exists(dbTypesPath))
        {
            return;
        }
        string[] jsonFiles = Directory.GetFiles(dbTypesPath, "*.json");
        if (jsonFiles.Length == 0)
        {
            return;
        }
        int brain = (int)WildSpawnType.exUsec;
        int currentIndex = 0;
        foreach (string file in jsonFiles)
        {
            string typeName = Path.GetFileNameWithoutExtension(file);
            string scavRole = typeName.ToUpper();
            BotAPIEnums.AddBot(typeName, brain, scavRole, suitableList);
            currentIndex++;
        }
        var wildSpawnType = assembly.MainModule.GetType("EFT.WildSpawnType");
        foreach (var botType in BotAPIEnums.BotAPITypes)
        {
            Utils.AddEnumValue(ref wildSpawnType, botType.typeName, botType.wildSpawnType);
        }
    }
    public static bool IsCustom(this WildSpawnType role)
    {
        return BotAPIEnums.BotAPITypesDict.ContainsKey((int)role);
    }
    private static bool ShouldPatchAssembly()
    {
        var patcherLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var bepDir = Directory.GetParent(patcherLoc);
        var sptRoot = Directory.GetParent(bepDir.FullName);
        var userMods = Path.Combine(sptRoot.FullName, "user", "mods");
        var modFolder = Path.Combine(userMods, "TacticalToaster-UNTARGH");
        return Directory.Exists(modFolder);
    }
    private static string GetDbTypesPath()
    {
        var patcherLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var bepDir = Directory.GetParent(patcherLoc);
        var sptRoot = Directory.GetParent(bepDir.FullName);
        var userMods = Path.Combine(sptRoot.FullName, "user", "mods");
        var modFolder = Path.Combine(userMods, "TacticalToaster-UNTARGH");
        return Path.Combine(modFolder, "db", "types");
    }
}