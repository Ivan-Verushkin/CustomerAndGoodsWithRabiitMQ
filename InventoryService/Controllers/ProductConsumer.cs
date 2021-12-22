using MassTransit;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService
{
    internal class ProductConsumer : IConsumer<DeliverMessage>
    {
        private readonly ILogger<ProductConsumer> logger;

        ProductContext db;

        public ProductConsumer(ProductContext context,ILogger<ProductConsumer> logger)
        {
            this.logger = logger;
            db = context;
            if (!db.Products.Any())
            {
                db.Products.Add(new Product { Name = "Book", Description = "Harry Potter" });
                db.Products.Add(new Product { Name = "Car", Description = "Tesla" });
                db.SaveChanges();
            }
        }

        public ILogger<ProductConsumer> Logger { get; }

        public async Task Consume(ConsumeContext<DeliverMessage> context)
        {
            //DeliverMessage deliverMessage = new DeliverMessage();
            TypeOfRequest requestType = context.Message.typeOfRequest;

            switch (requestType)
            {
                case TypeOfRequest.PostRequest:
                    db.Products.Add(new Product { Name = context.Message.product.Name, Description = context.Message.product.Description });
                    break;
                case TypeOfRequest.PutRequest:
                    db.Update(context.Message.product);
                    break;
                case TypeOfRequest.DeleteRequest:
                    Product product = db.Products.FirstOrDefault(x => x.Id == context.Message.id);
                    db.Products.Remove(product);
                    break;
            }

            //db.Products.Add(new Product { Name = context.Message.product.Name, Description = context.Message.product.Description });
            await db.SaveChangesAsync();

            ////message test with console
            //await Console.Out.WriteLineAsync(context.Message.product.Name);
            //await Console.Out.WriteLineAsync(context.Message.product.Description);
            //logger.LogInformation($"Got new message {context.Message.product.Description}");
            //logger.LogInformation($"Got new message {context.Message.product.Description}");
        }

    }

}
