using FluentValidation;
using RPGDataEditor.Core.Connection;
using System.IO;

namespace RPGDataEditor.Core.Validation
{
    public class LocalFileClientValidator : AbstractValidator<LocalFileClient>
    {
        public LocalFileClientValidator() => RuleFor(x => x.FolderPath).Must(path => Directory.Exists(path)).WithMessage("Folder doesn't exists");
    }
}