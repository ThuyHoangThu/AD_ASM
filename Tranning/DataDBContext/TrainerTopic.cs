using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Tranning.DataDBContext
{
    public class TrainerTopic
    {
      
        public int id { get; set; }
        [ForeignKey("topicid")]
        public int topicid { get; set; }
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
