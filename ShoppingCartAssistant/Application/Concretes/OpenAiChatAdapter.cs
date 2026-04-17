using OpenAI.Chat;
using ShoppingCartAssistant.Application.Abstracts;
using ShoppingCartAssistant.Application.Extensions;
using ShoppingCartAssistant.Application.Models;
using ShoppingCartAssistant.Application.Models.Chat;
using ShoppingCartAssistant.Application.Models.Tools;

namespace ShoppingCartAssistant.Application.Concretes;

public class OpenAiChatAdapter : IChatAdapter
{
    private readonly ChatClient _chatClient;

    public OpenAiChatAdapter(ChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<IChatResponse> CompleteAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        var messages = request.Messages.ToOpenAiChatMessages();
        var tools = request.Tools.ToOpenAiChatTools();
        var options = new ChatCompletionOptions();
        foreach (var tool in tools)
        {
            options.Tools.Add(tool);
        }

        var completionResult = await _chatClient.CompleteChatAsync(messages, options, cancellationToken);
        if (completionResult == null)
        {
            throw new InvalidOperationException("Chat completion failed: no result returned.");
        }

        var completion = completionResult.Value;
        if (completion.FinishReason == ChatFinishReason.ToolCalls)
        {
            var calls = completion.ToolCalls
                .Select(tc => new ToolCall(
                    Id: tc.Id,
                    Name: tc.FunctionName,
                    ArgumentsJson: tc.FunctionArguments.ToString())).ToList();

            return new ToolCallChatResponse(calls);
        }

        var text = completion.Content.Count > 0
            ? string.Join("\n", completion.Content.Select(part => part.Text))
            : string.Empty;

        return new TextChatResponse(text);
    }
}