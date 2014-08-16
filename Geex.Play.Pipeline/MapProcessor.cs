using Microsoft.Xna.Framework.Content.Pipeline;
using Geex.Run;


namespace Geex.Content
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentProcessor(DisplayName = "Map - Geex Framework")]
    public class MapProcessor : ContentProcessor<Map, Map>
    {
        public override Map Process(Map input, ContentProcessorContext context)
        {
            return input;
        }
    }
}