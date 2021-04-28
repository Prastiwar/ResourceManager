using Prism.Services.Dialogs;
using RPGDataEditor.Extensions.Prism.Wpf.Navigation;
using System.Collections.Generic;

namespace RPGDataEditor.Extensions.Prism.Wpf
{
    public static class DialogExtensions
    {
        public static Mvvm.Navigation.INavigationContext ToDomain(this global::Prism.Regions.NavigationContext context) => new PrismNavigationContext(context);

        public static IDialogParameters BuildPrism(this Mvvm.Navigation.DialogParametersBuilder builder)
        {
            DialogParameters prismParameters = new DialogParameters();
            foreach (KeyValuePair<string, object> parameter in builder)
            {
                prismParameters.Add(parameter.Key, parameter.Value);
            }
            return prismParameters;
        }

        public static IDialogParameters ToPrism(this Mvvm.Navigation.IDialogParameters parameters)
        {
            DialogParameters prismParameters = new DialogParameters();
            foreach (string parameter in parameters.Parameters)
            {
                prismParameters.Add(parameter, parameters.GetValue(parameter));
            }
            return prismParameters;
        }

        public static Mvvm.Navigation.IDialogParameters ToDomain(this IDialogParameters parameters)
        {
            Mvvm.Navigation.DialogParameters domainParameters = new Mvvm.Navigation.DialogParameters();
            foreach (string parameter in parameters.Keys)
            {
                domainParameters.Add(parameter, parameters.GetValue<object>(parameter));
            }
            return domainParameters;
        }

        public static IDialogResult ToPrism(this Mvvm.Navigation.IDialogResult result)
            => new DialogResult(result.IsSuccess ? ButtonResult.OK : ButtonResult.Cancel, result.Parameters.ToPrism());

        public static Mvvm.Navigation.IDialogResult ToDomain(this IDialogResult result)
            => new Mvvm.Navigation.DialogResult(result.Result.HasFlag(ButtonResult.OK) || result.Result.HasFlag(ButtonResult.Yes), result.Parameters.ToDomain());
    }
}
