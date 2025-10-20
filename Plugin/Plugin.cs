using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using EFT;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TacticalToasterUNTARGH.Interop;
using TacticalToasterUNTARGH.Patches;
using UnityEngine;
using BepInEx.Configuration;
using DrakiaXYZ.BigBrain.Brains;
using TacticalToasterUNTARGH.Behavior.Brains;
using TacticalToasterUNTARGH.Behavior;

namespace TacticalToasterUNTARGH;
[BepInDependency("xyz.drakia.bigbrain", "1.3.2")]
[BepInDependency("me.sol.sain", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(ClientInfo.UNTARGHGUID, ClientInfo.UNTARGHPluginName, ClientInfo.UNTARGHVersion)]
public class Plugin : BaseUnityPlugin
{
    public static ManualLogSource LogSource;
    public static string pluginPath = Path.Combine(Environment.CurrentDirectory, "BepInEx", "plugins", "UNTARGH");
    public static string resourcePath = Path.Combine(pluginPath, "Resources");
    public static string botJsonPath = Path.Combine(pluginPath, "normalUNTARSettings.json");
    internal static TextAsset botText;
    public static ConfigEntry<bool> EnableQuestCondition { get; private set; }
    public static ConfigEntry<string> QuestId { get; private set; }
    public static ConfigEntry<bool> OnQuestStart { get; private set; }
    public static ConfigEntry<bool> OnQuestComplete { get; private set; }
    private void Awake()
    {
        LogSource = Logger;
        LogSource.LogInfo("plugin loaded!");
        EnableQuestCondition = Config.Bind("Quest Condition", "Enable Quest Condition", true, "Tie bot behavior activation to quest status");
        QuestId = Config.Bind("Quest Condition", "Quest ID", "default_quest_id", "The ID of the quest to check");
        OnQuestStart = Config.Bind("Quest Condition", "On Start", true, "Activate when quest is started");
        OnQuestComplete = Config.Bind("Quest Condition", "On Complete", false, "Activate when quest is completed");
        FieldInfo excludedDifficultiesField = typeof(GClass598).GetField("ExcludedDifficulties", BindingFlags.Static | BindingFlags.Public) ?? throw new InvalidOperationException("ExcludedDifficulties field not found.");
        var excludedDifficulties = (Dictionary<WildSpawnType, List<BotDifficulty>>)excludedDifficultiesField.GetValue(null);
        var botDifficulties = new List<BotDifficulty> {
BotDifficulty.easy,
BotDifficulty.hard,
BotDifficulty.impossible
};
        foreach (var botType in BotAPIEnums.BotAPITypes)
        {
            if (!excludedDifficulties.ContainsKey((WildSpawnType)botType.wildSpawnType))
            {
                excludedDifficulties.Add((WildSpawnType)botType.wildSpawnType, botDifficulties);
                Logger.LogInfo($"Successfully added {botType.typeName} to the excluded difficulties list");
            }
            Traverse.Create(typeof(BotSettingsRepoClass)).Field<Dictionary<WildSpawnType, GClass790>>("dictionary_0").Value.Add((WildSpawnType)botType.wildSpawnType, new GClass790(true, true, false, $"ScavRole/{botType.scavRole}", (ETagStatus)0));
        }
        LoadBotSettings();
        BotAPIBrainManager.AddBotAPIBrainLayers();
        bool sainLoaded = Chainloader.PluginInfos.ContainsKey("me.sol.sain");
        if (sainLoaded)
        {
            Logger.LogMessage("SAIN detected, initializing SAIN interop.");
            new SAINInterop().Init();
        }
        else
        {
            Logger.LogMessage("SAIN not detected, skipping SAIN interop.");
        }
        new TarkovInitPatch().Enable();
        new BotAPIRolePatch().Enable();
        new BotAPIControllerPatch().Enable();
        new BotAPIShootGroundWarnPatch().Enable();
        new BotAPIFenceLoyaltyPatch().Enable();
        new BotBrainPatch().Enable();
    }
    public static void LoadBotSettings()
    {
        if (File.Exists(botJsonPath))
        {
            try
            {
                string typesDir = Path.Combine(pluginPath, "db", "types");
                if (Directory.Exists(typesDir))
                {
                    foreach (string file in Directory.GetFiles(typesDir, "*.json"))
                    {
                        // Load and process each as botText or store in dict for multi-bot handling
                    }
                }
            }
            catch (Exception ex)
            {
                LogSource.LogError($"Error loading bot settings from {botJsonPath}: {ex.Message}");
            }
        }
        else
        {
            LogSource.LogError($"Bot settings file not found at {botJsonPath}");
        }
    }
}