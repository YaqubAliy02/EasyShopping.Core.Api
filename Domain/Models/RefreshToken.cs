using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("refresh_token")]
    public class RefreshToken
    {
        [Column("refresh_token_id")]
        public Guid RefreshTokenId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column("refresh_token_value")]
        public string RefreshTokenValue { get; set; }

        [Column("expired_date")]
        public DateTime ExpiredDate { get; set; }
    }
}
