using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TPL.Pres.Demo.Pizzeria;

namespace TPL.Pres.Demo.WithCancellation
{
    public class PizzaRestaraurentWithCancellation : PizzaRestaurentBase
    {
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        public PizzaRestaraurentWithCancellation()
        {
            receptionBlock = new TransformBlock<Commande, Ingredients>(commande => commande.GetAndPrepareIngredients(), new ExecutionDataflowBlockOptions() { CancellationToken = tokenSource.Token });
            kitchenBlock = new TransformBlock<Ingredients, Pizza>(ingredients => Pizza.Make(ingredients), new ExecutionDataflowBlockOptions() { CancellationToken = tokenSource.Token });
            deliveryBlock = new TransformBlock<Pizza, float>(pizza => Deliver(pizza), new ExecutionDataflowBlockOptions() { CancellationToken = tokenSource.Token, MaxDegreeOfParallelism = 2 });
            cashRegisterBlock = new ActionBlock<float>(paiement => Balance += paiement);

            receptionBlock.LinkTo(kitchenBlock, new DataflowLinkOptions { PropagateCompletion = true });
            kitchenBlock.LinkTo(deliveryBlock, new DataflowLinkOptions { PropagateCompletion = true });
            deliveryBlock.LinkTo(cashRegisterBlock, new DataflowLinkOptions { PropagateCompletion = true });
        }

        public float Deliver(Pizza pizza)
        {
            var payment = pizza.DeliverAndGetPayment();
            if (pizza.Name == "Orientale")
                tokenSource.Cancel();
            return payment;
        }
    }
}
