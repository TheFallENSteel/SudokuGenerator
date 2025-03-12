using System.Linq;

namespace SudokuGenerator.Args
{
    public class CommandArgsInfo
    {
        private ParameterInfo[] ParametersHelp { get; }

        public CommandArgsInfo(ParameterInfo[] parametersHelp)
        {
            ParametersHelp = parametersHelp;
        }
        public string GetShortInfo() => string.Concat(ParametersHelp.Select(param => param.Name + " "));
        public string GetHelpString(int index)
        {
            if (ParametersHelp.Length > index)
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
