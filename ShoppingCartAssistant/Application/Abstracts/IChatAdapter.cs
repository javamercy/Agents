using ShoppingCartAssistant.Application.Models;

namespace ShoppingCartAssistant.Application.Abstracts;

public interface IChatAdapter
{
    Task<ChatResponse> CompleteAsync(ChatRequest request, CancellationToken cancellationToken = default);
}