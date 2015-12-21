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
                    int maxSS = CalculateMaxSSFromMatrix(weightedGraph);
                    Console.WriteLine(maxSS);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("error {0}", e.Message);
            }
        }

        private static int CalculateMaxSSFromMatrix(double[,] weightedGraph)
        {
            int result = 0;
            double[,] minimizationMatrix = ConvertIntoMinimizationMatrix(weightedGraph);
            minimizationMatrix = GetAssignmentMatrix(minimizationMatrix);
            result = GetMaximumAssignmentValue(minimizationMatrix, weightedGraph);
            return result;
        }

        private static int GetMaximumAssignmentValue(double[,] minimizationMatrix, double[,] weightedGraph)
        {
            return 0;
        }

        private static double[,] GetAssignmentMatrix(double[,] minimizationMatrix)
        {
            //Find minimum number of lines required to cover all the 0s.
            List<Line> lines = GetMinimumLinesRequiredToCoverAllZero(minimizationMatrix);
           //If it does not matches with the the rank of matrix, do some extra staff, untill we get the one.
           while(lines.Count < minimizationMatrix.GetLength(0))
           {
                minimizationMatrix = DoSpecialTransformation(minimizationMatrix, lines);
                lines = GetMinimumLinesRequiredToCoverAllZero(minimizationMatrix);
            }
            //Find the row/columns which have exactly one zero and draw line. and remove other zeros from the row/column

            return minimizationMatrix;
        }

        private static double[,] DoSpecialTransformation(double[,] minimizationMatrix, List<Line> lines)
        {
            //Get minimum number which is not covered by lines.

            //Subtract it from the uncovered elements.

            //Add it to the intersection elements.
            return null;
        }

        private static Boolean isZero(int[] array)
        {
            foreach ( var val in array)
            {
                if (val != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static List<Line> GetMinimumLinesRequiredToCoverAllZero(double[,] minimizationMatrix)
        {
          //  List<Line> linesWithZero = GetAllLinesWithZero(minimizationMatrix);
          //  List<Line> minimalLine = new List<Line>();
           // double[,] tempMinimizationMatrix = new double[minimizationMatrix.GetLength(0),minimizationMatrix.GetLength(1)];
          //  Array.Copy(minimizationMatrix,0, tempMinimizationMatrix,0, minimizationMatrix.Length);
            

            int SIZE = minimizationMatrix.GetLength(0);
            int[] zerosPerRow = new int[SIZE];
            int[] zerosPerCol = new int[SIZE];

            // Count the number of 0's per row and the number of 0's per column        
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (minimizationMatrix[i,j] == 0)
                    {
                        zerosPerRow[i]++;
                        zerosPerCol[j]++;
                    }
                }
            }

            // There should be at must SIZE lines, 
            // initialize the list with an initial capacity of SIZE
            List<Line> lines = new List<Line>(SIZE);

            LineType lastInsertedLineType = LineType.NONE;

            // While there are 0's to count in either rows or colums...
            while (!isZero(zerosPerRow) && !isZero(zerosPerCol))
            {
                // Search the largest count of 0's in both arrays
                int max = -1;
                Line lineWithMostZeros = null;
                for (int i = 0; i < SIZE; i++)
                {
                    // If exists another count of 0's equal to "max" but in this one has
                    // the same direction as the last added line, then replace it with this
                    if (zerosPerRow[i] > max || (zerosPerRow[i] == max && lastInsertedLineType == LineType.HORIZONTAL))
                    {
                        lineWithMostZeros = new Line(i, LineType.HORIZONTAL);
                        max = zerosPerRow[i];
                    }
                }

                for (int i = 0; i < SIZE; i++)
                {
                    // Same as above
                    if (zerosPerCol[i] > max || (zerosPerCol[i] == max && lastInsertedLineType == LineType.VERTICAL))
                    {
                        lineWithMostZeros = new Line(i, LineType.VERTICAL);
                        max = zerosPerCol[i];
                    }
                }

                // Delete the 0 count from the line 
                if (lineWithMostZeros.isHorizontal())
                {
                    zerosPerRow[lineWithMostZeros.getLineIndex()] = 0;
                }
                else {
                    zerosPerCol[lineWithMostZeros.getLineIndex()] = 0;
                }


                int index = lineWithMostZeros.getLineIndex();
                if (lineWithMostZeros.isHorizontal())
                {
                    for (int j = 0; j < SIZE; j++)
                    {
                        if (minimizationMatrix[index,j] == 0)
                        {
                            zerosPerCol[j]--;
                        }
                    }
                }
                else {
                    for (int j = 0; j < SIZE; j++)
                    {
                        if (minimizationMatrix[j,index] == 0)
                        {
                            zerosPerRow[j]--;
                        }
                    }
                }

                // Add the line to the list of lines
                lines.Add(lineWithMostZeros);
                lastInsertedLineType = lineWithMostZeros.getLineType();
            }
            return lines;
        }

        //private static List<Line> GetAllLinesWithZero(double[,] minimizationMatrix)
        //{
        //    List<Line> linesWithZero = new List<Line>();
        //    int zeroCount = 0;
        //    int rank = minimizationMatrix.GetLength(0);
        //    for (int i = 0; i < rank; i++)
        //    {
        //        zeroCount = 0;
        //        for (int j = 0; j < rank; j++)
        //        {
        //            if (minimizationMatrix[i,j] == 0)
        //            {
        //                zeroCount++;
        //            }
        //        }
        //        if (zeroCount > 0)
        //        {
        //            linesWithZero.Add(new Line(new int[] { i, 0 }, new int[] { i, rank-1 }, zeroCount));
        //        }
        //    }

        //    for (int i = 0; i < rank; i++)
        //    {
        //        zeroCount = 0;
        //        for (int j = 0; j < rank; j++)
        //        {
        //            if (minimizationMatrix[j,i] == 0)
        //            {
        //                zeroCount++;
        //            }
        //        }
        //        if (zeroCount > 0)
        //        {
        //            linesWithZero.Add(new Line(new int[] { 0, i }, new int[] { rank-1, i }, zeroCount));
        //        }
        //    }

        //    List<Line> sorteList = linesWithZero.OrderByDescending(o => o.zeroCount).ToList();
        //    return sorteList;
        //}

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
        enum LineType { NONE, HORIZONTAL, VERTICAL }

        class Line
        {
            int lineIndex;
            LineType rowType;
            public Line(int lineIndex, LineType rowType)
            {
                this.lineIndex = lineIndex;
                this.rowType = rowType;
            }
            public LineType getLineType()
            {
                return rowType;
            }

            public int getLineIndex()
            {
                return lineIndex;
            }
            public Boolean isHorizontal()
            {
                return rowType == LineType.HORIZONTAL;
            }
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

		class BipartiteGraph
		{
			List<AdvancedEdge> Edges;
			BipartiteGraph(double[][] graph)
			{
				this.Edges = new List<AdvancedEdge>();

			}

		}

		class AdvancedEdge
		{
			double edgeValue;
			List<AdvancedEdge> victimEdges;

		}
	}
}
