using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTrivia.Models
{
    public class Question1
    {
        public int ID { get; set; }

        public string Question { get; set; } = "";


        public string Q1P1Answer { get; set; } = "";


        public string Q1P2Answer { get; set; } = "";


        public string Q1P3Answer { get; set; } = "";

        //Navigation
        public int MasterAnswerID { get; set; }
        public MasterAnswer? MasterAnswers { get; set; }
    }
}
