using System;
using System.Runtime.CompilerServices;
using AutoMapper;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.ServerlessApi.Mediators.Internal.Classifications;
using Master.Database.Management.ServerlessApi.Mediators.Internal.FixTemplates;

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

    public IFixTemplateMediator RequestFixTemplateMediator()
    {
      return new FixTemplateMediator(_mapper, _requestMdmDalFactory);
    }

    public IFixUnitMediator RequestFixUnitMediator()
    {
      return new FixUnitMediator(_requestMdmDalFactory);
    }

    public IWorkCategoryMediator RequestWorkCategoryMediator()
    {
      return new WorkCategoryMediator(_requestMdmDalFactory);
    }

    public IWorkTypeMediator RequestWorkTypeMediator()
    {
      return new WorkTypeMediator(_requestMdmDalFactory);
    }
  }
}
