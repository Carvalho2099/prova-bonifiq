
namespace ProvaPub.Services
{
	public class RandomService 
    {
        public Task<int> GetRandom()
        {
            return  Task.Run(() => new Random().Next(100));
        }
    }
}
