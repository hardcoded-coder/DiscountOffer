using System;
using System.Linq;
using System.Numerics;

namespace DiscountOffers
{
    //It's responsibility would be create a weighted graph out of input file.
    public class InputProcessor
    {
        private string[] customerNames;
        private string[] productNames;
        private string inputString;
        private double[,] weightedGraph;

        public InputProcessor(string inputString)
        {
            this.inputString = inputString;
        }

        /// <summary>
        /// It converts the input string into a matrix representation of product-customer complete bipartite graph
        /// </summary>
        /// <returns>Matrix representation of product-customer bipartite graph.</returns>
        public double[,] CreateGraphFromInput()
        {
            ExtractProductAndCustomerNames();
            if (customerNames == null || customerNames.Length == 0 || productNames == null || productNames.Length == 0)
            {
                return null;
            }
            weightedGraph = new double[productNames.Length, customerNames.Length];
            int i = 0; int j = 0;
            //TODO (subrata): We can parallelize this loop.
            //Run super secret algorithm to get the sutability score.
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
                    if (productLetterCount % 2 == 0)
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
                    if (gcd > 1)
                    {
                        ss *= 1.5;
                    }
                    weightedGraph[i, j] = ss;
                    j++;
                }
                i++;
            }

            return weightedGraph;
        }

        private int GetTotalLetters(string input)
        {
            if (input == null)
                return 0;
            int total = 0;

            //Please uncomment this section if you want any non-space char as letter.
            //int space = input.Count(Char.IsWhiteSpace);
            //total = input.Length - space;

            //Please comment this section if you want any non-space char as letter.
            total = GetConsonantCount(input) + GetVowelsCount(input);
            return total;
        }

        private int GetVowelsCount(string input)
        {
            if (input == null)
                return 0;

            const string vowels = "aeiou";
            return input.Count(chr => vowels.Contains(char.ToLower(chr)));
        }

        private int GetConsonantCount(string input)
        {
            if (input == null)
                return 0;

            const string consonants = "bcdfghjklmnpqrstvwxyz";
            return input.Count(chr => consonants.Contains(char.ToLower(chr)));
        }

        private void ExtractProductAndCustomerNames()
        {
            if (this.inputString == null)
                throw new Exception("Invalid input string");

            var lines = this.inputString.Split(';');

            if (lines.Length != 2)
            {
                throw new Exception("Invalid input string:" + this.inputString);
            }
            this.customerNames = lines[0].Split(',');
            this.productNames = lines[1].Split(',');

            if(customerNames.Length == 0 || productNames.Length == 0)
            {
                throw new Exception("Invalid input string (customer or product name cannot be empty) :" + this.inputString);
            }
        }
    }
}
