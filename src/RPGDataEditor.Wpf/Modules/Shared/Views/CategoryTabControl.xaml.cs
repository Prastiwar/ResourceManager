using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Views
{
    public partial class CategoryTabControl : UserControl
    {
        public CategoryTabControl()
        {
            InitializeComponent();
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ListView));
            dpd.AddValueChanged(CategoryModelList, OnModelListBindingChanged);
            DependencyPropertyDescriptor tagDpd = DependencyPropertyDescriptor.FromProperty(TagProperty, typeof(FrameworkElement));
            tagDpd.AddValueChanged(CategoryModelList, OnModelListTagBindingChanged);
        }

        private void OnModelListTagBindingChanged(object sender, EventArgs e) => CategoryModelList.Refresh();

        private void OnModelListBindingChanged(object sender, EventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(CategoryModelList.ItemsSource);
            if (view != null)
            {
                view.Filter = CategoryFilter;
            }
        }

        private bool CategoryFilter(object item)
        {
            if (CategoryModelList.Tag is string tag)
            {
                if (string.IsNullOrEmpty(tag))
                {
                    return true;
                }
                dynamic categorizedItem = item;
                return categorizedItem.Category.CompareTo(tag) == 0;
            }
            return true;
        }

        private void RenameCategory_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            if (senderButton.Tag is StackPanel editorPanel)
            {
                editorPanel.Visibility = Visibility.Visible;
                if (editorPanel.Children[0] is TextBox textBox)
                {
                    textBox.Text = senderButton.DataContext as string;
                }
                if (senderButton.CommandParameter is TextBlock categoryLabel)
                {
                    categoryLabel.Visibility = Visibility.Collapsed;
                }
            }
        }

        public static class CommandExtensions
        {
            public static Task<object> ExecuteAsync(ICommand command)
            {
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                command.Execute(tcs);
                return tcs.Task;
            }
        }

        private async void AcceptRenameCategory_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            if (senderButton.Parent is StackPanel editorPanel)
            {
                editorPanel.Visibility = Visibility.Collapsed;
                if (editorPanel.Children[0] is TextBox textBox)
                {
                    string newCategory = textBox.Text;
                    string oldCategory = senderButton.DataContext as string;
                    dynamic viewModel = DataContext;
                    bool canRename = await viewModel.RenameCategoryAsync(oldCategory, newCategory);
                    if (senderButton.Tag is TextBlock categoryLabel)
                    {
                        if (canRename)
                        {
                            categoryLabel.Text = newCategory;
                        }
                        categoryLabel.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void DeclineRenameCategory_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            if (senderButton.Parent is StackPanel editorPanel)
            {
                editorPanel.Visibility = Visibility.Collapsed;
            }
            if (senderButton.Tag is TextBlock categoryLabel)
            {
                categoryLabel.Visibility = Visibility.Visible;
            }
        }

        private void ShowCategory_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            CategoryModelList.Tag = senderButton.CommandParameter as string;
            CategoryModelList.Visibility = Visibility.Visible;
            CategoriesList.Visibility = Visibility.Collapsed;
        }

        private void BackToCategories_Click(object sender, RoutedEventArgs e)
        {
            CategoryModelList.Visibility = Visibility.Collapsed;
            CategoriesList.Visibility = Visibility.Visible;
        }
    }
}
