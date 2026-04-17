namespace ShoppingCartAssistant.Application.Abstracts;

public interface IConversationOrchestrator
{
    Task<string> AskAsync(string userInput, CancellationToken cancellationToken = default);
}