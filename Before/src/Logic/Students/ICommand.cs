using CSharpFunctionalExtensions;

namespace Logic.Students
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }
}
