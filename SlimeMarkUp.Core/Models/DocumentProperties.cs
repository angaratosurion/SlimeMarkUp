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
        /// <summary>
        /// Gets or sets the filename of the document.
        /// </summary>
        public string? Filename { get; set; }
        /// <summary>
        /// Gets or sets the title of the document.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Gets or sets the author of the document.
        /// </summary>
        public string? Author { get; set; }
        /// <summary>
        /// Gets or sets a brief description of the document.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Gets or sets the subject of the document.
        /// </summary>
        public string? Subject { get; set; }
        /// <summary>
        /// Gets or sets a list of keywords associated with the document.
        /// </summary>
        public string? Keywords { get; set; }
        /// <summary>
        /// Gets or sets comments or notes about the document.
        /// </summary>
        public string? Comments { get; set; }
        /// <summary>
        /// Gets or sets the company or organization associated with the document.
        /// </summary>
        public string? Company { get; set; }
        /// <summary>
        /// Gets or sets the category or type of the document.
        /// </summary>
        public string? Category { get; set; }
        /// <summary>
        /// Gets or sets the revision number of the document.
        /// </summary>
        public string? RevisionNumber { get; set; }
        /// <summary>
        /// Gets or sets the language of the document (e.g., "en-US").
        /// </summary>
        public string? Language { get; set; }
        /// <summary>
        /// Gets or sets a list of contributors to the document.
        /// </summary>
        public List<string>? Contributors { get; set; }
        /// <summary>
        /// Gets or sets the version history of the document, if available.
        /// </summary>
        public string? VersionHistory { get; set; }
        /// <summary>
        /// Gets or sets the publication date of the document.
        /// </summary>
        public DateTime ? Published { get; set; }
    }
}
