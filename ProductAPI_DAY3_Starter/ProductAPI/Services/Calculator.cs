namespace ProductAPI.Services
{
    public interface ICalculator
    {
        int Add(int a, int b);
    }

    public class Calculator : ICalculator
    {
        public int Add(int a, int b)
        {
            throw new NotImplementedException();
        }
    }
}
