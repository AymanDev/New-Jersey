namespace NewJersey.Engine.Commands;

public abstract class BaseCommand
{
    public enum CommandType
    {
        Logic,
        Graphics
    }

    public CommandType Type;

    protected BaseCommand(CommandType type)
    {
        Type = type;
    }

    public abstract void Execute(Engine engine);
}