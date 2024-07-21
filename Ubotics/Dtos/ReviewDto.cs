namespace Ubotics.Dtos
{
    public class ReviewDto
    {
        public string ProductName { get; set; }
        public int Rating { get; set; }
        public IFormFile ProductImage { get; set; }
        public string ReviewSnippet { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewerName { get; set; }
        public string StarRating => new string('★', Rating) + new string('☆', 5 - Rating);

    }
}
