using FluentValidation;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using VbApi.Controllers;

namespace VbApi.Validations;

public class EmployeeValidator : AbstractValidator<Employee2>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).Length(min:10, max:250).WithMessage("Invalid Name");

        RuleFor(x => x.DateOfBirth).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email address is not valid.");
        RuleFor(x => x.Phone).Matches("^[0-9]{10}$").WithMessage("Phone is not valid.");

        RuleFor(x => x.HourlySalary).InclusiveBetween(50, 400)
            .WithMessage("Hourly salary does not fall within allowed range.");

        RuleFor(x => x.HourlySalary).MinLegalSalaryRequired(MinJuniorSalary: 50, MinSeniorSalary: 200);

        RuleFor(x => x.DateOfBirth).Must(dateOfBirth =>
        {
            var minAllowedBirthDate = DateTime.Today.AddYears(-65);
            return minAllowedBirthDate <= dateOfBirth;
        })
            .WithMessage("Birthdate is not valid.");
    }
}

public static class MinLegalSalaryRequiredRule
{
    public static IRuleBuilderOptions<T, TElement> MinLegalSalaryRequired<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder, double MinJuniorSalary, double MinSeniorSalary)
    {
        return ruleBuilder.Must((element,salary) =>
        {
            var employee2 = element as Employee2;
            var dateBeforeThirtyYears = DateTime.Today.AddYears(-30);
            var isOlderThanThirdyYears = employee2.DateOfBirth <= dateBeforeThirtyYears;
            var hourlySalary = (double)employee2.HourlySalary;

            return isOlderThanThirdyYears ? hourlySalary >= MinSeniorSalary : hourlySalary >= MinJuniorSalary;
        }).WithMessage("Minimum hourly salary is not valid.");

    }
}