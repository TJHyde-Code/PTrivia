using PTrivia.Models;
using PTrivia.Data;
using PTrivia.Utilities;
using PTrivia.ViewModels;
using System.Text;
using PTrivia.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;

namespace PTrivia.Views;

public partial class Question3Page : ContentPage
{

    private App thisApp;
    private bool needRefresh;
    private Question3 currentQ3;
    private int tryCount = 0;
    public Question3Page()
	{
		InitializeComponent();
        thisApp = Application.Current as App;
        thisApp.masterAnswerRepository ??= new MasterAnswerRepository();
        needRefresh = true;
        entryAnswer.Focus();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        welcomeLabel.Text = $"You're brilliant {UserName.User}!";
        scoreLabel.Text = $"Points: {ScoreTracker.Points}";

        if (needRefresh)
        {
            await ShowData();
            needRefresh=false;
        }
    }

    private async Task ShowData()
    {
        btnQuestion3.IsEnabled = false;
        tryCount = 0;

        try
        {
            currentQ3 = await thisApp.masterAnswerRepository.GetRandomMasterQ3();
            questionLabel.Text = currentQ3.Question;
        }
        catch (Exception)
        {

            await DisplayAlertAsync("Error", "Could not load the question", "OK");
            questionLabel.Text = "Oops! could not load a question. Check connection or Wi-Fi";
        }
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        btnQuestion3.IsEnabled = !string.IsNullOrWhiteSpace(entryAnswer.Text);
    }




    private async void btnQuestion3_Clicked(object sender, EventArgs e)
    {
        string answer1 = entryAnswer.Text?.Trim().ToLower() ?? "";

        var userAnswer = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            answer1
        };

        var correctAnswer = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            currentQ3.Q3P1Answer
        };

        bool isCorrect = userAnswer.SetEquals(correctAnswer);

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
            ScoreTracker.CorrectAnswer++;
            ScoreTracker.Points += pointsEarned;
            
            // celebrationpopup
            string resultMessage = $"Hitting the home stretch {UserName.User}!";

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
            await DisplayAlertAsync("Try again!", $"You have {3 - tryCount} tries left!!", "OK");
            entryAnswer.Text = "";
            entryAnswer.Focus();
            btnQuestion3.IsEnabled = false;
        }

        //Feedback to user on attempts left
        if(tryCount > 3)
        {
            await DisplayAlertAsync("Oopsie!", "Too many tries", "Ok");
            await ShowData();
            return;
        }
        else
        {
            await DisplayAlertAsync("Nice try!", $"Sorry that's not quite right. You have {3 - tryCount} attempt(s) left.", "OK");
            entryAnswer.Text = "";
            entryAnswer.Focus();
        }

    }
}