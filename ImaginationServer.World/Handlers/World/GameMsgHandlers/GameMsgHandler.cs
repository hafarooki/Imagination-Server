using System.IO;

namespace ImaginationServer.World.Handlers.World.GameMsgHandlers
{
    public abstract class GameMsgHandler
    {
        public abstract void Handle(long objectId, string flags, BinaryReader reader);
    }
}