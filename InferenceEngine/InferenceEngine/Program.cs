using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            string searchMethodName = args[0]; //search method name
            string fileName = args[1]; //input file name

            string currentDirectory = Directory.GetCurrentDirectory(); //text file needs to be placed in the current directory 

            try
            {
                string[] data = File.ReadAllLines(currentDirectory + "\\" + fileName);

                InferenceEngine inferenceEngine = new InferenceEngine(data);

                //List of search methods available 
                List<SearchMethod> searchMethods = new List<SearchMethod>();
                searchMethods.Add(new TruthTable(inferenceEngine.HornClauses));
                searchMethods.Add(new ForwardChaining(inferenceEngine.HornClauses));
                searchMethods.Add(new BackwardChaining(inferenceEngine.HornClauses));

                bool searchMethodFound = false; //search method error flag

                foreach (SearchMethod s in searchMethods)
                {
                    if (searchMethodName.ToLower() == s.ShortName.ToLower())
                    {
                        searchMethodFound = true; //set flag
                        Console.WriteLine(s.Solve(inferenceEngine.Q));
                    }
                }

                //search method error
                if (searchMethodFound == false)
                {
                    Console.WriteLine("Error : Search method not found");
                }

            }
            catch (FileNotFoundException e)
            {
                // FileNotFoundExceptions are handled here.
                Console.WriteLine("Error : Invalid File Name Provided");
                Console.WriteLine(e);
            }
        }
    }
}
