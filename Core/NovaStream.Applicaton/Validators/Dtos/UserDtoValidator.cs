﻿namespace NovaStream.Application.Validators.Dtos;

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

        RuleFor(dto => dto.Nickname).NotEmpty().WithMessage("User nickname cannot be empty!");
        RuleFor(dto => dto.Nickname).MinimumLength(4).WithMessage("The length of the user nickname cannot be less than 4 characters!");
        RuleFor(dto => dto.Nickname).MaximumLength(7).WithMessage("The length of the user nickname cannot be greater than 7 characters!");

        RuleFor(dto => dto.Password).NotEmpty().WithMessage("User password cannot be empty!");
        RuleFor(dto => dto.Password).MinimumLength(6).WithMessage("The length of the user password cannot be less than 6 characters!");
        RuleFor(dto => dto.Password).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$").WithMessage("Invalid password!");
    }
}
