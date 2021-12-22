namespace Model
{
    public class DeliverMessage
    {
        public TypeOfRequest typeOfRequest;
        public Product product;
        public int id;

        public DeliverMessage()
        {

        }
        
        public DeliverMessage(TypeOfRequest typeOfRequest, Product product, int id)
        {
            this.typeOfRequest = typeOfRequest;
            this.product = product;
            this.id = id;
        }
    }
}
