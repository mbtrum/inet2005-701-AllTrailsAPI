using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AllTrailsAPI.DTOs
{
    public class Review
    {
        public int Id { get; set; } // pk
        public int TrailId { get; set; } // fk
        public string Description { get; set; } = string.Empty;
        public int Rating { get; set; }
        public bool IsPublished { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public User User { get; set; } = null!;
    }
}
