using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ChatMessages")]
    public class ChatMessage : BaseEntity
    {
        [Column("ToId")]
        [Required]
        public long ToId { get; set; }

        [Column("FromId")]
        [Required]
        public long FromId { get; set; }

        [Column("Message")]
        [Required]
        public string Message { get; set; }

        [Column("Date")]
        [Required]
        public DateTime Date { get; set; }

        public User To { get; set; }
        public User From { get; set; }
    }
}
