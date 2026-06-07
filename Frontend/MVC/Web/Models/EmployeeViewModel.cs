using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Employee Number")]
        public string? EmployeeNumber { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Daily Rate")]
        [DataType(DataType.Currency)]
        public decimal DailyRate { get; set; }

        [Required]
        [Display(Name = "Working Days")]
        public string WorkingDays { get; set; }
    }
}