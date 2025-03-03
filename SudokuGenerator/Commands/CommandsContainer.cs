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
        Dictionary<string, Command> commandDictionary = new Dictionary<string, Command>();
        public bool IsPositionalCommandCommand(List<string> fullCommand, out string? returnValue) 
        {
            returnValue = null;
            return false;
        }
        public bool Execute(string commandString, List<string> rawArgs, out string? returnValue) 
        {
            bool success = false;
            commandDictionary.TryGetValue(commandString, out Command? command);
            if (command == null) 
            {
                returnValue = null;
                return false;
            }
            try
            {
                returnValue = command.Execute(rawArgs, out success);
            }
            catch (Exception ex) 
            {
                returnValue = ex.Message;
                success = false;
            }
            return success;
        }
        public bool GetCommand(string commandString, out Command? command)
        {
            return commandDictionary.TryGetValue(commandString, out command);
        }
        public void AddCommand(Command command) 
        {
            foreach (var alias in command.Aliases)
            {
                if(!commandDictionary.TryAdd(alias, command)) 
                {
                    throw new Exception($"Alias '{alias}' already used by another command '{commandDictionary[alias]}'!");
                }
            }
        }
    }
}
