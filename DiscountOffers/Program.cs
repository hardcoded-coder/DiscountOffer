using System;
using System.IO;

namespace DiscountOffers
{
    class Program
	{
		static void Main(string[] args)
		{
            try
            {
                //Take the input from file.
                string fileName = Directory.GetCurrentDirectory()+@"\input.txt";

                //If input file name is specified during call, use that file.
                if (args.Length> 0 && args[0] != null)
                    fileName = args[0];
                
                //Not handling any file related exception explicitly. The try-catch would take care of it.
                var file = new StreamReader(fileName);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var processedInput = new InputProcessor(line);
                    var weightedGraph = processedInput.CreateGraphFromInput();
                    double maxSS = HungarianAlgorithmHelper.CalculateMaxSSFromMatrix(weightedGraph);
                    Console.WriteLine(maxSS);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("An error occured {0}", e.Message);
            }
        }
	}
}
