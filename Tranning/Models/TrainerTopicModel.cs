using System.ComponentModel.DataAnnotations;

namespace Tranning.Models
{
    public class TrainerTopicModel
    {
        public List<TrainerTopicDetail> TrainerTopicDetailLists { get; set; }
    }

    public class TrainerTopicDetail
    {
        [Key]
        public int id { get; set; } 
        [Required(ErrorMessage = "Choose Topic, please")]
        
        public int topicid { get; set; }
        

        public int userid { get; set; }

        public string? trainerName { get; set; }
        public string? topicName { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }
    }
}
