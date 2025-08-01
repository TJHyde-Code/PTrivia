using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTrivia.ViewModels
{
    internal class ScoreTracker
    {
        public static int Points { get; set; } = 0;
        public static int CorrectAnswer { get; set; } = 0;

        public static void Reset()
        {
            Points= 0; CorrectAnswer= 0;
            UserName.User = "";
        }
    }
}
