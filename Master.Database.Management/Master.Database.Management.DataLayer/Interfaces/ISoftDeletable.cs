namespace Master.Database.Management.DataLayer.Interfaces
{
  public interface ISoftDeletable
  {
    public bool IsDeleted { get; set; }

    public long DeletedTimestampUtc { get; set; }
  }
}
