# MathToolCalling

`MathToolCalling` is a small .NET console app that demonstrates native OpenAI tool calling with local math functions.

The assistant can call three tools:

- `add(a, b)`
- `multiply(a, b)`
- `factorial(n)`

The model decides when to use a tool, the app executes the matching local function, and the result is sent back into the
conversation.

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

Example prompt:

```text
What is 11 * 63 and 7 factorial?
```
