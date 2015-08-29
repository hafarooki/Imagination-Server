using System;

namespace ImaginationServer.Common
{
    [Flags]
    public enum ServerId
    {
        Auth = 0x01,
        Chat = 0x02,
        World = 0x04
    }
}