namespace NewJersey.Engine.Commands;

public class CommandsManager : IUpdate
{
    private readonly Queue<BaseCommand> _logicCommands = new();
    private readonly Queue<BaseCommand> _graphicsCommands = new();

    private readonly HashSet<BaseCommand> _commands = new();

    public int LogicCommandsLength => _logicCommands.Count;
    public int GraphicsCommandsLength => _graphicsCommands.Count;

    private const uint MaxLogicCommandsPerFrame = 512;
    private const uint MaxGraphicsCommandsPerFrame = 60;

    public void Enqueue(BaseCommand baseCommand)
    {
        if (_commands.Contains(baseCommand))
        {
            return;
        }

        switch (baseCommand.Type)
        {
            case BaseCommand.CommandType.Logic:
                _logicCommands.Enqueue(baseCommand);
                return;
            case BaseCommand.CommandType.Graphics:
                _graphicsCommands.Enqueue(baseCommand);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _commands.Add(baseCommand);
    }

    public void Update()
    {
        if (_logicCommands.Count > 0)
        {
            for (var idx = 0; idx < MaxLogicCommandsPerFrame; idx++)
            {
                if (_logicCommands.Count == 0)
                {
                    break;
                }

                var command = _logicCommands.Dequeue();
                command.Execute(Engine.Instance);

                _commands.Remove(command);
            }
        }


        if (_graphicsCommands.Count > 0)
        {
            
            for (var idx = 0; idx < MaxGraphicsCommandsPerFrame; idx++)
            {
                if (_graphicsCommands.Count == 0)
                {
                    break;
                }

                var command = _graphicsCommands.Dequeue();
                command.Execute(Engine.Instance);

                _commands.Remove(command);
            }
        }
    }
}