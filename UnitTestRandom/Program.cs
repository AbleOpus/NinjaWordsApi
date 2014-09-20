using System;
using System.Net;
using System.Threading;
using NinjaWordsApi;

namespace UnitTestRandom
{
    /// <summary>
    /// Throttles the service with random requests.
    /// </summary>
    class Program
    {            
        // Increase throttle interval for long tests as to not
        // get your IP blocked and to be considerate
        const int INTERVAL = 0;
        const int LOOKUPS = 500;

        static void Main(string[] args)
        {
            BeginRandomLookups();
        }

        /// <summary>
        /// Begin interating random words to test robustness of API.
        /// </summary>
        private static void BeginRandomLookups()
        {
            for (int i = 0; i < LOOKUPS; i++)
            {
                Thread.Sleep(INTERVAL);

                try
                {
                    NinjaTerm term = Ninja.GetRandomTerm();
                    Console.WriteLine((i + 1) + ": " + term.Term);
                }
                catch (WebException)
                {
                    Console.WriteLine("****************");
                    Console.WriteLine("WebException");
                    Console.WriteLine("****************");
                }
                catch (CategoryNotEnumeratedException ex)
                {
                    Console.WriteLine("****************");
                    Console.WriteLine("CategoryNotEnumeratedException on category: " + ex.CategoryString);
                    Console.WriteLine("****************");
                }
            }

            Console.ReadLine();
        }
    }
}
