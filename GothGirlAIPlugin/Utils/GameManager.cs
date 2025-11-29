using Exiled.API.Features;

namespace GothGirlAIPlugin.Utils
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
        private static IGame? currentGame = null;
        private static Action<string>? _answerCallback;
        private static GameState _gameState = GameState.Sleeping;

        public static void SetAnswerCallback(Action<string> callback)
        {
            _answerCallback = callback;
        }

        private static void ShowMenu()
        {
            string menu = string.Join("\n", games.Select(kvp => $"{kvp.Key} - {kvp.Value.Name}"));
            _answerCallback!(menu);
        }

        private static void StartGame(int option)
        {
            currentGame = games[option];
            currentGame.Start(_answerCallback!);
            Log.Info($"Пользователь выбрал игру {currentGame.Name}");
        }

        private static void EndGame()
        {
            currentGame!.End();
            currentGame = null;
            Log.Info("Игра закончилась");
        }

        private static void CycleGameState()
        {
            _gameState = _gameState.Next();
        }

        public static void HandleInput(IEnumerable<string> arguments)
        {
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
                _answerCallback!("Игры с таким номером не существует.");
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
            else if (_gameState == GameState.Running && currentGame is not null)
            {
                bool isGameEnded = currentGame.HandleInput(arguments);
                if (isGameEnded)
                {
                    CycleGameState();
                    EndGame();
                }
                else
                {
                    Log.Info($"Пользователь передал параметры в игру: {arguments.ToArray()}");
                }
            }
            else
            {
                _answerCallback!("Произошла непредвиденная ошибка...");
                _gameState = GameState.Sleeping;
                Log.Warn($"Произошла непредвиденная ошибка...");
            }
        }
    }

    public interface IGame
    {
        public string Name { get; }
        public void Start(Action<string> answerCallback);
        public bool HandleInput(IEnumerable<string> arguments);
        public void End();
    }

    public class GuessTheNumberGame : IGame
    {
        public string Name { get; } = "Угадай число";
        private Action<string>? _answerCallback;
        private Random RandomNumberGenerator = new();
        private int? RandomNumber = null;

        public void Start(Action<string> answerCallback)
        {
            RandomNumber = RandomNumberGenerator.Next(1, 101);
            _answerCallback = answerCallback;

            _answerCallback("Игра началась, передавайте числа через команду!");
        }

        public bool HandleInput(IEnumerable<string> arguments)
        {
            string firstArgument = arguments.FirstOrDefault();
            if (firstArgument is null)
            {
                _answerCallback!("Для использования команды нужно передать число.");
                Log.Info("Пользователь не передал параметр");
                return false;
            }

            bool isNumber = int.TryParse(firstArgument, out int guessedNumber);
            if (!isNumber)
            {
                _answerCallback!("Первый передаваемый параметр должен быть числом.");
                Log.Info($"Пользователь передал параметр неправильного типа: {firstArgument}");
                return false;
            }

            if (RandomNumber == guessedNumber)
            {
                _answerCallback!("Вы угадали число! Загаданное число: " + RandomNumber);
                Log.Info($"Пользователь угадал загаданное число: {RandomNumber}");
                return true;
            }
            else if (RandomNumber > guessedNumber)
            {
                _answerCallback!("Загаданное число больше введённого");
                Log.Info($"Пользователь ввёл число меньше загаданного");
            } else
            {
                _answerCallback!("Загаданное число меньше введённого");
                Log.Info($"Пользователь ввёл число больше загаданного");
            }

            return false;
        }

        public void End()
        {
            _answerCallback!("Игра завершена");
            _answerCallback = null;
            RandomNumber = null;
        }
    }
}
