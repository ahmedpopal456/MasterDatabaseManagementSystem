using System.Collections.Generic;
using System.Net.Http;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Sections;
using Newtonsoft.Json;

namespace Master.Database.Management.ServerlessApi.Helpers.Validators.Sections
{
  public static class SectionCreateRequestValidators
  {
    public static bool IsValidSectionCreateRequest(HttpContent httpContent, out SectionCreateRequestDto sectionCreateRequestDto)
    {
      var isValid = false;
      sectionCreateRequestDto = null;

      try
      {
        var sectionCreateRequestDeserialized = JsonConvert.DeserializeObject<SectionCreateRequestDto>(httpContent.ReadAsStringAsync().Result);
        if (sectionCreateRequestDeserialized != null)
        {
          isValid = !HasNullOrEmpty(sectionCreateRequestDeserialized);

          if (isValid)
          {
            sectionCreateRequestDto = sectionCreateRequestDeserialized;
          }
        }
      }
      catch
      {
        // fallthrough
      }
      return isValid;
    }

    public static bool HasNullOrEmpty(SectionCreateRequestDto sectionCreateRequestDto)
    {
      return string.IsNullOrWhiteSpace(sectionCreateRequestDto.Name);
    }
  }
}
