using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace HelloWorld.SeleniumTests;

public static class SeleniumTestUtility
{
    public static string GetChromeDriverPath()
    {
        var variableName = "CHROMEWEBDRIVER";

        var returnValue = Environment.GetEnvironmentVariable(variableName);

        Assert.IsFalse(string.IsNullOrWhiteSpace(returnValue), $"Chrome web driver path value in env var '{variableName}' was empty or null");

        return returnValue;
    }

    public static void WritePageSource(this EdgeDriver driver)
    {
        var content = driver.PageSource;

        Console.WriteLine("*** PAGE SOURCE START ***");
        Console.WriteLine(content);
        Console.WriteLine("*** PAGE SOURCE END ***");
    }

    public static void WriteLogs(this EdgeDriver driver)
    {
        Console.WriteLine("*** READING BROWSER LOGS START ***");

        var logs = driver.Manage().Logs.GetLog(LogType.Browser);

        foreach (var item in logs)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("*** READING BROWSER LOGS END ***");
    }
}