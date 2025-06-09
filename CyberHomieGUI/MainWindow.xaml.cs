using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CyberHomieGUI
{
    public partial class MainWindow : Window
    {
        // Reminder Task model
        public class ReminderTask
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime ReminderDate { get; set; }
            public bool IsDone { get; set; }
        }

        // Quiz Question model
        public class QuizQuestion
        {
            public string Question { get; set; }
            public List<string> Answers { get; set; }
            public int CorrectAnswerIndex { get; set; }
        }

        private List<ReminderTask> tasks = new();
        private List<QuizQuestion> quizQuestions = new();
        private int currentQuestionIndex = 0;
        private int score = 0;

        public MainWindow()
        {
            InitializeComponent();
            LoadQuizQuestions();
            DisplayQuestion();
            ShowHelpOptions();
        }

        private void ShowHelpOptions()
        {
            ChatHistoryTextBlock.Text += "CyberHomie 🧠: Yo! Ask me anything cyber-related. Try these:\n" +
                "- What's phishing?\n" +
                "- Tips for strong passwords?\n" +
                "- What is malware?\n" +
                "- What's 2FA?\n" +
                "- Why should I update software?\n" +
                "- I need advice!\n\n";
        }

        // CHATBOT -- SEND MESSAGE
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInputTextBox.Text.Trim();

            if (string.IsNullOrEmpty(userMessage))
                return;

            ChatHistoryTextBlock.Text += $"You: {userMessage}\n";
            UserInputTextBox.Clear();

            string response = GetNlpResponse(userMessage);
            ChatHistoryTextBlock.Text += $"CyberHomie 🧠: {response}\n";
        }

        // CHATBOT -- NLP MATCHING
        private string GetNlpResponse(string input)
        {
            string message = input.ToLower();

            // Create keyword matching
            var responses = new Dictionary<string[], List<string>>
            {
                { new[] { "phishing", "scam", "fake link" }, new List<string> {
                    "Yo fam, phishing is when someone tries to trick you with fake emails. Don't click weird links 🧃",
                    "That’s a scam move, bro. If it smells phishy, delete it!",
                    "Nah dawg, that link ain’t it. Real homies don’t get phished."
                }},
                { new[] { "password", "strong password", "password tip", "password safety" }, new List<string> {
                    "Use 12+ characters, homie. Mix numbers, letters, and 🔥 symbols!",
                    "Never use '123456', bro. That's rookie level.",
                    "Make it strong, make it weird. Ain’t nobody cracking that!"
                }},
                { new[] { "malware", "virus", "spyware" }, new List<string> {
                    "That’s bad news bro — malware messes up your stuff. Always run protection 💾",
                    "Malware be lurking like a thief. Stay updated and scan often.",
                    "One click on a shady file and boom 💥 you got malware. Don’t play."
                }},
                { new[] { "2fa", "two factor", "authentication" }, new List<string> {
                    "2FA is like a secret handshake for your accounts 🤝",
                    "Extra layer = extra safety. Always turn on 2FA, bro!",
                    "Hackers hate 2FA. That’s why we love it 😎"
                }},
                { new[] { "update", "patch", "software" }, new List<string> {
                    "Always update your gear, homie. Patches fix weak spots.",
                    "Outdated software is like an open window. Keep it closed!",
                    "Stay sharp, update often 🔧"
                }},
                { new[] { "tip", "advice", "suggestion" }, new List<string> {
                    "Don't share personal info. Even your cat's name could be a clue 🐱",
                    "Lock your screen when you leave. Always.",
                    "Use different passwords for everything, trust me 🔑"
                }},
                { new[] { "hello", "hi", "hey", "yo" }, new List<string> {
                    "Yo! You good? Wanna learn some cyber-stuff, homie?",
                    "Hey hey! CyberHomie’s in the building 🛡️",
                    "Wassup, legend? Ask me anything techy!"
                }},
                { new[] { "help", "what can i ask", "what do you know" }, new List<string> {
                    "Ask me about phishing, passwords, malware, 2FA, or just tips to stay safe, bro.",
                    "Try 'what’s a strong password?' or 'tell me about 2FA'. I gotchu.",
                    "I’m the plug for cybersecurity facts. Fire away!"
                }},
            };

            // Match message to a category
            foreach (var pair in responses)
            {
                foreach (var keyword in pair.Key)
                {
                    if (message.Contains(keyword))
                    {
                        var possibleResponses = pair.Value;
                        var random = new Random();
                        return possibleResponses[random.Next(possibleResponses.Count)];
                    }
                }
            }

            // Fallback if no match
            return "Hmm... I didn’t catch that, bro. Try asking me about phishing, passwords, or 2FA. 💬";
        }

        // TASK ASSISTANT
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            DateTime? reminderDate = ReminderDatePicker.SelectedDate;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || reminderDate == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            tasks.Add(new ReminderTask
            {
                Title = title,
                Description = description,
                ReminderDate = reminderDate.Value,
                IsDone = false
            });

            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
            ReminderDatePicker.SelectedDate = null;

            RefreshTaskList();
        }

        private void MarkDoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem is ReminderTask task)
            {
                task.IsDone = true;
                RefreshTaskList();
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem is ReminderTask task)
            {
                tasks.Remove(task);
                RefreshTaskList();
            }
        }

        private void RefreshTaskList()
        {
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = tasks;
        }

        // QUIZ TAB
        private void LoadQuizQuestions()
        {
            quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion { Question = "What should you do if you receive a suspicious email?", Answers = new List<string> { "Reply to it", "Ignore it", "Report it", "Click the link" }, CorrectAnswerIndex = 2 },
                new QuizQuestion { Question = "What does 2FA stand for?", Answers = new List<string> { "Two-Factor Authentication", "Fast Action", "Free Access", "Firewall Access" }, CorrectAnswerIndex = 0 },
                new QuizQuestion { Question = "Which is the strongest password?", Answers = new List<string> { "password123", "P@ssw0rd!", "123456", "qwerty" }, CorrectAnswerIndex = 1 }
            };
        }

        private void DisplayQuestion()
        {
            if (currentQuestionIndex >= quizQuestions.Count)
            {
                QuestionTextBlock.Text = $"Quiz complete! Your score: {score}/{quizQuestions.Count}";
                AnswerButtonsPanel.Children.Clear();
                NextQuestionButton.Visibility = Visibility.Collapsed;
                return;
            }

            var question = quizQuestions[currentQuestionIndex];
            QuestionTextBlock.Text = question.Question;
            AnswerButtonsPanel.Children.Clear();

            for (int i = 0; i < question.Answers.Count; i++)
            {
                var button = new Button
                {
                    Content = question.Answers[i],
                    Tag = i,
                    Margin = new Thickness(4),
                    Padding = new Thickness(10),
                    Background = System.Windows.Media.Brushes.LightCyan
                };
                button.Click += AnswerButton_Click;
                AnswerButtonsPanel.Children.Add(button);
            }

            FeedbackTextBlock.Text = string.Empty;
            NextQuestionButton.Visibility = Visibility.Hidden;
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int selected = (int)button.Tag;

            var question = quizQuestions[currentQuestionIndex];

            if (selected == question.CorrectAnswerIndex)
            {
                FeedbackTextBlock.Text = "🔥 Correct!";
                score++;
            }
            else
            {
                FeedbackTextBlock.Text = $"❌ Nope! Correct answer: {question.Answers[question.CorrectAnswerIndex]}";
            }

            foreach (Button b in AnswerButtonsPanel.Children)
                b.IsEnabled = false;

            NextQuestionButton.Visibility = Visibility.Visible;
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex++;
            DisplayQuestion();
        }
    }
}
