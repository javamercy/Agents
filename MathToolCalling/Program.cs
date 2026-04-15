using System.Text.Json;
using DotNetEnv;
using OpenAI.Chat;

namespace MathToolCalling;

public class Program
{
    public static void Main(string[] args)
    {
        Env.TraversePath().Load();

        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                     ?? throw new InvalidOperationException("OPENAI_API_KEY is not set");

        var model = Environment.GetEnvironmentVariable("OPENAI_MODEL")
                    ?? "gpt-5.4-nano";

        var client = new ChatClient(apiKey: apiKey, model: model);
        var calculator = new MathToolbox();
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(
                "You are a math assistant. Always respond with only the final results, one per line, and do not use markdown, explanations, or LaTeX.")
        };
        var options = new ChatCompletionOptions();

        foreach (var tool in calculator.CreateSdkTools()) options.Tools.Add(tool);

        while (true)
        {
            Console.WriteLine("\nYou: ");
            var userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput)) continue;

            messages.Add(new UserChatMessage(userInput));

            bool needsAnotherRound;

            do
            {
                var completion = client.CompleteChat(messages, options).Value;

                if (completion.FinishReason == ChatFinishReason.Stop)
                {
                    messages.Add(new AssistantChatMessage(completion));
                    Console.WriteLine($"Assistant: {completion.Content[0].Text}");
                    break;
                }

                if (completion.FinishReason != ChatFinishReason.ToolCalls)
                    throw new NotSupportedException($"Tool calls are not supported: {completion.FinishReason}");

                messages.Add(new AssistantChatMessage(completion));

                foreach (var toolCall in completion.ToolCalls)
                {
                    using var argumentsJson = JsonDocument.Parse(toolCall.FunctionArguments);
                    var toolResult = calculator.Invoke(toolCall.FunctionName, argumentsJson.RootElement);
                    messages.Add(new ToolChatMessage(toolCall.Id, toolResult));
                }

                needsAnotherRound = true;
            } while (needsAnotherRound);
        }
    }
}