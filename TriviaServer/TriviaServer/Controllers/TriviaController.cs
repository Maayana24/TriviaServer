using Microsoft.AspNetCore.Mvc;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TriviaServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriviaController : ControllerBase
    {
        // GET: api/<TriviaController>
        [HttpGet]
        public async Task<IEnumerable<Question>> Get()
        {
            List<Question> QuestionList = await DatabaseManager.Instance.GetQuestions();
            return QuestionList;
        }

        // GET api/<TriviaController>/5
        [HttpGet("GetQuestion_{id}")]
        public async Task<Question> Get(int id)
        {
            Question question = await DatabaseManager.Instance.GetQuestion(id);
            return question;
        }

        // POST api/<TriviaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            
        }

        // PUT api/<TriviaController>/5
        [HttpGet("AddPlayer_{value}")]
        public Task<int> Put(string value)
        {
            return DatabaseManager.Instance.AddPlayer(value);
        }

        [HttpGet("GetActive")]
/*        public Task<PlayerInformation> GetActivePlayer()
        {
            List<PlayerInformation> playerInfo = 
        }*/
        // DELETE api/<TriviaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("MakePlayerWait_{value}")]
        public Task MakePlayerWait(int value)
        {
            return DatabaseManager.Instance.MakePlayerWait(value);
        }

        [HttpPost("UpdatePlayerQuestion_{id}_{question}")]
        public Task UpdatePlayerQuestion(int id, int question)
        {
            return DatabaseManager.Instance.UpdatePlayerQuestion(id, question);
        }

        [HttpPost("UpdatePlayerProgress_{id}_{progress}")]
        public Task UpdatePlayerQuestion(int id, bool progress)
        {
            return DatabaseManager.Instance.UpdatePlayerProgress(id, progress);
        }

        [HttpPost("UpdatePlayerScore_{id}_{score}_{time}")]
        public Task UpdatePlayerQuestion(int id, int score, float time)
        {
            return DatabaseManager.Instance.UpdatePlayerScore(id, score, time);
        }

        [HttpGet("GetWaitingPlayer_{value}")]
        public Task<string> GetWaingPlayer(int value)
        {
            return DatabaseManager.Instance.GetWaitingPlayer(value);
        }

        [HttpPost("DeactivateAll")]
        public Task DeactivateAll()
        {
            return DatabaseManager.Instance.DeactivateAll();
        }
    }
}