using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HisDemo.Test
{
    /// <summary>
    /// Tests for the B32ID Library Class which provides ID encoding and decoding.
    /// </summary>
    [TestClass]
    public class B32IDTests
    {
        //[TestMethod] // Takes long time, not used for regular regression
        public void FullSequentialEncodeDecode25Bit()
        {
            int testCount = (1 << 25) - 1; // ~ 1 Minute
            for (int run = 0; run < testCount; run++)
            {
                uint id = (uint)run;
                string symbols = B32ID.Encode(id);
                uint decoded = B32ID.Decode(symbols);
                if (id != decoded)
                {
                    Assert.IsTrue(false, $"Symbols for ID 0x{id:X06} were: {symbols} decoded to 0x{decoded:X06}");
                }
            }
        }

        //[TestMethod] // Takes long time, not used for regular regression
        public void FullSequentialUniqueCheck25Bit()
        {
            int testCount = (1 << 25) - 1; // ~ 1 Minute
            HashSet<string> encodings = new HashSet<string>();
            for (int run = 0; run < testCount; run++)
            {
                uint id = (uint)run;
                string symbols = B32ID.Encode(id);

                if (encodings.Contains(symbols))
                {
                    Assert.IsTrue(false, $"Duplicate Symbol for ID 0x{id:X06} were {symbols} already maps to another id.");
                }
                else
                {
                    encodings.Add(symbols);
                }
            }
        }

        [TestMethod]
        public void RandomEncodeDecode31Bit()
        {
            int testCount = 250000;
            int seed = 1307;
            Random random = new Random(seed);
            for (int run = 0; run < testCount; run++)
            {
                uint id = (uint)random.Next(0, int.MaxValue);
                string symbols = B32ID.Encode(id);
                Assert.AreEqual<uint>(id, B32ID.Decode(symbols), $"Symbols for ID 0x{id:X06} were: {symbols}");
            }
        }

        [TestMethod]
        public void RandomUniqueCheck31Bit()
        {
            int testCount = 250000;
            int seed = 1841;
            Random random = new Random(seed);
            Dictionary<string, uint> encodings = new Dictionary<string, uint>(testCount);
            for (int run = 0; run < testCount; run++)
            {
                uint id = (uint)random.Next(0, int.MaxValue);
                string symbols = B32ID.Encode(id);

                if (encodings.ContainsKey(symbols))
                {
                    Assert.IsFalse(encodings[symbols] != id,
                    $"Duplicate Symbol for ID 0x{id:X06} were {symbols} already maps to another id 0x{encodings[symbols]:X06}.");
                }
                else
                {
                    encodings.Add(symbols, id);
                }
            }
        }

        [DataTestMethod]
        [DataRow((uint)1 << 15, "BAAA7")]
        [DataRow((uint)15<<15, "PAAAR")]
        [DataRow(0U, "AAAA")]
        [DataRow(1359U, "BKP2")]
        [DataRow(2718U, "CU6U")]
        [DataRow(4077U, "D7NJ")]
        [DataRow(5436U, "FJ44")]
        [DataRow(6795U, "GULR")]
        [DataRow(8154U, "H62L")]
        [DataRow(9513U, "JJJZ")]
        [DataRow(10872U, "KTYT")]
        [DataRow(12231U, "L6HI")]
        [DataRow(13590U, "NIW3")]
        [DataRow(14949U, "OTFQ")]
        [DataRow(16308U, "P5UK")]
        [DataRow(17667U, "RIDY")]
        [DataRow(19026U, "SSSS")]
        [DataRow(20385U, "T5BH")]
        [DataRow(21744U, "VHQ2")]
        [DataRow(23103U, "WR7U")]
        [DataRow(24462U, "X4OJ")]
        [DataRow(25821U, "ZG54")]
        [DataRow(27180U, "2RMR")]
        [DataRow(28539U, "333L")]
        [DataRow(29898U, "5GKZ")]
        [DataRow(31257U, "6QZT")]
        [DataRow(32616U, "73II")]
        [DataRow(33975U, "BBFX2")]
        [DataRow(35334U, "BCQGP")]
        [DataRow(36693U, "BD2VJ")]
        [DataRow(38052U, "BFFEX")]
        [DataRow(39411U, "BGPTR")]
        [DataRow(40770U, "BH2CG")]
        [DataRow(42129U, "BJERZ")]

        public void KnownSequenceCheck(uint id, string symbols)
        {
            Assert.AreEqual<string>(symbols, B32ID.Encode(id), "Encoding mismatch.");
            Assert.AreEqual<uint>(id, B32ID.Decode(symbols), "Decoding mismatch.");
        }

        [TestMethod]
        public void SingleSymbolErrorDetection()
        {
            int testCount = 50000;
            int seed = 1756;
            double minimumRate = 1.0; // Expect at least 100% of single errors to be detected

            string errorSymbolPool = "abcdefghijklmnopqrstuvwxyz0123456789-_#!".ToUpper();

            Random random = new Random(seed);
            int failCount = 0;
            for (int run = 0; run < testCount; run++)
            {
                uint id = (uint)random.Next(0, int.MaxValue);

                string symbols = B32ID.Encode(id);

                int symbolToChange = random.Next(0, symbols.Length - 1);
                int symbolToPlace = random.Next(0, errorSymbolPool.Length - 2);

                char errorSymbol = errorSymbolPool.Replace($"{symbols[symbolToChange]}", "")[symbolToPlace];

                var sb = new StringBuilder(symbols);
                sb[symbolToChange] = errorSymbol;
                string symbolsWithError = sb.ToString();

                try
                {
                    B32ID.Decode(symbolsWithError);
                }
                catch (B32ID.DecodingException)
                {
                    continue;
                }
                failCount++;
            }
            double detectionRate = (double)(testCount - failCount) / (double)testCount;
            if (detectionRate < minimumRate)
                Assert.Fail($"Single symbol error detection rate is {detectionRate * 100.0:0.00} % which is less than minimum expected detection rate ({minimumRate * 100.0:0.00} %).");
        }

        [TestMethod]
        public void DualSymbolErrorDetection()
        {
            int testCount = 50000;
            int seed = 8013;
            double minimumRate = 0.975; // Expect at least 97.5% of dual errors to be detected

            string errorSymbolPool = "abcdefghijklmnopqrstuvwxyz0123456789-_#!";

            Random random = new Random(seed);
            int failCount = 0;
            for (int run = 0; run < testCount; run++)
            {
                uint id = (uint)random.Next(0, int.MaxValue);

                string symbols = B32ID.Encode(id);

                int symbol1ToChange = random.Next(0, symbols.Length - 1);
                int symbol1ToPlace = random.Next(0, errorSymbolPool.Length - 2);
                int symbol2ToChange = (symbol1ToChange + random.Next(1, symbols.Length - 1)) % symbols.Length;
                int symbol2ToPlace = random.Next(0, errorSymbolPool.Length - 2);

                char errorSymbol1 = errorSymbolPool.Replace($"{symbols[symbol1ToChange]}", "")[symbol1ToPlace];
                char errorSymbol2 = errorSymbolPool.Replace($"{symbols[symbol2ToChange]}", "")[symbol2ToPlace];

                var sb = new StringBuilder(symbols);
                sb[symbol1ToChange] = errorSymbol1;
                sb[symbol2ToChange] = errorSymbol2;
                string symbolsWithError = sb.ToString();

                try
                {
                    B32ID.Decode(symbolsWithError);
                }
                catch (B32ID.DecodingException)
                {
                    continue;
                }
                failCount++;
            }
            double detectionRate = (double)(testCount - failCount) / (double)testCount;
            if (detectionRate < minimumRate)
                Assert.Fail($"Dual symbol error detection rate is {detectionRate * 100.0:0.00} % which is less than minimum expected detection rate ({minimumRate * 100.0:0.00} %).");
        }

        [TestMethod]
        public void DuplicateSymbolErrorDetection()
        {
            int testCount = 50000;
            int seed = 7181;
            double minimumRate = 0.985; // Expect at least 98.5% of duplication errors to be detected

            Random random = new Random(seed);
            int failCount = 0;
            for (int run = 0; run < testCount; run++)
            {
                uint id = (uint)random.Next(0, int.MaxValue);

                string symbols = B32ID.Encode(id);

                int symbolToDuplicate = random.Next(0, symbols.Length - 1);

                var sb = new StringBuilder(symbols);
                sb.Insert(symbolToDuplicate, sb[symbolToDuplicate]);
                string symbolsWithError = sb.ToString();

                try
                {
                    B32ID.Decode(symbolsWithError);
                }
                catch (B32ID.DecodingException)
                {
                    continue;
                }
                failCount++;
            }
            double detectionRate = (double)(testCount - failCount) / (double)testCount;
            if (detectionRate < minimumRate)
                Assert.Fail($"Symbol duplication error detection rate is {detectionRate * 100.0:0.00} % which is less than minimum expected detection rate ({minimumRate * 100.0:0.00} %).");
        }

        [TestMethod]
        public void AdjacientSymbolSwapErrorDetectionRate()
        {
            int testCount = 50000;
            int seed = 7918;
            double minimumRate = 0.965; // Expect at least 96.5% of swaps to be detected
            double minimumCoverage = 0.95; // At least 95% of tests must be validated, i.e. at most 5% skipped due to false positives 

            int actualCount = 0;
            Random random = new Random(seed);
            int failCount = 0;
            for (int run = 0; run < testCount; run++)
            {
                uint id = (uint)random.Next(0, int.MaxValue);

                string symbols = B32ID.Encode(id);

                int symbolIndexA = random.Next(0, symbols.Length - 1);
                int symbolIndexB = (symbolIndexA + 1) % symbols.Length;

                var sb = new StringBuilder(symbols);
                char symbolA = sb[symbolIndexA];
                sb[symbolIndexA] = sb[symbolIndexB];
                sb[symbolIndexB] = symbolA;
                string symbolsWithError = sb.ToString();

                if (symbolsWithError == symbols)
                    continue; // avoid false positive

                actualCount++;
                try
                {
                    B32ID.Decode(symbolsWithError);
                }
                catch (B32ID.DecodingException)
                {
                    continue;
                }
                failCount++;
            }

            double coverage = (double)(actualCount) / (double)testCount;
            if (coverage < minimumCoverage)
                Assert.Fail($"Test coverage is {coverage * 100.0:0.00} % which is less than minimum expected ({minimumCoverage * 100.0:0.00} %).");

            double detectionRate = (double)(testCount-failCount) / (double)testCount;
            if (detectionRate < minimumRate)
                Assert.Fail($"Adjacent symbol swap detection rate is {detectionRate*100.0:0.00} % which is less than minimum expected detection rate ({minimumRate*100.0:0.00} %).");
        }

        [TestMethod]
        public void RandomSymbolSwapErrorDetectionRate()
        {
            int testCount = 50000;
            int seed = 1345;
            double minimumRate = 0.79; // Expect at least 79% of swaps to be detected
            double minimumCoverage = 0.95; // At least 95% of tests must be validated, i.e. at most 5% skipped due to false positives 

            int actualCount = 0;
            Random random = new Random(seed);
            int failCount = 0;
            for (int run = 0; run < testCount; run++)
            {
                uint id = (uint)random.Next(0, int.MaxValue);

                string symbols = B32ID.Encode(id);

                int symbolIndexA = random.Next(0, symbols.Length - 1);
                int symbolIndexB = (symbolIndexA + random.Next(1, symbols.Length - 1)) % symbols.Length;

                var sb = new StringBuilder(symbols);
                char symbolA = sb[symbolIndexA];
                sb[symbolIndexA] = sb[symbolIndexB];
                sb[symbolIndexB] = symbolA;
                string symbolsWithError = sb.ToString();

                if (symbolsWithError == symbols)
                    continue; // avoid false positive

                actualCount++;

                try
                {
                    B32ID.Decode(symbolsWithError);
                }
                catch (B32ID.DecodingException)
                {
                    continue;
                }
                failCount++;
            }

            double coverage = (double)(actualCount) / (double)testCount;
            if (coverage < minimumCoverage)
                Assert.Fail($"Test coverage is {coverage * 100.0:0.00} % which is less than minimum expected ({minimumCoverage * 100.0:0.00} %).");

            double detectionRate = (double)(testCount - failCount) / (double)testCount;
            if (detectionRate < minimumRate)
                Assert.Fail($"Adjacent symbol swap detection rate is {detectionRate * 100.0:0.00} % which is less than minimum expected detection rate ({minimumRate * 100.0:0.00} %).");
        }

        //public void RejectInvalidInput
    }
}
