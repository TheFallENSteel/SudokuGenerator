namespace SudokuGenerator.Args
{
    public class ParameterInfo
    {
        private readonly string name;
        private readonly string typeInfo;
        private readonly string description;

        public string Name => $"[{name}]";
        public string TypeInfo => $"<{typeInfo}>";
        public string ShortDescription => $"{Name} {TypeInfo}";
        public string FullDescription => $"{Name} {TypeInfo} - {description}";
        public ParameterInfo(string name, string typeInfo, string description)
        {
            this.name = name;
            this.description = description;
            this.typeInfo = typeInfo;
        }
    }
}
