using PTrivia.Data;
using PTrivia.Models;

namespace PTrivia
{
    public partial class App : Application
    {
        internal IMasterAnswerRepository masterAnswerRepository;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
