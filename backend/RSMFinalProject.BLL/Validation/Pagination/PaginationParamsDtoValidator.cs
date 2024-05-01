namespace RSMFinalProject.BLL.Validation.Pagination
{
    using FluentValidation;
    using RSMFinalProject.DTO.Pagination;
    public class PaginationParamsDtoValidator : AbstractValidator<PaginationParamsDto>
    {
        public PaginationParamsDtoValidator()
        {
            RuleFor(p => p.CurrentPage)
                .NotNull()
                .GreaterThan(0);

            RuleFor(p => p.PageSize)
                .NotNull()
                .InclusiveBetween(1, 100);
        }
    }
}
