using System.Threading.Tasks.Sources;

using Benday.CalculatorDemo.Api;
using Benday.CalculatorDemo.WebUi.Controllers;
using Benday.CalculatorDemo.WebUi.Models;

using Microsoft.AspNetCore.Mvc;

namespace Benday.CalculatorDemo.UnitTests;

[TestClass]
public class CalculatorControllerFixture
{
    [TestInitialize]
    public void OnTestInitialize()
    {
        _systemUnderTest = null;
    }

    private CalculatorController? _systemUnderTest;

    private CalculatorController SystemUnderTest
    {
        get
        {
            if (_systemUnderTest == null)
            {
                _systemUnderTest = new CalculatorController(new Calculator());
            }

            return _systemUnderTest;
        }
    }

    [TestMethod]
    public void Index_Get()
    {
        // arrange
        var expected = 0;
        var value1 = 0;
        var value2 = 0;

        // act
        var actual = SystemUnderTest.Index() as ViewResult;

        // assert
        Assert.IsNotNull(actual);
        var model = actual.Model as CalculatorViewModel;
        Assert.IsNotNull(model);

        Assert.AreEqual<int>(expected, model.Result, $"Result was wrong");
        Assert.AreEqual<int>(value1, model.Value1, $"Value1 was wrong");
        Assert.AreEqual<int>(value2, model.Value2, $"Value2 was wrong");
    }

    [TestMethod]
    public void Index_Post_Add()
    {
        // arrange
        var expected = 5;
        var value1 = 2;
        var value2 = 3;

        var input = new CalculatorViewModel()
        {
            Value1 = value1,
            Value2 = value2
        };

        // act
        var actual = SystemUnderTest.Index(input) as ViewResult;

        // assert
        Assert.IsNotNull(actual);
        var model = actual.Model as CalculatorViewModel;
        Assert.IsNotNull(model);

        Assert.AreEqual<int>(expected, model.Result, $"Result was wrong");
        Assert.AreEqual<int>(value1, model.Value1, $"Value1 was wrong");
        Assert.AreEqual<int>(value2, model.Value2, $"Value2 was wrong");
    }

    [TestMethod]
    public void Index_Post_Add_ResultMessage()
    {
        // arrange
        var expected = 7;
        var value1 = 5;
        var value2 = 2;

        var expectedResultMessage = "Whoomp! there it is: 5 + 2 = 7";

        var input = new CalculatorViewModel()
        {
            Value1 = value1,
            Value2 = value2
        };

        // act
        var actual = SystemUnderTest.Index(input) as ViewResult;

        // assert
        Assert.IsNotNull(actual);
        var model = actual.Model as CalculatorViewModel;
        Assert.IsNotNull(model);

        Assert.AreEqual<int>(expected, model.Result, $"Result was wrong");
        Assert.AreEqual<int>(value1, model.Value1, $"Value1 was wrong");
        Assert.AreEqual<int>(value2, model.Value2, $"Value2 was wrong");
        Assert.AreEqual<string>(expectedResultMessage, model.ResultMessage, $"ResultMessage was wrong");
    }
}