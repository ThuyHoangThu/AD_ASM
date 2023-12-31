﻿using System.ComponentModel.DataAnnotations;
using Tranning.DataDBContext;
using Tranning.Validations;

namespace Tranning.Models
{
    public class TraineeCourseModel
    {
        public List<TraineeCourseDetail> TraineeCourseDetailLists { get; set; }
    }

    public class TraineeCourseDetail
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Choose Course, please")]
        public int courseid { get; set; }
        [Required(ErrorMessage = "Choose Trainee, please")]

        public int userid { get; set; }
        public string? traineeName { get; set; }
        public string? courseName { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }
    }
}
