using System;

namespace ApiPoc.Models
{
    public class Error
    {
        public Guid? Id { get; set; }
        public string? Detail { get; set; }
        public string? Code { get; set; }
        public SourceItem[]? Source { get; set; }
    }
}