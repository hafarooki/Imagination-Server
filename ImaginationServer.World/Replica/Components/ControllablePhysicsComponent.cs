using System;

namespace ImaginationServer.World.Replica.Components
{
    public class ControllablePhysicsComponent : ReplicaComponent
    {
        public bool Flag3 { get; set; }
        public Data3 Data3 { get; set; }
        public bool Flag4 { get; set; }
        public Data4 Data4 { get; set; }
        public bool Flag5 { get; set; }
        public Data5 Data5 { get; set; }
        public bool Flag6 { get; set; }
        public Data6 Data6 { get; set; }

        #region Serialization only

        public bool Flag7 { get; set; }

        #endregion

        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            if (type == ReplicaPacketType.Construction)
            {
                bitStream.Write(Flag1);
                if (Flag1)
                {
                    bitStream.Write(Data1.D1);
                    bitStream.Write(Data1.D2);
                    bitStream.Write(Data1.D3);
                }

                bitStream.Write(Flag2);
                if (Flag2)
                {
                    bitStream.Write(Data2.D1);
                    bitStream.Write(Data2.D2);
                    bitStream.Write(Data2.D3);
                    bitStream.Write(Data2.D4);
                    bitStream.Write(Data2.D5);
                    bitStream.Write(Data2.D6);
                    bitStream.Write(Data2.D7);
                }
            }

            bitStream.Write(Flag3);
            if (Flag3)
            {
                bitStream.Write(Data3.D1);
                bitStream.Write(Data3.D2);
            }

            bitStream.Write(Flag4);
            if (Flag4)
            {
                bitStream.Write(Data4.D1);
                bitStream.Write(Data4.D2);
            }

            bitStream.Write(Flag5);
            if (Flag5)
            {
                bitStream.Write(Data5.Flag);
                if (Data5.Flag)
                {
                    bitStream.Write(Data5.D1);
                    bitStream.Write(Data5.D2);
                }
            }

            bitStream.Write(Flag6);
            if (Flag6)
            {
                bitStream.Write(Data6.PosX);
                bitStream.Write(Data6.PosY);
                bitStream.Write(Data6.PosZ);

                bitStream.Write(Data6.RotationX);
                bitStream.Write(Data6.RotationY);
                bitStream.Write(Data6.RotationZ);
                bitStream.Write(Data6.RotationW);

                bitStream.Write(Data6.IsOnGround);
                bitStream.Write(Data6.Unknown1);

                bitStream.Write(Data6.VelocityFlag);
                if (Data6.VelocityFlag)
                {
                    bitStream.Write(Data6.VelocityX);
                    bitStream.Write(Data6.VelocityY);
                    bitStream.Write(Data6.VelocityZ);
                }

                bitStream.Write(Data6.AngularVelocityFlag);
                if (Data6.AngularVelocityFlag)
                {
                    bitStream.Write(Data6.AngularVelocityX);
                    bitStream.Write(Data6.AngularVelocityY);
                    bitStream.Write(Data6.AngularVelocityZ);
                }

                bitStream.Write(Data6.MovingPlatformFlag);
                if (Data6.MovingPlatformFlag)
                {
                    bitStream.Write(Data6.MpUnknown1);
                    bitStream.Write(Data6.MpUnknown2);
                    bitStream.Write(Data6.MpUnknown3);
                    bitStream.Write(Data6.MpUnknown4);
                    bitStream.Write(Data6.MpFlag1);
                    if (Data6.MpFlag1)
                    {
                        bitStream.Write(Data6.MpUnknownD1);
                        bitStream.Write(Data6.MpUnknownD2);
                        bitStream.Write(Data6.MpUnknownD3);
                    }
                }
            }

            if (type == ReplicaPacketType.Serialization)
            {
                bitStream.Write(Flag7);
            }
        }

        public override uint GetComponentId()
        {
            return 1;
        }

        #region Creation only

        public bool Flag1 { get; set; }
        public Data1 Data1 { get; set; }
        public bool Flag2 { get; set; }
        public Data2 Data2 { get; set; }

        #endregion
    }

    public struct Data1
    {
        public uint D1 { get; set; }
        public bool D2 { get; set; }
        public bool D3 { get; set; }
    }

    public struct Data2
    {
        public uint D1 { get; set; }
        public uint D2 { get; set; }
        public uint D3 { get; set; }
        public uint D4 { get; set; }
        public uint D5 { get; set; }
        public uint D6 { get; set; }
        public uint D7 { get; set; }
    }

    public struct Data3
    {
        public float D1 { get; set; }
        public float D2 { get; set; }
    }

    public struct Data4
    {
        public uint D1 { get; set; }
        public bool D2 { get; set; }
    }

    public struct Data5
    {
        public bool Flag { get; set; }
        public uint D1 { get; set; }
        public bool D2 { get; set; }
    }

    public struct Data6
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float RotationW { get; set; }
        public bool IsOnGround { get; set; }
        public bool Unknown1 { get; set; }
        public bool VelocityFlag { get; set; }

        #region Velocity

        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float VelocityZ { get; set; }

        #endregion

        public bool AngularVelocityFlag { get; set; }

        #region Angular Velocity

        public float AngularVelocityX { get; set; }
        public float AngularVelocityY { get; set; }
        public float AngularVelocityZ { get; set; }

        #endregion

        public bool MovingPlatformFlag { get; set; }

        #region Moving Platform

        public long MpUnknown1 { get; set; }
        public float MpUnknown2 { get; set; }
        public float MpUnknown3 { get; set; }
        public float MpUnknown4 { get; set; }
        public bool MpFlag1 { get; set; }

        #region Flag data

        public float MpUnknownD1 { get; set; }
        public float MpUnknownD2 { get; set; }
        public float MpUnknownD3 { get; set; }

        #endregion

        #endregion
    }
}