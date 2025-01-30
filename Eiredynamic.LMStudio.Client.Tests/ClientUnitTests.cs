namespace Eiredynamic.LMStudio.Client.Tests
{
    public class ClientUnitTests
    {
        ChatClient _chat = new ChatClient(new DummyChatService());
        string _endpointRoot = "http://localhost:1234/v1/";
        string _expectedEndpoint = "http://localhost:1234/v1/chat/completions";

        [Fact]
        public async Task StreamChatAsyncRequiredParams_ShouldReturnExpectedStrings()
        {

            // Arrange
            var expected = new List<string> { "Hello", "World", _expectedEndpoint, "False" };

            // Act
            var actual = await _chat.StreamChatAsync("Hello World").ToListAsync();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task StreamChatAsyncOptionalParams_ShouldReturnExpectedStrings()
        {
            // Arrange
            var expected = new List<string> { "Hello", "World", _expectedEndpoint, "True" };

            // Act
            var actual = await _chat.StreamChatAsync(
                usrPrompt: "Hello World",
                sysPromt: "You are a helpful assistant.",
                endpointRoot: _endpointRoot,
                includeReasoning: true
                ).ToListAsync();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ChatAsyncRequiredParams_ShouldReturnExpectedStrings()
        {

            // Arrange
            var expected = $"HelloWorld{_expectedEndpoint}False";

            // Act
            var actual = await _chat.ChatAsync("Hello World");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ChatAsyncOptionalParams_ShouldReturnExpectedStrings()
        {
            // Arrange
            var expected = $"HelloWorld{_expectedEndpoint}True";

            // Act
            var actual = await _chat.ChatAsync(
                usrPrompt: "Hello World",
                sysPromt: "You are a helpful assistant.",
                endpointRoot: _endpointRoot,
                includeReasoning: true
                );

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ChatClient_ShouldCatchBadUris()
        {
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _chat.ChatAsync("Hello World", endpointRoot: "test"));
        }

        [Fact]
        public async Task ChatClient_TestTrailingSlash()
        {
            // Arrange
            var expected = _expectedEndpoint;

            // Act
            var actual = await _chat.ChatAsync("Hello World", endpointRoot: _endpointRoot);
            // Assert
            Assert.Contains(expected, actual);
        }

        [Fact]
        public async Task ChatClient_TestNoTrailingSlash()
        {
            // Arrange
            var expected = _expectedEndpoint;

            // Act
            var actual = await _chat.ChatAsync("Hello World", endpointRoot: _endpointRoot.TrimEnd('/'));
            // Assert
            Assert.Contains(expected, actual);
        }

        [Fact]
        public async Task ChatClient_ShouldHandleEmptyUserInput()
        {
            // Arrange
            var expected = _expectedEndpoint + "False";

            // Act
            var actual = await _chat.ChatAsync(""); ;
            
            Assert.Equal(expected, actual);  // Expected behavior?
        }
    }
}
