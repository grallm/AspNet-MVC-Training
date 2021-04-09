using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AspNet_MVC_Training.Models
{
    public class Training
    {
        public int TrainingID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Image { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public string Category { get; set; }
        [Required]
        public decimal Price { get; set; }

        public string UserId {get; set;}

        [ForeignKey("UserId")]
        public virtual IdentityUser Former { get; set; }

    public override string ToString()
    {
      return $"{{TrainingID = {this.TrainingID}, Title = {this.Title}, Image = {this.Image}, ReleaseDate = {this.ReleaseDate}, Category = {this.Category}, Price = {this.Price}, Former = {this.Former}}}";
    }

    }
}