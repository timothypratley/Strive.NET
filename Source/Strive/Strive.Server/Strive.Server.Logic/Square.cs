using System.Collections.Generic;
using System.Linq;
using Strive.Common;
using Strive.Model;
using Strive.Network.Messaging;


namespace Strive.Server.Logic
{
    /// <summary>
    /// A square encompasses all the physical objects and clients
    /// in a discreet area.
    /// The World is split into Squares so that nearby objects and clients
    /// may be referenced with ease.
    /// Squares are used to define a clients world view,
    /// as only neighboring physical objects affect the client.
    /// </summary>
    public class Square
    {
        public const int SquareSize = Constants.ObjectScopeRadius;
        public List<EntityModel> PhysicalObjects = new List<EntityModel>();
        public List<ClientConnection> Clients = new List<ClientConnection>();

        public void Add(EntityModel po)
        {
            PhysicalObjects.Add(po);
            if (po is Avatar)
            {
                var a = (Avatar)po;
                if (a.Client != null)
                {
                    Clients.Add(a.Client);
                }
            }
        }

        public void Remove(EntityModel po)
        {
            PhysicalObjects.Remove(po);
            if (po is Avatar)
            {
                var c = ((Avatar)po).Client;
                if (c != null)
                    Clients.Remove(c);
            }
        }

        public void NotifyClients(object message)
        {
            foreach (ClientConnection c in Clients)
                c.Send(message);
        }

        public void NotifyClientsExcept(object message, ClientConnection client)
        {
            foreach (ClientConnection c in Clients.Where(x => x != client))
                c.Send(message);
        }
    }
}
