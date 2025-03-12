using SudokuGenerator.Args;
using System.Collections.Generic;

namespace SudokuGenerator.Commands
{
    public static class QuickStart
    {
        private const string Name = "quickStart";
        private const string ShortDescription = "Quick tutorial with SudokuSolver program.";
        private const string LongDescription =
            "Welcome to the Sudoku Solver app. " +
            "\nThis program was made by Martin Komárek as a learning project. " +
            "\nThe main purpose of this program is to randomly generate sudoku" +
            "\nand than print it for user to solve on paper. You can generate " +
            "\nas many sudokus as you wish and select your skill to prevent   " +
            "\ngenerating sudoku that you cannot solve. You can also set paper" +
            "\nformat and how many sudokus should be fit on one page allowing " +
            "\nyou to choose the best fit for you." +
            "\n\n";
        public static CommandInfo CommandInfo = new CommandInfo(Name, ShortDescription, LongDescription + Program.ProgramState.CommandContainer.ListAllCommands() + "\n", EmptyArgs.CommandArgsInfo);

        public static readonly string[] Aliases = ["quickstart", "tutorial"];

        public static void Init()
        {
            Program.ProgramState.CommandContainer.AddCommand(new Command(Execute, CommandInfo, Aliases));
        }
        public static void Update()
        {
            CommandInfo = new CommandInfo(
                Name,
                ShortDescription,
                LongDescription + Program.ProgramState.CommandContainer.ListAllCommands() + "\n",
                EmptyArgs.CommandArgsInfo);
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            success = true;
            Update();
            return CommandInfo.FullDescription;
        }
    }
}
