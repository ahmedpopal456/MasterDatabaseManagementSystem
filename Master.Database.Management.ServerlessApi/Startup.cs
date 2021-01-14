using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Master.Database.Management.DataLayer;
using Master.Database.Management.ServerlessApi;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Mappers;
using Master.Database.Management.DataLayer.DataAccess.Internal;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Mediators.Internal;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Master.Database.Management.ServerlessApi
{
  public class Startup : FunctionsStartup
  {
    private IConfiguration _configuration;

    public override void Configure(IFunctionsHostBuilder builder)
    {
      _configuration = (IConfiguration)builder.Services.BuildServiceProvider().GetService(typeof(IConfiguration));

      var mapperConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new MdmMapper());
      });
      
      builder.Services.AddSingleton<IMapper>(mapperConfig.CreateMapper());
      builder.Services.AddScoped<MdmContext>(provider =>
      {
        var options = new DbContextOptionsBuilder<MdmContext>().UseSqlServer(_configuration["FIXIT-MDM-DB-CS"],options => options.EnableRetryOnFailure());
        return new MdmContext(options.Options);
      });

      builder.Services.AddScoped<IRequestMdmDalFactory, RequestMdmDalFactory>();
      builder.Services.AddScoped<IRequestMediatorFactory, RequestMediatorFactory>();
    }
  }
}
