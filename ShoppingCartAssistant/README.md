# ShoppingCartAssistant

`ShoppingCartAssistant` is a small .NET console app that demonstrates tool calling with a shopping cart domain.

The assistant can:

- add items to a cart
- remove items from a cart
- update item quantities
- calculate the cart total
- answer cart questions in natural language

The model decides when to use a tool, the app executes the matching local logic, and the result is sent back into the conversation.

## Requirements

- .NET 10 SDK
- An OpenAI API key

## Configuration

Create a `.env` file in the project folder or repository root:

```dotenv
OPENAI_API_KEY=your_api_key_here
OPENAI_MODEL=model_here
```

## Run

Restore dependencies:

```bash
dotnet restore
```

Start the app:

```bash
dotnet run
```

## Example prompts

```text
Add 2 apples at 1.50
Add 3 bananas at 2.99
What is the total price of cart?
Update apples to 5
Remove bananas
What is in cart now and what is total price?
```

## Notes

- `Domain/` holds the cart data model.
- `Application/` contains the tool executor, chat adapter, and orchestration logic.
- `Program.cs` wires everything together and runs the console loop.
