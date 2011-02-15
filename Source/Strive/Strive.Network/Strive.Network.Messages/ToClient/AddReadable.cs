using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class AddReadable : AddPhysicalObject
    {
        public Readable Readable;

        public AddReadable(Readable r)
        {
            Readable = r;
        }
    }
}
