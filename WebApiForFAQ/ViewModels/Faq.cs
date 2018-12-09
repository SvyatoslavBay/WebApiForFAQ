using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiForFAQ.ViewModels
{
    public class FaqQuestionVM
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public FaqGroupVMWithoutNavigationProperty FaqGroup { get; set; }
    }

    public class FaqQuestionVMWithoutNavigationProperty
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class FaqQuestionCreateUpdateVM
    {
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        public int FaqGroupId { get; set; }
    }

    public class FaqGroupVM
    {
        public int FaqGroupId { get; set; }
        public string Title { get; set; }
        public List<FaqQuestionVMWithoutNavigationProperty> Questions { get; set; }
    }

    public class FaqGroupVMWithoutNavigationProperty
    {
        public int FaqGroupId { get; set; }
        public string Title { get; set; }
    }
}
