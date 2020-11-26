﻿using RPGDataEditor.Wpf.Dialogue.ViewModels;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Dialogue.Views
{
    public partial class DialogueTab : UserControl
    {
        public DialogueTab() => InitializeComponent();

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
                    if (DataContext is DialogueTabViewModel vm)
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
            CategoryDialoguesList.Tag = senderButton.CommandParameter as string;
            CategoryDialoguesList.Visibility = System.Windows.Visibility.Visible;
            CategoriesList.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void BackToCategories_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CategoryDialoguesList.Visibility = System.Windows.Visibility.Collapsed;
            CategoriesList.Visibility = System.Windows.Visibility.Visible;
        }
    }
}