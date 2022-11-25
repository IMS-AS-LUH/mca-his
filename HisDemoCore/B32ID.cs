using System;

namespace HisDemo
{
    /// <summary>
    /// Alphanumeric Identifier Representation with Check Digit
    /// 
    /// This class encodes an unsigned integer ID into an alphanumeric case-insensitive string.
    /// The encoding scheme is derived from Base32 (https://tools.ietf.org/html/rfc4648).
    /// For further details, see documentation file: B32ID.md
    /// 
    /// DO NOT START USING IDs WITHOUT PROPER DOCUMENTED ALLOCATION OF SCOPE AND RANGE!
    /// </summary>
    public static class B32ID
    {
        /// <summary>
        /// The symbol alphabet is derived from base32 Encoding according to RFC4648.
        /// https://tools.ietf.org/html/rfc4648
        /// </summary>
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /// <summary>
        /// Coefficients are the weights per symbol to calculate the check-symbol.
        /// These must be coprime to the modulus, so any primes except 2 are coprime,
        /// since we use a power of two (32). Weighting digits by 1 is also okay as with any such check digit.
        /// For our purpose, we apply the coefficients from right to left starting with the check 
        /// symbol itself, which is multiplied by 1 (CheckSymbolCoefficients[0] = 1).
        /// </summary>
        private static readonly uint[] CheckSymbolCoefficients = { 1, 3, 5, 7 };

        /// <summary>
        /// For better clarity in short numbers, we define a minimum number of symbols to be shown.
        /// We choose 4. This is only relevant for "small" IDs which require up to 10 bits.
        /// Any number up to 15 bits will thus have 4 symbols fixed, i.e. filled up on the left with 'A'.
        /// </summary>
        private const int MinimumLength = 4;

        /// <summary>
        /// Helper function to find the value (index) of a symbol. Could be optimized.
        /// </summary>
        /// <param name="symbol">The symbol for which the numeric value is requested.</param>
        /// <returns>The numeric value (index).</returns>
        private static uint InverseAlphabet(char symbol)
        {
            int position = Alphabet.IndexOf("" + symbol, StringComparison.OrdinalIgnoreCase);
            if (position < 0)
                throw new DecodingException("Unknown symbol: " + symbol);
            return (uint) position;
        }

        /// <summary>
        /// Encode a numeric ID into an B32ID string.
        /// The result is left-padded to reach the defined minimum length.
        /// </summary>
        /// <param name="id">Numeric integer id. Must be positive.</param>
        /// <returns>B32ID symbol representation including check digit.</returns>
        public static string Encode(int id)
        {
            if (id < 0)
                throw new ArgumentException("Negative IDs cannot be encoded to B32ID.", "id");
            return Encode((uint)id);
        }

        /// <summary>
        /// Encode a numeric ID into an B32ID string.
        /// The result is left-padded to reach the defined minimum length.
        /// </summary>
        /// <param name="id">Numeric positive integer id.</param>
        /// <returns>B32ID symbol representation including check digit.</returns>
        public static string Encode(uint id)
        {
            string symbols = "";
            uint checksum = 0;
            int coefficientIndex = 1; // Check digit character will be modulated by 1 (index 0).

            // Begin encoding 5 bit parts of the identifier until no more bits are set.
            // Prepend existing symbols so that most significant non-zero is first (left).
            while (id > 0)
            {
                uint part = id & 0x1F;
                symbols = Alphabet[(int)part] + symbols;
                checksum += part * CheckSymbolCoefficients[coefficientIndex % CheckSymbolCoefficients.Length];
                id >>= 5;
                coefficientIndex++;
            }

            // Append check symbol such that it will cancel out to zero
            // Note: No further modulation required since we define CheckSymbolCoefficients[0] = 1
            checksum = (0x20 - (checksum & 0x1F)) & 0x1F;
            symbols += Alphabet[(int)checksum];

            // Prepend zero symbols if necessary
            while (symbols.Length < MinimumLength)
                symbols = Alphabet[0] + symbols;

            return symbols;
        }

        /// <summary>
        /// Decode an B32ID string into its numeric ID.
        /// The input is checked against the defined minimum length.
        /// </summary>
        /// <param name="symbols">B32ID symbol representation including check digit.</param>
        /// <returns>Numeric positive integer id.</returns>
        /// <exception cref="DecodingException">Thrown if input is invalid. Present to user.</exception>
        public static uint Decode(string symbols)
        {
            if (symbols.Length > 8)
                throw new DecodingException("ID string too long for 32 bit integer.");
            if (symbols.Length < MinimumLength || symbols.Length < 2)
                throw new DecodingException("ID string too short.");

            uint part;
            uint checksum = 0;
            uint id = 0;
            int coefficientIndex = symbols.Length - 1;

            // Begin decoding symbols into 5 bit parts except the checksum (last symbol).
            // Accumulate and shift the resulting ID.
            for (int i = 0; i < (symbols.Length - 1); i++)
            {
                part = InverseAlphabet(symbols[i]);
                if (part > 0x1F)
                    throw new ArithmeticException("Internal error in B32ID Decoding: InverseAlphabet result out of range.");
                
                checksum += part * CheckSymbolCoefficients[coefficientIndex % CheckSymbolCoefficients.Length];
                id = (id << 5) | part;
                coefficientIndex--;
            }

            // Test checksum symbol (Checksum character will be modulated by 1)
            part = InverseAlphabet(symbols[symbols.Length - 1]);
            if (part > 0x1F)
                throw new ArithmeticException("Internal error in B32ID Decoding: InverseAlphabet result out of range.");

            if (coefficientIndex != 0)
                throw new ArithmeticException("Internal error in B32ID Decoding: Modulator sanity check failed.");

            // Note: No further modulation required since we define ChecksumCoefficients[0] = 1
            checksum += part;
            checksum &= 0x1F;

            if (checksum != 0)
                throw new DecodingException($"ID Checksum is invalid. Remainder: 0x{checksum:X02}");

            return id;
        }

        /// <summary>
        /// Special exception used for any "regular" decoding errors that may be introduced by human error.
        /// If this exception is thrown, present the user with a message indicating the invalid input.
        /// Any other exception thrown represents an unexpected internal program error which needs to be investigated.
        /// </summary>
        public class DecodingException : Exception
        {
            /// <summary>
            /// Create a user-targeted exception indicating an invalid B32ID string.
            /// </summary>
            /// <param name="message">Reason for B32ID being evaluated as invalid.</param>
            public DecodingException(string message) : base(message) { }
        }
    }
}
