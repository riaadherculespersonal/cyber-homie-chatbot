using System.Windows;

namespace CyberHomieGUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // When "Send" is clicked
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInputTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(userInput))
            {
                // Show user input in chat
                ChatHistoryTextBlock.Text += $"\nYou: {userInput}";

                // Simple bot reply for now
                string botResponse = GetBotReply(userInput);
                ChatHistoryTextBlock.Text += $"\nCyberHomie: {botResponse}\n";

                UserInputTextBox.Text = ""; // clear box
            }
        }

        // Simple test response logic
        private string GetBotReply(string input)
        {
            if (input.ToLower().Contains("hello") || input.ToLower().Contains("hi"))
                return "Hey there! Ready to learn about cybersecurity?";

            return "Thanks for your message! (More features coming soon...)";
        }
    }
}
