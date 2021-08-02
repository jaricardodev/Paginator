namespace Jaricardodev.Paginator.Model.Capabilities
{
    public interface IPaginatedList
    {
        int TotalItemsCount { get; set; }
        int TotalPageCount { get; set; }
    }
}
