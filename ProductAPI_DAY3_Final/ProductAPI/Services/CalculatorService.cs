namespace ProductAPI.Services
{
    public class CalculatorService
    {
        private readonly ICalculator _calculator;
        public CalculatorService(ICalculator calculator)
        {
            _calculator = calculator;
        }

        public int AddNumbers(int a, int b)
        {           
            //TODO....
            return _calculator.Add(a, b);
        }

    }
}
