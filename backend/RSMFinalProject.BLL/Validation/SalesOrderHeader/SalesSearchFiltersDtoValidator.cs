namespace RSMFinalProject.BLL.Validation.SalesOrderHeader
{
    using FluentValidation;
    using RSMFinalProject.DTO.SalesOrderHeader.Filters;

    public class SalesSearchFiltersDtoValidator : AbstractValidator<SalesSearchtFiltersDto>
    {
        public SalesSearchFiltersDtoValidator()
        {
            RuleFor(filter => filter.StartDate)
                .LessThanOrEqualTo(s => s.EndDate)
                .When(s => s.EndDate is not null);

            RuleFor(filter => filter.EndDate)
                .GreaterThanOrEqualTo(s => s.StartDate)
                .When(s => s.StartDate is not null);

            RuleFor(filter => filter.ProductCategoryId)
                .GreaterThan(0);
        }
    }
}
