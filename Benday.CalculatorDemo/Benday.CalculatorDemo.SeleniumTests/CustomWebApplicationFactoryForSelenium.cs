using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelloWorld.SeleniumTests;

public class CustomWebApplicationFactoryForSelenium<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
{
    private readonly string _baseAddress = "https://localhost";
    private IWebHost _webHost;
    private Action<IWebHostBuilder>? _addDevelopmentConfigs;
    private readonly bool _verbose;
    private readonly string? _solutionRelativeContentRoot;

    public CustomWebApplicationFactoryForSelenium(
        bool verbose = false,
        Action<IWebHostBuilder>? addDevelopmentConfigurations = null,
        string? solutionRelativeContentRoot = null)
    {
        _verbose = verbose;
        _solutionRelativeContentRoot = solutionRelativeContentRoot;

        WriteToLog($"CustomWebApplicationFactory.ctor starting...");

        if (addDevelopmentConfigurations != null)
        {
            _addDevelopmentConfigs = addDevelopmentConfigurations;
        }

        ClientOptions.BaseAddress = new Uri(_baseAddress);

        CreateServer();

        WriteToLog($"CustomWebApplicationFactory.ctor exiting...");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _addDevelopmentConfigs?.Invoke(builder);
    }

    private void WriteToLog(string message)
    {
        if (_verbose == true)
        {
            Console.WriteLine(message);
        }
    }

    public string GetServerAddress()
    {
        var serverAddresses = _webHost.ServerFeatures.Get<IServerAddressesFeature>();

        if (serverAddresses == null)
        {
            throw new InvalidOperationException($"Could not get instance of IServerAddressFeature.");
        }

        var addresses = serverAddresses.Addresses;

        var returnValue = addresses.FirstOrDefault();

        if (returnValue == null)
        {
            throw new InvalidOperationException($"Could not get address from server addresses.");
        }

        return returnValue;
    }

    public string GetServerAddressForRelativeUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentException($"{nameof(url)} is null or empty.", nameof(url));
        }

        var baseAddr = GetServerAddress();

        if (baseAddr.EndsWith("/") == true)
        {
            baseAddr = baseAddr[0..^1];
        }

        if (url.StartsWith("/") == true)
        {
            url = url[1..];
        }

        return $"{baseAddr}/{url}";
    }

    private TestServer TestServer { get; set; }

    protected override TestServer CreateServer(IWebHostBuilder builder)
    {
        WriteToLog($"CustomWebApplicationFactory.CreateServer() starting...");

        WriteToLog($"CustomWebApplicationFactory.CreateServer() calling builder.Build()...");
        _webHost = builder.Build();

        WriteToLog($"CustomWebApplicationFactory.CreateServer() calling webhost.start()...");
        _webHost.Start();

        var returnValue = InitializeTestServer();

        WriteToLog($"CustomWebApplicationFactory.CreateServer() exiting...");

        return returnValue;
    }

    private void CreateServer()
    {
        var builder = GetWebHostBuilderInstance();

        _addDevelopmentConfigs?.Invoke(builder);

        CreateServer(builder);
    }

    private IWebHostBuilder GetWebHostBuilderInstance()
    {
        IWebHostBuilder builder;

        if (string.IsNullOrEmpty(_solutionRelativeContentRoot) == true)
        {
            builder = WebHost.CreateDefaultBuilder<TStartup>(Array.Empty<string>())
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                });
        }
        else
        {
            builder = WebHost.CreateDefaultBuilder<TStartup>(Array.Empty<string>())
               .UseSolutionRelativeContentRoot(_solutionRelativeContentRoot)
               .ConfigureAppConfiguration((context, config) =>
               {
                   config.AddJsonFile("appsettings.json", optional: false);
               });
        }
        return builder;
    }

    private TestServer InitializeTestServer()
    {
        WriteToLog($"CustomWebApplicationFactory.InitializeTestServer() creating instance of test server...");

        // var builder = new WebHostBuilder().UseStartup<TStartup>();
        var builder = GetWebHostBuilderInstance();

        WriteToLog($"CustomWebApplicationFactory.InitializeTestServer() invoking add dev config...");
        _addDevelopmentConfigs?.Invoke(builder);
        WriteToLog($"CustomWebApplicationFactory.InitializeTestServer() invoked add dev config...");

        var returnValue = new TestServer(builder);

        TestServer = returnValue;
        return returnValue;
    }

    protected IServiceScope _scope;
    protected IServiceScope Scope
    {
        get
        {
            if (_scope == null)
            {
                var scopeFactory =
                    TestServer.Services.GetRequiredService<IServiceScopeFactory>();

                if (scopeFactory == null)
                {
                    throw new InvalidOperationException("Could not create instance of IServiceScopeFactory.");
                }

                _scope = scopeFactory.CreateScope();
            }

            return _scope;
        }
    }

    public T CreateInstance<T>()
    {
        var provider = Scope.ServiceProvider;

        var returnValue = provider.GetRequiredService<T>();

        return returnValue;
    }

    protected override void Dispose(bool disposing)
    {
        _addDevelopmentConfigs = null;

        _scope?.Dispose();
        _scope = null;

        _webHost?.Dispose();
        _webHost = null;

        base.Dispose(disposing);
        if (disposing)
        {
            _webHost?.Dispose();
        }
    }
}