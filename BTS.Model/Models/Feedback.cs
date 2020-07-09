using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Feedbacks")]
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [MaxLength(250)]
        [Required]
        public string Name { set; get; }

        [MaxLength(250)]
        public string Email { set; get; }

        [MaxLength(500)]
        public string Message { set; get; }

        public DateTime CreatedDate { set; get; }

        [Required]
        public bool Status { set; get; }
    }
}