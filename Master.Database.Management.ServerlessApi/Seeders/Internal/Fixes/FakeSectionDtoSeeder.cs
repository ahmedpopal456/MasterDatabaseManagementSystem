using System;
using System.Collections.Generic;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.FixTemplates.Sections;
using Master.Database.Management.DataLayer.Models.FixTemplates.Sections;

namespace Master.Database.Management.ServerlessApi.Seeders.Internal.Fixes
{
  internal class FakeSectionDtoSeeder : IFakeSeederAdapter<SectionDto>
  {
    public IList<SectionDto> SeedFakeDtos()
    {
      return new List<SectionDto>
      {
        new SectionDto()
        {
          Id = new Guid("0b7d5429-cad0-487f-40f8-08d8af5a64f5"),
          Name = "Section 1"
        }
      };
    }
  }
}
