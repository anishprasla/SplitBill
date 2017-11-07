using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitBill
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFile = @"..\..\IO files\expenses.txt";

                //Check if file exists
                if (File.Exists(inputFile))
                {
                    Console.WriteLine("Sample Input:");
                    ReadFile(inputFile);
                }
                else
                {
                    Console.WriteLine("File not found");
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.Error.WriteLine("Directory doesn't exist");
            }

        }

        public static void ReadFile(string fileName)
        {
            try
            {
                // Create an instance of StreamReader to read from a file
                StreamReader reader = new StreamReader(fileName);

                using (reader)
                {
                    // Read first line from the text file
                    string line = reader.ReadLine();

                    //n = number of participants, p = number of receipts
                    int p = 0, n = 0;
                    List<decimal> tempReceipt = new List<decimal>();
                    List<decimal> participantExpenses = new List<decimal>();
                    List<string> expenseSplit = new List<string>();

                    /***
                     * Steps:
                     * tempReceipt list holds all receipt for current participant
                     * participantExpense list holds total expense spend by particiapnt
                     * expenseSplit list holds how much he/she must pay or be paid 
                     * **/
                    while (line != null)
                    {
                        Console.WriteLine(decimal.Parse(line));
                        if (line.Contains("."))
                        {
                            decimal amount = decimal.Parse(line);
                            tempReceipt.Add(amount);

                            //add all receipt expense in tempReceipt list when list count equals to total no. of participant receipts
                            if (tempReceipt.Count == p)
                            {
                                
                                decimal test = tempReceipt.Sum();
                                participantExpenses.Add(tempReceipt.Sum());

                                if (participantExpenses.Count == n)
                                {
                                    //adding all expenses and rounding close to 2 decimal
                                    decimal total = participantExpenses.Sum();
                                    total = Math.Round(total / n, 2);
                                    foreach (var exp in participantExpenses)
                                    {
                                        //calculating how much he/she must pay or to be paid
                                        decimal split = Math.Round((total - exp), 2);
                                        if (split >= 0)
                                            expenseSplit.Add(split.ToString());
                                        else
                                            expenseSplit.Add("(" + split.ToString().Replace("-", "") + ")");
                                    }
                                    expenseSplit.Add("");
                                    n = 0;
                                    participantExpenses.Clear();
                                }
                                tempReceipt.Clear();

                            }
                        }
                        else if (int.Parse(line) > 0)
                        {
                            if (n > 0)
                                p = int.Parse(line);
                            else
                            {
                                n = int.Parse(line);
                            }
                        }
                        else
                        {
                            // when line contains zero then Write expenseSplit list in new file
                            WriteFile(expenseSplit);
                        }
                        line = reader.ReadLine();
                    }
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Cannot read file {0}", fileName);
            }
        }

        public static void WriteFile(List<string> split)
        {
            try
            {
                //Write new file with .out 
                string outputFile = @"..\..\IO files\expenses.txt.out";
                StreamWriter writer = new StreamWriter(outputFile, false);
                Console.WriteLine("\nOutput for Sample Input:");

                using (writer)
                {
                    //loop through the list
                    foreach (var exp in split)
                    {
                        Console.WriteLine(exp);
                        writer.WriteLine(exp);
                    }
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Cannot write file");
            }
        }

        
    }
}
