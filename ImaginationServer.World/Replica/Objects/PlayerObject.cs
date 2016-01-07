using ImaginationServer.World.Replica.Components;

namespace ImaginationServer.World.Replica.Objects
{
    public class PlayerObject : ReplicaObject
    {
        public PlayerObject(long objectId, string name) : base(WorldServer.Server)
        {
            ObjectId = objectId;
            Name = name;
            Lot = 1;

            Components.Add(new ControllablePhysicsComponent());
            Components.Add(new DestructibleComponent());
            Components.Add(new CharacterComponent());
            Components.Add(new InventoryComponent());
            Components.Add(new SkillComponent());
            Components.Add(new RenderComponent());
            Components.Add(new Index36Component());
        }
    }
}