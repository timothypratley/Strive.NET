using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	public class AddMobile : AddPhysicalObject {
		public Mobile Mobile;

		public AddMobile(){}
        public AddMobile(Mobile m)
        {
            Mobile = m;
        }
	}
}
