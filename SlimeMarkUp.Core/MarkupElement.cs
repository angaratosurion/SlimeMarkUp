namespace SlimeMarkUp.Core
{
    /// <summary>
    /// Represents an element in a markup structure, including its tag name, content, attributes, and child elements.
    /// </summary>
    /// <remarks>Use this class to model hierarchical markup such as HTML or XML. Each instance can contain
    /// nested child elements, allowing for complex document structures. The class does not perform validation or
    /// rendering; it is intended for representing markup data in memory.</remarks>
    public class MarkupElement
    {
        /// <summary>
        /// Gets or sets the HTML tag name associated with the element.
        /// </summary>
        /// <remarks>The default value is "p", representing a paragraph element. This property can be set
        /// to any valid HTML tag name to customize the type of element rendered.</remarks>
        public string Tag { get; set; } = "p";
        /// <summary>
        /// Gets or sets the textual content associated with this instance.
        /// </summary>
        public string Content { get; set; } = "";
        /// <summary>
        /// Gets or sets a collection of key-value pairs that represent custom attributes associated with the object.
        /// </summary>
        /// <remarks>The dictionary keys and values are user-defined and may be used to store additional
        /// metadata. The property may be null if no attributes are set.</remarks>
        public Dictionary<string, string>? Attributes { get; set; }
        /// <summary>
        /// Gets or sets the collection of child markup elements contained within this element.
        /// </summary>
        /// <remarks>The order of elements in the collection reflects their sequence within the parent
        /// markup element. The property may be null if there are no child elements.</remarks>
        public List<MarkupElement>? Children { get; set; }
    }
}
