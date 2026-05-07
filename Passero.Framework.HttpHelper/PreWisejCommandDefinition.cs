namespace Passero.Framework.HttpHelper
{
    public sealed class PreWisejCommandDefinition
    {
        public string Name { get; set; } = string.Empty;

        public bool RequiresHeaders { get; set; }

        public bool RequiresQueryString { get; set; }

        public bool RequiresBody { get; set; }
    }
}