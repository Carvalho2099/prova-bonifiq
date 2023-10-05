using ProvaPub.Models;


namespace ProvaPub.Services
{
    public class OrderService
    {

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            //Faz pagamento enviando paymentMethod ...

            return await Task.FromResult(new Order()
            {
                Value = paymentValue
            });
        }
    }
}
