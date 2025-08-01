using PTrivia.Models;
using PTrivia.Data;
using PTrivia.Utilities;
using PTrivia.ViewModels;
using System.Text;
using PTrivia.Views;
using CommunityToolkit.Maui.Views;

namespace PTrivia.Views;

public partial class Question5Page : ContentPage
{

    private App thisApp;
    private bool needRefresh;
    private Question5 currentQ5;
    private int tryCount = 0;
    public Question5Page()
    {
        InitializeComponent();
        thisApp = Application.Current as App;
        thisApp.masterAnswerRepository ??= new MasterAnswerRepository();
        needRefresh = true;
        entryAnswer1.Focus();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        welcomeLabel.Text = $"Where are we going with this one????, {UserName.User}!";
        scoreLabel.Text = $"Points: {ScoreTracker.Points}";

        if (needRefresh)
        {
            await ShowData();
            needRefresh = false;
        }
    }

    private async Task ShowData()
    {
        btnLeadUp.IsEnabled = false;
        tryCount = 0;

        try
        {
            currentQ5 = await thisApp.masterAnswerRepository.GetQuestion5();
            questionLabel.Text = currentQ5.Question;
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Could not load the question", "OK");
            questionLabel.Text = "Oops! Could not load a question.";
        }
    }

    private async void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        btnLeadUp.IsEnabled = !string.IsNullOrWhiteSpace(entryAnswer1.Text) ||
            !string.IsNullOrWhiteSpace(entryAnswer2.Text) || !string.IsNullOrWhiteSpace(entryAnswer3.Text) || !string.IsNullOrWhiteSpace(entryAnswer4.Text);

        int totalChars = (entryAnswer1.Text?.Length ?? 0) +
            (entryAnswer2.Text?.Length ?? 0) +
            (entryAnswer3.Text?.Length ?? 0) + (entryAnswer4.Text?.Length ?? 0);
        
        //Max characters across all entries.
        //used in calculation. each character should only increase opacity by 0.8%
        int maxChars = 14;

        double progress = Math.Min((double)totalChars / maxChars, 1.0);

        //easing for smoother visual fade (exponential easing)
        double easedProgress = Math.Pow(progress, 2.5);

        double targetOpacity = 0.05 + easedProgress * 0.95;

        await wotImage.FadeTo(targetOpacity, 250);


    }
    private async void btnLeadUp_Clicked(object sender, EventArgs e)
    {
        string answer1 = entryAnswer1.Text?.Trim().ToLower() ?? "";
        string answer2 = entryAnswer2.Text?.Trim().ToLower() ?? "";
        string answer3 = entryAnswer3.Text?.Trim().ToLower() ?? "";
        string answer4 = entryAnswer4.Text?.Trim().ToLower() ?? "";

        var userAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            answer1, answer2, answer3, answer4
        };

        var correctAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            currentQ5.Q5P1Answer, currentQ5.Q5P2Answer, currentQ5.Q5P3Answer, currentQ5.Q5P4Answer
        };

        int correctCount = userAnswers.Intersect(correctAnswers).Count();
        bool isCorrect = userAnswers.SetEquals(correctAnswers);

        if (isCorrect)
        {
            ScoreTracker.CorrectAnswer++;
            // Award points based on try count
            tryCount++;
            int pointsEarned = 0;
            if (tryCount == 1)
                pointsEarned = 3;
            else if (tryCount == 2)
                pointsEarned = 2;
            else if (tryCount == 3)
                pointsEarned = 1;

            ScoreTracker.Points += pointsEarned;

            // celebrationpopup
            string resultMessage = $"One final question and it's a doozy! Are you ready {UserName.User}!";

            var popup = new CelebrationPopup(resultMessage, pointsEarned);
            await this.ShowPopupAsync(popup);

            var nextPage = PageSequenceManager.GetNextPage();
            if (nextPage != null)
            {
                await Shell.Current.GoToAsync(nextPage);
            }
            return;
        }       
        else
        {
            tryCount++;
            await DisplayAlert("Try again!", $"You got {correctCount} parts right! \n You have {3 - tryCount} tries left.", "OK");
            entryAnswer1.Text = "";
            entryAnswer2.Text = "";
            entryAnswer3.Text = "";
            entryAnswer4.Text = "";
            entryAnswer1.Focus();
            btnLeadUp.IsEnabled = false;
        }

        //Feedback to user on attempts left
        if (tryCount > 3)
        {
            await DisplayAlert("Oopsie!", "Too many tries", "Ok");
            await ShowData();
            return;
        }
        else
        {
            await DisplayAlert("Nice try!", $"Sorry that's not quite right though. You have {3 - tryCount} attempt(s) left.", "OK");
            entryAnswer1.Text = "";
            entryAnswer2.Text = "";
            entryAnswer3.Text = "";
            entryAnswer4.Text = "";
            entryAnswer1.Focus();
        }

    }
}