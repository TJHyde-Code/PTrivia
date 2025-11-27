using PTrivia.Models;
using PTrivia.Data;
using PTrivia.Utilities;
using PTrivia.ViewModels;
using System.Text;
using PTrivia.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;

namespace PTrivia.Views;

public partial class ProposalPage : ContentPage
{

    private App thisApp;
    private bool needRefresh;
    private Proposal proposal;
    private int tryCount = 0;
    public ProposalPage()
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
        welcomeLabel.Text = $"{UserName.User}!";
        scoreLabel.Text = $"You've earned {ScoreTracker.Points} Points! That has earned you this BONUS ...";

        if (needRefresh)
        {
            await ShowData();
            needRefresh = false;
        }
    }

    private async Task ShowData()
    {
        btnProposal.IsEnabled = false;
        tryCount = 0;

        try
        {
            proposal = await thisApp.masterAnswerRepository.GetProposal();
            questionLabel.Text = proposal.Question;
        }
        catch (Exception)
        {
            await DisplayAlertAsync("Error", "Could not load the question", "OK");
            questionLabel.Text = "Oops! Could not load a question.";
        }
    }

    private async void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        btnProposal.IsEnabled = !string.IsNullOrWhiteSpace(entryAnswer1.Text) ||
            !string.IsNullOrWhiteSpace(entryAnswer2.Text) || !string.IsNullOrWhiteSpace(entryAnswer3.Text) || !string.IsNullOrWhiteSpace(entryAnswer4.Text);

        int totalChars = (entryAnswer1.Text?.Length ?? 0) +
            (entryAnswer2.Text?.Length ?? 0) +
            (entryAnswer3.Text?.Length ?? 0) + (entryAnswer4.Text?.Length ?? 0);

        //Max characters across all entries.
        //used in calculation.
        int maxChars = 12;

        double progress = Math.Min((double)totalChars / maxChars, 1.0);

        //easing for smoother visual fade (exponential easing)
        double easedProgress = Math.Pow(progress, 2.5);

        double targetOpacity = 0.05 + easedProgress * 0.95;

        await proposalImage.FadeToAsync(targetOpacity, 250);

    }

    //Unused call. Clicked event handler calls the Popup directly.
    //private async void ShowProposalPopup()
    //{
    //    var popup = new ProposalPopup(); 
    //    await this.ShowPopupAsync(popup);
    //}
    private async void btnProposal_Clicked(object sender, EventArgs e)
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
            proposal.ProposalP1, proposal.ProposalP2, proposal.ProposalP3, proposal.ProposalP4
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

            try
            {
                await this.ShowPopupAsync(new ProposalPopup());
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Popup failed: {ex.Message}", "OK");
            }

            //await this.ShowPopupAsync(new ProposalPopup());

            return;
        }
        else
        {
            tryCount++;
            await DisplayAlertAsync("Try again!", $"You got {correctCount} parts right! \n You have {3 - tryCount} tries left.", "OK");
            entryAnswer1.Text = "";
            entryAnswer2.Text = "";
            entryAnswer3.Text = "";
            entryAnswer4.Text = "";
            entryAnswer1.Focus();
            btnProposal.IsEnabled = false;
        }

        //Feedback to user on attempts left
        if (tryCount > 3)
        {
            await DisplayAlertAsync("Ouch!", "This should be easy!", "Ok");
            await ShowData();
            return;
        }
        else
        {
            await DisplayAlertAsync("Nice try!", $"Sorry that's not quite right. You have {3 - tryCount} attempt(s) left.", "OK");
            entryAnswer1.Text = "";
            entryAnswer2.Text = "";
            entryAnswer3.Text = "";
            entryAnswer4.Text = "";
            entryAnswer1.Focus();
        }
    }
}