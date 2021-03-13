using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fixit.Core.DataContracts.Classifications;
using Fixit.Core.DataContracts.Users.Skills;
using Master.Database.Management.DataLayer.Models;
using Master.Database.Management.DataLayer.Models.Classifications;

namespace Master.Database.Management.DataLayer.DataAccess.Mappers
{
  public class MdmClassificationsMappers : Profile
  {
    public MdmClassificationsMappers()
    {
      #region Categories

      CreateMap<WorkCategoryDto, WorkCategory>()
        .ForMember(workCategory => workCategory.Id, opts => opts.MapFrom(workCategoryDto => workCategoryDto != null ? workCategoryDto.Id : Guid.Empty))
        .ForMember(workCategory => workCategory.Name, opts => opts.MapFrom(workCategoryDto => workCategoryDto != null ? workCategoryDto.Name : string.Empty));

      CreateMap<WorkCategory, WorkCategoryDto>()
        .ForMember(workCategoryDto => workCategoryDto.Id, opts => opts.MapFrom(workCategory => workCategory != null ? workCategory.Id : Guid.Empty))
        .ForMember(workCategoryDto => workCategoryDto.Name, opts => opts.MapFrom(workCategory => workCategory != null ? workCategory.Name : string.Empty))
        .ForMember(WorkCategoryDto => WorkCategoryDto.Skills, opts => opts.MapFrom(workCategory => workCategory != null ? workCategory.WorkCategorySkills.Select(workCategorySkill => workCategorySkill.Skill).ToList() : new List<Skill>()));

      #endregion

      #region Fix Units

      CreateMap<FixUnitDto, FixUnit>()
        .ForMember(fixUnit => fixUnit.Id, opts => opts.MapFrom(fixUnitDto => fixUnitDto != null ? fixUnitDto.Id : Guid.Empty))
        .ForMember(fixUnit => fixUnit.Name, opts => opts.MapFrom(FixUnitDto => FixUnitDto != null ? FixUnitDto.Name : string.Empty))
        .ReverseMap();

      #endregion

      #region Skills

      CreateMap<SkillDto, Skill>()
        .ForMember(skill => skill.Id, opts => opts.MapFrom(skillDto => skillDto != null ? skillDto.Id : Guid.Empty))
        .ForMember(skill => skill.Name, opts => opts.MapFrom(skillDto => skillDto != null ? skillDto.Name : string.Empty))
        .ReverseMap();

      #endregion

      #region Types

      CreateMap<WorkTypeDto, WorkType>()
        .ForMember(workType => workType.Id, opts => opts.MapFrom(workTypeDto => workTypeDto != null ? workTypeDto.Id : Guid.Empty))
        .ForMember(workType => workType.Name, opts => opts.MapFrom(workTypeDto => workTypeDto != null ? workTypeDto.Name : string.Empty))
        .ReverseMap();

      #endregion
    }
  }
}
