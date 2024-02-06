using FluentValidation;
using WebApplication1.Entities;

namespace WebApplication1.Models.Validators
{
    public class InventoryQueryValidator : AbstractValidator<InventoryQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = { nameof(Inventory.Id) };
        public InventoryQueryValidator()
        {
            RuleFor(a => a.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(a => a.PageSize).Custom((searchPageSize, context) =>
            {
                if (!allowedPageSizes.Contains(searchPageSize))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }

            });

            RuleFor(a => a.SortBy)
                .Must(sortByValue => string.IsNullOrEmpty(sortByValue) || allowedSortByColumnNames.Contains(sortByValue))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");


        }
    }
}
