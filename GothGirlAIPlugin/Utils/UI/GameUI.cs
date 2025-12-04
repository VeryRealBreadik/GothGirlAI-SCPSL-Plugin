using Exiled.API.Features;
using MEC;

namespace GothGirlAIPlugin.Utils.UI
{
    public static class GameUI
    {
        private static Queue<AIMessage> _messagesQueue = new();
        public static bool IsTyping = false;
        private static float _charDelay = 0.06f;

        public static void EnqueueMessage(AIMessage message)
        {
            _messagesQueue.Enqueue(message);
            TrySendNextMessage();
        }

        private static void TrySendNextMessage()
        {
            Log.Info(Cassie.IsSpeaking);
            if (!IsTyping && !Cassie.IsSpeaking && _messagesQueue.Count > 0)
            {
                AIMessage message = _messagesQueue.Dequeue();
                string nextIntercomMessage = message.IntercomMessage;
                string? nextCASSIEMessage = message.CASSIEMessage;
                IsTyping = true;

                Timing.RunCoroutine(TypeMessageCoroutine(nextIntercomMessage));
                if (nextCASSIEMessage is not null)
                {
                    SayMessageCoroutine(nextCASSIEMessage);
                }
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
            TrySendNextMessage();
        }

        private static void SayMessageCoroutine(string message)
        {
            Cassie.Message(message, isNoisy: false, isSubtitles: true);
        }
    }
}
