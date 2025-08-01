using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using PTrivia.Models;

namespace PTrivia.Data
{
    public interface IMasterAnswerRepository
    {
        Task<Song> GetRandomSongQ();
        Task<Question1> GetRandomMasterQ1();
        Task<Question2> GetRandomMasterQ2();
        Task<Question3> GetRandomMasterQ3();
        Task<Question5> GetQuestion5();
        Task<Proposal> GetProposal();
    }

}
