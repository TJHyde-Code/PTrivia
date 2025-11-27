using PTrivia.Models;
using PTrivia.Data;
using PTrivia.Utilities;
using PTrivia.ViewModels;
using System.Text;
using PTrivia.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;

namespace PTrivia.Views;

public partial class Question2Page : ContentPage
{
	private App thisApp;
	private bool needRefresh;
	private Question2? currentQ2;
	private int tryCount = 0;


	public Question2Page()
	{
		InitializeComponent();
		thisApp = Application.Current as App;
		thisApp.masterAnswerRepository ??= new MasterAnswerRepository();
		needRefresh = true;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		welcomeLabel.Text = $"You're cookin {UserName.User}!";
		scoreLabel.Text = $"Points: {ScoreTracker.Points}";

		if (needRefresh)
		{
			await ShowData();
			needRefresh = false;
		}
	}

	private async Task ShowData()
	{
		btnQuestion2.IsEnabled = false;
		tryCount = 0;

		try
		{
			currentQ2 = await thisApp.masterAnswerRepository.GetRandomMasterQ2();
			questionLabel.Text = currentQ2.Question;
		}
		catch (Exception)
		{
            await DisplayAlertAsync("Error", "Could not load the question", "OK");
            questionLabel.Text = "Oops! Could not load a question.";
        }
	}

	private void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		btnQuestion2.IsEnabled = !string.IsNullOrWhiteSpace(entryAnswer1.Text) ||
			!string.IsNullOrWhiteSpace(entryAnswer2.Text);
    }


    private async void btnQuestion2_Clicked(object sender, EventArgs e)
    {
		string answer1 = entryAnswer1.Text?.Trim().ToLower() ?? "";
        string answer2 = entryAnswer2.Text?.Trim().ToLower() ?? "";

		var userAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			answer1, answer2
		};

		var correctAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			currentQ2.Q2P1Answer, currentQ2.Q2P2Answer
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

            //celebrationpopup
            string resultMessage = $"OMG you're unstoppable {UserName.User}!";

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
            entryAnswer1.Focus();
            btnQuestion2.IsEnabled = false;
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
            entryAnswer1.Focus();
        }

    }
}