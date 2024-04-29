using System.ComponentModel;

namespace Benday.CalculatorDemo.WebUi.Models;

public class CalculatorViewModel
{
    [DisplayName("Value 1")]
    public int Value1 { get; set; }

    [DisplayName("Value 2")]
    public int Value2 { get; set; }

    [DisplayName("Result")]
    public int Result { get; set; }
}