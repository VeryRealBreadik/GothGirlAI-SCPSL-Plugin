namespace GothGirlAIPlugin.Utils.Games
{
    public interface IGame
    {
        public string Name { get; }
        public void Start(Action<string> answerCallback);
        public bool HandleInput(IEnumerable<string> arguments);
        public void End();
    }
}
