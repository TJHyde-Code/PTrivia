using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTrivia.Views;

namespace PTrivia.Models
{
    public static class PageSequenceManager
    {
        private static readonly List<string> _pages = new()
    {
        nameof(SongQuestionPage),
        nameof(Question1Page),
        nameof(Question2Page),
        nameof(Question3Page),
        nameof(Question5Page),
        nameof(ProposalPage)
    };

        private static int _currentIndex = 0;

        public static string GetNextPage()
        {
            if (_currentIndex + 1 < _pages.Count)
            {
                _currentIndex++;
                return _pages[_currentIndex];
            }

            return null; // No more pages left
        }

        public static void Reset() => _currentIndex = 0;

        public static void SetCurrentPage(string pageName)
        {
            var index = _pages.IndexOf(pageName);
            if (index >= 0)
            {
                _currentIndex = index;
            }
        }
    }
}
