using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNet_MVC_Training.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AspNet_MVC_Training.Models
{
    public class Training
    {
        public int TrainingID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        public string Video { get; set; }

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
        public virtual ApplicationUser Former { get; set; }

      public override string ToString()
      {
        return $"{{TrainingID = {this.TrainingID}, Title = {this.Title}, Image = {this.Image}, ReleaseDate = {this.ReleaseDate}, Category = {this.Category}, Price = {this.Price}, Former = {this.Former}}}";
      }

    }

    public enum Status {
      Cart,
      Registered,
      Finished
    }

    public class UserTraining {
      public int TrainingID { get; set; }

      [ForeignKey("TrainingID")]
      [Required]
      public virtual Training Training { get; set; }
      
      public string UserId {get; set;}
      
      [ForeignKey("UserId")]
      [Required]
      public virtual ApplicationUser User { get; set; }
      
      [DefaultValue(Status.Cart)]
      public Status Status { get; set; }
    }
}