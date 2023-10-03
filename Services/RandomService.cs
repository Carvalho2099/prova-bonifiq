namespace ProvaPub.Services
{
	public class RandomService
	{		
		public int GetRandom()
		{
			return new Random().Next(100);
		}

	}
}
