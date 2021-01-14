using AutoMapper;
using Fixit.Core.DataContracts;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Mappers;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Seeders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Master.Database.Management.ServerlessApi.Mediators.Internal;
using Moq;

namespace Master.Database.Management.ServerlessApi.UnitTests
{
  [TestClass]
  public class TestBase
  {
    public IFakeSeederFactory _fakeDtoSeederFactory;
    public FakeHttpRequestMessageAdapterBase _fakeHttpRequestMessageAdapterBase;

    public IRequestMediatorFactory _requestMediatorFactory;

    // Mocks
    protected Mock<IConfiguration> _fakeConfiguration;
    protected Mock<IRequestMediatorFactory> _fakeRequestMediatorFactory;
    protected Mock<IRequestMdmDalFactory> _fakeRequestMdmDalFactory;

    public TestBase()
    {
      _fakeDtoSeederFactory = new FakeDtoSeederFactory();
      _fakeHttpRequestMessageAdapterBase = new FakeHttpRequestMessageAdapter();
    }

    // Mapper
    protected MapperConfiguration _mapperConfiguration = new MapperConfiguration(config =>
    {
      config.AddProfile(new MdmMapper());
    });

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext)
    {
    }

    [AssemblyCleanup]
    public static void AfterSuiteTests()
    {
    }
  }
}
