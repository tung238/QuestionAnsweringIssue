using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.AI.Language.QuestionAnswering.Projects;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace QuestionAnsweringIssue
{
    public class MyBenchmarks
    {
        private QuestionAnsweringClient client;
        private QuestionAnsweringProjectsClient projectsClient;
        private string NewString => Guid.NewGuid().ToString();
        private string ProjectName => "TrabbleKnowledgebaseTest";
        public MyBenchmarks()
        {
            var endpoint = new Uri("");
            var credential = new AzureKeyCredential("");
            client = new QuestionAnsweringClient(endpoint, credential);
            projectsClient = new QuestionAnsweringProjectsClient(endpoint, credential);
        }

        [Benchmark]
        public async Task UpdateSourcesAsync()
        {
            var request = Helper.GetDeleteSourceRequest(NewString);
            await projectsClient.UpdateSourcesAsync(WaitUntil.Completed, ProjectName, request);
        }

        [Benchmark]
        public async Task UpdateQnasAsync()
        {
            var tenantId = NewString;
            var blocks = new List<Content>
            {
                new Content {Answer = NewString, Id = NewString, Questions = new string[]{NewString}, TenantId = tenantId},
                new Content {Answer = NewString, Id = NewString, Questions = new string[]{NewString}, TenantId = tenantId},
                new Content {Answer = NewString, Id = NewString, Questions = new string[]{NewString}, TenantId = tenantId},
                new Content {Answer = NewString, Id = NewString, Questions = new string[]{NewString}, TenantId = tenantId}
            };
            var request = blocks.ToUpdateQnasRequest("add");
            await projectsClient.UpdateQnasAsync(WaitUntil.Completed, ProjectName, request);
        }

        [Benchmark]
        public async Task DeployProjectAsync()
        {
            await projectsClient.DeployProjectAsync(WaitUntil.Completed, ProjectName, "production");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<MyBenchmarks>();
        }
    }
}
