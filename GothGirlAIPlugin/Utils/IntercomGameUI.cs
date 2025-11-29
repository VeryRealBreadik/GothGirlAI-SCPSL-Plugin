using Exiled.API.Features;
using MEC;

namespace GothGirlAIPlugin.Utils
{
    public static class IntercomGameUI
    {
        private static Queue<string> _messagesQueue = new();
        public static bool IsTyping = false;
        private static float _charDelay = 0.1f;

        public static void EnqueueIntercomMessage(string message)
        {
            _messagesQueue.Enqueue(message);
            TryTypeNextMessage();
        }

        private static void TryTypeNextMessage()
        {
            if (!IsTyping && _messagesQueue.Count > 0)
            {
                string nextMessage = _messagesQueue.Dequeue();
                IsTyping = true;
                Timing.RunCoroutine(TypeMessageCoroutine(nextMessage));
            }
        }

        private static IEnumerator<float> TypeMessageCoroutine(string message)
        {
            string current = "";

            foreach (char c in message)
            {
                current += c;
                Intercom.DisplayText = current;
                yield return Timing.WaitForSeconds(_charDelay);
            }

            IsTyping = false;
            TryTypeNextMessage();
        }
    }
}
