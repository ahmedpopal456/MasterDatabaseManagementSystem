using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fixit.Core.DataContracts.Users.Skills;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;

namespace Master.Database.Management.DataLayer.DataAccess.Internal
{
  internal class MdmSkillDal : IMdmSkillDal
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;
    private const int PageSize = 20;

    public MdmSkillDal(MdmContext mdmContext, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(MdmSkillDal)} expects a value for {nameof(mapper)}... null argument was provided");
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(MdmSkillDal)} expects a value for {nameof(mdmContext)}... null argument was provided");
    }

    public async Task<SkillDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(GetByIdAsync)} expects id to be a valid {nameof(Guid)}... null argument was provided");
      }

      var skillDto = default(SkillDto);

      var skillResult = await _mdmContext.Skills.FirstOrDefaultAsync(skill => skill.Id.Equals(id), cancellationToken);

      if (skillResult != null)
      {
        skillDto = _mapper.Map<Skill, SkillDto>(skillResult);
        await _mdmContext.SaveChangesAsync(true, cancellationToken);
      }

      return skillDto;
    }

    public async Task<IEnumerable<SkillDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var skillResult = _mdmContext.Skills.Where(skill => (filterBaseDto.Name == null || skill.Name.ToLower().Contains(filterBaseDto.Name.ToLower()))
                                                           && (filterBaseDto.MinTimestampUtc == null || skill.CreatedTimestampUtc >= filterBaseDto.MinTimestampUtc)
                                                           && (filterBaseDto.MaxTimestampUtc == null || skill.CreatedTimestampUtc <= filterBaseDto.MaxTimestampUtc)).AsEnumerable();

      var skillDtos = skillResult.Select(skill => _mapper.Map<Skill, SkillDto>(skill)).ToList();
      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return skillDtos;
    }

    public async Task<PagedModelCollectionDto<SkillDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var skillResult = new PagedModelCollectionDto<SkillDto>();

      var skillQuery = _mdmContext.Skills.Where(skill => (paginationRequestDto.Name == null || skill.Name.ToLower().Contains(paginationRequestDto.Name.ToLower()))
                                                          && (paginationRequestDto.MinTimestampUtc == null || skill.CreatedTimestampUtc >= paginationRequestDto.MinTimestampUtc)
                                                          && (paginationRequestDto.MaxTimestampUtc == null || skill.CreatedTimestampUtc <= paginationRequestDto.MaxTimestampUtc));

      int validPageSize = paginationRequestDto.PageSize.Equals(default(int)) ? PageSize : paginationRequestDto.PageSize.Value;
      var skillsByPage = await skillQuery.ToPagedListAsync(validPageSize, paginationRequestDto.PageNumber, cancellationToken);
      if (skillsByPage != null && skillsByPage.Any())
      {
        skillResult = new PagedModelCollectionDto<SkillDto>
        {
          Results = skillsByPage.Select(skill => _mapper.Map<Skill, SkillDto>(skill)).ToList(),
          TotalModelCount = skillsByPage.TotalCount,
          PageNumber = skillsByPage.PageIndex
        };
      }

      await _mdmContext.SaveChangesAsync(true, cancellationToken);
      return skillResult;
    }
  }
}
