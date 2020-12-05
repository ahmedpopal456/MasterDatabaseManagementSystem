﻿namespace Master.Database.Management.DataLayer.Interfaces
{
  public interface IAuditable
  {
    public long CreatedTimestampUtc { get; set; }

    public long UpdatedTimestampUtc { get; set; }

    public long LastAccessedTimestampUtc { get; set; }
  }
}
