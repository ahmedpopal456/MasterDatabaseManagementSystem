using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Newtonsoft.Json;

namespace Master.Database.Management.ServerlessApi.Helpers.Validators.FixTemplates
{
  public static class FixTemplateCreateRequestValidators
  {
    public static bool IsValidFixTemplateCreateRequest(HttpContent httpContent, out FixTemplateCreateRequestDto fixTemplateCreateRequestDto)
    {
      var isValid = false;
      fixTemplateCreateRequestDto = null;

      try
      {
        var fixTemplateCreateRequestDeserialized = JsonConvert.DeserializeObject<FixTemplateCreateRequestDto>(httpContent.ReadAsStringAsync().Result);
        if (fixTemplateCreateRequestDeserialized != null)
        {
          isValid = !HasNullOrEmpty(fixTemplateCreateRequestDeserialized);

          if (isValid)
          {
            fixTemplateCreateRequestDto = fixTemplateCreateRequestDeserialized;
          }
        }
      }
      catch
      {
        // fallthrough
      }
      return isValid;
    }

    public static bool HasNullOrEmpty(FixTemplateCreateRequestDto fixTemplateCreateRequestDto)
    {
      return string.IsNullOrWhiteSpace(fixTemplateCreateRequestDto.Name)
             || fixTemplateCreateRequestDto.WorkTypeId.Equals(Guid.Empty)
             || fixTemplateCreateRequestDto.WorkCategoryId.Equals(Guid.Empty)
             || fixTemplateCreateRequestDto.FixUnitId.Equals(Guid.Empty)
             || string.IsNullOrWhiteSpace(fixTemplateCreateRequestDto.Description)
             || fixTemplateCreateRequestDto.CreatedByUserId.Equals(Guid.Empty)
             || fixTemplateCreateRequestDto.UpdatedByUserId.Equals(Guid.Empty)
             || !fixTemplateCreateRequestDto.Tags.Any()
             || fixTemplateCreateRequestDto.Tags.Any(tag => string.IsNullOrWhiteSpace(tag))
             || HasEmpty(fixTemplateCreateRequestDto.Sections);
    }

    public static bool HasEmpty(IEnumerable<FixTemplateSectionCreateRequestDto> fixTemplateSectionCreateRequestDtos)
    {
      var isEmpty = false;
      if (fixTemplateSectionCreateRequestDtos.Any())
      {
        isEmpty = fixTemplateSectionCreateRequestDtos.Any(fixTemplateSectionCreateRequestDto => string.IsNullOrWhiteSpace(fixTemplateSectionCreateRequestDto.Name)
                                                                                                || HasNullOrEmpty(fixTemplateSectionCreateRequestDto.Fields));
      }
      return isEmpty;
    }

    public static bool HasNullOrEmpty(IEnumerable<FixTemplateFieldCreateRequestDto> fixTemplateFieldCreateRequestDtos)
    {
      return fixTemplateFieldCreateRequestDtos.Any(fixTemplateFieldCreateRequestDto => string.IsNullOrWhiteSpace(fixTemplateFieldCreateRequestDto.Name)
                                                                                       || !fixTemplateFieldCreateRequestDto.Values.Any()
                                                                                       || fixTemplateFieldCreateRequestDto.Values.Any(value => string.IsNullOrWhiteSpace(value)));
    }
  }
}
