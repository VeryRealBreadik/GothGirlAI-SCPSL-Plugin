using Exiled.API.Features;
using GothGirlAIPlugin.Utils;
using GothGirlAIPlugin.Utils.UI;

namespace GothGirlAIPlugin.Utils.Games
{
    public enum GameState
    {
        Sleeping,
        Choosing,
        Running,
    }

    public static class GameManager
    {
        private static readonly Dictionary<int, IGame> games = new()
        {
            {1, new GuessTheNumberGame() }
        };
        private static IGame? _currentGame = null;
        private static Action<AIMessage>? _answerCallback = GameUI.EnqueueMessage;
        private static GameState _gameState = GameState.Sleeping;

        private static void ShowMenu()
        {
            string menu = string.Join("\n", games.Select(kvp => $"{kvp.Key} - {kvp.Value.Name}"));
            AIMessage message = new()
            {
                IntercomMessage = menu,
            };
            _answerCallback!(message);
        }

        private static void StartGame(int option)
        {
            _currentGame = games[option];
            _currentGame.Start(_answerCallback!);
            Log.Info($"Пользователь выбрал игру {_currentGame.Name}");
        }

        private static void EndGame()
        {
            _currentGame!.End();
            _currentGame = null;
            Log.Info("Игра закончилась");
        }

        private static void CycleGameState()
        {
            _gameState = _gameState.Next();
        }

        public static void HandleInput(IEnumerable<string> arguments)
        {
            if (GameUI.IsTyping)
            {
                return;
            }

            if (_gameState == GameState.Sleeping)
            {
                ShowMenu();
                CycleGameState();
                Log.Info($"Пользователь выбирает игру");
                return;
            }

            string firstArgument = arguments.FirstOrDefault();
            bool isValidArgument = int.TryParse(firstArgument, out int option);
            if (_gameState == GameState.Choosing && !isValidArgument)
            {
                AIMessage message = new()
                {
                    IntercomMessage = "Игры с таким номером не существует.",
                    CASSIEMessage = "", // FIXME
                };
                _answerCallback!(message);
                ShowMenu();
                Log.Info($"Пользователь выбрал несуществующую игру");
            }
            else if (_gameState == GameState.Choosing && isValidArgument)
            {
                bool isGameExists = games.TryGetValue(option, out IGame game);
                if (isGameExists)
                {
                    StartGame(option);
                    CycleGameState();
                }
            }
            else if (_gameState == GameState.Running && _currentGame is not null)
            {
                bool isGameEnded = _currentGame.HandleInput(arguments);
                if (isGameEnded)
                {
                    CycleGameState();
                    EndGame();
                }
                else
                {
                    Log.Info($"Пользователь передал параметры в игру: {string.Join(" ", arguments)}");
                }
            }
            else
            {
                AIMessage message = new()
                {
                    IntercomMessage = "Произошла непредвиденная ошибка...",
                    CASSIEMessage = "", // FIXME
                };
                _answerCallback!(message);
                _gameState = GameState.Sleeping;
                Log.Warn($"Произошла непредвиденная ошибка...");
            }
        }
    }
}
