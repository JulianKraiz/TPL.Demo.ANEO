using System;
using System.Threading.Tasks;
using TPL.Pres.Demo.Pizzeria;
using TPL.Pres.Demo.WithCancellation;
using TPL.Pres.Demo.WithConcurrency;

namespace TPL.Pres.Demo
{
    class Program
    {
        static void Main(string[] args)
        {

            PizzaRestaurentBase restaurant = new PizzaRestaraurent();
            Task.Run(async () => // for async console execution
            {
                try
                {
                    DoService(restaurant);
                    restaurant.FinishService();
                    await restaurant.ServiceEnded;
                    Console.WriteLine($"Service Ended. New Balance ${restaurant.Balance}.");
                    Console.ReadLine();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Service Aborted.");
                    Console.ReadLine();

                }
            }).GetAwaiter().GetResult();
        }

        public static void DoService(PizzaRestaurentBase restaurant)
        {
            restaurant.NewCommand("Reine");
            restaurant.NewCommand("Cheesy");
            restaurant.NewCommand("Orientale");
            restaurant.NewCommand("Reine");
            restaurant.NewCommand("Cheesy");
        }
    }
}
