using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string EmployeeNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public decimal DailyRate { get; set; }

        [Required]
        public string WorkingDays { get; set; } // "MWF" or "TTHS"

        // Not mapped property for age or other calculations if needed
        [NotMapped]
        public int Age => (int)((DateTime.Today - DateOfBirth).TotalDays / 365.25);
    }
}