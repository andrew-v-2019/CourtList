using CourtList.Services;

namespace CourtList.Tests;

public class TestFixture : IDisposable
{
    public IServiceProvider serviceProvider { get; private set; }

    public TestFixture()
    {
        var serviceProvider = ServiceInjector.BuilServiceProvider();
        this.serviceProvider = serviceProvider;
    }

    public void Dispose()
    {
    }
}