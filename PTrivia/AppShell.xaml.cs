using PTrivia.Views;

namespace PTrivia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Page registry
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage)); 
            Routing.RegisterRoute(nameof(SongQuestionPage), typeof(SongQuestionPage));
            Routing.RegisterRoute(nameof(Question1Page), typeof(Question1Page));
            Routing.RegisterRoute(nameof(Question2Page), typeof(Question2Page));
            Routing.RegisterRoute(nameof(Question3Page), typeof(Question3Page));
            Routing.RegisterRoute(nameof(Question5Page), typeof(Question5Page));
            Routing.RegisterRoute(nameof(ProposalPage), typeof(ProposalPage));
            Routing.RegisterRoute(nameof(TheBeginning), typeof(TheBeginning));
        }
    }
}
