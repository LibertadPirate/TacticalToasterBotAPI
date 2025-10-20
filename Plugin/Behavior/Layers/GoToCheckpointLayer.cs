using DrakiaXYZ.BigBrain.Brains;
using EFT;
using EFT.Quests;
using TacticalToasterUNTARGH.Behavior.Actions;
using UnityEngine;
using Comfort.Common;
using System.Linq;

namespace TacticalToasterUNTARGH.Behavior.Layers
{
    internal class GoToCheckpointLayer : CustomLayer
    {
        public static float innerRadius = 10f;
        public static float outerRadius = 45f;
        public static Vector3 patrolPoint = new Vector3(-140f, -1f, 410f);
        public bool isInside = false;
        public bool hasRun = false;
        public float runTime;
        public GoToCheckpointLayer(BotOwner botOwner, int priority) : base(botOwner, priority)
        {
        }
        public override void Start()
        {
            isInside = BotOwner.Position.SqrDistance(patrolPoint) <= (outerRadius * outerRadius);
            hasRun = true;
            runTime = Time.time + 2f;
            base.Start();
        }
        public override void Stop()
        {
            isInside = BotOwner.Position.SqrDistance(patrolPoint) <= (outerRadius * outerRadius);
            hasRun = false;
            base.Stop();
        }
        public override string GetName()
        {
            return "GoToCheckpoint";
        }
        public override Action GetNextAction()
        {
            return new Action(typeof(GoToCheckpointAction), "ToCheckpoint");
        }
        public override bool IsActive()
        {
            if (!IsQuestConditionMet()) return false;
            if (BotOwner.Position.SqrDistance(patrolPoint) <= (outerRadius * outerRadius))
            {
                if (!isInside && !hasRun)
                {
                    runTime = Time.time + 2f;
                }
            }
            return (!isInside && BotOwner.Position.SqrDistance(patrolPoint) > (innerRadius * innerRadius) || runTime > Time.time) || BotOwner.Position.SqrDistance(patrolPoint) > (outerRadius * outerRadius);
        }
        public override bool IsCurrentActionEnding()
        {
            if (CurrentAction?.Type == typeof(GoToCheckpointAction))
            {
                return false;
            }
            return true;
        }
        private bool IsQuestConditionMet()
        {
            if (!Plugin.EnableQuestCondition.Value) return true;
            var mainPlayer = Singleton<GameWorld>.Instance?.MainPlayer;
            if (mainPlayer == null) return false;
            var quest = mainPlayer.Profile.QuestsData.FirstOrDefault(q => q.Id == Plugin.QuestId.Value);
            if (quest == null) return false;
            bool started = quest.Status == EQuestStatus.Started;
            bool completed = quest.Status == EQuestStatus.Success;
            return (Plugin.OnQuestStart.Value && started) || (Plugin.OnQuestComplete.Value && completed);
        }
    }
}