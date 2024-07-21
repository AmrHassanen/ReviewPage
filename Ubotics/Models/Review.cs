using System.ComponentModel.DataAnnotations;

namespace Ubotics.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public string ProductImage { get; set; }
        public string ReviewSnippet { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewerName { get; set; }

        public string GetStarRating()
        {
            return new string('★', Rating) + new string('☆', 5 - Rating);
        }
    }
}