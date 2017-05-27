using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Pres.Demo.Pizzeria
{
    public abstract class PizzaRestaurentBase
    {
        public float Balance = 0;
        public TransformBlock<Commande, Ingredients> receptionBlock;
        public TransformBlock<Ingredients, Pizza> kitchenBlock;
        public TransformBlock<Pizza, float> deliveryBlock;
        public ActionBlock<float> cashRegisterBlock;

        public Task ServiceEnded => deliveryBlock.Completion;
        public void FinishService() { receptionBlock.Complete(); }

        public void NewCommand(string nomPizza) { receptionBlock.Post(new Commande() { NomPizza = nomPizza }); }
    }

    public class PizzaRestaraurent : PizzaRestaurentBase
    {
        public PizzaRestaraurent()
        {
            receptionBlock = new TransformBlock<Commande, Ingredients>(commande => commande.GetAndPrepareIngredients());
            kitchenBlock = new TransformBlock<Ingredients, Pizza>(ingredients => Pizza.Make(ingredients));
            deliveryBlock = new TransformBlock<Pizza, float>(pizza => pizza.DeliverAndGetPayment());
            cashRegisterBlock = new ActionBlock<float>(paiement => Balance += paiement);

            receptionBlock.LinkTo(kitchenBlock, new DataflowLinkOptions { PropagateCompletion = true });
            kitchenBlock.LinkTo(deliveryBlock, new DataflowLinkOptions { PropagateCompletion = true });
            deliveryBlock.LinkTo(cashRegisterBlock, new DataflowLinkOptions { PropagateCompletion = true });
        }
    }
}
