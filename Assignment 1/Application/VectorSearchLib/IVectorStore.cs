using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorSearchLib
{
    public interface IVectorStore
    {
        void Add(VectorDocument doc);
        bool Delete(string id);
        IEnumerable<(VectorDocument Doc, float Score)> Search(float[] queryVector, int topN);
        void Export(string filePath);
        void Import(string filePath);
        IReadOnlyCollection<VectorDocument> GetAll();
    }
}
