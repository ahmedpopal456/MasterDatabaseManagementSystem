using System;
using System.Linq;
using AutoMapper;
using Master.Database.Management.DataLayer.Models.FixTemplates.Sections;
using Master.Database.Management.DataLayer.Models.FixTemplates.Fields;
using Master.Database.Management.DataLayer.Models.FixTemplates;
using Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities;
using Master.Database.Management.DataLayer.Models;
using Fixit.Core.DataContracts.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Sections;
using Fixit.Core.DataContracts.FixTemplates.Sections;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Fields;
using Fixit.Core.DataContracts.FixTemplates.Fields;
using Fixit.Core.DataContracts.Fixes.Categories;
using Fixit.Core.DataContracts.Fixes.Types;

namespace Master.Database.Management.DataLayer.DataAccess.Mappers
{
  public class MdmMapper : Profile
  {
    public MdmMapper ()
    {
      #region FixTemplates

      CreateMap<FixTemplateDto, FixTemplate>()
        .ForMember(fixTemplate => fixTemplate.Id, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null && !fixTemplateDto.Id.Equals(Guid.Empty) ? fixTemplateDto.Id : Guid.Empty))
        .ForMember(fixTemplate => fixTemplate.Status, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null ? fixTemplateDto.Status : default))
        .ForMember(fixTemplate => fixTemplate.Name, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null ? fixTemplateDto.Name : string.Empty))
        .ForMember(fixTemplate => fixTemplate.CategoryId, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null && fixTemplateDto.Category != null ? fixTemplateDto.Category.Id : Guid.Empty))
        .ForMember(fixTemplate => fixTemplate.Category, opts => opts.Ignore())
        .ForMember(fixTemplate => fixTemplate.TypeId, opts => opts.MapFrom(fixTemplateDto => fixTemplateDto != null && fixTemplateDto.Type != null ? fixTemplateDto.Type.Id : Guid.Empty))
        .ForMember(fixTemplate => fixTemplate.Type, opts => opts.Ignore())
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
        .ForMember(fixTemplateDto => fixTemplateDto.Category, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.Category : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Type, opts => opts.MapFrom(fixTemplate => fixTemplate != null ? fixTemplate.Type : default))
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
        .ForMember(fixTemplateDto => fixTemplateDto.Category, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null && fixTemplateCreateRequestDto.CategoryId != null ? new FixCategoryDto() { Id = fixTemplateCreateRequestDto.CategoryId } : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Type, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null && fixTemplateCreateRequestDto.TypeId != null ? new FixTypeDto() { Id = fixTemplateCreateRequestDto.TypeId } : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Description, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.Description : string.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.SystemCostEstimate, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.SystemCostEstimate : default))
        .ForMember(fixTemplateDto => fixTemplateDto.CreatedByUserId, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.CreatedByUserId : Guid.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.UpdatedByUserId, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.UpdatedByUserId : Guid.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.Tags, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.Tags : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Sections, opts => opts.MapFrom(fixTemplateCreateRequestDto => fixTemplateCreateRequestDto != null ? fixTemplateCreateRequestDto.Sections : default));

      CreateMap<FixTemplateUpdateRequestDto, FixTemplateDto>()
        .ForMember(fixTemplateDto => fixTemplateDto.Id, opts => opts.UseDestinationValue())
        .ForMember(fixTemplateDto => fixTemplateDto.Name, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null ? fixTemplateUpdateRequestDto.Name : string.Empty))
        .ForMember(fixTemplateDto => fixTemplateDto.Category, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null && fixTemplateUpdateRequestDto.CategoryId != null ? new FixCategoryDto() { Id = fixTemplateUpdateRequestDto.CategoryId } : default))
        .ForMember(fixTemplateDto => fixTemplateDto.Type, opts => opts.MapFrom(fixTemplateUpdateRequestDto => fixTemplateUpdateRequestDto != null && fixTemplateUpdateRequestDto.TypeId != null ? new FixTypeDto() { Id = fixTemplateUpdateRequestDto.TypeId } : default))
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
        .ForMember(sectionDto => sectionDto.Id, opts => opts.MapFrom(section => section != null && section.Id != null ? section.Id : Guid.Empty))
        .ForMember(sectionDto => sectionDto.Name, opts => opts.MapFrom(section => section != null && section.Name != null ? section.Name.ToLower().Trim() : string.Empty))
        .ReverseMap();

      CreateMap<SectionDto, FixTemplateSectionDto>()
        .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.SectionId, opts => opts.MapFrom(sectionDto => sectionDto != null && sectionDto.Id != null ? sectionDto.Id : Guid.Empty))
        .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.Name, opts => opts.MapFrom(sectionDto => sectionDto != null ? sectionDto.Name : string.Empty))
        .ForMember(fixTemplateSectionDto => fixTemplateSectionDto.Fields, opts => opts.UseDestinationValue());

      CreateMap<FixTemplateSectionDto, FixTemplateSection>()
        .ForMember(fixTemplateSection => fixTemplateSection.Id, opts => opts.UseDestinationValue())
        .ForMember(fixTemplateSection => fixTemplateSection.SectionId, opts => opts.MapFrom(fixTemplateSectionDto => fixTemplateSectionDto != null && fixTemplateSectionDto.SectionId != null ? fixTemplateSectionDto.SectionId : Guid.Empty))
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
        .ForMember(field => field.Value, opts => opts.MapFrom(fixTemplateSectionField => fixTemplateSectionField != null ? string.Join(';', fixTemplateSectionField.Values) : default));

      CreateMap<FixTemplateSectionField, FixTemplateFieldDto>()
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Id, opts => opts.MapFrom(fixTemplateSectionField => fixTemplateSectionField != null ? fixTemplateSectionField.FieldId : Guid.Empty))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Name, opts => opts.MapFrom(fixTemplateSectionField => fixTemplateSectionField != null && fixTemplateSectionField.Field != null? fixTemplateSectionField.Field.Name : string.Empty))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Values, opts => opts.MapFrom(fixTemplateSectionField => fixTemplateSectionField != null ? fixTemplateSectionField.Value.Split(';', StringSplitOptions.None) : default));

      CreateMap<FixTemplateFieldCreateRequestDto, FixTemplateFieldDto>()
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Id, opts => opts.Ignore())
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Name, opts => opts.MapFrom(fixTemplateFieldCreateRequestDto => fixTemplateFieldCreateRequestDto != null ? fixTemplateFieldCreateRequestDto.Name : string.Empty))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Values, opts => opts.MapFrom(fixTemplateFieldCreateRequestDto => fixTemplateFieldCreateRequestDto.Values.ToList()));

      CreateMap<FixTemplateFieldDto, FixTemplateFieldDto>()
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Id, opts => opts.MapFrom(sourceFixTemplateFieldDto => sourceFixTemplateFieldDto.Id))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Name, opts => opts.MapFrom(sourceFixTemplateFieldDto => sourceFixTemplateFieldDto.Name))
        .ForMember(fixTemplateFieldDto => fixTemplateFieldDto.Values, opts => opts.MapFrom(sourceFixTemplateFieldDto => sourceFixTemplateFieldDto.Values.ToList()));

      #endregion

      #region Categories

      CreateMap<FixCategoryDto, FixCategory>()
        .ForMember(fixCategory => fixCategory.Id, opts => opts.MapFrom(fixCategoryDto => fixCategoryDto != null ? fixCategoryDto.Id : Guid.Empty))
        .ForMember(fixCategory => fixCategory.Name, opts => opts.MapFrom(fixCategoryDto => fixCategoryDto != null ? fixCategoryDto.Name : string.Empty))
        .ReverseMap();

      #endregion

      #region Types

      CreateMap<FixTypeDto, FixType>()
        .ForMember(fixType => fixType.Id, opts => opts.MapFrom(fixTypeDto => fixTypeDto != null ? fixTypeDto.Id : Guid.Empty))
        .ForMember(fixType => fixType.Name, opts => opts.MapFrom(fixTypeDto => fixTypeDto != null ? fixTypeDto.Name : string.Empty))
        .ReverseMap();

      #endregion
    }
  }
}
