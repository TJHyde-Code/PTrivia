using CommunityToolkit.Maui.Media;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;

namespace PTrivia.Views;

public partial class TheBeginning : ContentPage
{
    

    public TheBeginning()
	{
		InitializeComponent();        
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();      
        
        PlayGrowOld();
        
    }
   

    private async void PlayGrowOld()
    {
        growOld.Source = MediaSource.FromResource("growold.wav");
        growOld.Volume = 0.50;
        growOld.ShouldLoopPlayback = true;
        await Task.Delay(500);
        growOld.Play();
    }


    private void btnAppClose_Clicked(object sender, EventArgs e)
    {
        growOld.Stop();
        Application.Current.Quit();
          
    
    }
}