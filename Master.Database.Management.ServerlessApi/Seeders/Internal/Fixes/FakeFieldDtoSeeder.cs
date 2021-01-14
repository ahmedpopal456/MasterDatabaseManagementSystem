using System.Collections.Generic;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.FixTemplates.Fields;
using Master.Database.Management.DataLayer.Models.FixTemplates.Fields;

namespace Master.Database.Management.ServerlessApi.Seeders.Internal.Fixes
{
  internal class FakeFieldDtoSeeder : IFakeSeederAdapter<FieldDto>
  {
    public IList<FieldDto> SeedFakeDtos()
    {
      return new List<FieldDto>
      {
        new FieldDto()
        {
          Id = new System.Guid("9be203c5-d1c1-4ee5-e1e8-08d8af5a6528"),
          Name = "Field 1.1"
        },
        new FieldDto()
        {
          Id = new System.Guid("ad5ceffd-89cb-4a8d-e1e9-08d8af5a6528"),
          Name = "Field 1.2"
        }
      };
    }
  }
}
