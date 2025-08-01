using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTrivia.Models
{
    public class Song
    {
        public int ID { get; set; }
       
        public string Question { get; set; } = "";
        
        public string SongAnswer { get; set; } = "";
        //Navigation
        public int MasterAnswerID { get; set; }
        public MasterAnswer? MasterAnswers { get; set; }
    }
}
