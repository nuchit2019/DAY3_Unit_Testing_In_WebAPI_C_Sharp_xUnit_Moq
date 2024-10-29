using Moq;
using ProductAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProductAPI.Services
{
    public class CalculatorServiceTests
    {
        [Fact (DisplayName ="ป้อน 2 และ 3 ต้องได้ 5", Skip ="TODO...")]
        public void AddNumber_ShouldReturnCorrectSum()
        {
            // Arrange
            // 2+3 =5
            int expected = 5;
            var mock = new Mock<ICalculator>();
            mock.Setup(c => c.Add(2, 3)).Returns(5);
            var service = new CalculatorService(mock.Object);

            // Act
            var actual = service.AddNumbers(2, 3);

            // Assert
            Assert.Equal(expected, actual);


        }


        [Theory]
        [InlineData(2,3,5)]
        [InlineData(0, 0, 0)]
        [InlineData(10, 10, 20)]
        [InlineData(100, 100, 200)]
        public void AddNumber_ShouldReturnCorrectSum1(int a, int b,int expected)
        {
            // Arrange 
            var mock = new Mock<ICalculator>();
            mock.Setup(c => c.Add(a, b)).Returns(expected);
            var service = new CalculatorService(mock.Object);

            // Act
            var actual = service.AddNumbers(a, b);

            // Assert
            Assert.Equal(expected, actual);
            mock.Verify(x=> x.Add(a, b),Times.Once);

        }
    }
}
