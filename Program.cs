using System;
using System.Numerics;
using System.Reflection;

namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define what we want to look at
            // First the string, then the datattype
            var numericTypes = new object[,] {
              {"Byte" , typeof(byte)},
              {"SByte" , typeof(sbyte)},
              {"short" , typeof(short)},
              {"ushort" , typeof(ushort)},
              {"int" , typeof(int)},
              {"uint" , typeof(uint)},
              {"long" , typeof(long)},
              {"ulong" , typeof(ulong)},
              {"decimal" , typeof(decimal)},
              {"float" , typeof(float)},
              {"double" , typeof(double)},
              {"Decimal" , typeof(System.Decimal)},
              {"Single" , typeof(System.Single)},
              {"Double" , typeof(System.Double)},
              {"Int16" , typeof(System.Int16)},
              {"Int32" , typeof(System.Int32)},
              {"Int64" , typeof(System.Int64)},
              {"UInt16" , typeof(System.UInt16)},
              {"UInt32" , typeof(System.UInt32)},
              {"UInt64" , typeof(System.UInt64)},
              {"BigInteger" , typeof(System.Numerics.BigInteger) }
            };

            // Print out header
            Console.WriteLine($"Name,Type,Is derived,Bit size,Max precision,Max string length,Has negative,Max value,Min value");

            // Iterate through the types
            for (var n = 0; n < numericTypes.Length / 2; n += 1)
            {
                // Get the name
                var name = (string)numericTypes[n, 0];
                // Get the type
                var t = (Type)numericTypes[n, 1];
                // Make an object from the type
                dynamic obj = Activator.CreateInstance(t);

                // The data variables we will populate
                dynamic minValue = null;
                dynamic maxValue = null;
                string minValueLength;
                string maxValueLength;
                string maxStringLength;

                // BigInteger works differently
                if (t == typeof(BigInteger))
                {
                    minValue = "-∞";
                    maxValue = "∞";
                    minValueLength = "-∞";
                    maxValueLength = "∞";
                    maxStringLength = "∞";
                }
                else
                {
                    // Casting object
                    object i = null;

                    // Get the min max values
                    minValue = t.InvokeMember("MinValue", BindingFlags.GetField, null, i, null);
                    maxValue = t.InvokeMember("MaxValue", BindingFlags.GetField, null, i, null);

                    // Get the full string of min max values
                    minValue = minValue.ToString("F99", System.Globalization.CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
                    maxValue = maxValue.ToString("F99", System.Globalization.CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');

                    // Get the number of characters the values could take up
                    minValueLength = minValue.ToString().Length.ToString();
                    maxValueLength = maxValue.ToString().Length.ToString();
                    maxStringLength = Math.Max(int.Parse(minValueLength), int.Parse(maxValueLength)).ToString();
                }

                // If the descriptive name isn't the actual type
                var isDerived = name != t.Name ? "*" : "";

                // Get the object size in bytes and convert to bit
                var bitSize = System.Runtime.InteropServices.Marshal.SizeOf(obj) * 8;

                // Can this be negative
                var hasNegative = minValue.ToString().Contains('-') ? "*" : "";

                // Find smallest number
                // Make it 1 by incrementing it. If we set it to 1 then the object type changes
                obj++;

                dynamic s1 = obj;
                dynamic s2 = "";
                int p = -2;
                // Find how many decimals we can hold in this object type
                while (s1.ToString() != s2.ToString())
                {
                    s2 = s1;
                    // Divide it by 10
                    s1 = s1 / 10;
                    p++;
                };

                // Print out the results
                Console.WriteLine($"{name},{t.Name},{isDerived},{bitSize},{p},{maxStringLength},{hasNegative},{maxValue},{minValue.ToString()}");

            }
        }
    }
}
