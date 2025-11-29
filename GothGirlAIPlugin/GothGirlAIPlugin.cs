using Exiled.API.Enums;
using Exiled.API.Features;
using GothGirlAIPlugin.Utils;

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

        public override void OnEnabled()
        {
            GameManager.SetAnswerCallback(IntercomGameUI.EnqueueIntercomMessage);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            GameManager.SetAnswerCallback(null);

            base.OnDisabled();
        }
    }
}
