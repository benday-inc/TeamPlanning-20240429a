﻿using Benday.CalculatorDemo.Api;
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

    [HttpPost]
    public IActionResult Index(CalculatorViewModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model), $"{nameof(model)} is null.");
        }

        var result = _calculator.Add(model.Value1, model.Value2);

        var message = $"The result of {model.Value1} + {model.Value2} is {result}.";

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