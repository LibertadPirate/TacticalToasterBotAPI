using System;
using System.Collections.Generic;
using System.Linq;
using EFT;
namespace TacticalToasterUNTARGH
{
    public static class BotAPIEnums
    {
        public const int BotAPIValue = 256;
        public static List<BotEnum> BotAPITypes = new List<BotEnum>();
        public static Dictionary<int, BotEnum> BotAPITypesDict = new Dictionary<int, BotEnum>();
        public static void AddBot(string typeName, int brain, string scavRole, List<int> suitableList)
        {
            int wildSpawnType = suitableList.Count > 0 ? (suitableList.Max() + 1) : BotAPIValue;
            suitableList.Add(wildSpawnType);
            BotEnum newBot = new BotEnum { typeName = typeName, wildSpawnType = wildSpawnType, scavRole = scavRole, brain = brain };
            BotAPITypes.Add(newBot);
            BotAPITypesDict.Add(wildSpawnType, newBot);
        }
        public static bool IsCustomBot(this WildSpawnType role)
        {
            return BotAPITypesDict.ContainsKey((int)role);
        }
    }
    public class BotEnum
    {
        public string typeName;
        public int wildSpawnType;
        public string scavRole;
        public int brain;
    }
}