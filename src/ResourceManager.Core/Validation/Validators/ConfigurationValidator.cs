using FluentValidation;
using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource;
using System.IO;

namespace ResourceManager.Core.Validation
{
    public class ConfigurationValidator : AbstractValidator<IConfiguration>
    {
        public ConfigurationValidator()
        {
            RuleFor(x => x.GetDataSourceSection()["ConnectionString"]).NotEmpty().WithMessage(CustomMessages.Empty)
                                                                      .When(x => IsType(x, "sql"))
                                                                      .WithName("ConnectionString");

            RuleFor(x => x.GetDataSourceSection()["FolderPath"]).Must(x => x != null && Directory.Exists(x.ToString())).WithMessage("Folder doesn't exists")
                                                                .When(x => IsType(x, "local"))
                                                                .WithName("FolderPath");

            RuleFor(x => x.GetDataSourceSection()["Host"]).NotEmpty().WithMessage(CustomMessages.Empty)
                                                          .When(x => IsType(x, "ftp"))
                                                          .WithName("Host");
            RuleFor(x => x.GetDataSourceSection()["UserName"]).NotEmpty().WithMessage(CustomMessages.Empty)
                                                              .When(x => IsType(x, "ftp"))
                                                              .WithName("UserName");
            RuleFor(x => x.GetDataSourceSection()["Password"].Length).GreaterThan(0).WithMessage(CustomMessages.Empty)
                                                                     .When(x => IsType(x, "ftp"))
                                                                     .WithName("Password");

            static bool IsType(IConfiguration x, string type) => string.Compare(x[DataSourceExtensions.NameKey], type, true) == 0;
        }
    }
}
