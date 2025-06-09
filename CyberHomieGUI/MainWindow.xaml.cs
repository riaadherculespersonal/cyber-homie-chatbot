using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CyberHomieGUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadQuizQuestions();
            DisplayCurrentQuestion();
        }

        // ========== CHATBOT ==========
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            ChatHistoryTextBlock.Text += $"You: {userInput}\nCyberHomie: Hey there! Ready to learn about cybersecurity?\n";
            UserInputTextBox.Clear();
        }

        // ========== TASK ASSISTANT ==========
        public class TaskItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string ReminderDate { get; set; }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            TaskItem task = new TaskItem
            {
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                ReminderDate = ReminderDatePicker.SelectedDate?.ToShortDateString() ?? "No Date"
            };

            TaskListView.Items.Add(task);
            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
            ReminderDatePicker.SelectedDate = null;
        }

        private void MarkDoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem != null)
            {
                MessageBox.Show("Task marked as done!");
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem != null)
            {
                TaskListView.Items.Remove(TaskListView.SelectedItem);
            }
        }

        // ========== QUIZ ==========
        public class QuizQuestion
        {
            public string Question { get; set; }
            public List<string> Answers { get; set; }
            public int CorrectIndex { get; set; }
        }

        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
        private int currentQuestionIndex = 0;
        private int score = 0;

        private void LoadQuizQuestions()
        {
            quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Answers = new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                    CorrectIndex = 2
                },
                new QuizQuestion
                {
                    Question = "Which of the following is a strong password?",
                    Answers = new List<string> { "123456", "password", "Welcome123", "A$7dFg!2kLm" },
                    CorrectIndex = 3
                },
                new QuizQuestion
                {
                    Question = "Which one is an example of social engineering?",
                    Answers = new List<string> { "Brute force attack", "Phishing email", "Firewall misconfiguration", "SQL injection" },
                    CorrectIndex = 1
                }
            };
        }

        private void DisplayCurrentQuestion()
        {
            if (currentQuestionIndex >= quizQuestions.Count)
            {
                QuestionTextBlock.Text = "Quiz Complete!";
                AnswerButtonsPanel.Children.Clear();
                ScoreTextBlock.Text = $"Your Score: {score} / {quizQuestions.Count}";
                NextQuestionButton.Visibility = Visibility.Collapsed;
                return;
            }

            var question = quizQuestions[currentQuestionIndex];
            QuestionTextBlock.Text = question.Question;
            AnswerButtonsPanel.Children.Clear();

            for (int i = 0; i < question.Answers.Count; i++)
            {
                int answerIndex = i;
                Button answerButton = new Button
                {
                    Content = question.Answers[i],
                    Margin = new Thickness(0, 5, 0, 5),
                    Tag = answerIndex
                };
                answerButton.Click += AnswerButton_Click;
                AnswerButtonsPanel.Children.Add(answerButton);
            }

            FeedbackTextBlock.Text = "";
            NextQuestionButton.Visibility = Visibility.Hidden;
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                int selectedIndex = (int)clickedButton.Tag;
                int correctIndex = quizQuestions[currentQuestionIndex].CorrectIndex;

                if (selectedIndex == correctIndex)
                {
                    FeedbackTextBlock.Text = "Correct!";
                    score++;
                }
                else
                {
                    FeedbackTextBlock.Text = $"Incorrect. Correct answer: {quizQuestions[currentQuestionIndex].Answers[correctIndex]}";
                }

                foreach (Button btn in AnswerButtonsPanel.Children)
                    btn.IsEnabled = false;

                NextQuestionButton.Visibility = Visibility.Visible;
            }
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex++;
            DisplayCurrentQuestion();
        }
    }
}
