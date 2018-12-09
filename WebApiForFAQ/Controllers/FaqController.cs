using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiForFAQ.DataContext;
using WebApiForFAQ.Models;
using WebApiForFAQ.ViewModels;

namespace WebApiForFAQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaqController : ControllerBase
    {
        private FaqContext db;
        public FaqController(FaqContext context)
        {
            this.db = context;
        }

        [HttpGet("groups")]
        public ActionResult<IEnumerable<FaqGroupVMWithoutNavigationProperty>> GetAllGroups()
        {
            var result = new List<FaqGroupVMWithoutNavigationProperty>();
            var groups = db.FaqGroups.ToList();
            foreach (FaqGroup fg in groups)
            {
                var group = new FaqGroupVMWithoutNavigationProperty
                {
                    FaqGroupId = fg.FaqGroupId,
                    Title = fg.Title
                };
                result.Add(group);
            }
            return result;
        }

        [HttpGet("groupsWithQuestions")]
        public ActionResult<IEnumerable<FaqGroupVM>> GetAllGroupsWithRelatedQuestions()
        {
            var result = new List<FaqGroupVM>();
            var groups = db.FaqGroups.Include(fg => fg.Questions).ToList();
            foreach (FaqGroup fg in groups)
            {
                var questionList = new List<FaqQuestionVMWithoutNavigationProperty>();
                foreach (FaqQuestion fq in fg.Questions)
                {
                    var question = new FaqQuestionVMWithoutNavigationProperty
                    {
                        Id = fq.Id,
                        Question = fq.Question,
                        Answer = fq.Answer
                    };
                    questionList.Add(question);
                }
                var group = new FaqGroupVM
                {
                    FaqGroupId = fg.FaqGroupId,
                    Title = fg.Title,
                    Questions = questionList
                };
                result.Add(group);
            }
            return result;
        }

        [HttpPost("groups")]
        public ActionResult CreateGroup([FromBody] string title)
        {
            if (title == null)
            {
                return BadRequest();
            }
            var groupEntity = new FaqGroup
            {
                Title = title
            };
            db.FaqGroups.Add(groupEntity);
            db.SaveChanges();

            return Ok();
        }

        [HttpPut("groups/{id}")]
        public ActionResult UpdateGroup(int id, [FromBody] string newTitle)
        {
            if (newTitle == null)
            {
                return BadRequest();
            }
            var groupEntity = db.FaqGroups.FirstOrDefault(g => g.FaqGroupId == id);
            if (groupEntity == null)
            {
                return NotFound();
            }
            groupEntity.Title = newTitle;
            db.SaveChanges();

            var groupVM = new FaqGroupVMWithoutNavigationProperty
            {
                FaqGroupId = groupEntity.FaqGroupId,
                Title = groupEntity.Title
            };
            return Ok(groupVM);
        }

        [HttpDelete("groups/{id}")]
        public ActionResult DeleteGroup(int id)
        {
            var groupEntity = db.FaqGroups.FirstOrDefault(g => g.FaqGroupId == id);
            if (groupEntity == null)
            {
                return NotFound();
            }
            db.FaqGroups.Remove(groupEntity);
            db.SaveChanges();

            var groupVM = new FaqGroupVMWithoutNavigationProperty
            {
                FaqGroupId = groupEntity.FaqGroupId,
                Title = groupEntity.Title
            };
            return Ok(groupVM);
        }

        [HttpGet("questions")]
        public ActionResult<IEnumerable<FaqQuestionVMWithoutNavigationProperty>> GetAllQuestions()
        {
            var result = new List<FaqQuestionVMWithoutNavigationProperty>();
            var questions = db.FaqQuestions.ToList();
            foreach (FaqQuestion fq in questions)
            {
                var question = new FaqQuestionVMWithoutNavigationProperty
                {
                    Id = fq.Id,
                    Question = fq.Question,
                    Answer = fq.Answer
                };
                result.Add(question);
            }
            return result;
        }

        [HttpGet("questionsWithGroups")]
        public ActionResult<IEnumerable<FaqQuestionVM>> GetAllQuestionsWithRelatedGroups()
        {
            var result = new List<FaqQuestionVM>();
            var questions = db.FaqQuestions.Include(fq => fq.FaqGroup).ToList();
            foreach (FaqQuestion fq in questions)
            {
                var question = new FaqQuestionVM
                {
                    Id = fq.Id,
                    Question = fq.Question,
                    Answer = fq.Answer,
                    FaqGroup = new FaqGroupVMWithoutNavigationProperty
                    {
                        FaqGroupId = fq.FaqGroup.FaqGroupId,
                        Title = fq.FaqGroup.Title
                    }
                };
                result.Add(question);
            }
            return result;
        }

        [HttpPost("questions")]
        public ActionResult CreateQuestion([FromBody] FaqQuestionCreateUpdateVM question)
        {
            if (question == null || db.FaqGroups.FirstOrDefault(g => g.FaqGroupId == question.FaqGroupId) == null)
            {
                return BadRequest();
            }
            var questionEntity = new FaqQuestion
            {
                Question = question.Question,
                Answer = question.Answer,
                FaqGroupId = question.FaqGroupId
            };
            db.FaqQuestions.Add(questionEntity);
            db.SaveChanges();

            return Ok();
        }

        [HttpPut("questions/{id}")]
        public ActionResult UpdateQuestion(int id, [FromBody] FaqQuestionCreateUpdateVM question)
        {
            if (question == null || db.FaqGroups.FirstOrDefault(g => g.FaqGroupId == question.FaqGroupId) == null)
            {
                return BadRequest();
            }
            var questionEntity = db.FaqQuestions.FirstOrDefault(q => q.Id == id);
            if (questionEntity == null)
            {
                return NotFound();
            }
            questionEntity.Question = question.Question;
            questionEntity.Answer = question.Answer;
            questionEntity.FaqGroupId = question.FaqGroupId;
            db.SaveChanges();

            var questionVM = new FaqQuestionVMWithoutNavigationProperty
            {
                Id = questionEntity.Id,
                Question = questionEntity.Question,
                Answer = questionEntity.Answer
            };
            return Ok(questionVM);
        }
        
        [HttpDelete("questions/{id}")]
        public ActionResult DeleteQuestion(int id)
        {
            var questionEntity = db.FaqQuestions.FirstOrDefault(q => q.Id == id);
            if (questionEntity == null)
            {
                return NotFound();
            }
            db.FaqQuestions.Remove(questionEntity);
            db.SaveChanges();

            var questionVM = new FaqQuestionVMWithoutNavigationProperty
            {
                Id = questionEntity.Id,
                Question = questionEntity.Question,
                Answer = questionEntity.Answer
            };
            return Ok(questionVM);
        }

        [HttpGet("search/{value}")]
        public ActionResult<ArrayList> Search(string value)
        {
            var result = new ArrayList();
            var groups = db.FaqGroups.Include(fg => fg.Questions).ToList();
            foreach (FaqGroup fg in groups)
            {
                if (fg.Title.Contains(value))
                {
                    result.Add(new FaqGroupVMWithoutNavigationProperty
                    {
                        FaqGroupId = fg.FaqGroupId,
                        Title = fg.Title
                    });
                }
                foreach (FaqQuestion fq in fg.Questions)
                {
                    if (fq.Question.Contains(value) || fq.Answer.Contains(value))
                    {
                        result.Add(new FaqQuestionVMWithoutNavigationProperty
                        {
                            Id = fq.Id,
                            Question = fq.Question,
                            Answer = fq.Answer
                        });
                    }
                }
            }
            return result;
        }
    }
}
