using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using SpeculatorModel.MainData;

namespace Speculator.Components
{
    public class SelectedSymbol
    {
        public DataSource DataSource { get; set; }
        public Symbol Symbol { get; set; }
        public DateTime? DateStart { get; set; }

        public override bool Equals(object obj)
        {
            var result = this.DataSource.Id == (obj as SelectedSymbol).DataSource.Id
                         && this.Symbol.Id == (obj as SelectedSymbol).Symbol.Id
                         && this.DateStart == (obj as SelectedSymbol).DateStart;
            return result;
        }

        public override int GetHashCode()
        {
            return this.Symbol.Id.GetHashCode() ^ DateStart.Value.GetHashCode();
        }
    }

    public static class XElementSerialazer
    {
        public static XElement ToXElement<T>(this object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof (T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                }
            }
        }

        public static T FromXElement<T>(this XElement xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof (T));
            return (T) xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }
}
