using System;
using System.Linq;
using AutoMapper;
using Master.Database.Management.DataLayer.Models.FixTemplates.Sections;
using Master.Database.Management.DataLayer.Models.FixTemplates.Fields;
using Master.Database.Management.DataLayer.Models.FixTemplates;
using Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities;
using Fixit.Core.DataContracts.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Sections;
using Fixit.Core.DataContracts.FixTemplates.Sections;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Fields;
using Fixit.Core.DataContracts.FixTemplates.Fields;
using Fixit.Core.DataContracts.Classifications;

namespace Master.Database.Management.DataLayer.DataAccess.Mappers
{
  public class MdmFixTemplatesMappers : Profile
  {
    public MdmFixTemplatesMappers()
    {
      #region Fix Templates

      CreateMap<FixTemplateDto, FixTemplate>()
          .ForMember(fixTemplate => fixTemplate.Id, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null && !fixTemplateDto.Id.Equals(Guid.Empty) ? fixTemplateDto.Id : Guid.Empty))
          .ForMember(fixTemplate => fixTemplate.Status, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null ? fixTemplateDto.Status : default))
          .ForMember(fixTemplate => fixTemplate.Name, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null ? fixTemplateDto.Name : string.Empty))
          .ForMember(fixTemplate => fixTemplate.WorkCategoryId, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null && fixTemplateDto.WorkCategory != null ? fixTemplateDto.WorkCategory.Id : Guid.Empty))
          .ForMember(fixTemplate => fixTemplate.WorkCategory, opts => opts.Ignore())
          .ForMember(fixTemplate => fixTemplate.WorkTypeId, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null && fixTemplateDto.WorkType != null ? fixTemplateDto.WorkType.Id : Guid.Empty))
          .ForMember(fixTemplate => fixTemplate.WorkType, opts => opts.Ignore())
          .ForMember(fixTemplate => fixTemplate.FixUnitId, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null && fixTemplateDto.FixUnit != null ? fixTemplateDto.FixUnit.Id : Guid.Empty))
          .ForMember(fixTemplate => fixTemplate.FixUnit, opts => opts.Ignore())
          .ForMember(fixTemplate => fixTemplate.Description, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null ? fixTemplateDto.Description : string.Empty))
          .ForMember(fixTemplate => fixTemplate.SystemCostEstimate, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null ? fixTemplateDto.SystemCostEstimate : default))
          .ForMember(fixTemplate => fixTemplate.CreatedByUserId, opts => opts.Condition(fixTemplateDto => fixTemplateDto != null && !fixTemplateDto.CreatedByUserId.Equals(Guid.Empty)))
          .ForMember(fixTemplate => fixTemplate.UpdatedByUserId, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null ? fixTemplateDto.UpdatedByUserId : Guid.Empty))
          .ForMember(fixTemplate => fixTemplate.Tags, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null ? fixTemplateDto.Tags.Select(tagValue => new FixTemplateTag() { FixTemplateId = fixTemplateDto.Id, Name = tagValue }).ToList() : default))
          .ForMember(fixTemplate => fixTemplate.FixTemplateSections, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null ? fixTemplateDto.Sections : default));

      CreateMap<FixTemplate, FixTemplateDto>()
        .ForMember(fixTemplateDto => fixTemplateDto.Id, opts => opts.MapFrom(fixTemplate => fixTemplate != null && !fixTemplate.Id.Equals(Guid.Empty) ? fixTemplate.Id : Guid.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.Status, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.Status : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Name, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.Name : string.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.WorkCategory, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.WorkCategory : default))
        .ForMember(fixTemplateDto => fixTemplateDto.WorkType, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.WorkType : default))
        .ForMember(fixTemplateDto => fixTemplateDto.FixUnit, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.FixUnit : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Description, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.Description : string.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.SystemCostEstimate, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.SystemCostEstimate : default))
        .ForMember(fixTemplateDto => fixTemplateDto.CreatedByUserId, opts => opts.Condition(fixTemplate => fixTemplate != null && !fixTemplate.CreatedByUserId.Equals(Guid.Empty)))
        .ForMember(fixTemplateDto => fixTemplateDto.UpdatedByUserId, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.UpdatedByUserId : Guid.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.Tags, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.Tags.Select(tag => tag.Name).ToList() : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Sections, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.FixTemplateSections : default));

      CreateMap<FixTemplateCreateRequestDto, FixTemplateDto>()
        .ForMember(fixTemplateDto => fixTemplateDto.Id, opts => opts.MapFrom(_ => Guid.NewGuid()))
        .ForMember(fixTemplateDto => fixTemplateDto.Status, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.Status : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Name, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.Name : string.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.WorkCategory, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null && fixTemplateCreateRequestDto.WorkCategoryId != default ? new WorkCategoryDto() { Id = fixTemplateCreateRequestDto.WorkCategoryId } : default))
        .ForMember(fixTemplateDto => fixTemplateDto.WorkType, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null && fixTemplateCreateRequestDto.WorkTypeId != default ? new WorkTypeDto() { Id = fixTemplateCreateRequestDto.WorkTypeId } : default))
        .ForMember(fixTemplateDto => fixTemplateDto.FixUnit, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null && fixTemplateCreateRequestDto.FixUnitId != default ? new FixUnitDto() { Id = fixTemplateCreateRequestDto.FixUnitId } : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Description, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.Description : string.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.SystemCostEstimate, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.SystemCostEstimate : default))
        .ForMember(fixTemplateDto => fixTemplateDto.CreatedByUserId, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.CreatedByUserId : Guid.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.UpdatedByUserId, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.UpdatedByUserId : Guid.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.Tags, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.Tags : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Sections, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.Sections : default));

      CreateMap<FixTemplateUpdateRequestDto, FixTemplateDto>()
        .ForMember(fixTemplateDto => fixTemplateDto.Id, opts => opts.UseDestinationValue())
        .ForMember(fixTemplateDto => fixTemplateDto.Name, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null ? fixTemplateUpdateRequestDto.Name : string.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.WorkCategory, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null && fixTemplateUpdateRequestDto.WorkCategoryId != default ? new WorkCategoryDto() { Id = fixTemplateUpdateRequestDto.WorkCategoryId } : default))
        .ForMember(fixTemplateDto => fixTemplateDto.WorkType, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null && fixTemplateUpdateRequestDto.WorkTypeId != default ? new WorkTypeDto() { Id = fixTemplateUpdateRequestDto.WorkTypeId } : default))
        .ForMember(fixTemplateDto => fixTemplateDto.FixUnit, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null && fixTemplateUpdateRequestDto.FixUnitId != default ? new FixUnitDto() { Id = fixTemplateUpdateRequestDto.FixUnitId } : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Description, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null ? fixTemplateUpdateRequestDto.Description : string.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.SystemCostEstimate, opts => opts.UseDestinationValue())
        .ForMember(fixTemplateDto => fixTemplateDto.CreatedByUserId, opts => opts.UseDestinationValue())
        .ForMember(fixTemplateDto => fixTemplateDto.UpdatedByUserId, opts => opts.UseDestinationValue())
        .ForMember(fixTemplateDto => fixTemplateDto.Tags, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null ? fixTemplateUpdateRequestDto.Tags : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Sections, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null ? fixTemplateUpdateRequestDto.Sections : default));

      #endregion

      #region Sections

      CreateMap<Section, SectionCreateRequestDto>()
        .ForMember(sectionCreateRequestDto => sectionCreateRequestDto.Name, opts => opts.MapFrom(section => section != null && section.Name != null ? section.Name.ToLower().Trim() : string.Empty))
        .ReverseMap();

      CreateMap<Section, SectionDto>()
          .ForMember(sectionDto => sectionDto.Id, opts => opts.MapFrom(section => section != null && section.Id != default ? section.Id : Guid.Empty))
          .ForMember(sectionDto => sectionDto.Name, opts => opts.MapFrom(section => section != null && section.Name != null ? section.Name.ToLower().Trim() : string.Empty))
          .ReverseMap();

      CreateMap<SectionDto, FixTemplateSectionDto>()
          .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.SectionId, opts => opts.MapFrom(sectionDto => sectionDto != null && sectionDto.Id != default ? sectionDto.Id : Guid.Empty))
          .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.Name, opts => opts.MapFrom(sectionDto => sectionDto != null ? sectionDto.Name : string.Empty))
          .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.Fields, opts => opts.UseDestinationValue());

      CreateMap<FixTemplateSectionDto, FixTemplateSection>()
          .ForMember(fixTemplateSection => fixTemplateSection.Id, opts => opts.UseDestinationValue())
          .ForMember(fixTemplateSection => fixTemplateSection.SectionId, opts => opts.MapFrom(fixTemplateSectionDto => fixTemplateSectionDto != null && fixTemplateSectionDto.SectionId != default ? fixTemplateSectionDto.SectionId : Guid.Empty))
          .ForMember(fixTemplateSection => fixTemplateSection.FixTemplateId, opts => opts.UseDestinationValue())
          .ForMember(fixTemplateSection => fixTemplateSection.FixTemplateSectionFields, opts => opts.MapFrom(fixTemplateSectionDto => fixTemplateSectionDto != null ? fixTemplateSectionDto.Fields : default));

      CreateMap<FixTemplateSection, FixTemplateSectionDto>()
        .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.SectionId, opts => opts.MapFrom(fixTemplateSection => fixTemplateSection != null ? fixTemplateSection.SectionId : Guid.Empty))
        .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.Name, opts => opts.MapFrom(fixTemplateSection => fixTemplateSection != null && fixTemplateSection.Section != null ? fixTemplateSection.Section.Name : string.Empty))
        .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.Fields, opts => opts.MapFrom(fixTemplateSection => fixTemplateSection != null ? fixTemplateSection.FixTemplateSectionFields : default));

      CreateMap<FixTemplateSectionCreateRequestDto, FixTemplateSectionDto>()
        .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.SectionId, opts => opts.Ignore())
        .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.Name, opts => opts.MapFrom(fixTemplateSectionCreateRequestDto => fixTemplateSectionCreateRequestDto != null ? fixTemplateSectionCreateRequestDto.Name : string.Empty))
        .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.Fields, opts => opts.MapFrom(fixTemplateSectionCreateRequestDto => fixTemplateSectionCreateRequestDto != null ? fixTemplateSectionCreateRequestDto.Fields : default));

      #endregion

      #region Fields

      CreateMap<FieldCreateRequestDto, Field>()
        .ForMember(field => field.Name, opts => opts.MapFrom(fieldCreateRequestDto => fieldCreateRequestDto != null ? fieldCreateRequestDto.Name : string.Empty))
        .ReverseMap();

      CreateMap<Field, FieldDto>()
          .ForMember(fieldDto => fieldDto.Name, opts => opts.MapFrom(field => field != null ? field.Name : string.Empty))
          .ForMember(fieldDto => fieldDto.Id, opts => opts.MapFrom(field => field != null ? field.Id : Guid.Empty))
          .ReverseMap();

      CreateMap<FixTemplateFieldDto, FixTemplateSectionField>()
          .ForMember(field => field.FixTemplateSectionId, opts => opts.UseDestinationValue())
          .ForMember(field => field.FieldId, opts => opts.MapFrom(fixTemplateSectionField => fixTemplateSectionField != null ? fixTemplateSectionField.Id : Guid.Empty))
          .ForMember(field => field.Value, opts => opts.MapFrom(fixTemplateSectionField => fixTemplateSectionField != null ? string.Join(';', fixTemplateSectionField.Value) : default));

      CreateMap<FixTemplateSectionField, FixTemplateFieldDto>()
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Id, opts => opts.MapFrom(fixTemplateSectionField => fixTemplateSectionField != null ? fixTemplateSectionField.FieldId : Guid.Empty))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Name, opts => opts.MapFrom(fixTemplateSectionField => fixTemplateSectionField != null && fixTemplateSectionField.Field != null ? fixTemplateSectionField.Field.Name : string.Empty))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Value, opts => opts.MapFrom(fixTemplateSectionField => fixTemplateSectionField != null ? fixTemplateSectionField.Value.Split(';', StringSplitOptions.None) : default));

      CreateMap<FixTemplateFieldCreateRequestDto, FixTemplateFieldDto>()
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Id, opts => opts.Ignore())
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Name, opts => opts.MapFrom(fixTemplateFieldCreateRequestDto => fixTemplateFieldCreateRequestDto != null ? fixTemplateFieldCreateRequestDto.Name : string.Empty))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Value, opts => opts.MapFrom(fixTemplateFieldCreateRequestDto => fixTemplateFieldCreateRequestDto.Value));

      CreateMap<FixTemplateFieldDto, FixTemplateFieldDto>()
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Id, opts => opts.MapFrom(sourceFixTemplateFieldDto => sourceFixTemplateFieldDto.Id))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Name, opts => opts.MapFrom(sourceFixTemplateFieldDto => sourceFixTemplateFieldDto.Name))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Value, opts => opts.MapFrom(sourceFixTemplateFieldDto => sourceFixTemplateFieldDto.Value));

      #endregion

    }
  }
}
