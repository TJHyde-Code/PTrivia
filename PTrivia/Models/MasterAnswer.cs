using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PTrivia.Models
{
    public class MasterAnswer
    {
        public int ID { get; set; }

        public string Type { get; set; }

        #region 

        public ICollection<Song> Songs { get; set; }
        public ICollection<Question1> Question1s { get; set; } 
        public ICollection<Question2> Question2s { get; set; } 

        public ICollection<Question3> Question3s { get; set; } 

        public ICollection<Question5> Question5s { get; set; } 

        public ICollection<Proposal> Proposals { get; set; } 

        #endregion
    }
}
