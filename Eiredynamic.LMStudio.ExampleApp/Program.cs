using Eiredynamic.LMStudio.Client;

namespace Eiredynamic.LMStudio.ExampleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await StreamChat();
        }
        private static async Task StreamChat()
        {
            ChatClient _chat = new ChatClient();
            await foreach (string line in _chat.StreamChatAsync("What is the capital of France?", includeReasoning: true))
            {
                Console.Write(line);
            }

            await foreach (string line in _chat.StreamChatAsync("Explain how rockets work?", sysPromt: "You speak only in rhyme."))
            {
                Console.Write(line);
            }

            Console.Write(await _chat.ChatAsync("What is the capital of Canada?", includeReasoning: true));
        }
    }
}
