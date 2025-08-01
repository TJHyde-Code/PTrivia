using Microsoft.Maui.Controls;
using PTrivia.Utilities;
using PTrivia.Models;
using PTrivia.Data;
using PTrivia.ViewModels;
using System.Text;
using PTrivia.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;



namespace PTrivia.Views;

public partial class ProposalPopup : Popup
{


    //private MediaElement mediaPlayer;
    public ProposalPopup()
    {
        InitializeComponent();

        // Set the personalized message
        proposalMessage.Text = $"You’ve made it through every question, {UserName.User}... \n But I want to be sure!!";
        this.CanBeDismissedByTappingOutsideOfPopup = false;

        this.Closed += OnPopupClosed;

        PlayRomanticMusic();
    }



    private async void PlayRomanticMusic()
    {
        
        mediaPlayer.Source = MediaSource.FromResource("thegirl.wav");        
        mediaPlayer.Volume = 1.0;
        mediaPlayer.ShouldLoopPlayback = true;
        await Task.Delay(500);
        mediaPlayer.Play();  


    }

    //Event Handler for when/if they accidentally click off the popup to pause the music.
    //Double protecting as clicking off the popup is disabled.
    private void OnPopupClosed(object? sender, PopupClosedEventArgs e)
    {
        mediaPlayer?.Pause();
    }

    private async void OnYesClicked(object sender, EventArgs e)
    {
        mediaPlayer?.Pause(); // Stop music
        this.Close();        
        await Shell.Current.GoToAsync(nameof(TheBeginning));         
    }   
}