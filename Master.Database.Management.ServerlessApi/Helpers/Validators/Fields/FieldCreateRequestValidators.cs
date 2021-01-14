using System;
using System.Collections.Generic;
using System.Net.Http;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Fields;
using Newtonsoft.Json;

namespace Master.Database.Management.ServerlessApi.Helpers.Validators.Fields
{
  public static class FieldCreateRequestValidators
  {
    public static bool IsValidFieldCreateRequest(HttpContent httpContent, out FieldCreateRequestDto fieldCreateRequestDto)
    {
      var isValid = false;
      fieldCreateRequestDto = null;

      try
      {
        var fieldCreateRequestDeserialized = JsonConvert.DeserializeObject<FieldCreateRequestDto>(httpContent.ReadAsStringAsync().Result);
        if (fieldCreateRequestDeserialized != null)
        {
          isValid = !HasNullOrEmpty(fieldCreateRequestDeserialized);

          if (isValid)
          {
            fieldCreateRequestDto = fieldCreateRequestDeserialized;
          }
        }
      }
      catch
      {
        // fallthrough
      }
      return isValid;
    }

    public static bool HasNullOrEmpty(FieldCreateRequestDto fieldCreateRequestDto)
    {
      return fieldCreateRequestDto.Name.Equals(string.Empty);
    }
  }
}
