using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculationWorks.BusinessModel;

namespace BcfSudoku.Bcf
{
    internal static class Util
    {

        static Util()
        {
            BitCountRegister = new int[512];
            BitRegister = new int[512][];
            for (var i = 0; i < 512; i++)
            {
                var bitCount = 0;
                var flags = new List<int>();
                for (var bitIndex = 0; bitIndex < 9; bitIndex++)
                {
                    var flag = (1 << bitIndex);
                    if ((i & flag) != 0)
                    {
                        bitCount++;
                        flags.Add(flag);
                    }
                }
                BitCountRegister[i] = bitCount;
                BitRegister[i] = flags.ToArray();
            }
        }

        /// <summary>
        /// Int array containing the bit-count at each index
        /// </summary>
        public static readonly int[] BitCountRegister;
        public static readonly int[][] BitRegister;

        /// <summary>
        /// All bits
        /// </summary>
        public const int AllBits = 511;

        /// <summary>
        /// Returns a bitmask containing bits missing in all related clusters; or 0 if cellsValue != 0
        /// </summary>
        /// <param name="cellValue">The cells value.</param>
        /// <param name="clusterMissingValues">Missing values (bitmask) of related clusters.</param>
        public static int GetCellAllowedValues(int cellValue, IEnumerable<int> clusterMissingValues)
        {
            if (cellValue != 0) return 0;
            return clusterMissingValues.Aggregate(AllBits, (agg, mask) => agg & mask);
        }

        /// <summary>
        /// Returns clusters missing values as bit mask (0=none; 1=1; 2=2; 3=1+2; 511=1+2+4+8+16+32+64+128+256).
        /// </summary>
        public static int GetClusterMissingValues(IEnumerable<int> cellValues)
        {
            return AllBits ^ cellValues.Aggregate((agg, bit) => agg | bit);
        }

        /// <summary>
        /// Returns an array containing number of possible target-cells for each allowed value.
        /// </summary>
        public static int[] GetClusterMissingValuesTargetCount(int[] missingValuesArray, int[] allowed)
        {
            var targetCount = new int[missingValuesArray.Length];
            for (var index = 0; index < missingValuesArray.Length; index++)
            {
                var value = missingValuesArray[index];
                var count = 0;
                for (var i = 0; i < allowed.Length; i++)
                {
                    var allowedValues = allowed[i];
                    if ((allowedValues & value) != 0) count++;
                }
                targetCount[index] = count;
            }
            return targetCount;
        }

        /// <summary>
        /// Returns an error-message if cluster is invalid
        /// </summary>
        public static BcfFaultMessage ClusterIsValid(int[] missingValuesArray, int[] missingValuesTargetCount, IEnumerable<int> cellValues)
        {
            foreach (var cnt in missingValuesTargetCount)
            {
                if (cnt == 0) return new BcfFaultMessage("no cell for value");
            }
            var count = 0;
            foreach (var v in cellValues)
            {
                if (v != 0) count++;
            }
            if (missingValuesArray.Length != (9 - count)) return new BcfFaultMessage("value duplicate");
            return null;
        }

        /// <summary>
        /// Returns source in a randomized order.
        /// </summary>
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source, Random rnd)
        {
            var list = source.ToList();
            while (list.Count != 0)
            {
                var index = rnd.Next(list.Count);
                yield return list[index];
                list.RemoveAt(index);
            }
        }

        /// <summary>
        /// Returns the char representation of a value
        /// </summary>
        /// <param name="cellValue">The value flag.</param>
        /// <exception cref="System.ArgumentException">cellValue is not a single bit</exception>
        public static char CellValueToChar(int cellValue)
        {
            if (cellValue == 0) return '0';
            for (var i = 0; i < 9; i++)
            {
                var mask = 1 << i;
                if (cellValue == mask) return (char)(i + '1');
            }
            throw new ArgumentException("not a single bit", nameof(cellValue));
        }

        /// <summary>
        /// Returns the value flag of a decimal
        /// </summary>
        /// <param name="chr">The char.</param>
        public static int CharToCellValue(char chr)
        {
            if (chr == '0') return 0;
            return 1 << (chr - '1');
        }
    }
}
