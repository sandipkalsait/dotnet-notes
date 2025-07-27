using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorSearchLib;


namespace SemantixEngine
{
    class Program
    {
        private static readonly IVectorStore store = new VectorIndex();
        static void Main()
        {
            Console.WriteLine("=== SemantixEngine CLI ===");
            while (true)
            {
                Console.WriteLine("\n1) Add Document\n2) List Documents\n3) Delete Document\n4) Search\n5) Export\n6) Import\n0) Exit");
                Console.Write("Select option: ");
                switch (Console.ReadLine())
                {
                    case "1": Add(); break;
                    case "2": List(); break;
                    case "3": Delete(); break;
                    case "4": Search(); break;
                    case "5": Export(); break;
                    case "6": Import(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        static void Add()
        {
            Console.Write("ID: "); var id = Console.ReadLine();
            Console.Write("Title: "); var title = Console.ReadLine();
            Console.Write("Vector (comma-separated floats): ");
            var vector = Console.ReadLine().Split(',').Select(float.Parse).ToArray();

            var doc = new VectorDocument
            {
                Id = id,
                Title = title,
                Metadata = new Dictionary<string, string>(),
                Vector = vector
            };
            store.Add(doc);

            Console.WriteLine("Document added.");
        }

        static void List()
        {
            var docs = store.GetAll();
            if (!docs.Any()) { Console.WriteLine("No documents."); return; }
            Console.WriteLine("Stored Documents:");
            foreach (var d in docs) Console.WriteLine($"- {d.Id}: {d.Title}");
        }

        static void Delete()
        {
            Console.Write("ID to delete: ");
            var success = store.Delete(Console.ReadLine());
            Console.WriteLine(success ? "Deleted." : "Not found.");
        }

        static void Search()
        {
            Console.Write("Vector (comma-separated floats): ");
            var query = Console.ReadLine().Split(',').Select(float.Parse).ToArray();
            Console.Write("Top N: "); var n = int.Parse(Console.ReadLine());
            var results = store.Search(query, n);
            Console.WriteLine("\nResults:");
            foreach (var (doc, score) in results)
                Console.WriteLine($"{doc.Id}: {doc.Title} (Score: {score:F4})");
        }

        static void Export()
        {
            Console.Write("Export path: ");
            store.Export(Console.ReadLine());
            Console.WriteLine("Export completed.");
        }

        static void Import()
        {
            Console.Write("Import path: ");
            store.Import(Console.ReadLine());
            Console.WriteLine("Import completed.");
        }
    }
}
