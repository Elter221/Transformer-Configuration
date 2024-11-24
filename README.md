# Transformer. Configuration

- Implement [Transformer](/TransformerWithConfiguration/Transformer.cs#L10) class, whose `Transform` instance method converts real number to it's "word format" in some language. The language and the set of words are presented by a settings class. However, the `Transformer` class controls the creation time of an object of this class.
- Add [new unit и mock tests](/TransformerWithConfiguration.Tests/TransformerTests.cs).
- Use ability [Configuration in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration) for testing `Transformer` class with various languages (russian, english, german).
