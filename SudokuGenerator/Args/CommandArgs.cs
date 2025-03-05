using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Args
{
    public abstract class CommandArgs
    {
        public abstract void Parse(List<string> args);
    }
}
