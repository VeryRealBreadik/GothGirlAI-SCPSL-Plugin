using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using GothGirlAIPlugin.Utils;

namespace GothGirlAIPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class GothGirlAICommand : ICommand
    {
        public string Command { get; } = "waifu";

        public string[] Aliases { get; } = {"girl", "goth"};

        public string Description { get; } = "Весёлая игра 'Угадай число' с альтушкой";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "";
            if (!PlayerIsNearIntercom(Player.Get(sender)))
            {
                response = "Использовать данную команду можно только в интеркоме";
                Log.Info("Пользователь попытался использовать команду не в интеркоме");
                return false;
            }

            GameManager.HandleInput(arguments);
            return true;
        }

        private bool PlayerIsNearIntercom(Player player)
        {
            return player.CurrentRoom == Room.Get(RoomType.EzIntercom);
        }
    }
}
