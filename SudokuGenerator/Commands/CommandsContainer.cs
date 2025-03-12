using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuGenerator.Commands
{
    public class CommandsContainer
    {
        readonly Dictionary<string, Command> commandDictionary = new Dictionary<string, Command>();
        readonly Dictionary<string, PositionalCommand> positionalCommandDictionary = new Dictionary<string, PositionalCommand>();
        public bool IsPositionalCommandCommand(List<string> fullCommand, out string? returnValue)
        {
            returnValue = null;
            return false;
        }
        public string ListAllCommands()
        {
            return "List of all the commands:\n"
                + string.Concat(
                    commandDictionary.Values
                    .Distinct()
                    .Select(comm => comm.CommandInfo.ShortParameterDescription + "\n"))
                + "Positional commands:\n"
                + string.Concat(
                    positionalCommandDictionary.Values
                    .Distinct()
                    .Select(comm => PositionalCommand.PositionalCommandPrefix + comm.CommandInfo.ShortParameterDescription + "\n"));
        }
        //string.Concat(commandDictionary.Values.Distinct().Select(comm => comm.CommandInfo.ShortParameterDescription + "\n"))
        public bool Execute(List<string> fullCommand, out string? returnValue)
        {
            bool success = false;
            returnValue = null;
            if (fullCommand.Count == 0)
            {
                returnValue = "No command entered!";
                return success;
            }
            //No need to check the first command, as positional commands cannot be first
            if (fullCommand.Count > 0) TryExecutePositionalCommand(fullCommand, out returnValue, out success);
            if (success) return success;
            string commandString = fullCommand[0];
            fullCommand.RemoveAt(0);
            TryExecuteCommand(fullCommand, commandString, out returnValue, out success);
            return success;
        }

        private bool TryExecutePositionalCommand(List<string> fullCommand, out string? returnValue, out bool success)
        {
            success = false;
            returnValue = null;

            for (int i = 0; i < fullCommand.Count; i++)
            {
                var args = fullCommand[i];
                if (args.Length != 0
                    && args[0] == PositionalCommand.PositionalCommandPrefix
                    && positionalCommandDictionary.TryGetValue(args.TrimStart(PositionalCommand.PositionalCommandPrefix), out PositionalCommand? command))
                {
                    returnValue = command.ExecutePositional(fullCommand, out success);
                    break;
                }
            }
            return success;
        }

        private bool TryExecuteCommand(List<string> commandArgs, string commandString, out string? returnValue, out bool success)
        {
            success = false;
            commandDictionary.TryGetValue(commandString, out Command? command);
            if (command == null)
            {
                returnValue = $"Command '{commandString}' not found!";
                return success;
            }
            try
            {
                returnValue = command.Execute(commandArgs, out success);
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }
            return success;
        }

        public bool GetCommand(string? commandString, out Command command)
        {
            if (commandString == null)
            {
                command = null;
                return false;
            }
            return commandDictionary.TryGetValue(commandString, out command);
        }
        public void AddCommand(Command command)
        {
            foreach (var alias in command.Aliases)
            {
                if (!commandDictionary.TryAdd(alias, command))
                {
                    throw new Exception($"Alias '{alias}' already used in {nameof(commandDictionary)}. Command '{commandDictionary[alias]}'!");
                }
            }
        }
        public void AddCommand(PositionalCommand positionalCommand)
        {
            foreach (var alias in positionalCommand.Aliases)
            {
                if (!positionalCommandDictionary.TryAdd(alias, positionalCommand))
                {
                    throw new Exception($"Alias '{alias}' already used in {nameof(positionalCommandDictionary)}. Command '{positionalCommandDictionary[alias]}'!");
                }
            }
        }
        public void AddCommand<TCommand>(TCommand command, Dictionary<string, TCommand> commandDict) where TCommand : Command
        {
            foreach (var alias in command.Aliases)
            {
                if (!commandDict.TryAdd(alias, command))
                {
                    throw new Exception($"Alias '{alias}' already used in {nameof(commandDict)}. Command '{commandDictionary[alias]}'!");
                }
            }
        }
    }
}
