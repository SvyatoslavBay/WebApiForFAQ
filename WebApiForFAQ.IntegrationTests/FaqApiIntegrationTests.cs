using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FluentAssertions;
using Xunit;
using WebApiForFAQ.ViewModels;

namespace WebApiForFAQ.IntegrationTests
{
    public class FaqApiIntegrationTests
    {
        private readonly int testGroupIdForUpdate = 20;
        private readonly int testGroupIdForDelete = 26;
        private readonly int testQuestionIdForUpdate = 1007;
        private readonly int testQuestionIdForDelete = 1010;

        [Fact]
        public async Task TestGetAllGroups()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/faq/groups");

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestGetAllGroupsWithQuestions()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/faq/groupsWithQuestions");

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestCreateGroup()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/api/faq/groups",
                    new StringContent(JsonConvert.SerializeObject("TestGroup"), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestUpdateGroup()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PutAsync("/api/faq/groups/" + this.testGroupIdForUpdate.ToString(),
                    new StringContent(JsonConvert.SerializeObject("Test Group"), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var json = await response.Content.ReadAsStringAsync();

                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                json.Should().Be(JsonConvert.SerializeObject(new FaqGroupVMWithoutNavigationProperty
                {
                    FaqGroupId = this.testGroupIdForUpdate,
                    Title = "Test Group"
                }, serializerSettings));
            }
        }

        [Fact]
        public async Task TestDeleteGroup()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/faq/groups/" + this.testGroupIdForDelete.ToString());

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var json = await response.Content.ReadAsStringAsync();

                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                json.Should().Be(JsonConvert.SerializeObject(new FaqGroupVMWithoutNavigationProperty
                {
                    FaqGroupId = this.testGroupIdForDelete,
                    Title = "TestGroup"
                }, serializerSettings));
            }
        }

        [Fact]
        public async Task TestGetAllQuestions()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/faq/questions");

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestGetAllQuestionsWithGroups()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/faq/questionsWithGroups");

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestCreateQuestion()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/api/faq/questions",
                    new StringContent(JsonConvert.SerializeObject(new FaqQuestionCreateUpdateVM
                    {
                        Question = "TestQuestion",
                        Answer = "TestAnswer",
                        FaqGroupId = 1
                    }),
                    Encoding.UTF8,
                    "application/json"));

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestUpdateQuestion()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PutAsync("/api/faq/questions/" + this.testQuestionIdForUpdate.ToString(),
                    new StringContent(JsonConvert.SerializeObject(new FaqQuestionCreateUpdateVM
                    {
                        Question = "Test Question",
                        Answer = "Test Answer",
                        FaqGroupId = 1
                    }),
                    Encoding.UTF8,
                    "application/json"));

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var json = await response.Content.ReadAsStringAsync();

                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                json.Should().Be(JsonConvert.SerializeObject(new FaqQuestionVMWithoutNavigationProperty
                {
                    Id = this.testQuestionIdForUpdate,
                    Question = "Test Question",
                    Answer = "Test Answer"
                }, serializerSettings));
            }
        }

        [Fact]
        public async Task TestDeleteQuestion()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/faq/questions/" + this.testQuestionIdForDelete.ToString());

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var json = await response.Content.ReadAsStringAsync();

                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                json.Should().Be(JsonConvert.SerializeObject(new FaqQuestionVMWithoutNavigationProperty
                {
                    Id = this.testQuestionIdForDelete,
                    Question = "TestQuestion",
                    Answer = "TestAnswer"
                }, serializerSettings));
            }
        }

        [Fact]
        public async Task TestSearch()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/faq/search/some string");

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
