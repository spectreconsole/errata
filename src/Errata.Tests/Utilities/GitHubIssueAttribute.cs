using System;

namespace Errata.Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GitHubIssueAttribute : Attribute
    {
        public int Issue { get; set; }

        public GitHubIssueAttribute(int issue)
        {
            Issue = issue;
        }
    }
}
