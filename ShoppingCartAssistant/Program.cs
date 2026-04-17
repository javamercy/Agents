using OpenAI.Chat;
using ShoppingCartAssistant.Application.Concretes;
using DotNetEnv;

Env.TraversePath().Load();

var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
             ?? throw new InvalidOperationException("OPENAI_API_KEY is not set");

var model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-5.4-nano";

var chatClient = new ChatClient(apiKey: apiKey, model: model);
var cartManager = new CartManager();
var toolExecutor = new CartToolExecutor(cartManager);
var chatAdapter = new OpenAiChatAdapter(chatClient);
var orchestrator = new ConversationOrchestrator(chatAdapter, toolExecutor);

Console.WriteLine("Shopping Cart Assistant");
Console.WriteLine("Type 'exit' to quit");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    var response = await orchestrator.AskAsync(input);
    Console.WriteLine(response);
}