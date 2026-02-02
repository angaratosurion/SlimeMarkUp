namespace SlimeMarkUp.Core
{
    /// <summary>
    /// Defines the contract for a markup extension that can parse and process markup lines within a document or text
    /// stream.
    /// </summary>
    /// <remarks>Implementations of this interface provide mechanisms for identifying, parsing, and
    /// prioritizing markup elements. Markup extensions are typically used to extend the capabilities of a markup
    /// processor by handling custom syntax or inline elements. The interface includes members for determining parse
    /// capability, parsing lines, specifying processing order and priority, and indicating whether the extension should
    /// be processed. Implementers should ensure thread safety if extensions are used concurrently.</remarks>
    public interface IMarkupExtension
    {
        /// <summary>
        /// Determines whether the specified line can be parsed by the current parser.
        /// </summary>
        /// <param name="line">The line of text to evaluate for parse compatibility. Cannot be null.</param>
        /// <returns>true if the line can be parsed; otherwise, false.</returns>
        bool CanParse(string line);
        /// <summary>
        /// Parses the specified line of text and returns a corresponding inline markup element if recognized.
        /// </summary>
        /// <param name="line">The line of text to parse for inline markup. Cannot be null.</param>
        /// <returns>A <see cref="MarkupElement"/> representing the parsed inline markup element, or <see langword="null"/> if
        /// the line does not contain a recognized markup element.</returns>
        MarkupElement? Parse(string line); 
        /// <summary>
        /// Determines whether the current operation or item has priority status.
        /// </summary>
        /// <returns>true if the operation or item is considered a priority; otherwise, false.</returns>
        bool Priority();
        /// <summary>
        /// Gets the zero-based position of the item within its containing collection or sequence.
        /// </summary>
        int Order { get; }
        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        int Count  { get;  } 
        /// <summary>
        /// Gets a value indicating whether the item is marked to be processed.
        /// </summary>
        bool IsToBeProccessed { get; }

    }

    public interface IBlockMarkupExtension : IMarkupExtension
    {
        IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines);
       
    }

}
