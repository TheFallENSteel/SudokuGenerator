using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public interface ICommand
    {
        public string[] Aliases { get; }
        public bool IsAlias(string alias);
        public string? Execute(List<string> commandArgs, int index, out bool success);
    }
}
