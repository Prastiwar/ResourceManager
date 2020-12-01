﻿using RPGDataEditor.Core.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Views
{
    public partial class RequirementView : UserControl
    {
        public RequirementView()
        {
            InitializeComponent();
            DataContextChanged += RequirementView_DataContextChanged;
        }

        private void RequirementView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) =>
            SetRequirementPanel(DataContext as PlayerRequirementModel);

        private void QuestStage_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    if (DataContext is QuestRequirement requirement)
                    {
                        requirement.Stage = (QuestStage)Enum.Parse(typeof(QuestStage), selected.Name.ToString());
                    }
                }
            }
        }

        private void SetRequirementPanel(PlayerRequirementModel model)
        {
            bool isDialogue = model is DialogueRequirement;
            bool isQuest = model is QuestRequirement;
            bool isItem = model is ItemRequirement;
            RequirementType.SelectedIndex = isDialogue ? 0 :
                                            isQuest ? 1 :
                                            2;
            DialogueRequirementPanel.Visibility = isDialogue ? Visibility.Visible : Visibility.Collapsed;
            QuestRequirementPanel.Visibility = isQuest ? Visibility.Visible : Visibility.Collapsed;
            ItemRequirementPanel.Visibility = isItem ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RequirementType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    bool isDialogue = string.Compare(selected.Name, "dialogue", true) == 0;
                    bool isQuest = string.Compare(selected.Name, "quest", true) == 0;
                    bool isItem = string.Compare(selected.Name, "item", true) == 0;

                    if (isDialogue)
                    {
                        DataContext = new DialogueRequirement();
                    }
                    else if (isQuest)
                    {
                        DataContext = new QuestRequirement();
                    }
                    else if (isItem)
                    {
                        DataContext = new ItemRequirement();
                    }
                }
            }
        }
    }
}
