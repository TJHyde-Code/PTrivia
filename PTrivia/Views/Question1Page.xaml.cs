using PTrivia.Models;
using PTrivia.Data;
using PTrivia.Utilities;
using PTrivia.ViewModels;
using System.Text;
using PTrivia.Views;
using Microsoft.Extensions.Logging.Abstractions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;

namespace PTrivia.Views;

public partial class Question1Page : ContentPage
{

    private App thisApp;
    private bool needRefresh;
    private Question1 currentQ1;
    private int tryCount = 0;

    public Question1Page()
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
        welcomeLabel.Text = $"Good luck, {UserName.User}!";
        scoreLabel.Text = $"Points: {ScoreTracker.Points}";

        if (needRefresh)
        {
            await ShowData();
            needRefresh = false;
        }
    }

    private async Task ShowData()
    {
        btnQuestion1.IsEnabled = false;
        tryCount = 0;

        try
        {
            currentQ1 = await thisApp.masterAnswerRepository.GetRandomMasterQ1();
            questionLabel.Text = currentQ1.Question;
        }
        catch (Exception)
        {

            await DisplayAlertAsync("Error", "Could not load the question", "OK");
            questionLabel.Text = "Oops! Could not load a question.";
        }
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        btnQuestion1.IsEnabled = !string.IsNullOrWhiteSpace(entryAnswer1.Text) ||
            !string.IsNullOrWhiteSpace(entryAnswer2.Text) ||
            !string.IsNullOrWhiteSpace(entryAnswer3.Text);
    }

    private async void Question1Clicked(object sender, EventArgs e)
    {
        string answer1 = entryAnswer1.Text?.Trim().ToLower() ?? "";
        string answer2 = entryAnswer2.Text?.Trim().ToLower() ?? "";
        string answer3 = entryAnswer3.Text?.Trim().ToLower() ?? "";

       

        var userAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
       {
           answer1, answer2, answer3
       };

        var correctAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            currentQ1.Q1P1Answer, currentQ1.Q1P2Answer, currentQ1.Q1P3Answer
        };

        int correctCount = userAnswers.Intersect(correctAnswers).Count();
        bool isCorrect = userAnswers.SetEquals(correctAnswers);

        if (isCorrect)
        {
            tryCount++;
            int pointsEarned = 0;
            if (tryCount == 1)
                pointsEarned = 3;
            else if (tryCount == 2)
                pointsEarned = 2;
            else if (tryCount == 3)
                pointsEarned = 1;

            ScoreTracker.CorrectAnswer++;
            ScoreTracker.Points += pointsEarned;


           // celebrationpopup
            string resultMessage = $"You're killin it {UserName.User}!";

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
            await DisplayAlertAsync("Try again!", $"You got {correctCount} parts right! \n You have {3 - tryCount} tries left.", "OK");
            entryAnswer1.Text = "";
            entryAnswer2.Text = "";
            entryAnswer3.Text = "";
            entryAnswer1.Focus();
            btnQuestion1.IsEnabled = false;
        }

        //Feedback to user on attempts left
        if (tryCount > 3)
        {
            await DisplayAlertAsync("Oopsie!", "Too many tries", "Ok");
            await ShowData();
            return;
        }
        else
        {
            await DisplayAlertAsync("Nice try!", $"Sorry that's not quite right though. You have {3 - tryCount} attempt(s) left.", "OK");
            entryAnswer1.Text = "";
            entryAnswer2.Text = "";
            entryAnswer3.Text = "";
            entryAnswer1.Focus();
        }

    }
}