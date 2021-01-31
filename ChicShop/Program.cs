using System;
using System.Threading.Tasks;
using RestSharp;

namespace ChicShop
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            Console.WriteLine("Hello World but Async!");

            await Task.Delay(-1);
        }
    }
}
