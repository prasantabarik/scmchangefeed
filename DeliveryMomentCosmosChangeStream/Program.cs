using System;
using System.Threading.Tasks;
using TCS.MVP.DeliveryMoment.DeliveryMomentCosmosRepository;
namespace TCS.MVP.DeliveryMoment.DeliveryMomentCosmosChangeStream
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Strat Program.Main.StartChangeStream");
            await DeliveryMomentChangeStream.StartChangeStream();
            Console.WriteLine("End Program.Main.StartChangeStream");
        }
    }
}
