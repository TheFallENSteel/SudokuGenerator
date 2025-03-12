using SudokuGenerator.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SudokuGenerator
{
    public class Program
    {
        public static ProgramState ProgramState { get; } = new ProgramState();
        static void Main()
        {
            QuickStart.Init();
            Console.WriteLine("SudokuCenter 1.0 by Martin Komárek\nUse h or help for list of commands:");
            bool clear = false;
            while (ProgramState.Continue)
            {
                if (!clear) Console.WriteLine("Enter your command:");
                clear = false;
                string command = "";
                try
                {
                    string? fullCommandRaw = Console.ReadLine();
                    if (fullCommandRaw == null || fullCommandRaw.Trim() == "")
                    {
                        clear = true;
                        continue;
                    }
                    if (fullCommandRaw == null) break;
                    List<string> fullCommand = fullCommandRaw.Split().Select(com => com.ToLower()).ToList();
                    ProgramState.CommandContainer.Execute(fullCommand, out string? commandResult);
                    Console.WriteLine(commandResult);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Invalid parameter: '{e.ParamName}' \nin command: '{command}'");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Invalid operation: '{e.Message}'");
                }
            }
        }
        //000060030720000004096704001000090060109008403640005000005209300960007005007800109
        //000060030720000004096704001000090060109008403640005000005209300960007005007800009
    }
}
