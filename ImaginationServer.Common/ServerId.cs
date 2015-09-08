using System;

namespace ImaginationServer.Common
{
    [Flags]
    public enum ServerId : byte
    {
        Auth = 0x01,
        Chat = 0x02,
        World = 0x04,
        Character = 0x08,
        VentureExplorer = 0x16
    }
}