namespace Master.Database.Management.DataLayer.DataAccess.Models.Paginations
{
  public interface IPaginationRequestDto
  {
    public int PageNumber { get; set; }

    public int? PageSize { get; set; }
  }
}
