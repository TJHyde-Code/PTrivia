using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTrivia.Models
{
    public class Proposal
    {
        public int ID { get; set; }

        public string Question { get; set; } = "";


        public string ProposalP1 { get; set; } = "";


        public string ProposalP2 { get; set; } = "";


        public string ProposalP3 { get; set; } = "";


        public string ProposalP4 { get; set; } = "";

        //Navigation
        public int MasterAnswerID { get; set; }
        public MasterAnswer? MasterAnswers { get; set; }
    }
}
