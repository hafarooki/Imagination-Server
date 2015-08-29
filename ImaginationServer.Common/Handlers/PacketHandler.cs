using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationServer.Common.Handlers
{
    public abstract class PacketHandler
    {
        public abstract void Handle(byte[] data, string address);
    }
}
