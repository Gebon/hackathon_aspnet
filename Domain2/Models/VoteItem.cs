namespace Domain
{
    public class VoteItem
    {
        public int Id { get; set; }

        public virtual Vote Vote { get; set; }

        public int Position { get; set; }

        public virtual Character Hero { get; set; }
    }
}
