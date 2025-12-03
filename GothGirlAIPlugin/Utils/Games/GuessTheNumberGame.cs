using Exiled.API.Features;
using GothGirlAIPlugin.Utils.UI;

namespace GothGirlAIPlugin.Utils.Games
{
    public class GuessTheNumberGame : IGame
    {
        public string Name { get; } = "Угадай число";
        private Action<AIMessage>? _answerCallback;
        private Random RandomNumberGenerator = new();
        private int? RandomNumber = null;

        public void Start(Action<AIMessage> answerCallback)
        {
            RandomNumber = RandomNumberGenerator.Next(1, 101);
            _answerCallback = answerCallback;

            AIMessage message = new()
            {
                IntercomMessage = "Игра началась, передавайте числа через команду!",
                CASSIEMessage = "", // FIXME
            };
            _answerCallback!(message);
        }

        public bool HandleInput(IEnumerable<string> arguments)
        {
            string firstArgument = arguments.FirstOrDefault();
            if (firstArgument is null)
            {
                AIMessage message = new()
                {
                    IntercomMessage = "Для использования команды нужно передать число.",
                    CASSIEMessage = "", // FIXME
                };
                _answerCallback!(message);
                Log.Info("Пользователь не передал параметр");
                return false;
            }

            bool isNumber = int.TryParse(firstArgument, out int guessedNumber);
            if (!isNumber)
            {
                AIMessage message = new()
                {
                    IntercomMessage = "Первый передаваемый параметр должен быть числом.",
                    CASSIEMessage = "", // FIXME
                };
                _answerCallback!(message);
                Log.Info($"Пользователь передал параметр неправильного типа: {firstArgument}");
                return false;
            }

            if (RandomNumber == guessedNumber)
            {
                AIMessage message = new()
                {
                    IntercomMessage = "Вы угадали число! Загаданное число: " + RandomNumber,
                    CASSIEMessage = "", // FIXME
                };
                _answerCallback!(message);
                Log.Info($"Пользователь угадал загаданное число: {RandomNumber}");
                return true;
            }
            else if (RandomNumber > guessedNumber)
            {
                AIMessage message = new()
                {
                    IntercomMessage = "Загаданное число больше введённого",
                    CASSIEMessage = "", // FIXME
                };
                _answerCallback!(message);
                Log.Info($"Пользователь ввёл число меньше загаданного");
            }
            else
            {
                AIMessage message = new()
                {
                    IntercomMessage = "Загаданное число меньше введённого",
                    CASSIEMessage = "", // FIXME
                };
                _answerCallback!(message);
                Log.Info($"Пользователь ввёл число больше загаданного");
            }

            return false;
        }

        public void End()
        {
            AIMessage message = new()
            {
                IntercomMessage = "Игра завершена",
                CASSIEMessage = "", // FIXME
            };
            _answerCallback = null;
            RandomNumber = null;
        }
    }
}
