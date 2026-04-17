using ShoppingCartAssistant.Application.Models;
using ShoppingCartAssistant.Application.Models.Chat;

namespace ShoppingCartAssistant.Application.Abstracts;

public interface IChatAdapter
{
    Task<IChatResponse> CompleteAsync(ChatRequest request, CancellationToken cancellationToken = default);
}