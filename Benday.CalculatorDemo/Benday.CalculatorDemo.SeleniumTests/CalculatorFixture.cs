using Benday.CalculatorDemo.WebUi;

using Microsoft.AspNetCore.Hosting;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Extensions;


namespace HelloWorld.SeleniumTests;

[TestClass]
public class CalculatorFixture
{
    private const string SOLUTION_RELATIVE_CONTENT_ROOT_PATH = "Benday.CalculatorDemo.WebUi";
    private const string ElementId_Value1 = "Value1";
    private const string ElementId_Value2 = "Value2";
    private const string ElementId_Result = "Result";
    private const string ElementId_CalculateButton = "calculateButton";

    [TestInitialize]
    public void OnTestInitialize()
    {
        _systemUnderTest = null;       
    }

    [TestCleanup]
    public void OnTestCleanup()
    {
        _systemUnderTest?.Dispose();
    }

    public required TestContext TestContext { get; set; }

    private CustomWebApplicationFactory<Startup>? _systemUnderTest;
    public CustomWebApplicationFactory<Startup> SystemUnderTest
    {
        get
        {
            if (_systemUnderTest == null)
            {
                _systemUnderTest = new CustomWebApplicationFactory<Startup>(
                    addDevelopmentConfigurations: builder =>
                    {
                        builder.UseEnvironment("Development");
                        builder.UseSetting("https_port", "5001");
                    });
            }

            return _systemUnderTest;
        }
    }

    [TestMethod]
    public void CalculatorStartsWithAllZeros()
    {
        // arrange

        Assert.IsNotNull(SystemUnderTest.CreateDefaultClient(), "Client initialization failed");

        var relativeUrl = $"calculator";

        var url = SystemUnderTest.GetServerAddressForRelativeUrl(relativeUrl);

        Console.WriteLine($"Calling url: {url}");

        var driverOptions = new EdgeOptions();
        driverOptions.AddArgument("headless");

        using var driver = new EdgeDriver(driverOptions);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

        // act
        driver.Navigate().GoToUrl(url);

        var elementValue1 = wait.Until(driver => driver.FindElement(By.Id(ElementId_Value1)));
        var elementValue2 = wait.Until(driver => driver.FindElement(By.Id(ElementId_Value2)));
        var elementResult = wait.Until(driver => driver.FindElement(By.Id(ElementId_Result)));

        TakeScreenshot(driver, "screen-loaded");

        // assert
        Assert.IsNotNull(elementValue1, $"Could not find element '{ElementId_Value1}'");
        Assert.IsNotNull(elementValue2, $"Could not find element '{ElementId_Value2}'");
        Assert.IsNotNull(elementResult, $"Could not find element ' {ElementId_Result} '");

        driver.WriteLogs();
        driver.WritePageSource();

        Assert.AreEqual<string>("0", elementValue1.GetAttribute("value"), "Value 1 text was wrong");
        Assert.AreEqual<string>("0", elementValue2.GetAttribute("value"), "Value 2 text was wrong");
        Assert.AreEqual<string>("0", elementResult.GetAttribute("value"), "Result text was wrong");
    }

    private void TakeScreenshot(EdgeDriver driver, string name)
    {
        var screenshot = driver.GetScreenshot();

        var path = Path.Combine(Path.GetTempPath(), "test-attachments", DateTime.Now.Ticks.ToString(), TestContext.TestName ?? "unknown-test");

        if (Directory.Exists(path) == false)
        {
            Console.WriteLine($"TakeScreenshot() creating directory: {path}");
            Directory.CreateDirectory(path!);
        }

        var screenshotPath = Path.Combine(path, $"screenshot-{name}.png");

        Console.WriteLine($"TakeScreenshot() saving screenshot to: {screenshotPath}");

        screenshot.SaveAsFile(screenshotPath);

        Assert.IsTrue(File.Exists(screenshotPath), $"Screenshot file not found after save to '{screenshotPath}'.");

        Console.WriteLine($"TakeScreenshot() adding screenshot to test results: {screenshotPath}");

        TestContext.AddResultFile(screenshotPath);

        Console.WriteLine($"TakeScreenshot() added screenshot to test results: {screenshotPath}");
    }

    [TestMethod]
    public void Calculate()
    {
        // arrange
        var expected = "55";

        Assert.IsNotNull(SystemUnderTest.CreateDefaultClient(), "Client initialization failed");

        var relativeUrl = $"calculator";

        var url = SystemUnderTest.GetServerAddressForRelativeUrl(relativeUrl);

        Console.WriteLine($"Calling url: {url}");

        var driverOptions = new EdgeOptions();
        driverOptions.AddArgument("headless");

        using var driver = new EdgeDriver(driverOptions);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

        // act
        driver.Navigate().GoToUrl(url);

        TakeScreenshot(driver, "screen-loaded");

        var elementValue1 = wait.Until(driver => driver.FindElement(By.Id(ElementId_Value1)));
        var elementValue2 = wait.Until(driver => driver.FindElement(By.Id(ElementId_Value2)));
        var elementResult = wait.Until(driver => driver.FindElement(By.Id(ElementId_Result)));
        var elementCalculateButton = wait.Until(driver => driver.FindElement(By.Id(ElementId_CalculateButton)));

        Assert.IsNotNull(elementValue1, $"Could not find element '{ElementId_Value1}'");
        Assert.IsNotNull(elementValue2, $"Could not find element '{ElementId_Value2}'");
        Assert.IsNotNull(elementResult, $"Could not find element ' {ElementId_Result} '");
        Assert.IsNotNull(elementCalculateButton, $"Could not find element ' {ElementId_CalculateButton} '");

        elementValue1.Click();
        elementValue1.SendKeys("23");

        elementValue2.Click();
        elementValue2.SendKeys("32");

        TakeScreenshot(driver, "screen-populated-before-click");

        Console.WriteLine($"Clicking calculate button...");

        elementCalculateButton.Click();

        elementResult = wait.Until(driver => driver.FindElement(By.Id(ElementId_Result)));

        // assert
        TakeScreenshot(driver, "screen-after-calculate");

        Assert.IsNotNull(elementResult, $"Could not find element after post ' {ElementId_Result} '");

        var actual = elementResult.GetAttribute("value");

        driver.WriteLogs();
        driver.WritePageSource();

        Assert.AreEqual<string>(expected, actual, "Result was wrong");
    }
}