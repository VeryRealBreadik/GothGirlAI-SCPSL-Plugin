using Exiled.API.Interfaces;

namespace GothGirlAIPlugin
{
    public sealed class GothGirlAIConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
}
