using SudokuGenerator.Args;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string commandString = fullCommand[0];
            fullCommand.RemoveAt(0);
            TryExecutePositionalCommand(fullCommand, out returnValue, out success);
            if (success) return success;
            TryExecuteCommand(fullCommand, commandString, out returnValue, out success);
            return success;
        }

        private bool TryExecutePositionalCommand(List<string> fullCommand, out string? returnValue, out bool success)
        {
            success = false;
            returnValue = null;

            foreach (var args in fullCommand)
            {
                if (args.Length != 0 
                    && args[0] == '-' 
                    && positionalCommandDictionary.TryGetValue(args.TrimStart('-'), out PositionalCommand? command))
                {
                    returnValue = command.Execute(fullCommand, out success);
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

        public bool GetCommand(string commandString, out Command? command)
        {
            return commandDictionary.TryGetValue(commandString, out command);
        }
        public void AddCommand(Command command) 
        {
            AddCommand<Command>(command, commandDictionary);
        }
        public void AddPositionalCommand(PositionalCommand positionalCommand)
        {
            AddCommand<PositionalCommand>(positionalCommand, positionalCommandDictionary);
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
