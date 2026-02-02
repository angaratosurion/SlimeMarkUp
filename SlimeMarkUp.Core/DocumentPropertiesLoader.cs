using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using SlimeMarkUp.Core.Models;
using System.Net.WebSockets;

namespace SlimeMarkUp.Core
{
    /// <summary>
    /// Provides static methods for loading, serializing, and saving document properties in YAML format, including
    /// support for extracting properties from YAML front matter within a text document.
    /// </summary>
    /// <remarks>Use this class to read and write document metadata represented by the DocumentProperties
    /// type. Methods support serialization to YAML, extraction from text input, and saving to files. All methods are
    /// static and thread-safe. If the input does not contain valid YAML front matter or serialization fails, methods
    /// will return null or perform no action as appropriate.</remarks>
    public static class DocumentPropertiesLoader
    {
        /// <summary>
        /// Parses the YAML front matter from the specified input string and returns the corresponding document
        /// properties.
        /// </summary>
        /// <remarks>The method expects the YAML front matter to appear at the start of the input,
        /// enclosed between '---' lines. If the input does not contain valid YAML front matter or deserialization
        /// fails, the method returns <see langword="null"/>. Unmatched YAML properties are ignored during
        /// deserialization.</remarks>
        /// <param name="input">The input string containing the document content with YAML front matter delimited by '---' markers at the
        /// beginning.</param>
        /// <returns>A <see cref="DocumentProperties"/> object containing the deserialized properties if YAML front matter is
        /// found and successfully parsed; otherwise, <see langword="null"/>.</returns>
        public static DocumentProperties? Load(string input)
        {
            // Εντοπίζει το YAML front matter: ξεκινά και τελειώνει με ---
            var match = Regex.Match(input, @"^\s*---\s*\r?\n(.*?)\r?\n\s*---", 
                RegexOptions.Singleline);

            if (!match.Success)
                return null;

            var yaml = match.Groups[1].Value;

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance) // filename → FileName
                .IgnoreUnmatchedProperties()
                .Build();

            try
            {
                return deserializer.Deserialize<DocumentProperties>(yaml);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Serializes the specified document properties to a YAML-formatted string using camel case naming.
        /// </summary>
        /// <remarks>The serialization uses camel case for property names. This method is useful for
        /// persisting or transmitting document property data in a standardized format.</remarks>
        /// <param name="prop">The document properties to serialize. If <paramref name="prop"/> is <see langword="null"/>, the method
        /// returns <see langword="null"/>.</param>
        /// <returns>A YAML-formatted string representing the serialized document properties, or <see langword="null"/> if
        /// <paramref name="prop"/> is <see langword="null"/>.</returns>
        public static string ? SerializeProperties(DocumentProperties prop)
        {
            string ap = null;
            if (prop != null)
            {
                var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance)
                    
                .Build();
                var tmp = serializer.Serialize(prop);
                ap = tmp;

            }
            return ap;
        }
        /// <summary>
        /// Generates an XML comment block containing the serialized properties of the specified document.
        /// </summary>
        /// <remarks>Use this method to embed document property information as an XML comment in generated
        /// output. The returned string can be inserted directly into XML or other text files to provide metadata about
        /// the document.</remarks>
        /// <param name="prop">The document properties to serialize and include in the comment block. Cannot be null.</param>
        /// <returns>A string containing an XML comment block with the serialized document properties, or null if <paramref
        /// name="prop"/> is null.</returns>
        public static string? CommentProperties(DocumentProperties prop)
        {
            string ap = null;

            //DocumentProperties
            if ( prop !=null)
            {
                 
                var tmp= SerializeProperties(prop);
                ap = "<!-- \n"+tmp + "\n-->";

            }


            return ap;


            

        }
        /// <summary>
        /// Saves the specified document properties to a file in serialized format.
        /// </summary>
        /// <remarks>If <paramref name="filename"/> is null or empty, or <paramref name="prop"/> is null,
        /// the method does not perform any operation and no file is created or modified. The file will be overwritten
        /// if it already exists.</remarks>
        /// <param name="filename">The path and name of the file to which the properties will be saved. Cannot be null or empty.</param>
        /// <param name="prop">The document properties to serialize and save. If null, no data will be written.</param>
        public static void SaveToFile(string filename , DocumentProperties prop)
        {
            if (prop != null && !string.IsNullOrEmpty(filename))
            {
                var cont = SerializeProperties(prop);
                if ( cont != null )
                {
                    File.WriteAllText(filename, cont);  

                }
            }
        }
    }
}
