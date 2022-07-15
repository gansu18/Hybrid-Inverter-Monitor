﻿using InverterMon.Server.InverterService.Commands;
using System.Collections.Concurrent;

namespace InverterMon.Server.InverterService;

public class CommandQueue
{
    public bool IsAcceptingCommands { get; set; } = true;
    public GetStatus StatusCommand { get; } = new();

    private readonly ConcurrentQueue<ICommand> toProcess = new();

    public bool AddCommand(ICommand command)
    {
        if (IsAcceptingCommands)
        {
            toProcess.Enqueue(command);
            return true;
        }
        return false;
    }

    public ICommand? GetCommand()
    {
        return toProcess.TryPeek(out var command) ? command : null;
    }

    public void RemoveCommand()
        => toProcess.TryDequeue(out _);
}
