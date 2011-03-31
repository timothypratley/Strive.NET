using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Ncqrs;
using Ncqrs.Commanding.ServiceModel;
using Ncqrs.Eventing.Storage;


namespace Strive.DataModel
{
    public class CommandService
    {
        static CommandService()
        {
            var service = new InProcessCommandService();
            service.RegisterExecutor(new CommandExecutorCreateJunk());
            NcqrsEnvironment.SetDefault<ICommandService>(service);
            var store = new InMemoryEventStore();
            NcqrsEnvironment.SetDefault<IEventStore>(store);

            // TODO: remove this is for testing only
            Execute(new CommandCreateJunk("junk", new Vector3D(1, 2 , 3), Quaternion.Identity));
        }

        public static void Execute(CommandCreateJunk command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }
    }
}
