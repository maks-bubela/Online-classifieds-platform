using System.ComponentModel.DataAnnotations;

namespace OnlineClassifiedsPlatform.Models
{
    public class UserRegistrationModel
    {
        /// <summary>
        /// User username
        /// </summary>
        [Required, MinLength(4), MaxLength(16)]
        public string Username { get; set; }
        /// <summary>
        /// User password
        /// </summary>

        [Required, MinLength(6), MaxLength(16)]
        public string Password { get; set; }
        /// <summary>
        /// User first name
        /// </summary>

        [Required, MinLength(3), MaxLength(16)]
        public string Firstname { get; set; }
        /// <summary>
        /// User last name
        /// </summary>
        [Required, MinLength(3), MaxLength(16)]
        public string Lastname { get; set; }
    }
}
