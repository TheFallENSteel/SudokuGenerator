using System.Linq;

namespace SudokuGenerator
{
    public static class SudokuLoader
    {
        public static int[] LoadFromString(string source, string? divider)
        {
            if (divider == null)
            {
                return source
                    .ToCharArray()
                    .Where(c => c != ' ')
                    .Select(val =>
                        {
                            return int.Parse(val.ToString());
                        })
                    .ToArray();
            }
            else
            {
                return source
                    .Split(divider)
                    .Select(val =>
                    {
                        return int.Parse(val.Trim());
                    })
                    .ToArray();
            }
        }
    }
}
