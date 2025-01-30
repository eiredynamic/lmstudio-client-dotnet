using System.Text;

namespace Eiredynamic.LMStudio.Client
{
    public class ChatClient
    {
        private const string _sysPromt = "You are a helpful assistant.";
        private const string _endpointRoot = "http://localhost:1234/v1/";
        private const string _endpointSuffix = "/chat/completions";
        
        private readonly IChatService _chatService;

        public ChatClient(IChatService? chatService = null)
        {
            _chatService = chatService ?? new ConcreteChatService();
        }
        public async Task<string> ChatAsync(string usrPrompt, string sysPromt = _sysPromt, string endpointRoot = _endpointRoot, bool includeReasoning = false)
        {
            var result = new StringBuilder();
            await foreach (var line in _chatService.ChatClient(usrPrompt, sysPromt, BuildEndpoint(endpointRoot), includeReasoning))
            {
                result.Append(line);
            }
            return result.ToString();
        }
        public async IAsyncEnumerable<string> StreamChatAsync(string usrPrompt, string sysPromt = _sysPromt, string endpointRoot = _endpointRoot, bool includeReasoning = false)
        {
            await foreach (string line in _chatService.ChatClient(usrPrompt, sysPromt, BuildEndpoint(endpointRoot), includeReasoning))
            {
                yield return line;
            }
        }

        private static string BuildEndpoint(string endpointRoot)
        {
            endpointRoot = endpointRoot.TrimEnd('/');
            return endpointRoot + _endpointSuffix;
        }
    }
}
