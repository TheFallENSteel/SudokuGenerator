using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public class CommandInfo
    {
        private readonly string name;
        private readonly string shortDescription;
        private readonly string longDescription;

        public CommandInfo(string name, string shortDescription, string longDescription)
        {
            this.name = name;
            this.shortDescription = shortDescription;
            this.longDescription = longDescription;
        }

        string Name => name; 
        string ShortDescription => shortDescription; 
        string FullDescription => $"{Name} - {ShortDescription}\n {longDescription}";
    }
}
