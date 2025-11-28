using LibraryManagement.ModuleBootstrapper.Extensions;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Moq;

namespace LibraryManagement.ModuleBootstrapper.Tests.Extensions;

public class ApplicationBuilderExtensionsTests
{
    [Fact]
    public void InitializeApplicationModuleConfiguration_returns_registrator_wrapping_original_builder()
    {
        Mock<IHostApplicationBuilder> builderMock = CreateBuilderMock();

        IModuleRegistrator<IHostApplicationBuilder> registrator =
            builderMock.Object.InitializeApplicationModuleConfiguration();

        Assert.Same(builderMock.Object, registrator.Builder);
        Assert.Same(builderMock.Object.Services, registrator.Services);
        Assert.Same(builderMock.Object.Configuration, registrator.ConfigurationManager);
        Assert.Same(builderMock.Object.Environment, registrator.Environment);

        builderMock.VerifyGet(mock => mock.Services, Times.AtLeastOnce);
        builderMock.VerifyGet(mock => mock.Configuration, Times.AtLeastOnce);
        builderMock.VerifyGet(mock => mock.Environment, Times.AtLeastOnce);
    }

    private static Mock<IHostApplicationBuilder> CreateBuilderMock()
    {
        ServiceCollection services = new();
        ConfigurationManager configuration = new();
        Mock<IHostEnvironment> environmentMock = new();

        Mock<IHostApplicationBuilder> builderMock = new();
        builderMock.SetupGet(mock => mock.Services).Returns(services);
        builderMock.SetupGet(mock => mock.Configuration).Returns(configuration);
        builderMock.SetupGet(mock => mock.Environment).Returns(environmentMock.Object);

        return builderMock;
    }
}
