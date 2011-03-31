using Ncqrs.Commanding.CommandExecution;

namespace Strive.DataModel
{
    class CommandExecutorCreateJunk : CommandExecutorBase<CommandCreateJunk>
    {
        protected override void ExecuteInContext(Ncqrs.Domain.IUnitOfWorkContext context, CommandCreateJunk command)
        {
            var x = new PhysicalObject(command.Name);

            context.Accept();
        }
    }
}
