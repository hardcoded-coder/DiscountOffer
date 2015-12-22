using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DiscountOffers
{
	class Program
	{
		static void Main(string[] args)
		{
            try
            {
                //Take the input from file.
                string fileName = @"C:\Users\subrata\Documents\Visual Studio 2012\Projects\DiscountOffers\DiscountOffers\input.txt";
                var file = new StreamReader(fileName);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var processedInput = new ProcessedInput(line);
                    var weightedGraph = processedInput.CreateGraphFromInput();
                    double maxSS = CalculateMaxSSFromMatrix(weightedGraph);
                    Console.WriteLine(maxSS);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("error {0}", e.Message);
            }
        }

        private static double CalculateMaxSSFromMatrix(double[,] weightedGraph)
        {
            double result = 0;
            double[,] minimizationMatrix = ConvertIntoMinimizationMatrix(weightedGraph);
            HungarianAlgorithm algo = new HungarianAlgorithm(minimizationMatrix);
            var res = algo.Run();
            for(int i = 0;i < weightedGraph.GetLength(0); i++)
            {
                result += weightedGraph[i , res[i]];
            }
            return result;
        }

        private static double[,] ConvertIntoMinimizationMatrix(double[,] weightedGraph)
        {
            //Get the maximun value from the matrix
            int i = 0, j = 0;
            double max = double.MinValue;
            int maxRow=weightedGraph.GetLength(0), maxCol=weightedGraph.GetLength(1); 
            for(i=0; i < maxRow;i++)
            {
                //Expecting that the number of col is constant and same;
                for(j = 0; j< maxCol ;j++)
                {
                    if (weightedGraph[i,j] > max)
                    {
                        max = weightedGraph[i,j];
                    }
                }
            }

            //Balance the matrix with dummy row if it's not balanced and initilize it to 0
            int balancedRank = maxCol > maxRow ? maxCol : maxRow;
            double[,] minimizationMatrix = new double[balancedRank, balancedRank];
            for (i = 0; i < balancedRank; i++)
            {
                for (j = 0; j < balancedRank; j++)
                {
                    minimizationMatrix[i,j] = 0;
                }
            }

            //subtract other element from max and convert it to a minimization matrix.
            for (i=0; i<maxRow; i++)
            {
                for( j =0; j < maxCol; j++)
                {
                    minimizationMatrix[i,j] = max - weightedGraph[i,j];
                }
            }

            //Get the minimum of each row and subtract it from other elements
            double min = 0;
            for(i=0;i<balancedRank;i++)
            {
                min = double.MaxValue;
                for(j=0;j<balancedRank;j++)
                {
                    //find min
                    if (min > minimizationMatrix[i,j])
                        min = minimizationMatrix[i,j];
                }
                for (j = 0; j < balancedRank; j++)
                {
                    //subtract minimum
                    minimizationMatrix[i,j] -= min;
                }
            }

            return minimizationMatrix;
        }
       
        //It's responsibility would be create a weighted graph out of input file.
        class ProcessedInput
		{
			string[] customerNames;
			string[] productNames;
			string inputString;
			double[,] weightedGraph;
			
			public ProcessedInput(string inputFileName)
			{
				this.inputString = inputFileName;
			}

			public double[,] CreateGraphFromInput()
			{
				ExtractCustomerNames();
                if(customerNames == null || customerNames.Length == 0 || productNames == null || productNames.Length == 0)
                {
                    return null;
                }
                weightedGraph = new double[productNames.Length, customerNames.Length];
                int i = 0; int j = 0;
                foreach (var product in productNames)
                {
                    double ss = 0;
                    j = 0;
                    int productLetterCount = GetTotalLetters(product);
                    foreach (var customer in customerNames)
                    {
                        ss = 0;
                        int vowelsCount = 0;
                        //1.If the number of letters in the product's name is even then the SS is the number of vowels (a, e, i, o, u, y) in the customer's name multiplied by 1.5.
                        if(productLetterCount % 2 == 0)
                        {
                            vowelsCount = GetVowelsCount(customer);
                            ss = vowelsCount * 1.5;
                        }
                        //2.If the number of letters in the product's name is odd then the SS is the number of consonants in the customer's name. 
                        else
                        {
                            var consonantCount = GetConsonantCount(customer);
                            ss = consonantCount;
                        }
                        //3.If the number of letters in the product's name shares any common factors (besides 1) with the number of letters in the customer's name then the SS is multiplied by 1.5.
                        var gcd = BigInteger.GreatestCommonDivisor(productLetterCount, GetTotalLetters(customer));
                        if(gcd > 1)
                        {
                            ss *= 1.5;
                        }
                        weightedGraph[i,j] = ss;
                        j++;
                    }
                    i++;
                }

                return weightedGraph;
			}

            private int GetTotalLetters(string product)
            {
                return GetConsonantCount(product) + GetVowelsCount(product);
            }

            //Ref: http://stackoverflow.com/questions/6331964/counting-number-of-vowels-in-a-string
            //Asuumption is the customer and product names are in english only.
            private int GetVowelsCount(string customer)
            {
                const string vowels = "aeiou";
                return customer.Count(chr => vowels.Contains(char.ToLower(chr)));
            }

            private int GetConsonantCount(string customer)
            {
                const string consonants = "bcdfghjklmnpqrstvwxyz";
                return customer.Count(chr => consonants.Contains(char.ToLower(chr)));
            }

            private void ExtractCustomerNames()
			{
                var lines = this.inputString.Split(';');
                this.customerNames = lines[0].Split(',');
                this.productNames = lines[1].Split(',');
			} 

		}
	}
}
