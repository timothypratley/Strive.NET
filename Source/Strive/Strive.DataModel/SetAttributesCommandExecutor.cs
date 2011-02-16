using Ncqrs.Commanding.CommandExecution;

namespace Strive.DataModel
{
    class SetAttributesCommandExecutor : CommandExecutorBase<SetAttributesCommand>
    {
        protected override void ExecuteInContext(Ncqrs.Domain.IUnitOfWorkContext context, SetAttributesCommand command)
        {
            var x = new PhysicalObject("foo");

            context.Accept();
        }
    }
}
