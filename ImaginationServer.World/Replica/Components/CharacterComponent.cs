namespace ImaginationServer.World.Replica.Components
{
    public class CharacterComponent : ReplicaComponent
    {
        public Index1 IndexOne { get; set; }
        public Index2 IndexTwo { get; set; }
        public Index3 IndexThree { get; set; }
        public Index4 IndexFour { get; set; }

        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            // index 1
            bitStream.Write(IndexOne.Flag);
            if (IndexOne.Flag)
            {
                bitStream.Write(IndexOne.Data.Flag);
                if (IndexOne.Data.Flag) bitStream.Write(IndexOne.Data.VehicleObjectId);
                bitStream.Write(IndexOne.Data.Unknown);
            }

            // Index 2
            bitStream.Write(IndexTwo.Flag);
            if(IndexTwo.Flag) bitStream.Write(IndexTwo.Level);

            // Index 3
            bitStream.Write(IndexThree.Flag);
            if (IndexThree.Flag)
            {
                bitStream.Write(IndexThree.Unknown1);
                bitStream.Write(IndexThree.Unknown2);
            }

            // Index 4
            if (type == ReplicaPacketType.Construction)
            {
                bitStream.Write(IndexFour.Flag1);
                bitStream.Write(IndexFour.Unknown1);
                bitStream.Write(IndexFour.Flag2);
                bitStream.Write(IndexFour.Unknown2);
                bitStream.Write(IndexFour.Flag3);
                bitStream.Write(IndexFour.Unknown3);
                bitStream.Write(IndexFour.Flag4);
                bitStream.Write(IndexFour.Unknown4);

                bitStream.Write(IndexFour.HairColor);
                bitStream.Write(IndexFour.HairStyle);
                bitStream.Write(IndexFour.Unknown5);
                bitStream.Write(IndexFour.ShirtColor);
                bitStream.Write(IndexFour.PantsColor);
                bitStream.Write(IndexFour.Unknown6);
                bitStream.Write(IndexFour.Unknown7);
                bitStream.Write(IndexFour.EyebrowsStyle);
                bitStream.Write(IndexFour.EyesStyle);
                bitStream.Write(IndexFour.MouthStyle);
                bitStream.Write(IndexFour.AccountId);
                bitStream.Write(IndexFour.Llog);
                bitStream.Write(IndexFour.Unknown8);
                bitStream.Write(IndexFour.LegoScore);
                bitStream.Write(IndexFour.FreeToPlay);
                // TODO: Write stats instead of this

                for (var i = 0; i < 27; i++)
                {
                    bitStream.Write((ulong)0);
                }

                bitStream.Write(IndexFour.Flag6);
                bitStream.Write(IndexFour.Flag7);
                if (IndexFour.Flag7) bitStream.WriteWString(IndexFour.LdfText, false, false);
            }

            bitStream.Write(false);
            bitStream.Write(false);
            bitStream.Write(false);
        }

        public override uint GetComponentId()
        {
            return 4;
        }

        public struct Index1
        {
            public bool Flag { get; set; }
            public Data1 Data { get; set; }
            public struct Data1
            {
                public bool Flag { get; set; }
                public long VehicleObjectId { get; set; }
                public ushort Unknown { get; set; }
            }
        }

        public struct Index2
        {
            public bool Flag { get; set; }
            public uint Level { get; set; }
        }

        public struct Index3
        {
            public bool Flag { get; set; }
            public bool Unknown1 { get; set; }
            public bool Unknown2 { get; set; }
        }

        public struct Index4
        {
            public bool Flag1 { get; set; }
            public ulong Unknown1 { get; set; }
            public bool Flag2 { get; set; }
            public ulong Unknown2 { get; set; }
            public bool Flag3 { get; set; }
            public ulong Unknown3 { get; set; }
            public bool Flag4 { get; set; }
            public ulong Unknown4 { get; set; }

            public uint HairColor { get; set; }
            public uint HairStyle { get; set; }
            public uint Unknown5 { get; set; }
            public uint ShirtColor { get; set; }
            public uint PantsColor { get; set; }
            public uint Unknown6 { get; set; }
            public uint Unknown7 { get; set; }
            public uint EyebrowsStyle { get; set; }
            public uint EyesStyle { get; set; }
            public uint MouthStyle { get; set; }
            public ulong AccountId { get; set; }
            public ulong Llog { get; set; }
            public uint Unknown8 { get; set; }
            public uint LegoScore { get; set; }
            public bool FreeToPlay { get; set; }

            #region stats
            public ulong CurrencyCollected { get; set; }
            public ulong BricksCollected { get; set; }
            public ulong SmashablesSmashed { get; set; }
            public ulong QuickBuildsCompleted { get; set; }
            public ulong EnemiesSmashed { get; set; }
            public ulong RocketsUsed { get; set; }
            public ulong MissionsCompleted { get; set; }
            public ulong PetsTamed { get; set; }
            public ulong ImaginationPowersUpCollected { get; set; }
            public ulong LifePowerUpsCollected { get; set; }
            public ulong ArmorPowerUpsCollected { get; set; }
            public ulong TotalDistanceTravelled { get; set; }
            public ulong TimesSmashed { get; set; }
            public ulong TotalDamageTaken { get; set; }
            public ulong TotalDamageHealed { get; set; }
            public ulong TotalArmorRepaired { get; set; }
            public ulong TotalImaginationRestored { get; set; }
            public ulong TotalImaginationUsed { get; set; }
            public ulong TotalDistanceDriven { get; set; }
            public ulong TotalTimeAirborneInRacecar { get; set; }
            public ulong RacingImaginationPowerupsCollected { get; set; }
            public ulong RacingImaginationCratesSmashed { get; set; }
            public ulong TimesRacecarBoostActived { get; set; }
            public ulong WrecksInRacecar { get; set; }
            public ulong RacingSmashablesSmashed { get; set; }
            public ulong RacesFinished { get; set; }
            public ulong FirstPlaceRaceFinishes { get; set; }

            #endregion

            public bool Flag6 { get; set; }
            public bool Flag7 { get; set; }
            public ushort CharacterCount { get; set; }
            public string LdfText { get; set; }
        }
    }
}