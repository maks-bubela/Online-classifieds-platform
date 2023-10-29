using System.ComponentModel.DataAnnotations;

namespace OnlineClassifiedsPlatform.Models
{
    public class StripeCardModel
    {
        /// <summary>
        /// User's Id.
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// Stripe card card number.
        /// </summary>
        [Required, MinLength(16), MaxLength(16)]
        public int CardNumber { get; set; }

        /// <summary>
        /// Stripe card expiration year.
        /// </summary>
        [Required, MinLength(4), MaxLength(4)]
        public int ExpirationYear { get; set; }

        /// <summary>
        /// Stripe card expiration month.
        /// </summary>
        [Required, MinLength(2), MaxLength(2)]
        public int ExpirationMonth { get; set; }

        /// <summary>
        /// Stripe card cvc.
        /// </summary>
        [Required, MinLength(3), MaxLength(3)]
        public int Cvc { get; set; }
    }
}
