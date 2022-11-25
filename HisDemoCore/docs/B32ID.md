# Alphanumeric Identifier Representation with Checksum
This scheme, herein referred to as B32ID encodes an unsigned integer ID into an alphanumeric case-insensitive
string. It is derived from Base32 Encoding (https://tools.ietf.org/html/rfc4648).
Reasons for this are:
- Smaller Footprint by using 32 Symbols (Letters and Numbers) instead of 10 (Numbers only)
- 32 Symbols = 5 Bits per Symbol
- Append extra Symbol for Checksum (= 5 Bits Check/Redundancy)

## Identifier Pool
We can increase ID pool by extending string length to the left in 5 Bit increments. Pool size:

Symbols | Bit count | Pool size
------- | --------- | ---------
3 + 1   | 15 + 5    | 31 k
4 + 1   | 20 + 5    | 1 M
5 + 1   | 25 + 5    | 33.5 M
6 + 1   | 30 + 5    | 1 G
7 + 1   | 35 + 5    | 34 G

Strategy: Start with at least 4 (3+1) symbols and omit further padding zeros (symbol "A"). If pool grows, add symbols on the left.

## Encoding Alphabet
After first exlporing z-base-32 we decide to use the more trivial RFC4648 Alphabet. It consists only of capital letters and numbers,
which enables efficient barcodes (QR, MicroQR, Code 39, Code 128, ...).
Thus for a 5 Bit symbol, the decimal value of 0 maps to 'A', 1 to 'B' and so on until 25 maps to 'Z'. Then 26 maps to '2' thru 31 maps to '7'.
This way, 0, 1, 8 which are close to O, I and B are excluded.

## Checksum Design
The checksum is primarily designed for human error detection.
If the alphanumeric ID is encoded as QR-Code for example, this will itself add decent error correction.
Many schemes for checksums exist, ranging from simple summation to complex CRC-based methods (which might benefit from the zbase hamming distance).
For the application at hand, we assume the major errors being:

- single symbol error (replace any symbol by a random different one)
- dual symbol error (replace any two symbol by random different ones)
- adjacent symbol swap error (reverse the order of two neighboring symbols - for sake of definition includes first and last symbol swapping)
- random symbol swap error (exchange any two random places of the number)
- duplicate symbol error (repeat one symbol making the overall word longer)

Most likely is a single symbol error. Thus this must be covered in all cases (100% detection rate).
Next likely is a swap of neighboring symbols, two single symbol errors or duplicate symbol errors.
Less likely is a random swap of two symbols within the whole word.

From a random-sampling based test we choose the following checksum coefficients:

	1, 3, 5, 7

These have a empirical error detection rate of 
- 100% for single symbol errors
- over 96.5% for adjacient symbol swap
- over 97.5% for dual symbol errors
- over 98.5% for symbol duplication
- over 79% for random symbol swap

Especially coefficient sets including 2 as a factor for any coefficient will reduce single error detection below 100%.
Shorter sets degrade random swap detection (down to about 50% for sets of length 2 or 70% at length 3).
Longer sets tend to not improve significantly and will start to worsen at some point.

A checksum offset, that is adding a static value to the checksum, did not show any differences as expected in modulus operation.
The benefit is easy recognition of ID=0 with all the same symbol (0 is encoded as "AAAA").
