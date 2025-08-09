using System.Text.Json;
using Npgsql;


namespace TriviaServer
{
    public class DatabaseManager
    {
        private static DatabaseManager instance = null;
        private static readonly object padlock = new object();
        private string _connectionString = "Host=aws-0-eu-central-1.pooler.supabase.com;Database=postgres;Username=postgres.bwnhhzedlhrnmqjhqplm;Password=1234;Port=5432;SSL Mode=Require;Trust Server Certificate=true";

        DatabaseManager()
        {

        }

        public static DatabaseManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DatabaseManager();
                    }
                    return instance;
                }
            }
        }


        public async Task<Question> GetQuestion(int id)
        {
            try
            {
                Question question = new Question();
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                // Query Players table
                string query = "SELECT * FROM \"public\".\"Questions\" WHERE id = " + id;
                using var command = new NpgsqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    question.QuestionText = reader.GetString(reader.GetOrdinal("question"));
                    question.Answer1Text = reader.GetString(reader.GetOrdinal("answer1"));
                    question.Answer2Text = reader.GetString(reader.GetOrdinal("answer2"));
                    question.Answer3Text = reader.GetString(reader.GetOrdinal("answer3"));
                    question.Answer4Text = reader.GetString(reader.GetOrdinal("answer4"));
                    question.CorrectAnswer = reader.GetInt16(reader.GetOrdinal("correctAnswer"));
                }
                return question;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Question>> GetQuestions()
        {
            try
            {
                List<Question> questionList = new List<Question>();
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                // Query Players table
                string query = "SELECT * FROM \"public\".\"Questions\"";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Question question = new Question();
                    question.QuestionText = reader.GetString(reader.GetOrdinal("question"));
                    question.Answer1Text = reader.GetString(reader.GetOrdinal("answer1"));
                    question.Answer2Text = reader.GetString(reader.GetOrdinal("answer2"));
                    question.Answer3Text = reader.GetString(reader.GetOrdinal("answer3"));
                    question.Answer4Text = reader.GetString(reader.GetOrdinal("answer4"));
                    question.CorrectAnswer = reader.GetInt16(reader.GetOrdinal("correctAnswer"));
                    questionList.Add(question);
                }
                return questionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> AddPlayer(string name)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = $"INSERT INTO \"public\".\"Players\" (name) VALUES ('{name}') returning id";
            using var command = new NpgsqlCommand(query, connection);
            var result = await command.ExecuteScalarAsync();
            return int.Parse(result.ToString());
        }

        public async Task MakePlayerWait(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = $"UPDATE \"public\".\"Players\" SET \"waitingForProgress\" = true WHERE \"id\" = {id};";
            using var command = new NpgsqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<string> GetWaitingPlayer(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = $"SELECT * FROM \"public\".\"Players\" WHERE \"id\" != {id} AND \"waitingForProgress\" = true";
            using var command = new NpgsqlCommand(query, connection);
            var result = await command.ExecuteReaderAsync();
            while (result.Read())
            {
                PlayerInformation pi = new PlayerInformation(result.GetString(result.GetOrdinal("name")));
                pi.Score = result.GetInt32(result.GetOrdinal("score"));
                pi.ThinkingTime = result.GetFloat(result.GetOrdinal("thinkingTime"));
                pi.CurrentQuestion = result.GetInt32(result.GetOrdinal("currentQuetion"));
                pi.WaitingForProgress = true;
                pi.Active = true;
                return JsonSerializer.Serialize(pi);
            }
            return "{}";
        }

        public async Task<int> GetQuestionCount()
        {
            List<Question> questions = await GetQuestions();
            return questions.Count;
        }

        public async Task UpdatePlayerQuestion(int id, int question)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = $"UPDATE \"public\".\"Players\" SET \"waitingForProgress\" = false, \"currentQuetion\" = {question} WHERE \"id\" = {id};";
            using var command = new NpgsqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdatePlayerProgress(int id, bool prog)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = $"UPDATE \"public\".\"Players\" SET \"waitingForProgress\" = {prog}, \"active\" = {prog} WHERE \"id\" = {id};";
            using var command = new NpgsqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdatePlayerScore(int id, int score, float time)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = $"UPDATE \"public\".\"Players\" SET \"score\" = {score}, \"thinkingTime\" = {time} WHERE \"id\" = {id};";
            using var command = new NpgsqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}