namespace NovaStream.Application.Validators.Dtos;

public class SignInUserDtoValidator : AbstractValidator<SignInUserDto>
{
    public SignInUserDtoValidator()
    {
        RuleFor(dto => dto.Email).NotEmpty().WithMessage("User email address cannot be empty!");
        RuleFor(dto => dto.Email).EmailAddress().WithMessage("Invalid email address!");

        RuleFor(dto => dto.Password).NotEmpty().WithMessage("User password cannot be empty!");
    }
}

public class SignUpUserDtoValidator : AbstractValidator<SignUpUserDto>
{
    public SignUpUserDtoValidator()
    {
        RuleFor(dto => dto.Email).NotEmpty().WithMessage("User email cannot be empty!");
        RuleFor(dto => dto.Email).EmailAddress().WithMessage("Invalid email address!");

        RuleFor(dto => dto.Password).NotEmpty().WithMessage("User password cannot be empty!");

        RuleFor(dto => dto.Password).MinimumLength(8).WithMessage("The length of the user password cannot be less than 8 characters!");
        RuleFor(dto => dto.Password).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$").WithMessage("Invalid password!");
    }
}
