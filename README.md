# LMStudio.Client

LMStudio.Client is a lightweight .NET client library for interacting with the LMStudio API. Currently, it provides support for the `chat/completions` endpoint, offering both asynchronous and streamed responses from an LLM running in LMStudio.

## Features
- Async API call to `chat/completions` endpoint
- Streaming support for real-time token-by-token responses
- Simple and lightweight implementation
- Example console application (`ExampleApp`) demonstrating usage

## Installation

This library is available as a NuGet package. To install, run:

```sh
# Install via NuGet
dotnet add package Eiredynamic.LMStudio.Client
```

## Usage

### Basic Async Call
```csharp
using Eiredynamic.LMStudio.Client;

var _chatClient = new ChatClient();
var response = await _chatClient.ChatAsync("Hello, how are you?");

Console.WriteLine(response);
```

### Streaming Response
```csharp
await foreach (var token in _chatClient.StreamChatAsync("Tell me a joke."))
{
    Console.Write(token);
}
Console.WriteLine();
```

### Function Definitions

```csharp
public async Task<string> ChatAsync(string usrPrompt, string sysPromt = _sysPromt, string endpointRoot = _endpointRoot, bool includeReasoning = false)
```

- `usrPrompt`: The user's prompt to the LLM.
- `sysPromt`: (Optional) System prompt providing guidance to the model.
- `endpointRoot`: (Optional) Base URL of the LMStudio server (default: `http://localhost:1234/v1/`).
- `includeReasoning`: (Optional) Whether to include reasoning in the response, if the model support it.

```csharp
public async IAsyncEnumerable<string> StreamChatAsync(string usrPrompt, string sysPromt = _sysPromt, string endpointRoot = _endpointRoot, bool includeReasoning = false)
```

- `usrPrompt`: The user's prompt to the LLM.
- `sysPromt`: (Optional) System prompt providing guidance to the model.
- `endpointRoot`: (Optional) Base URL of the LMStudio server (default: `http://localhost:1234/v1/`).
- `includeReasoning`: (Optional) Whether to include reasoning in the streamed response, if the model support it.

## Example Application
An example console application (`ExampleApp`) is included to demonstrate how to interact with LMStudio using this client.

## Requirements
- .NET 9 or later
- LMStudio running locally with API access enabled

## Future Plans
- Add support for additional LMStudio API endpoints
- Improve configuration options
- Provide better error handling and logging

## Contributing
Contributions are welcome! Please open an issue or submit a pull request with improvements.

## License
This project is licensed under the MIT License. See `LICENSE` for details.

