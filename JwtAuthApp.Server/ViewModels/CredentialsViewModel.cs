using FluentValidation.Attributes;
using JwtAuthApp.Server.ViewModels.Validations;

namespace JwtAuthApp.Server.ViewModels
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}