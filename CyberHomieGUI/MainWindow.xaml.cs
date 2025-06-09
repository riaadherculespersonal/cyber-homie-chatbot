using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CyberHomieGUI
{
    public partial class MainWindow : Window
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();
            TaskDataGrid.ItemsSource = tasks;
        }

        // Chatbot interaction
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInputBox.Text.Trim();
            if (!string.IsNullOrEmpty(userInput))
            {
                ChatHistoryTextBlock.Text += $"You: {userInput}\n";
                string botReply = GetBotReply(userInput);
                ChatHistoryTextBlock.Text += $"CyberHomie: {botReply}\n";
                UserInputBox.Clear();
            }
        }

        private string GetBotReply(string input)
        {
            // Basic canned reply for now
            return "Hey there! Ready to learn about cybersecurity?";
        }

        // Task Assistant logic
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleBox.Text.Trim();
            string description = TaskDescriptionBox.Text.Trim();
            DateTime? date = ReminderDatePicker.SelectedDate;

            if (!string.IsNullOrWhiteSpace(title) && date != null)
            {
                TaskItem task = new TaskItem
                {
                    Title = title,
                    Description = description,
                    ReminderDate = date.Value,
                    IsCompleted = false
                };

                tasks.Add(task);
                TaskDataGrid.Items.Refresh();

                TaskTitleBox.Clear();
                TaskDescriptionBox.Clear();
                ReminderDatePicker.SelectedDate = null;
            }
            else
            {
                MessageBox.Show("Please enter a task title and select a date.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MarkDoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskDataGrid.SelectedItem is TaskItem selectedTask)
            {
                selectedTask.IsCompleted = true;
                TaskDataGrid.Items.Refresh();
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskDataGrid.SelectedItem is TaskItem selectedTask)
            {
                tasks.Remove(selectedTask);
                TaskDataGrid.Items.Refresh();
            }
        }
    }

    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
