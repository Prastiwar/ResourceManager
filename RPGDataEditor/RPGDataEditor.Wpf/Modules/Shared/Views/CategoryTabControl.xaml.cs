using RPGDataEditor.Core.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

        private void OnModelListTagBindingChanged(object sender, EventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(CategoryModelList.ItemsSource);
            if (view != null)
            {
                view.Refresh();
            }
        }

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

        private void RenameCategory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            if (senderButton.Tag is StackPanel editorPanel)
            {
                editorPanel.Visibility = System.Windows.Visibility.Visible;
                if (editorPanel.Children[0] is TextBox textBox)
                {
                    textBox.Text = senderButton.DataContext as string;
                }
                if (senderButton.CommandParameter is TextBlock categoryLabel)
                {
                    categoryLabel.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private async void AcceptRenameCategory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            if (senderButton.Parent is StackPanel editorPanel)
            {
                editorPanel.Visibility = System.Windows.Visibility.Collapsed;
                if (editorPanel.Children[0] is TextBox textBox)
                {
                    string newCategory = textBox.Text;
                    if (DataContext is ICategorizedTabViewModel vm)
                    {
                        string oldCategory = senderButton.DataContext as string;
                        bool canRename = await vm.RenameCategoryAsync(oldCategory, newCategory);
                        if (canRename && senderButton.Tag is TextBlock categoryLabel)
                        {
                            categoryLabel.Text = newCategory;
                            categoryLabel.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void DeclineRenameCategory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            if (senderButton.Parent is StackPanel editorPanel)
            {
                editorPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (senderButton.Tag is TextBlock categoryLabel)
            {
                categoryLabel.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ShowCategory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            CategoryModelList.Tag = senderButton.CommandParameter as string;
            CategoryModelList.Visibility = System.Windows.Visibility.Visible;
            CategoriesList.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void BackToCategories_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CategoryModelList.Visibility = System.Windows.Visibility.Collapsed;
            CategoriesList.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
