using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ubotics.Data;
using Ubotics.Dtos;
using Ubotics.Interfaces;
using Ubotics.Models;
using CloudinaryDotNet.Actions;

namespace Ubotics.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;

        public ReviewService(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task AddReviewAsync(ReviewDto reviewDto)
        {
            if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            var uploadResult = reviewDto.ProductImage != null ? await _photoService.AddPhotoAsync(reviewDto.ProductImage) : null;

            if (uploadResult?.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            var review = new Review
            {
                ProductName = reviewDto.ProductName,
                Rating = reviewDto.Rating,
                ProductImage = uploadResult?.SecureUrl.AbsoluteUri,
                ReviewSnippet = reviewDto.ReviewSnippet,
                ReviewText = reviewDto.ReviewText,
                ReviewDate = reviewDto.ReviewDate,
                ReviewerName = reviewDto.ReviewerName
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(int id, ReviewDto reviewDto)
        {
            if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            var existingReview = await _context.Reviews.FindAsync(id);
            if (existingReview == null)
            {
                throw new KeyNotFoundException("Review not found.");
            }

            if (reviewDto.ProductImage != null)
            {
                if (!string.IsNullOrEmpty(existingReview.ProductImage))
                {
                    var publicId = existingReview.ProductImage.Split('/').Last().Split('.').First();
                    await _photoService.DeletePhotoAsync(publicId);
                }

                var uploadResult = await _photoService.AddPhotoAsync(reviewDto.ProductImage);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                existingReview.ProductImage = uploadResult.SecureUrl.AbsoluteUri;
            }

            existingReview.ProductName = reviewDto.ProductName;
            existingReview.Rating = reviewDto.Rating;
            existingReview.ReviewSnippet = reviewDto.ReviewSnippet;
            existingReview.ReviewText = reviewDto.ReviewText;
            existingReview.ReviewDate = reviewDto.ReviewDate;
            existingReview.ReviewerName = reviewDto.ReviewerName;

            _context.Reviews.Update(existingReview);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                throw new KeyNotFoundException("Review not found.");
            }

            if (!string.IsNullOrEmpty(review.ProductImage))
            {
                var publicId = review.ProductImage.Split('/').Last().Split('.').First();
                await _photoService.DeletePhotoAsync(publicId);
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}
