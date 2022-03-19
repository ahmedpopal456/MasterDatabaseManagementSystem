using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fixit.Core.DataContracts.Classifications;
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
        .ForMember(workCategoryDto => workCategoryDto.Name, opts => opts.MapFrom(workCategory => workCategory != null ? workCategory.Name : string.Empty));

      #endregion

      #region Fix Units

      CreateMap<FixUnitDto, FixUnit>()
        .ForMember(fixUnit => fixUnit.Id, opts => opts.MapFrom(fixUnitDto => fixUnitDto != null ? fixUnitDto.Id : Guid.Empty))
        .ForMember(fixUnit => fixUnit.Name, opts => opts.MapFrom(FixUnitDto => FixUnitDto != null ? FixUnitDto.Name : string.Empty))
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
