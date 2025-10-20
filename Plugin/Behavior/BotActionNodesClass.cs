using System;
using System.Collections.Generic;
using EFT;
using UnityEngine;
namespace TacticalToasterUNTARGH.Behavior;
public class BotActionNodesClass
{
    public static BotNodeAbstractClass CreateNode(BotLogicDecision type, BotOwner bot)
    {
        switch (type)
        {
            case BotLogicDecision.doorOpen:
                return new GClass259(bot);
            case BotLogicDecision.warnPlayer:
                return new GClass289(bot);
            case BotLogicDecision.shootToSmoke:
                return new GClass185(bot);
            case BotLogicDecision.holdPosition:
                return new GClass278(bot);
            case BotLogicDecision.runToCover:
                return new GClass228(bot);
            case BotLogicDecision.attackMoving:
                return new GClass205(bot);
            case BotLogicDecision.attackMovingWithSuppress:
                return new GClass206(bot);
            case BotLogicDecision.shootFromPlace:
                return new GClass276(bot);
            case BotLogicDecision.goToEnemy:
                return new GClass223(bot);
            case BotLogicDecision.heal:
                return new GClass197(bot);
            case BotLogicDecision.goToCoverPoint:
                return new GClass212(bot);
            case BotLogicDecision.repairMalfunction:
                return new GClass273(bot);
            case BotLogicDecision.goToCoverPointTactical:
                return new GClass238(bot);
            case BotLogicDecision.goToPointTactical:
                return new GClass239(bot);
            case BotLogicDecision.lay:
                return new GClass198(bot);
            case BotLogicDecision.search:
                return new GClass235(bot);
            case BotLogicDecision.shootFromCover:
                return new GClass277(bot);
            case BotLogicDecision.dogFight:
                return new GClass203(bot);
            case BotLogicDecision.turnAwayLight:
                return new GClass288(bot);
            case BotLogicDecision.standBy:
                return new GClass282(bot);
            case BotLogicDecision.suppressFire:
                return new GClass281(bot);
            case BotLogicDecision.suppressGrenade:
                return new GClass195(bot);
            case BotLogicDecision.throwGrenadeFromPlace:
                return new GClass287(bot);
            case BotLogicDecision.runAndThrowGrenadeFromPlace:
                return new GClass286(bot);
            case BotLogicDecision.runToEnemy:
                return new GClass227(bot);
            case BotLogicDecision.runToEnemyZigZag:
                return new GClass226(bot);
            case BotLogicDecision.goToEnemyZigZag:
                return new GClass225(bot);
            case BotLogicDecision.goToPoint:
                return new GClass219(bot);
            case BotLogicDecision.panicSitting:
                return new GClass260(bot);
            case BotLogicDecision.runToStationary:
                return new GClass234(bot);
            case BotLogicDecision.shootFromStationary:
                return new GClass280(bot);
            case BotLogicDecision.suppressStationary:
                return new GClass284(bot);
            case BotLogicDecision.healStimulators:
                return new GClass283(bot);
            default:
                throw new NotImplementedException($"Node for {type} not implemented.");
        }
    }
    public static Dictionary<BotLogicDecision, BotNodeAbstractClass> CreateAllNodes(BotOwner bot)
    {
        var dictionary = new Dictionary<BotLogicDecision, BotNodeAbstractClass>();
        smethod_0(dictionary, BotLogicDecision.doorOpen, bot);
        smethod_0(dictionary, BotLogicDecision.warnPlayer, bot);
        smethod_0(dictionary, BotLogicDecision.shootToSmoke, bot);
        smethod_0(dictionary, BotLogicDecision.holdPosition, bot);
        smethod_0(dictionary, BotLogicDecision.runToCover, bot);
        smethod_0(dictionary, BotLogicDecision.attackMoving, bot);
        smethod_0(dictionary, BotLogicDecision.attackMovingWithSuppress, bot);
        smethod_0(dictionary, BotLogicDecision.shootFromPlace, bot);
        smethod_0(dictionary, BotLogicDecision.goToEnemy, bot);
        smethod_0(dictionary, BotLogicDecision.heal, bot);
        smethod_0(dictionary, BotLogicDecision.goToCoverPoint, bot);
        smethod_0(dictionary, BotLogicDecision.repairMalfunction, bot);
        smethod_0(dictionary, BotLogicDecision.goToCoverPointTactical, bot);
        smethod_0(dictionary, BotLogicDecision.goToPointTactical, bot);
        smethod_0(dictionary, BotLogicDecision.lay, bot);
        smethod_0(dictionary, BotLogicDecision.search, bot);
        smethod_0(dictionary, BotLogicDecision.shootFromCover, bot);
        smethod_0(dictionary, BotLogicDecision.dogFight, bot);
        smethod_0(dictionary, BotLogicDecision.turnAwayLight, bot);
        smethod_0(dictionary, BotLogicDecision.standBy, bot);
        smethod_0(dictionary, BotLogicDecision.suppressFire, bot);
        smethod_0(dictionary, BotLogicDecision.suppressGrenade, bot);
        smethod_0(dictionary, BotLogicDecision.runAndThrowGrenadeFromPlace, bot);
        smethod_0(dictionary, BotLogicDecision.throwGrenadeFromPlace, bot);
        smethod_0(dictionary, BotLogicDecision.alternativePatrol, bot);
        smethod_0(dictionary, BotLogicDecision.botDropItem, bot);
        smethod_0(dictionary, BotLogicDecision.goToLootPointNode, bot);
        smethod_0(dictionary, BotLogicDecision.goToExfiltrationPointNode, bot);
        smethod_0(dictionary, BotLogicDecision.botTakeItem, bot);
        smethod_0(dictionary, BotLogicDecision.flashed, bot);
        smethod_0(dictionary, BotLogicDecision.standBy, bot); // Duplicated
        smethod_0(dictionary, BotLogicDecision.turnAwayLight, bot); // Duplicated
        smethod_0(dictionary, BotLogicDecision.leaveMap, bot);
        smethod_0(dictionary, BotLogicDecision.runToCoverZigZag, bot);
        smethod_0(dictionary, BotLogicDecision.summon, bot);
        smethod_0(dictionary, BotLogicDecision.khorovodChristmasEvent, bot);
        smethod_0(dictionary, BotLogicDecision.doGiftChristmasEvent, bot);
        smethod_0(dictionary, BotLogicDecision.goToCoverPointTactical, bot);
        return dictionary;
    }
    public static void smethod_0(Dictionary<BotLogicDecision, BotNodeAbstractClass> dictionary, BotLogicDecision botLogicDecision, BotOwner bot)
    {
        dictionary.Add(botLogicDecision, CreateNode(botLogicDecision, bot));
    }
    [NonSerialized]
    public static HashSet<BotLogicDecision> HashSet_0 = new HashSet<BotLogicDecision>();
}