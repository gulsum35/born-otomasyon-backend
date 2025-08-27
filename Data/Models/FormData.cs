using System;
using System.ComponentModel.DataAnnotations;

namespace BornOtomasyonApi.Models
{
    public class FormData
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Text1 { get; set; } = string.Empty;

        [Range(50, 100)]
        [Required]
        public int Num1 { get; set; }

        [Required]
        public DateTime Date1 { get; set; }
    }
}
