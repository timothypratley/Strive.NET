using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Strive.Network.Client;

namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public event CollectionChangeEventHandler CollectionChanged;

        public ServerConnection connection;
        public InputBindings bindings;

        /*
        private static EnumSkill currentGameCommand = EnumSkill.None;
        public static EnumSkill CurrentGameCommand
        {
            get
            {
                return currentGameCommand;
            }
            set
            {
                currentGameCommand = value;
                ITexture texture = resources.GetCursor((int)currentGameCommand);
                CurrentWorld.RenderingScene.SetCursor(texture);
            }
        }
         */

        public WorldViewModel(ServerConnection connection)
        {
            bindings = new InputBindings();
            this.connection = connection;
            //connection.OnPositionSent += PhysicalObjects_CollectionChanged;
        }

        void PhysicalObjects_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            /*
            PhysicalObject po = (PhysicalObject)e.Element;
            Vector loc = Helper.GetPointForYardSpot(equipment.BlockID, equipment.RowIndex, equipment.StackIndex);
            OnCollectionChanged(
                new CollectionChangeEventArgs(
                    e.Action,
                    new ViewEntity(po.ID,
                        1,
                        (float)loc.X,
                        (float)loc.Y,
                        0,
                        (float)loc.X,
                        (float)loc.Y,
                        0)));
             */
        }
    }
}
