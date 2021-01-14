using System;
using System.ComponentModel;
using Fixit.Core.DataContracts.FixTemplates;

namespace Master.Database.Management.ServerlessApi.Helpers.Validators
{
  public static class OptionalQueryValidators
  {
    public static bool TryParseStatus(string status, out FixTemplateStatus? fixTemplateStatus)
    {
      fixTemplateStatus = null;
      var isParsable = !string.IsNullOrEmpty(status) && Enum.IsDefined(typeof(FixTemplateStatus), status);
      if (isParsable)
      {
        fixTemplateStatus = (FixTemplateStatus)Enum.Parse(typeof(FixTemplateStatus), status, true);
      }

      return isParsable;
    }

    public static bool TryParseTimestampUtc(string timestampUtcString, out long? timestampUtcLong)
    {
      timestampUtcLong = null;
      var typeConverter = TypeDescriptor.GetConverter(typeof(long));
      var isParsable = !string.IsNullOrEmpty(timestampUtcString) && typeConverter!= null && typeConverter.IsValid(timestampUtcString);
      if (isParsable)
      {
        timestampUtcLong = Convert.ToInt64(timestampUtcString);
      }

      return isParsable;
    }
  }
}
