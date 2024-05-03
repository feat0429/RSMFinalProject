namespace RSMFinalProject.DTO.Pagination
{
    using System.Collections.Generic;
    public record PagedListDto<T>
        (
            int PageSize,
            int CurrentPage,
            int TotalItemCount,
            int TotalPageCount,
            bool HasPrevious,
            bool HasNext,
            IEnumerable<T> Items
        );
}
