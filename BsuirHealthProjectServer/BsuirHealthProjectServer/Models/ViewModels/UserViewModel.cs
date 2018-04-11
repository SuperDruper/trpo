using System;
using System.ComponentModel.DataAnnotations;

namespace BsuirHealthProjectServer.Models.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool Sex { get; set; }
    }

    public class CreateAdminViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }

        [Required]
        [StringLength(128)]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirPassword { get; set; }
    }

    public class AdminViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(128)]
        public string Username { get; set; }
    }
}
