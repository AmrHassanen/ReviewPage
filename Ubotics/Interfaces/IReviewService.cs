using System.Collections.Generic;
using System.Threading.Tasks;
using Ubotics.Dtos;
using Ubotics.Models;

namespace Ubotics.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsAsync();
        Task<Review> GetReviewByIdAsync(int id);
        Task AddReviewAsync(ReviewDto review);
        Task UpdateReviewAsync(int id, ReviewDto review);
        Task DeleteReviewAsync(int id);
    }
}
