using Benday.CalculatorDemo.Api;

namespace Benday.CalculatorDemo.UnitTests;

[TestClass]
public class CalculatorFixture
{
    [TestInitialize]
    public void OnTestInitialize()
    {
        _systemUnderTest = null;
    }

    private Calculator? _systemUnderTest;

    private Calculator SystemUnderTest
    {
        get
        {
            if (_systemUnderTest == null)
            {
                _systemUnderTest = new Calculator();
            }

            return _systemUnderTest;
        }
    }

    [TestMethod]
    public void Add()
    {
        // arrange
        var expected = 5;
        var value1 = 2;
        var value2 = 3;

        // act
        var actual = SystemUnderTest.Add(value1, value2);

        // assert
        Assert.AreEqual<int>(expected, actual, $"Wrong value");
    }

    [TestMethod]
    public void Subtract()
    {
        // arrange
        var expected = -1;
        var value1 = 2;
        var value2 = 3;

        // act
        var actual = SystemUnderTest.Subtract(value1, value2);

        // assert
        Assert.AreEqual<int>(expected, actual, $"Wrong value");
    }

    [TestMethod]
    public void Multiply()
    {
        // arrange
        var expected = 6;
        var value1 = 2;
        var value2 = 3;

        // act
        var actual = SystemUnderTest.Multiply(value1, value2);

        // assert
        Assert.AreEqual<int>(expected, actual, $"Wrong value");
    }

    [TestMethod]
    public void Divide()
    {
        // arrange
        var expected = 5;
        var value1 = 10;
        var value2 = 2;

        // act
        var actual = SystemUnderTest.Divide(value1, value2);

        // assert
        Assert.AreEqual<int>(expected, actual, $"Wrong value");
    }

    private string? _memberVariable1 = null;
    private readonly string _memberVariable2 = "hello";
    private readonly string _memberVariable3 = "thingy";
    private readonly string _memberVariable4 = "wheee!";

    [TestMethod]
    public void UselessTest()
    {
        // arrange
        _memberVariable1 = Guid.NewGuid().ToString();

        // act
        Console.WriteLine($"{_memberVariable1}");
        Console.WriteLine($"{_memberVariable2}");
        Console.WriteLine($"{_memberVariable3}");
        Console.WriteLine($"{_memberVariable4}");

        // assert
        // Assert.Inconclusive();
    }
}