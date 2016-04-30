namespace Domain.Models
{
    public class CharacterViewModel
    {
        public Character Character { get; set; }
        public bool IsVoted { get; set; }
        public bool CanVote { get; set; }
        public bool WeeklyVoted { get; set; }
    }

}
