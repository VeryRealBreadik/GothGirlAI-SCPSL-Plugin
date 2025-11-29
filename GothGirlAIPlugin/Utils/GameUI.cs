using ExiledBroadcast = Exiled.API.Features.Broadcast;
using ExiledMap = Exiled.API.Features.Map;

namespace GothGirlAIPlugin.Utils
{
    public static class GameUI
    {
        public static void BroadcastAll(string message)
        {
            ExiledBroadcast broadcast = new()
            {
                Duration = 5,
                Content = message
            };
            ExiledMap.Broadcast(broadcast);
        }
    }
}
