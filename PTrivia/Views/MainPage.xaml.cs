using PTrivia.ViewModels;
using PTrivia.Views;
using PTrivia.Models;

namespace PTrivia
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
            ScoreTracker.Reset();
            
        }

        private async void StartTriviaClicked(object sender, EventArgs e)
        {
            string userName = await DisplayPromptAsync(
        "Let's Get Started!",
        "Enter your first name:",
        "Start",
        "Cancel",
        placeholder: "First name",
        maxLength: 20,
        keyboard: Keyboard.Text);

            if (string.IsNullOrWhiteSpace(userName))
                return;

            // Store name in session memory
            UserName.User = userName.Trim();

            // Navigate to the next page
            await Shell.Current.GoToAsync(nameof(SongQuestionPage));
        }
    }

}
