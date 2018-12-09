using System.Collections.Generic;

namespace WebApiForFAQ.Models
{
    public class FaqQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int FaqGroupId { get; set; }
        public FaqGroup FaqGroup { get; set; }
    }

    public class FaqGroup
    {
        public int FaqGroupId { get; set; }
        public string Title { get; set; }
        public List<FaqQuestion> Questions { get; set; }
    }
}
