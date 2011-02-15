using System;

using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    /// <summary>
    /// A helper class to construct add messages,
    /// do not create instances of this class!
    /// </summary>
    public class AddPhysicalObject : IMessage
    {
        public static IMessage CreateMessage(PhysicalObject po)
        {
            if (po is Equipable)
                return new AddEquipable((Equipable)po);
            if (po is Junk)
                return new AddJunk((Junk)po);
            if (po is Mobile)
                return new AddMobile((Mobile)po);
            if (po is Quaffable)
                return new AddQuaffable((Quaffable)po);
            if (po is Readable)
                return new AddReadable((Readable)po);
            if (po is Terrain)
                return new AddTerrain((Terrain)po);
            if (po is Wieldable)
                return new AddWieldable((Wieldable)po);

            throw new Exception("AddPhysicalObject of unknown type " + po.GetType());
        }

        public static PhysicalObject GetPhysicalObject(IMessage message)
        {
            if (message is AddEquipable)
                return ((AddEquipable)message).Equipable;
            if (message is AddJunk)
                return ((AddJunk)message).Junk;
            if (message is AddMobile)
                return ((AddMobile)message).Mobile;
            if (message is AddQuaffable)
                return ((AddQuaffable)message).Quaffable;
            if (message is AddReadable)
                return ((AddReadable)message).Readable;
            if (message is AddTerrain)
                return ((AddTerrain)message).Terrain;
            if (message is AddWieldable)
                return ((AddWieldable)message).Weildable;
            
            throw new Exception("Unknown AddPhysicalObject message type " + message.GetType());
        }
    }
}
