using PTrivia.Models;
using PTrivia.Utilities;
using System.Net.Http.Headers;

namespace PTrivia.Data
{
    public class MasterAnswerRepository :IMasterAnswerRepository
    {
        private readonly HttpClient client = new HttpClient();

        public MasterAnswerRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            //This may be a fix for the published api not supplying questions and causing errors
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            //This works for local host use.
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //HttpGets
        public async Task<Song> GetRandomSongQ()
        {
            HttpResponseMessage response = await client.GetAsync("api/MasterAnswer/randomSongQ");

            if (response.IsSuccessStatusCode)
            {
                Song randomSongQ = await response.Content.ReadAsAsync<Song>();
                return randomSongQ;
            }
            else
            {
                throw new Exception("Could not access the Song questions");
            }
        }

        public async Task<Question1> GetRandomMasterQ1()
        {
            HttpResponseMessage response = await client.GetAsync("api/MasterAnswer/randomQ1");

            if (response.IsSuccessStatusCode)
            {
                Question1 randomQ1 = await response.Content.ReadAsAsync<Question1>();
                return randomQ1;
            }
            else
            {
                throw new Exception("Could not access Question 1 questions");
            }
        }

        public async Task<Question2> GetRandomMasterQ2()
        {
            HttpResponseMessage response = await client.GetAsync("api/MasterAnswer/randomQ2");

            if (response.IsSuccessStatusCode)
            {
                Question2 randomQ2 = await response.Content.ReadAsAsync<Question2>();
                return randomQ2;
            }
            else
            {
                throw new Exception("Could not access Question 2 questions");
            }
        }

        public async Task<Question3> GetRandomMasterQ3()
        {
            HttpResponseMessage response = await client.GetAsync("api/MasterAnswer/randomQ3");

            if (response.IsSuccessStatusCode)
            {
                Question3 randomQ3 = await response.Content.ReadAsAsync<Question3>();
                return randomQ3;
            }
            else
            {
                throw new Exception("Could not access Question 3 questions");
            }
        }

        public async Task<Question5> GetQuestion5()
        {
            HttpResponseMessage response = await client.GetAsync("api/MasterAnswer/question5");

            if (response.IsSuccessStatusCode)
            {
                Question5 question5 = await response.Content.ReadAsAsync<Question5>();
                return question5;
            }
            else
            {
                throw new Exception("Could not access Question 5");
            }
        }

        public async Task<Proposal> GetProposal()
        {
            HttpResponseMessage response = await client.GetAsync("api/MasterAnswer/proposal");

            if (response.IsSuccessStatusCode)
            {
                Proposal proposal = await response.Content.ReadAsAsync<Proposal>();
                return proposal;
            }
            else
            {
                throw new Exception("Could not access the proposal");
            }
        }
          



    }//Class
}//Namespace
