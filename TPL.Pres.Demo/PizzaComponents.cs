using System;
using System.Threading;

namespace TPL.Pres.Demo
{

    #region pizza parts
    public class Commande
    {
        public string NomPizza;

        public Ingredients GetAndPrepareIngredients()
        {
            Console.WriteLine($"Preparing Command For Pizza {NomPizza}.");
            if (NomPizza == "Reine")
                return new Ingredients(NomPizza, "Normale", "Tomate", "Jambon");
            else if (NomPizza == "Cheesy")
                return new Ingredients(NomPizza, "Fourrée au Fromage", "Tomate", "Chevres");
            else
                return new Ingredients(NomPizza, "Normale", "Barbecue", "Boulettes de Proc");

        }
    }

    public class Ingredients
    {
        public Ingredients(string name, string pate, string sauce, string garniture)
        {
            Name = name;
            Pâte = pate;
            Sauce = sauce;
            Garniture = garniture;
        }

        public string Name { get; set; }
        public string Pâte { get; private set; }
        public string Sauce { get; private set; }
        public string Garniture { get; private set; }
    }

    public class Pizza
    {
        public static Pizza Make(Ingredients ingredients)
        {
            Console.WriteLine($"Making Pizza {ingredients.Name}.");
            return new Pizza(ingredients);
        }

        private Pizza(Ingredients ingredients)
        {
            Name = ingredients.Name;
            Pâte = ingredients.Pâte;
            Sauce = ingredients.Sauce;
            Garniture = ingredients.Garniture;
        }

        public float DeliverAndGetPayment()
        {
            Thread.Sleep(2000);
            Console.WriteLine($"Delivered Pizza {Name}.");
            return (float)Math.Round(8 + (new Random().NextDouble() * 8), 2);
        }

        public string Name { get; set; }
        public string Pâte { get; private set; }
        public string Sauce { get; private set; }
        public string Garniture { get; private set; }
    }

    #endregion
}
