using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Args
{
    public class CommandArgsInfo
    {
        private ParameterInfo[] ParametersHelp { get; }

        public CommandArgsInfo(ParameterInfo[] parametersHelp)
        {
            ParametersHelp = parametersHelp;
        }

        public string GetHelpString(int index)
        {
            if (ParametersHelp.Length < index)
            {
                return ParametersHelp[index].FullDescription;
            }
            else
            {
                return $"No parameter at index {index}, max index for {this.GetType().Name} is {ParametersHelp.Length}!";
            }
        }
    }
}
