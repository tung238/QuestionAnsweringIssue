using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionAnsweringIssue
{
    public static class Helper
    {
        public static RequestContent GetDeleteSourceRequest(string tenantId)
        {
            var model = new 
            {
                Op = "delete",
                Value = new 
                {
                    Source = tenantId
                }
            };
            return RequestContent.Create(new List<object> { model });
        }

        public static RequestContent ToUpdateQnasRequest(this List<Content> contents, string operation)
        {
            var items = contents.Select(b => b.ToRequestItem(operation));
            return RequestContent.Create(items);
        }

        public static object ToRequestItem(this Content block, string operation)
        {
            return new 
            {
                Op = operation,
                Value = new
                {
                    Source = block.TenantId,
                    Answer = block.Answer,
                    Questions = block.Questions,
                    Metadata = new Dictionary<string, string> {
                    { "ContentId", block.Id } ,
                    { "TenantId", block.TenantId }
                }
                }
            };
        }
    }

    public class Content
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string Answer { get; set; }
        public string[] Questions { get; set; }
    }
}
