using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace VectorSearchLib
{
    public class VectorDocument
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Dictionary<string, string> Metadata { get; set; }

        [JsonIgnore]
        public float[] Vector { get; set; }

        [JsonPropertyName("Vector")]
        public List<float> VectorSerializable
        {
            get => Vector.ToList();
            set => Vector = value.ToArray();
        }
    }

}
