using Benday.CalculatorDemo.Api;
using Benday.CalculatorDemo.WebUi.Models;

using Microsoft.AspNetCore.Mvc;

namespace Benday.CalculatorDemo.WebUi.Controllers;
public class CalculatorController : Controller
{
    private readonly ICalculator _calculator;

    public CalculatorController(ICalculator calculator)
    {
        _calculator = calculator;
    }

    public IActionResult Index()
    {
        return View(new CalculatorViewModel());
    }

    public IActionResult Reset()
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Index(CalculatorViewModel model, string action)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model), $"{nameof(model)} is null.");
        }

        var result = _calculator.Add(model.Value1, model.Value2);

        var message = $"Whoomp! there it is: {model.Value1} + {model.Value2} = {result}";

        var newModel = new CalculatorViewModel()
        {
            Result = result,
            Value1 = model.Value1,
            Value2 = model.Value2,
            ResultMessage = message
        };

        ModelState.Clear();

        return View(newModel);
    }
}