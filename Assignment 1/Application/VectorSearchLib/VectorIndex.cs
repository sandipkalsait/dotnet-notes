using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorSearchLib
{
    public class VectorIndex : IVectorStore
    {
        private readonly ConcurrentDictionary<string, VectorDocument> _store;

        public VectorIndex()
        {
            _store = new ConcurrentDictionary<string, VectorDocument>();
        }

        public void Add(VectorDocument doc)
        {
            if (doc.Vector == null) throw new ArgumentNullException(nameof(doc.Vector));
            _store[doc.Id] = doc;
        }

        public bool Delete(string id) => _store.TryRemove(id, out _);

        public IEnumerable<(VectorDocument Doc, float Score)> Search(float[] queryVector, int topN)
        {
            if (queryVector == null) throw new ArgumentNullException(nameof(queryVector));
            var normQuery = queryVector.Normalize();
            return _store.Values
                .Select(doc => (doc, Score: VectorMath.CosineSimilarity(doc.Vector.Normalize(), normQuery)))
                .OrderByDescending(x => x.Score)
                .Take(topN);
        }

        public IReadOnlyCollection<VectorDocument> GetAll() => _store.Values.ToList();

        public void Export(string filePath) => FileManager.SaveToJson(GetAll(), filePath);

        public void Import(string filePath)
        {
            var docs = FileManager.LoadFromJson<VectorDocument>(filePath);
            foreach (var doc in docs) Add(doc);
        }
    }
}
