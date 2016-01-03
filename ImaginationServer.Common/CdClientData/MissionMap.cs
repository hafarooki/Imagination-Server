using FluentNHibernate.Mapping;

namespace ImaginationServer.Common.CdClientData
{
    public class MissionMap : ClassMap<Mission>
    {
        public MissionMap()
        {
            Table("Missions");

            Id(x => x.Id, "id");

            Map(x => x.DefinedType, "defined_type");
            Map(x => x.DefinedSubtype, "defined_subtype");
            Map(x => x.UiSortOrder, "UISortOrder");
            Map(x => x.OfferObjectId, "offer_objectID");
            Map(x => x.TargetObjectId, "target_objectID");
            Map(x => x.RewardCurrency, "reward_currency");
            Map(x => x.LegoScore, "LegoScore");
            Map(x => x.RewardReputation, "reward_reputation");
            Map(x => x.IsChoiceReward, "isChoiceReward");
            Map(x => x.RewardItem1, "reward_item1");
            Map(x => x.RewardItem1Count, "reward_item1_count");
            Map(x => x.RewardItem2, "reward_item2");
            Map(x => x.RewardItem2Count, "reward_item2_count");
            Map(x => x.RewardItem3, "reward_item3");
            Map(x => x.RewardItem3Count, "reward_item3_count");
            Map(x => x.RewardItem4, "reward_item4");
            Map(x => x.RewardItem4Count, "reward_item4_count");
            Map(x => x.RewardEmote, "reward_emote");
            Map(x => x.RewardEmote2, "reward_emote2");
            Map(x => x.RewardEmote3, "reward_emote3");
            Map(x => x.RewardEmote4, "reward_emote4");
            Map(x => x.RewardMaxImagination, "reward_maximagination");
            Map(x => x.RewardMaxHealth, "reward_maxhealth");
            Map(x => x.RewardMaxInventory, "reward_maxinventory");
            Map(x => x.RewardMaxModel, "reward_maxmodel");
            Map(x => x.RewardMaxWidget, "reward_maxwidget");
            Map(x => x.RewardMaxWallet, "reward_maxwallet");
            Map(x => x.Repeatable, "repeatable");
            Map(x => x.RewardCurrencyRepeatable, "reward_currency_repeatable");
            Map(x => x.RewardItem1Repeatable, "reward_item1_repeatable");
            Map(x => x.RewardItem1RepeatCount, "reward_item1_repeat_count");
            Map(x => x.RewardItem2Repeatable, "reward_item2_repeatable");
            Map(x => x.RewardItem2RepeatCount, "reward_item2_repeat_count");
            Map(x => x.RewardItem3Repeatable, "reward_item3_repeatable");
            Map(x => x.RewardItem3RepeatCount, "reward_item3_repeat_count");
            Map(x => x.RewardItem4Repeatable, "reward_item4_repeatable");
            Map(x => x.RewardItem4RepeatCount, "reward_item4_repeat_count");
            Map(x => x.TimeLimit, "time_limit");
            Map(x => x.IsMission, "isMission");
            Map(x => x.MissionIconId, "missionIconID");
            Map(x => x.PrereqMissionId, "prereqMissionID");
            Map(x => x.Localize, "localize");
            Map(x => x.InMotd, "inMOTD");
            Map(x => x.CooldownTime, "cooldownTime");
            Map(x => x.IsRandom, "isRandom");
            Map(x => x.RandomPool, "randomPool");
            Map(x => x.UiPrereqId, "UIPrereqId");
            Map(x => x.GateVersion, "gate_version");
            Map(x => x.HudStates, "HUDStates");
            Map(x => x.LocStatus, "locStatus");
            Map(x => x.RewardBankInventory, "reward_bankinventory");
        }
    }
}