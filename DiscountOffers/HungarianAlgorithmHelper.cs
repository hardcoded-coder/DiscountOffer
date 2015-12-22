using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountOffers
{
    public class HungarianAlgorithmHelper
    {
        /// <summary>
        /// It returns the maximum suitability score.
        /// </summary>
        /// <param name="weightedGraph"></param>
        /// <returns>Maximized Assignment</returns>
        public static double CalculateMaxSSFromMatrix(double[,] weightedGraph)
        {
            if(weightedGraph == null || weightedGraph.Length == 0)
            {
                throw new Exception("Invalid WeightedGraph");
            }

            double result = 0;
            
            //Convert the weighted graph to minimization matrix before applying Hungarian Algorithm.
            double[,] minimizationMatrix = ConvertIntoMinimizationMatrix(weightedGraph);

            //I am using my customized version of the 3rd party implementation of hungarian algorithm.
            HungarianAlgorithm algo = new HungarianAlgorithm(minimizationMatrix);
            var res = algo.Run();
            for (int i = 0; i < weightedGraph.GetLength(0); i++)
            {
                result += weightedGraph[i, res[i]];
            }
            return result;
        }

        /// <summary>
        /// It converts a maximization matrix to a minimization matrix. Convert the matrix to NXN matrix with dummy row/column if required.
        /// </summary>
        /// <param name="weightedGraph"></param>
        /// <returns></returns>
        private static double[,] ConvertIntoMinimizationMatrix(double[,] weightedGraph)
        {
            //Get the maximun value from the matrix
            int i = 0, j = 0;
            double max = double.MinValue;
            double min = double.MaxValue;
            int maxRow = weightedGraph.GetLength(0), maxCol = weightedGraph.GetLength(1);
            int balancedRank = maxCol > maxRow ? maxCol : maxRow;
            double[,] minimizationMatrix = new double[balancedRank, balancedRank];
            
            //Finding the max element of the matrix
            for (i = 0; i < maxRow; i++)
            {
                for (j = 0; j < maxCol; j++)
                {
                    if (weightedGraph[i, j] > max)
                    {
                        max = weightedGraph[i, j];
                    }
                }
            }

            //Subtract other element from max and convert it to a minimization matrix.
            for (i = 0; i < maxRow; i++)
            {
                for (j = 0; j < maxCol; j++)
                {
                    minimizationMatrix[i, j] = max - weightedGraph[i, j];
                }
            }

            //Get the minimum of each row and subtract it from other elements          
            for (i = 0; i < balancedRank; i++)
            {
                min = double.MaxValue;
                for (j = 0; j < balancedRank; j++)
                {
                    //find min
                    if (min > minimizationMatrix[i, j])
                        min = minimizationMatrix[i, j];
                }
                for (j = 0; j < balancedRank; j++)
                {
                    //subtract minimum
                    minimizationMatrix[i, j] -= min;
                }
            }
            return minimizationMatrix;
        }
    }
}
