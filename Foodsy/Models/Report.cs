namespace Foodsy.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Report
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public DateTime GeneratedDate { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

    }


}
