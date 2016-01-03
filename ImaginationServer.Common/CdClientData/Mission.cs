namespace ImaginationServer.Common.CdClientData
{
    public class Mission
    {
        public virtual int Id { get; set; }
        public virtual string DefinedType { get; set; }
        public virtual string DefinedSubtype { get; set; }
        public virtual int UiSortOrder { get; set; }
        public virtual int OfferObjectId { get; set; }
        public virtual int TargetObjectId { get; set; }
        public virtual long RewardCurrency { get; set; }
        public virtual int LegoScore { get; set; }
        public virtual long RewardReputation { get; set; }
        public virtual bool IsChoiceReward { get; set; }
        public virtual int RewardItem1 { get; set; }
        public virtual int RewardItem1Count { get; set; }
        public virtual int RewardItem2 { get; set; }
        public virtual int RewardItem2Count { get; set; }
        public virtual int RewardItem3 { get; set; }
        public virtual int RewardItem3Count { get; set; }
        public virtual int RewardItem4 { get; set; }
        public virtual int RewardItem4Count { get; set; }
        public virtual int RewardEmote { get; set; }
        public virtual int RewardEmote2 { get; set; }
        public virtual int RewardEmote3 { get; set; }
        public virtual int RewardEmote4 { get; set; }
        public virtual int RewardMaxImagination { get; set; }
        public virtual int RewardMaxHealth { get; set; }
        public virtual int RewardMaxInventory { get; set; }
        public virtual int RewardMaxModel { get; set; }
        public virtual int RewardMaxWidget { get; set; }
        public virtual long RewardMaxWallet { get; set; }
        public virtual bool Repeatable { get; set; }
        public virtual long RewardCurrencyRepeatable { get; set; }
        public virtual int RewardItem1Repeatable { get; set; }
        public virtual int RewardItem1RepeatCount { get; set; }
        public virtual int RewardItem2Repeatable { get; set; }
        public virtual int RewardItem2RepeatCount { get; set; }
        public virtual int RewardItem3Repeatable { get; set; }
        public virtual int RewardItem3RepeatCount { get; set; }
        public virtual int RewardItem4Repeatable { get; set; }
        public virtual int RewardItem4RepeatCount { get; set; }
        public virtual int TimeLimit { get; set; }
        public virtual bool IsMission { get; set; }
        public virtual int MissionIconId { get; set; }
        public virtual string PrereqMissionId { get; set; }
        public virtual bool Localize { get; set; }
        public virtual bool InMotd { get; set; }
        public virtual long CooldownTime { get; set; }
        public virtual bool IsRandom { get; set; }
        public virtual string RandomPool { get; set; }
        public virtual int UiPrereqId { get; set; }
        public virtual string GateVersion { get; set; }
        public virtual string HudStates { get; set; }
        public virtual int LocStatus { get; set; }
        public virtual int RewardBankInventory { get; set; }
    }
}