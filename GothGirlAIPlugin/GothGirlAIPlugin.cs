using Exiled.API.Enums;
using Exiled.API.Features;
using GothGirlAIPlugin.Utils.Games;
using GothGirlAIPlugin.Utils.UI;

namespace GothGirlAIPlugin
{
    public class GothGirlAIPlugin : Plugin<GothGirlAIConfig>
    {
        public override string Name => "АльтушкаAI";
        public override string Author => "Великий и ужасный Breadik";

        private static readonly GothGirlAIPlugin Singleton = new();

        private GothGirlAIPlugin()
        {
        }

        public static GothGirlAIPlugin Instance => Singleton;

        public override PluginPriority Priority { get; } = PluginPriority.Last;
    }
}
