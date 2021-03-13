using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Users.Skills;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;

namespace Master.Database.Management.ServerlessApi.Mediators.Internal
{
  internal class SkillMediator : ISkillMediator
  {
    private readonly IRequestMdmDalFactory _requestMdmDalFactory;
    public SkillMediator(IRequestMdmDalFactory requestMdmDalFactory)
    {
      _requestMdmDalFactory = requestMdmDalFactory ?? throw new ArgumentNullException($"{nameof(SkillMediator)} expects a value for {nameof(requestMdmDalFactory)}... null argument was provided");
    }

    public async Task<SkillDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmSkillDal().GetByIdAsync(id, cancellationToken);

      return result;
    }

    public async Task<IEnumerable<SkillDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmSkillDal().GetManyAsync(filterBaseDto, cancellationToken);

      return result;
    }
    public async Task<PagedModelCollectionDto<SkillDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmSkillDal().GetManyByPageAsync(paginationRequestDto, cancellationToken);

      return result;
    }
  }
}
