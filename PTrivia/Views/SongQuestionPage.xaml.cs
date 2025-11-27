using PTrivia.Models;
using PTrivia.Data;
using PTrivia.Utilities;
using PTrivia.ViewModels;
using System.Text;
using PTrivia.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;

namespace PTrivia;

public partial class SongQuestionPage : ContentPage
{
    private App thisApp;
    private bool needRefresh;
    private Song currentSong;
    private int tryCount = 0;

    public SongQuestionPage()
    {
        InitializeComponent();
        thisApp = Application.Current as App;
        thisApp.masterAnswerRepository ??= new MasterAnswerRepository();
        needRefresh = true;
        songEntry.Focus();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        welcomeLabel.Text = $"Good luck {UserName.User}!";

        if (needRefresh)
        {
            await ShowData();
            needRefresh = false;
        }
    }

    private async Task ShowData()
    {
        btnSongAnswer.IsEnabled = false;
        tryCount = 0;

        try
        {
            currentSong = await thisApp.masterAnswerRepository.GetRandomSongQ();
            questionLabel.Text = currentSong.Question;
            
        }
        catch (Exception)
        {
            await DisplayAlertAsync("Error", "Could not load the song question", "OK");
            questionLabel.Text = "Oops! Could not load a question.";
        }
    }

    private void songEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        btnSongAnswer.IsEnabled = !string.IsNullOrWhiteSpace(songEntry.Text);
    }

    private async void SongAnswerClicked(object sender, EventArgs e)
    {
        string userAnswer = songEntry.Text?.Trim() ?? "";

        bool isCorrect = string.Equals(userAnswer, currentSong.SongAnswer, StringComparison.OrdinalIgnoreCase);
        btnSongAnswer.IsEnabled = true;
        if (string.IsNullOrEmpty(userAnswer))
        {
            await DisplayAlertAsync("Oops!", "Art is subjective and all but all songs have titles!", "OK");
            needRefresh = false;
            return;
        }                
        

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

            //Trial of celebrationpopup
            string resultMessage = $"Correct! A strong start {UserName.User}!";

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
            songEntry.Text = "";
            songEntry.Focus();
        }       
    }

    
}
