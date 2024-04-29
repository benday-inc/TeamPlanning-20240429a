using Microsoft.AspNetCore.Hosting;

namespace HelloWorld.SeleniumTests;

public abstract class SeleniumIntegrationTestBase<TStartup> where TStartup : class
{
    [TestInitialize]
    public void OnTestInitialize()
    {
        _systemUnderTest = null;
    }

    [TestCleanup]
    public void OnTestCleanup()
    {
        _systemUnderTest?.Dispose();
        _systemUnderTest = null;
    }

    private CustomWebApplicationFactoryForSelenium<TStartup>? _systemUnderTest;

    protected CustomWebApplicationFactoryForSelenium<TStartup> SystemUnderTest
    {
        get
        {
            Assert.IsNotNull(_systemUnderTest, "SystemUnderTest has not been initialized.");

            return _systemUnderTest;
        }
    }

    protected void InitializeSystemUnderTest(string solutionRelativeContentRootValue)
    {
        _systemUnderTest =
        new CustomWebApplicationFactoryForSelenium<TStartup>(
            solutionRelativeContentRoot: solutionRelativeContentRootValue);
    }
}