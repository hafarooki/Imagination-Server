using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ImaginationServer.World.Replica.Components;

namespace ImaginationServer.World.Replica.Objects
{
    public class PlayerObject : ReplicaObject
    {
        public ControllablePhysicsComponent ControllablePhysicsComponent { get; }
        public DestructibleComponent DestructibleComponent { get; }
        public CharacterComponent CharacterComponent { get; }
        public InventoryComponent InventoryComponent { get; }
        public SkillComponent SkillComponent { get; }
        public RenderComponent RenderComponent { get; }
        public Index36Component Index36Component { get; }

        public PlayerObject(long objectId, string name) : base(WorldServer.Server)
        {
            ObjectId = objectId;
            Name = name;
            Lot = 1;

            Components.Add(ControllablePhysicsComponent = new ControllablePhysicsComponent());
            Components.Add(DestructibleComponent = new DestructibleComponent());
            Components.Add(CharacterComponent = new CharacterComponent());
            Components.Add(InventoryComponent = new InventoryComponent());
            Components.Add(SkillComponent = new SkillComponent());
            Components.Add(RenderComponent = new RenderComponent());
            Components.Add(Index36Component = new Index36Component());
        }
    }
}