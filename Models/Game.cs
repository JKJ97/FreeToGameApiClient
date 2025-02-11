namespace FreeToGameApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Thumbnail { get; set; }
        public string? ShortDescription { get; set; }
        public string? Genre { get; set; }
        public string? Platform { get; set; }
        public string? Publisher { get; set; }
        public string? Developer { get; set; }
        public string? ReleaseDate { get; set; }
    }
}
