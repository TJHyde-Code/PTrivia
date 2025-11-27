using Microsoft.Maui.Controls;
using PTrivia.Utilities;
using PTrivia.Models;
using PTrivia.Data;
using PTrivia.ViewModels;
using System.Text;
using PTrivia.Views;
using CommunityToolkit.Maui.Views;

namespace PTrivia.Views;

public partial class CelebrationPopup : Popup
{
    
    public CelebrationPopup(string resultMessage, int pointsEarned)
    {
        InitializeComponent();        

        resultMessageLabel.Text = resultMessage;
        pointsEarnedLabel.Text = $"You got: {pointsEarned} point(s)!!";

        PlayCelebration();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        //await this.CloseAsync();
        await CloseAsync();
        await Task.Delay(300);
    }

    private async void PlayCelebration()
    {
        
        celebrationSound.Source = MediaSource.FromResource("woohoo.wav");

        celebrationSound.Volume = 1.0;
        celebrationSound.ShouldLoopPlayback = false;       
        celebrationSound.Play();     
        await Task.Delay(500);
    }
}