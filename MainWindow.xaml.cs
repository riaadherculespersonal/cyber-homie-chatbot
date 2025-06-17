using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CyberHomieGUI
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, List<string>> chatbotResponses = new();
        private readonly Dictionary<string, int> responseIndices = new();
        private readonly Random random = new();

        private List<QuizQuestion> quizQuestions = new();
        private int currentQuestionIndex = 0;
        private int score = 0;

        private List<string> activityLog = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadChatbotResponses();
            LoadQuizQuestions();
            DisplayGreetingMenu();
        }

        private void DisplayGreetingMenu()
        {
            ChatHistory.Text += "CyberHomie: Yo! I'm your digital homie 💻 – here's what I can school you on:\n";
            ChatHistory.Text += "[1] Phishing 🎣\n[2] Password Safety 🔑\n[3] Firewalls 🧱\n[4] Social Engineering 🎭\n[5] 2FA 🔐\n";
            ChatHistory.Text += "Type the number or ask a question to learn more!\n";
        }

        private void LoadChatbotResponses()
        {
            chatbotResponses["1"] = new List<string> {
                "Phishing’s when scammers bait you with fake links or emails. Stay alert, homie. 🎣",
                "Phishing = fraud vibes. Hover before you click. 😂",
                "Hackers out here sending fake login pages. Don’t bite. 🐟",
                "Fake banks? Fake invoices? That’s phishing too. Stay woke. 🧠",
                "If it feels sus, it probably is. Spam it and dip. 🚫"
            };

            chatbotResponses["2"] = new List<string> {
                "Passwords gotta be strong, bro — no birthdays or pet names. Use symbols! 🔐",
                "Longer = stronger. Passphrases work too, my G! 💪",
                "Use different passwords for each app – don’t get lazy. 🔄",
                "Switch it up every few months to stay ahead. 🔁",
                "Avoid '123456' or 'admin' — come on now 😅"
            };

            chatbotResponses["3"] = new List<string> {
                "Firewalls are like bouncers for your data — keep the sketch out. 🧱",
                "They filter bad traffic from reaching your system. 🚷",
                "Corporate networks use them heavy — for a reason. 💼",
                "No firewall = open doors to hackers. Nah fam. 🚪",
                "Windows Defender has one — make sure it’s on. ✔"
            };

            chatbotResponses["4"] = new List<string> {
                "Social engineers sweet talk you into leaking info. Don’t fall for the game. 🎭",
                "They’ll act like your boss or IT guy — verify always. 🧑‍💼",
                "If someone rushes you to act quick — 🚨 red flag.",
                "Hackers manipulate emotions. Stay sharp. 🧠",
                "Google their email or double-check internally. 🔎"
            };

            chatbotResponses["5"] = new List<string> {
                "2FA = backup squad. Use it wherever you can. 📲",
                "Even if someone gets your password, 2FA blocks them. Double up! 🔐",
                "SMS is okay, but authenticator apps are 🔥.",
                "Some platforms even offer biometric 2FA. Tech flex. 😎",
                "Never ignore that 6-digit code prompt — it’s your bodyguard. 🛡"
            };

            foreach (var key in chatbotResponses.Keys)
                responseIndices[key] = 0;
        }

        private void HandleChatInput()
        {
            string input = UserInputTextBox.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(input)) return;

            ChatHistory.Text += $"You: {input}\n";

            string? responseKey = chatbotResponses.Keys.FirstOrDefault(k =>
                input.Contains(k) ||
                (k == "1" && input.Contains("phish")) ||
                (k == "2" && input.Contains("password")) ||
                (k == "3" && input.Contains("firewall")) ||
                (k == "4" && input.Contains("social")) ||
                (k == "5" && (input.Contains("2fa") || input.Contains("two-factor")))
            );

            if (!string.IsNullOrEmpty(responseKey))
            {
                var responses = chatbotResponses[responseKey];
                int index = responseIndices[responseKey];

                ChatHistory.Text += $"CyberHomie: {responses[index]}\n";
                responseIndices[responseKey] = (index + 1) % responses.Count;

                LogActivity($"Asked about topic {responseKey}");
            }
            else
            {
                ChatHistory.Text += "CyberHomie: I’m not sure what you mean, try a number or keyword like ‘firewall’. 🤷\n";
            }

            UserInputTextBox.Clear();
            ChatScrollViewer.ScrollToEnd();
        }

        private void LogActivity(string message)
        {
            if (activityLog.Count >= 10)
                activityLog.RemoveAt(0);
            activityLog.Add($"{DateTime.Now:t} – {message}");
        }

        private void ShowLogButton_Click(object sender, RoutedEventArgs e)
        {
            ChatHistory.Text += "CyberHomie: Here's what we've done so far:\n";
            foreach (var entry in activityLog)
            {
                ChatHistory.Text += $"• {entry}\n";
            }
            ChatScrollViewer.ScrollToEnd();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) => HandleChatInput();

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                HandleChatInput();
        }

        public class TaskItem
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public DateTime ReminderDate { get; set; }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                !ReminderDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Fill in all the fields, homie.");
                return;
            }

            TaskItem newTask = new TaskItem
            {
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                ReminderDate = ReminderDatePicker.SelectedDate.Value
            };

            TaskListView.Items.Add(newTask);
            LogActivity($"Added task: {newTask.Title}");

            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
            ReminderDatePicker.SelectedDate = null;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem is TaskItem selected)
            {
                LogActivity($"Deleted task: {selected.Title}");
                TaskListView.Items.Remove(selected);
            }
        }

        private void MarkDoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem is TaskItem selected)
            {
                MessageBox.Show($"Task '{selected.Title}' marked as done! ✅");
                LogActivity($"Marked task done: {selected.Title}");
            }
        }

        public class QuizQuestion
        {
            public string? Question { get; set; }
            public List<string>? Answers { get; set; }
            public int CorrectIndex { get; set; }
        }

        private void LoadQuizQuestions()
        {
            quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion { Question = "What is phishing?", Answers = new List<string>{ "A scam to steal your data", "An email scam pretending to be real", "Clickbait used by cyber crooks", "A fake login page trick", "Emails from 'banks' asking for info" }, CorrectIndex = 0 },
                new QuizQuestion { Question = "Which is a strong password?", Answers = new List<string>{ "P@ssw0rd!234", "UniquePhrase!88", "123456", "YourName123", "admin" }, CorrectIndex = 0 },
                new QuizQuestion { Question = "What does a firewall do?", Answers = new List<string>{ "Filters network traffic", "Blocks unauthorized access", "Deletes junk files", "Boosts speed", "Manages user accounts" }, CorrectIndex = 0 },
                new QuizQuestion { Question = "What is social engineering?", Answers = new List<string>{ "Tricking someone for info", "Manipulating people to leak data", "Coding in public", "Building websites", "Hacking servers" }, CorrectIndex = 0 },
                new QuizQuestion { Question = "Why use 2FA?", Answers = new List<string>{ "Add extra protection", "Stops hackers even with password", "Change themes", "Faster logins", "Block spam" }, CorrectIndex = 0 }
            };
        }

        private void ShowCurrentQuestion()
        {
            if (currentQuestionIndex >= quizQuestions.Count)
            {
                QuestionTextBlock.Text = $"Quiz complete! Your score: {score}/{quizQuestions.Count}";
                AnswerButtonsPanel.Visibility = Visibility.Collapsed;
                NextQuestionButton.Visibility = Visibility.Collapsed;
                return;
            }

            var question = quizQuestions[currentQuestionIndex];
            QuestionTextBlock.Text = question.Question;

            AnswerButtonsPanel.Children.Clear();
            for (int i = 0; i < question.Answers.Count; i++)
            {
                Button btn = new Button
                {
                    Content = question.Answers[i],
                    Margin = new Thickness(5),
                    Tag = i
                };
                btn.Click += AnswerButton_Click;
                AnswerButtonsPanel.Children.Add(btn);
            }

            NextQuestionButton.Visibility = Visibility.Collapsed;
            FeedbackTextBlock.Text = "";
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            var question = quizQuestions[currentQuestionIndex];
            var selectedIndex = (int)((Button)sender).Tag;

            if (selectedIndex == question.CorrectIndex)
            {
                FeedbackTextBlock.Text = "Correct! You smart smart. 🔐✅";
                score++;
            }
            else
            {
                FeedbackTextBlock.Text = $"Oops! Correct answer: {question.Answers[question.CorrectIndex]}";
            }

            foreach (Button btn in AnswerButtonsPanel.Children)
                btn.IsEnabled = false;

            LogActivity($"Answered quiz question {currentQuestionIndex + 1}");
            NextQuestionButton.Visibility = Visibility.Visible;
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex++;
            ShowCurrentQuestion();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl && tabControl.SelectedItem is TabItem selectedTab)
            {
                if (selectedTab.Header.ToString().Contains("Quiz"))
                {
                    currentQuestionIndex = 0;
                    score = 0;
                    ShowCurrentQuestion();
                }
            }
        }
    }
}
