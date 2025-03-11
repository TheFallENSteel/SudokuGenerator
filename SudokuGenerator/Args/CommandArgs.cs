using PdfSharp;
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
        public static List<string> ProcessArgs(List<string> args) => args.Select(arg => arg.Trim().Trim('"').Trim('\'')).ToList();
        public static string ParseArg(List<string> args, int index, string defaultValue = "") 
            => args.Count > index ? args[index] : defaultValue;
        public static int ParseArg(List<string> args, int index, int defaultValue = 1) 
        {
            if (args.Count <= index) return defaultValue;
            if (!int.TryParse(args[index], out int returnValue)) throw new ArgumentException($"Invalid Difficulty argument at index: {index}.");
            return returnValue;
        }
        public static EnumType ParseArg<EnumType>(List<string> args, int index, EnumType defaultValue) where EnumType : Enum
        {
            if (args.Count <= index) return defaultValue;
            if (!Enum.TryParse(typeof(EnumType), args[index], true, out object? possiblePageSize)) throw new ArgumentException("Invalid PageSize argument", nameof(EnumType));
            return (EnumType)possiblePageSize;
        }
        public static bool ParseArg(List<string> args, int index, bool defaultValue = false)
        {
            if (args.Count <= index) return defaultValue;
            switch (args[index])
            {
                case "true": case "t": case "1":
                    return true;
                default:
                    return false;
            }
        }
    }
}
