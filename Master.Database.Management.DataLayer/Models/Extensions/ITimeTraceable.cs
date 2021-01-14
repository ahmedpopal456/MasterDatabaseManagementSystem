namespace Master.Database.Management.DataLayer.Models.Extensions
{
  public interface ITimeTraceable
  {
    public long CreatedTimestampUtc { get; set; }

    public long UpdatedTimestampUtc { get; set; }

    public long LastAccessedTimestampUtc { get; set; }
  }
}
