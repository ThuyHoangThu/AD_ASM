using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Tranning.DataDBContext
{
    public class TraineeCourse
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("courseid")]
        public int courseid { get; set; }
        [ForeignKey("userid")]
        public int userid { get; set; }

        [AllowNull]
        public DateTime? created_at { get; set; }
        [AllowNull]
        public DateTime? updated_at { get; set; }
        [AllowNull]
        public DateTime? deleted_at { get; set; }
    }
}