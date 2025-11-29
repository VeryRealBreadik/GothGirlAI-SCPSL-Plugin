using Exiled.API.Features;

namespace GothGirlAIPlugin.Utils.Games
{
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
            }
            else
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
