using System;
using Comfort.Common;
using EFT;
namespace TacticalToasterUNTARGH.Behavior.Brains
{
    public class BotAPIBrain : BaseBrain
    {
        public BotAPIBrain(BotOwner owner, bool withPursuit)
        : base(owner)
        {
            GClass45 gclass = new GClass45(owner, 80);
            base.method_0(5, gclass, true);
            GClass115 gclass2 = new GClass115(owner, 78, 100f);
            base.method_0(12, gclass2, true);
            GClass81 gclass3 = new GClass81(owner, 70);
            base.method_0(9, gclass3, true);
            GClass139 gclass4 = new GClass139(owner, 60);
            base.method_0(1, gclass4, true);
            GClass39 gclass5 = new GClass39(owner, 50);
            base.method_0(6, gclass5, true);
            GClass136 gclass6 = new GClass136(owner, 30, false, CoverLevel.Lay, false);
            base.method_0(4, gclass6, true);
            if (withPursuit)
            {
                Class104 @class = new Class104(owner, 25, true);
                base.method_0(13, @class, true);
            }
            GClass159 gclass7 = new GClass159(owner, 15);
            base.method_0(14, gclass7, true);
            GClass97 class2 = new GClass97(owner, 9);
            base.method_0(3, class2, true);
            GClass129 gclass8 = new GClass129(owner, 3);
            base.method_0(8, gclass8, true);
            GClass83 gclass9 = new GClass83(owner, 2); // PatrolFollower
            base.method_0(7, gclass9, true);
            GClass130 gclass10 = new GClass130(owner, 0); // PatrolAssault (for the leader of group)
            base.method_0(2, gclass10, true);
            if (this.Owner.Boss.IamBoss)
            {
                Singleton<BotEventHandler>.Instance.OnKill += this.method_6;
            }
        }
        public override GClass671 EventsPriority()
        {
            return new GClass671(-1, 75, 55, 76, 56, -1);
        }
        public void method_6(IPlayer killer, IPlayer target)
        {
            if (this.Owner.Boss.IamBoss && Singleton<GameWorld>.Instance.GetAlivePlayerByProfileID(target.ProfileId) == this.Owner.GetPlayer)
            {
                foreach (Player player in Singleton<GameWorld>.Instance.allAlivePlayersByID.Values)
                {
                    if (!player.AIData.IsAI)
                    {
                        this.Owner.BotsGroup.AddEnemy(player, EBotEnemyCause.pmcBossKill);
                    }
                }
                this.Owner.BotsGroup.SetAggressiveToAllNewPlayers(true);
                Singleton<BotEventHandler>.Instance.OnKill -= this.method_6;
            }
        }
        public override string ShortName()
        {
            return "BotAPI";
        }
        [NonSerialized]
        public const int int_0 = 1;
        [NonSerialized]
        public const int int_1 = 2;
        [NonSerialized]
        public const int int_2 = 3;
        [NonSerialized]
        public const int int_3 = 4;
        [NonSerialized]
        public const int int_4 = 5;
        [NonSerialized]
        public const int int_5 = 6;
        [NonSerialized]
        public const int int_6 = 7;
        [NonSerialized]
        public const int int_7 = 8;
        [NonSerialized]
        public const int int_8 = 9;
        [NonSerialized]
        public const int int_9 = 12;
        [NonSerialized]
        public const int int_10 = 13;
        [NonSerialized]
        public const int int_11 = 14;
    }
}