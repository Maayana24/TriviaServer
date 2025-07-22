using System.ComponentModel.DataAnnotations.Schema;

namespace TriviaServer
{
    public class PlayerInformation
    {
        public PlayerInformation(string Name)
        {
            this.Name = Name;
        }

        public string Name { get; set; }
        public int Score { get; set; }
        public float ThinkingTime { get; set; }
        public int CurrentQuestion { get; set; }
        public bool WaitingForProgress { get; set; }
        public bool Active { get; set; }
    }
}
