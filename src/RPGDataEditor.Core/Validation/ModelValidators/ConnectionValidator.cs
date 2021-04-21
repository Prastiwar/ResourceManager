//using FluentValidation;
//using RPGDataEditor.Connection;
//using RPGDataEditor.Core.Connection;
//using System.IO;

//namespace RPGDataEditor.Core.Validation
//{
//    public class ConnectionValidator : AbstractValidator<IResourceClient>
//    {
//        public ConnectionValidator()
//        {
//            RuleFor(x => ((ExplorerResourceClient)x).FolderPath).Must(path => Directory.Exists(path))
//                                                            .WithMessage("Folder doesn't exists")
//                                                            .When(x => x is ExplorerResourceClient);

//            RuleFor(x => ((FtpResourceClient)x).Host).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x is FtpResourceClient);
//            RuleFor(x => ((FtpResourceClient)x).UserName).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x is FtpResourceClient);
//            RuleFor(x => ((FtpResourceClient)x).Password.Length).GreaterThan(0).WithMessage(CustomMessages.Empty).When(x => x is FtpResourceClient);

//            RuleFor(x => ((MssqlResourceClient)x).ConnectionString.Length).GreaterThan(0).WithMessage(CustomMessages.Empty).When(x => x is FtpResourceClient);
//        }
//    }
//}
