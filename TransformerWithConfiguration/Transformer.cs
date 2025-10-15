using System.Globalization;
using System.Text;

namespace TransformerWithConfiguration
{
    /// <summary>
    /// Provides transforming double number to its string representation with specified dictionary.
    /// </summary>
    public class Transformer
    {
        private readonly SymbolsDictionary symbolsDictionary;
        private readonly CultureInfo culture;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transformer"/> class.
        /// </summary>.
        /// <param name="symbolsDictionary">Dictionary with rules of transforming.</param>
        public Transformer(SymbolsDictionary? symbolsDictionary)
        {
            if (symbolsDictionary == null)
            {
                throw new ArgumentNullException(nameof(symbolsDictionary));
            }

            if (symbolsDictionary.Dictionary == null || symbolsDictionary.CultureName == null)
            {
                throw new ArgumentException("SymbolsDictionary must contain a valid dictionary and culture name.");
            }

            this.symbolsDictionary = symbolsDictionary;
            this.culture = new CultureInfo(symbolsDictionary.CultureName);
        }

        /// <summary>
        /// Transforms double number into string.
        /// </summary>
        /// <param name="number">Double number to transform.</param>
        /// <returns>Transformed value.</returns>
        public string Transform(double number)
        {
            var stringBuilder = new StringBuilder();

            if (double.IsNaN(number))
            {
                return this.symbolsDictionary.Dictionary![Symbol.NaN];
            }

            if (double.IsPositiveInfinity(number))
            {
                return this.symbolsDictionary.Dictionary![Symbol.PositiveInfinity];
            }

            if (double.IsNegativeInfinity(number))
            {
                return this.symbolsDictionary.Dictionary![Symbol.NegativeInfinity];
            }

            if (number == double.Epsilon)
            {
                return this.symbolsDictionary.Dictionary![Symbol.Epsilon];
            }

            string numberString = number.ToString("G", this.culture);

            foreach (char c in numberString)
            {
                Symbol symbol = c switch
                {
                    '0' => Symbol.Zero,
                    '1' => Symbol.One,
                    '2' => Symbol.Two,
                    '3' => Symbol.Three,
                    '4' => Symbol.Four,
                    '5' => Symbol.Five,
                    '6' => Symbol.Six,
                    '7' => Symbol.Seven,
                    '8' => Symbol.Eight,
                    '9' => Symbol.Nine,
                    '+' => Symbol.Plus,
                    '-' => Symbol.Minus,
                    '.' => Symbol.Point,
                    ',' => Symbol.Comma,
                    'E' or 'e' => Symbol.Exponent,
                    _ => throw new InvalidOperationException($"Unexpected character: {c}")
                };

                stringBuilder.Append(this.symbolsDictionary.Dictionary![symbol]);
                stringBuilder.Append(' ');
            }

            return stringBuilder.ToString().Trim();
        }
    }
}
