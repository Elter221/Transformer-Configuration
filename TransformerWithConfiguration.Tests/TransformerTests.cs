using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace TransformerWithConfiguration.Tests
{
    [TestFixture]
    public class TransformerTests
    {
        private IConfiguration configuration;
        private SymbolsDictionary germanDictionary;
        private SymbolsDictionary englishDictionary;
        private SymbolsDictionary russianDictionary;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            (this.germanDictionary, this.englishDictionary, this.russianDictionary) =
                (this.CreateGermanDictionary(), this.CreateEnglishDictionary(), this.CreateRussianDictionary());
        }

        [TestCase(123.78, ExpectedResult = "один два три запятая семь восемь")]
        [TestCase(-12.78, ExpectedResult = "минус один два запятая семь восемь")]
        [TestCase(-0.78, ExpectedResult = "минус ноль запятая семь восемь")]
        [TestCase(double.PositiveInfinity, ExpectedResult = "положительная бесконечность")]
        [TestCase(double.NegativeInfinity, ExpectedResult = "отрицательная бесконечность")]
        [TestCase(double.NaN, ExpectedResult = "не число")]
        [TestCase(double.Epsilon, ExpectedResult = "эпсилон")]
        [TestCase(double.MinValue, ExpectedResult = "минус один запятая семь девять семь шесть девять три один три четыре восемь шесть два три один пять семь экспонента плюс три ноль восемь")]
        [TestCase(double.MaxValue, ExpectedResult = "один запятая семь девять семь шесть девять три один три четыре восемь шесть два три один пять семь экспонента плюс три ноль восемь")]
        [TestCase(6.67300E-11, ExpectedResult = "шесть запятая шесть семь три экспонента минус один один")]
        [TestCase(3.302e+23, ExpectedResult = "три запятая три ноль два экспонента плюс два три")]
        [TestCase(1234567890, ExpectedResult = "один два три четыре пять шесть семь восемь девять ноль")]
        public string TransformToWordsTestsWithRussianCulture(double number)
        {
            var transformer = new Transformer(this.russianDictionary);
            return transformer.Transform(number);
        }

        [TestCase(123.78, ExpectedResult = "one two three point seven eight")]
        [TestCase(-12.78, ExpectedResult = "minus one two point seven eight")]
        [TestCase(-0.78, ExpectedResult = "minus zero point seven eight")]
        [TestCase(double.PositiveInfinity, ExpectedResult = "positive infinity")]
        [TestCase(double.NegativeInfinity, ExpectedResult = "negative infinity")]
        [TestCase(double.NaN, ExpectedResult = "not a number")]
        [TestCase(double.Epsilon, ExpectedResult = "epsilon")]
        [TestCase(double.MinValue, ExpectedResult = "minus one point seven nine seven six nine three one three four eight six two three one five seven exponent plus three zero eight")]
        [TestCase(double.MaxValue, ExpectedResult = "one point seven nine seven six nine three one three four eight six two three one five seven exponent plus three zero eight")]
        [TestCase(6.67300E-11, ExpectedResult = "six point six seven three exponent minus one one")]
        [TestCase(3.302e+23, ExpectedResult = "three point three zero two exponent plus two three")]
        [TestCase(1234567890, ExpectedResult = "one two three four five six seven eight nine zero")]
        public string TransformToWordsTestsWithEnglishCulture(double number)
        {
            var transformer = new Transformer(this.englishDictionary);
            return transformer.Transform(number);
        }

        [TestCase(123.78, ExpectedResult = "eins zwei drei komma sieben acht")]
        [TestCase(-12.78, ExpectedResult = "minus eins zwei komma sieben acht")]
        [TestCase(-0.78, ExpectedResult = "minus null komma sieben acht")]
        [TestCase(double.PositiveInfinity, ExpectedResult = "positive unendlichkeit")]
        [TestCase(double.NegativeInfinity, ExpectedResult = "negative unendlichkeit")]
        [TestCase(double.NaN, ExpectedResult = "keine zahl")]
        [TestCase(double.Epsilon, ExpectedResult = "epsilon")]
        [TestCase(double.MinValue, ExpectedResult = "minus eins komma sieben neun sieben sechs neun drei eins drei vier acht sechs zwei drei eins fünf sieben exponent plus drei null acht")]
        [TestCase(double.MaxValue, ExpectedResult = "eins komma sieben neun sieben sechs neun drei eins drei vier acht sechs zwei drei eins fünf sieben exponent plus drei null acht")]
        [TestCase(6.67300E-11, ExpectedResult = "sechs komma sechs sieben drei exponent minus eins eins")]
        [TestCase(3.302e+23, ExpectedResult = "drei komma drei null zwei exponent plus zwei drei")]
        [TestCase(1234567890, ExpectedResult = "eins zwei drei vier fünf sechs sieben acht neun null")]
        public string TransformToWordsTestsWithGermanCulture(double number)
        {
            var transformer = new Transformer(this.germanDictionary);
            return transformer.Transform(number);
        }

        [Test]
        public void TransformToWords_DictionaryIsEmpty_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new Transformer(new SymbolsDictionary()
                {
                    Dictionary = new Dictionary<Symbol, string>(), CultureName = null,
                }), "Dictionary cannot be empty.");
        }

        [Test]
        public void TransformToWords_DictionaryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Transformer(null), "Dictionary cannot be null.");
        }

        private SymbolsDictionary CreateEnglishDictionary()
        {
            var pairs = this.configuration.GetSection("Dictionary")
                .GetChildren()
                .Select(section => new KeyValuePair<Symbol, string>(Enum.Parse<Symbol>(section.Key), section.Value));

            return new SymbolsDictionary
            {
                Dictionary = new Dictionary<Symbol, string>(pairs),
                CultureName = this.configuration.GetSection("CultureName").Value,
            };
        }

        private SymbolsDictionary CreateEnglishIDictionary() => new()
        {
            Dictionary = new Dictionary<Symbol, string>
            {
                [Symbol.Zero] = this.configuration.GetSection("Dictionary:Zero").Value,
                [Symbol.One] = this.configuration.GetSection("Dictionary:One").Value,
                [Symbol.Two] = this.configuration.GetSection("Dictionary:Two").Value,
                [Symbol.Three] = this.configuration.GetSection("Dictionary:Three").Value,
                [Symbol.Four] = this.configuration.GetSection("Dictionary:Four").Value,
                [Symbol.Five] = this.configuration.GetSection("Dictionary:Five").Value,
                [Symbol.Six] = this.configuration.GetSection("Dictionary:Six").Value,
                [Symbol.Seven] = this.configuration.GetSection("Dictionary:Seven").Value,
                [Symbol.Eight] = this.configuration.GetSection("Dictionary:Eight").Value,
                [Symbol.Nine] = this.configuration.GetSection("Dictionary:Nine").Value,
                [Symbol.Minus] = this.configuration.GetSection("Dictionary:Minus").Value,
                [Symbol.Plus] = this.configuration.GetSection("Dictionary:Plus").Value,
                [Symbol.Point] = this.configuration.GetSection("Dictionary:Point").Value,
                [Symbol.Comma] = this.configuration.GetSection("Dictionary:Comma").Value,
                [Symbol.Exponent] = this.configuration.GetSection("Dictionary:Exponent").Value,
                [Symbol.Epsilon] = this.configuration.GetSection("Dictionary:Epsilon").Value,
                [Symbol.NegativeInfinity] = this.configuration.GetSection("Dictionary:NegativeInfinity").Value,
                [Symbol.PositiveInfinity] = this.configuration.GetSection("Dictionary:PositiveInfinity").Value,
                [Symbol.NaN] = this.configuration.GetSection("Dictionary:NaN").Value,
            },
            CultureName = this.configuration.GetSection("CultureName").Value,
        };

        private SymbolsDictionary CreateRussianDictionary() => new()
        {
            Dictionary = new Dictionary<Symbol, string>
            {
                [Symbol.Zero] = "ноль",
                [Symbol.One] = "один",
                [Symbol.Two] = "два",
                [Symbol.Three] = "три",
                [Symbol.Four] = "четыре",
                [Symbol.Five] = "пять",
                [Symbol.Six] = "шесть",
                [Symbol.Seven] = "семь",
                [Symbol.Eight] = "восемь",
                [Symbol.Nine] = "девять",
                [Symbol.Minus] = "минус",
                [Symbol.Plus] = "плюс",
                [Symbol.Point] = "точка",
                [Symbol.Comma] = "запятая",
                [Symbol.Exponent] = "экспонента",
                [Symbol.Epsilon] = "эпсилон",
                [Symbol.NegativeInfinity] = "отрицательная бесконечность",
                [Symbol.PositiveInfinity] = "положительная бесконечность",
                [Symbol.NaN] = "не число",
            },
            CultureName = "ru-ru",
        };

        private SymbolsDictionary CreateGermanDictionary() => new()
        {
            Dictionary = new Dictionary<Symbol, string>
            {
                [Symbol.Zero] = "null",
                [Symbol.One] = "eins",
                [Symbol.Two] = "zwei",
                [Symbol.Three] = "drei",
                [Symbol.Four] = "vier",
                [Symbol.Five] = "fünf",
                [Symbol.Six] = "sechs",
                [Symbol.Seven] = "sieben",
                [Symbol.Eight] = "acht",
                [Symbol.Nine] = "neun",
                [Symbol.Minus] = "minus",
                [Symbol.Plus] = "plus",
                [Symbol.Point] = "punkt",
                [Symbol.Comma] = "komma",
                [Symbol.Exponent] = "exponent",
                [Symbol.Epsilon] = "epsilon",
                [Symbol.NegativeInfinity] = "negative unendlichkeit",
                [Symbol.PositiveInfinity] = "positive unendlichkeit",
                [Symbol.NaN] = "keine zahl",
            },
            CultureName = "de-de",
        };
    }
}
