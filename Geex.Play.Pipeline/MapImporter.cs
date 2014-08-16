using System.IO;
using System.Xml.Serialization;
using Geex.Run;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Geex.Content
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentImporter(".map", DisplayName = "Map - Geex Framework", DefaultProcessor = "MapProcessor")]
    public class MapImporter : ContentImporter<Map>
    {
        public override Map Import(string filename, ContentImporterContext context)
        {
            // Uses XmlSerializer to load a map file
            Map item = new Map();
            FileStream sw = File.Open(filename, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            item = (Map)serializer.Deserialize(sw);
            sw.Close();
            return item;
        }
    }
}
