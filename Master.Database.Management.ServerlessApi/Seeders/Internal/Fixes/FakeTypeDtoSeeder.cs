using System;
using System.Collections.Generic;
using System.Text;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Fixes.Types;
using Master.Database.Management.DataLayer.Models;

namespace Master.Database.Management.ServerlessApi.Seeders.Internal.Fixes
{
  internal class FakeTypeDtoSeeder : IFakeSeederAdapter<FixTypeDto>
  {
    /// <summary>
    /// Creates 5 new 'FixTypes': New, Demolish, Repair, Replace and Modernize,
    /// with their respective new unique Ids.
    /// </summary>
    /// <returns></returns>
    public IList<FixTypeDto> SeedFakeDtos()
    {
      return new List<FixTypeDto>
      {
        new FixTypeDto()
        {
          Id = new Guid("445e50d1-b2e7-4c25-a628-c610aed7a357"),
          Name = "New"
        },
        new FixTypeDto()
        {
          Id = new Guid("3e87aca0-28c6-4f2d-9516-c3e33cc3f1b6"),
          Name = "Demolish"
        },
        new FixTypeDto()
        {
          Id = new Guid("6f961743-3ae9-4a8e-9727-bd45b460bbaf"),
          Name = "Repair"
        },
        new FixTypeDto()
        {
          Id = new Guid("7a0b05e1-d90d-4195-bd14-f192e4d0132f"),
          Name = "Replace"
        },
        new FixTypeDto()
        {
          Id = new Guid("bf83cad2-8108-4809-beae-3b1e9f63b681"),
          Name = "Modernize"
        },
      };
    }
  }
}
