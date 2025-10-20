using BepInEx;
using DrakiaXYZ.BigBrain.Brains;
using EFT;
using HarmonyLib;
using SAIN;
using SAIN.Attributes;
using SAIN.Helpers;
using SAIN.Layers;
using SAIN.Layers.Combat;
using SAIN.Preset;
using SAIN.Preset.BotSettings;
using SAIN.Preset.BotSettings.SAINSettings;
using SAIN.Preset.GlobalSettings.Categories;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static HBAO_Core;

namespace TacticalToasterUNTARGH.Interop
{
    internal class SAINInterop
    {
        public void Init()
        {
            Plugin.LogSource.LogInfo("Initializing SAIN interop...");
            AddSAINLayers();
            CreateCustomBotTypes();
        }

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

        public static void AddSAINLayers()
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
        }

        private static FieldInfo[] GetFieldsInType(Type type, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
        {
            return type.GetFields(flags);
        }

        private static void CopyValuesAtoB(object A, object B, Func<FieldInfo, bool> shouldCopyFieldFunc = null)
        {
            List<string> ACatNames = AccessTools.GetFieldNames(A);
            foreach (FieldInfo BCatField in GetFieldsInType(B.GetType()))
            {
                if (ACatNames.Contains(BCatField.Name))
                {
                    object BCatObject = BCatField.GetValue(B);
                    FieldInfo[] BVariableFieldArray = GetFieldsInType(BCatField.FieldType);

                    FieldInfo ACatField = AccessTools.Field(A.GetType(), BCatField.Name);
                    if (ACatField != null)
                    {
                        object ACatObject = ACatField.GetValue(A);
                        List<string> AVariableNames = AccessTools.GetFieldNames(ACatObject);

                        foreach (FieldInfo BVariableField in BVariableFieldArray)
                        {
                            if (shouldCopyFieldFunc == null || shouldCopyFieldFunc(BVariableField))
                            {
                                FieldInfo AVariableField = AccessTools.Field(ACatField.FieldType, BVariableField.Name);
                                if (AVariableField != null)
                                {
                                    object AValue = AVariableField.GetValue(ACatObject);
                                    BVariableField.SetValue(BCatObject, AValue);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool ShallUseEFTBotDefault(FieldInfo field) => AttributesGUI.GetAttributeInfo(field)?.CopyValue == true;

        private static void UpdateSAINSettingsToEFTDefault(WildSpawnType wildSpawnType, SAINSettingsGroupClass sainSettingsGroup)
        {
            var Preset = SAINPresetClass.Instance;
            var botSettings = Preset.BotSettings;

            foreach (var keyPair in sainSettingsGroup.Settings)
            {
                SAINSettingsClass sainSettings = keyPair.Value;
                BotDifficulty Difficulty = keyPair.Key;

                object eftSettings = botSettings.GetEFTSettings(wildSpawnType, Difficulty);
                if (eftSettings != null)
                {
                    CopyValuesAtoB(eftSettings, sainSettings, (field) => ShallUseEFTBotDefault(field));
                }
            }
        }

        public static void CreateCustomBotTypes()
        {
            Plugin.LogSource.LogInfo("Creating custom bot types for SAIN...");

            List<BotType> botTypes = new List<BotType>();

            foreach (var customBot in BotAPIEnums.BotAPITypes)
            {
                string name = Regex.Replace(customBot.typeName, @"([a-z])([A-Z])", "$1 $2");
                name = char.ToUpper(name[0]) + name.Substring(1);

                string description = $"A custom bot of type {name}.";

                if (customBot.typeName.Contains("boss")) description = $"A custom boss bot of type {name}.";
                if (customBot.typeName.Contains("follower")) description = $"A custom follower bot of type {name}.";

                var botType = new BotType()
                {
                    Name = name,
                    Description = description,
                    Section = "Custom",
                    WildSpawnType = (WildSpawnType)customBot.wildSpawnType,
                    BaseBrain = "BotAPI"
                };
                botTypes.Add(botType);
            }

            var Preset = SAINPresetClass.Instance;
            var botSettings = Preset.BotSettings;
            BotDifficulty[] Difficulties = [BotDifficulty.easy, BotDifficulty.normal, BotDifficulty.hard, BotDifficulty.impossible];

            foreach (var botType in botTypes)
            {
                BotTypeDefinitions.BotTypesList.Add(botType);
                BotTypeDefinitions.BotTypes.Add(botType.WildSpawnType, botType);
                BotTypeDefinitions.BotTypesNames.Add(botType.Name);

                SAINBotSettingsClass.DefaultDifficultyModifier.Add(botType.WildSpawnType, 0.5f);

                var wildSpawnType = botType.WildSpawnType;
                var name = botType.Name;

                SAINSettingsGroupClass sainSettingsGroup;
                if (Preset.Info.IsCustom == false || !SAINPresetClass.Import(out sainSettingsGroup, Preset.Info.Name, name, "BotSettings"))
                {
                    sainSettingsGroup = new SAINSettingsGroupClass(Difficulties)
                    {
                        Name = name,
                        WildSpawnType = wildSpawnType,
                        DifficultyModifier = SAINBotSettingsClass.DefaultDifficultyModifier[wildSpawnType]
                    };

                    UpdateSAINSettingsToEFTDefault(wildSpawnType, sainSettingsGroup);

                    if (Preset.Info.IsCustom == true)
                    {
                        SAINPresetClass.Export(sainSettingsGroup, Preset.Info.Name, name, "BotSettings");
                    }
                }

                botSettings.SAINSettings.Add(wildSpawnType, sainSettingsGroup);

                Plugin.LogSource.LogInfo($"Added SAIN BotType: {botType.Name} with WildSpawnType {botType.WildSpawnType}");
            }
        }
    }
}