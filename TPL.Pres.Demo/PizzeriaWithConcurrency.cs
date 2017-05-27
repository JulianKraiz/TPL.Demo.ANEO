using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TPL.Pres.Demo.Pizzeria;

namespace TPL.Pres.Demo.WithConcurrency
{
    public class PizzaRestaraurentWithConcurrency : PizzaRestaurentBase
    {
        public PizzaRestaraurentWithConcurrency()
        {
            receptionBlock = new TransformBlock<Commande, Ingredients>(commande => commande.GetAndPrepareIngredients());
            kitchenBlock = new TransformBlock<Ingredients, Pizza>(ingredients => Pizza.Make(ingredients));
            deliveryBlock = new TransformBlock<Pizza, float>(pizza => pizza.DeliverAndGetPayment(), new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 2 });
            cashRegisterBlock = new ActionBlock<float>(paiement => Balance += paiement);

            receptionBlock.LinkTo(kitchenBlock, new DataflowLinkOptions { PropagateCompletion = true });
            kitchenBlock.LinkTo(deliveryBlock, new DataflowLinkOptions { PropagateCompletion = true });
            deliveryBlock.LinkTo(cashRegisterBlock, new DataflowLinkOptions { PropagateCompletion = true });
        }
    }
}
