using System;
using System.Linq;
using System.Net.Http;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Newtonsoft.Json;

namespace Master.Database.Management.ServerlessApi.Helpers.Validators.FixTemplates
{
  public class FixTemplateUpdateRequestValidators
  {
    public static bool IsValidFixTemplateUpdateRequest(HttpContent httpContent, out FixTemplateUpdateRequestDto fixTemplateUpdateRequestDto)
    {
      var isValid = false;
      fixTemplateUpdateRequestDto = null;

      try
      {
        var fixTemplateUpdateRequestDeserialized = JsonConvert.DeserializeObject<FixTemplateUpdateRequestDto>(httpContent.ReadAsStringAsync().Result);
        if (fixTemplateUpdateRequestDeserialized != null)
        {
          isValid = !HasNullOrEmpty(fixTemplateUpdateRequestDeserialized);

          if (isValid)
          {
            fixTemplateUpdateRequestDto = fixTemplateUpdateRequestDeserialized;
          }
        }
      }
      catch
      {
        // fallthrough
      }
      return isValid;
    }

    public static bool HasNullOrEmpty(FixTemplateUpdateRequestDto fixTemplateUpdateRequestDto)
    {
      return string.IsNullOrWhiteSpace(fixTemplateUpdateRequestDto.Name)
             || fixTemplateUpdateRequestDto.CategoryId.Equals(Guid.Empty)
             || fixTemplateUpdateRequestDto.TypeId.Equals(Guid.Empty)
             || string.IsNullOrWhiteSpace(fixTemplateUpdateRequestDto.Description)
             || fixTemplateUpdateRequestDto.UpdatedByUserId.Equals(Guid.Empty)
             || !fixTemplateUpdateRequestDto.Tags.Any()
             || fixTemplateUpdateRequestDto.Tags.Any(tag => string.IsNullOrWhiteSpace(tag))
             || FixTemplateCreateRequestValidators.HasNullOrEmpty(fixTemplateUpdateRequestDto.Sections);
    }
  }
}
