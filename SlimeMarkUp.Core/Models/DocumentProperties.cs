namespace SlimeMarkUp.Core.Models
{
    /// <summary>
    /// Represents a set of standard metadata properties associated with a document, such as title, author, and
    /// publication details.
    /// </summary>
    /// <remarks>Use this class to store and access descriptive information about a document, including its
    /// filename, subject, keywords, and contributors. These properties can be used for indexing, searching, or
    /// displaying document information in user interfaces. All properties are optional and may be null if not
    /// specified.</remarks>
    public class DocumentProperties
    {    

        public string? Filename { get; set; }
        
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public string? Keywords { get; set; }
        public string? Comments { get; set; }
        public string? Company { get; set; }
        public string? Category { get; set; }
        public string? RevisionNumber { get; set; }
        public string? Language { get; set; }
        public List<string>? Contributors { get; set; }
        public string? VersionHistory { get; set; }
        public DateTime ? Published { get; set; }
    }
}
