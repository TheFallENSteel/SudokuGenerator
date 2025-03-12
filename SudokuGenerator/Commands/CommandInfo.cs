using SudokuGenerator.Args;

namespace SudokuGenerator.Commands
{
    public class CommandInfo
    {
        public readonly CommandArgsInfo CommandArgsInfo;

        private readonly string name;
        private readonly string shortDescription;
        private readonly string longDescription;

        public CommandInfo(string name, string shortDescription, string longDescription, CommandArgsInfo commandArgsInfo)
        {
            this.CommandArgsInfo = commandArgsInfo;
            this.name = name;
            this.shortDescription = shortDescription;
            this.longDescription = longDescription;
        }


        public string Name => name;
        public string ShortParameterDescription => $"{Name}: {CommandArgsInfo.GetShortInfo()}- {shortDescription}";
        public string ShortDescription => $"{Name} - {shortDescription}";
        public string FullDescription => $"{ShortDescription} \n{longDescription}\n";
    }
}
