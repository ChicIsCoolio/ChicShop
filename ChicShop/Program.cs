using System;
using System.Threading.Tasks;
using Fortnite_API;

namespace ChicShop
{
    public class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            var api = new FortniteApi();

            Console.WriteLine(api.V2.Cosmetics.GetBr("CID_222_Athena_Commando_F_DarkViking").Data.Name);

            await Task.Delay(-1);
        }
    }
}
