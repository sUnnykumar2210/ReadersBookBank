namespace ReadersBookBank.Data
{
    public class BookDetail
    {
        public int Id { get; set; }

        public required string BookName { get; set; }

        public required string Genre { get; set; }

        public bool AvailabilityStatus { get; set; }
    }
}