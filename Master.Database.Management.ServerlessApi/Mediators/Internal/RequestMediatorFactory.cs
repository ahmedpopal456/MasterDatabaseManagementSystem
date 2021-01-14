using System;
using System.Runtime.CompilerServices;
using AutoMapper;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.ServerlessApi.Mediators.Internal.Fixes.Categories;
using Master.Database.Management.ServerlessApi.Mediators.Internal.Fixes.Templates;
using Master.Database.Management.ServerlessApi.Mediators.Internal.Fixes.Types;

[assembly: InternalsVisibleTo("Master.Database.Management.ServerlessApi.UnitTests")]
namespace Master.Database.Management.ServerlessApi.Mediators.Internal
{
  internal class RequestMediatorFactory : IRequestMediatorFactory
  {
    private readonly IMapper _mapper;
    private readonly IRequestMdmDalFactory _requestMdmDalFactory;

    public RequestMediatorFactory(IMapper mapper, IRequestMdmDalFactory requestMdmDalFactory)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(RequestMediatorFactory)} expects a value for {nameof(mapper)}... null argument was provided");
      _requestMdmDalFactory = requestMdmDalFactory ?? throw new ArgumentNullException($"{nameof(RequestMediatorFactory)} expects a value for {nameof(requestMdmDalFactory)}... null argument was provided");
    }

    public IFixCategoryMediator RequestFixCategoryMediator()
    {
      return new FixCategoryMediator(_requestMdmDalFactory);
    }

    public IFixTemplateMediator RequestFixTemplateMediator()
    {
      return new FixTemplateMediator(_mapper, _requestMdmDalFactory);
    }

    public IFixTypeMediator RequestFixTypeMediator()
    {
      return new FixTypeMediator(_requestMdmDalFactory);
    }
  }
}
