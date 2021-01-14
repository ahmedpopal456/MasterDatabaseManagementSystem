namespace Master.Database.Management.DataLayer.Models.Extensions
{
  public interface ISoftDeletable
  {
    public bool IsDeleted { get; set; }

    public long DeletedTimestampUtc { get; set; }
  }
}
