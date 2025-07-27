# C# Advanced Features 



This markdown file provides additional, self-contained code snippets that highlight deeper aspects of each topic introduced earlier.

---
## 1. Delegates – Multicast & Callbacks
```csharp
using System;

public class Rectangle
{
    public delegate void RectDelegate(double w, double h);

    public void Area(double w, double h) =>
        Console.WriteLine($"Area   = {w * h}");

    public void Perimeter(double w, double h) =>
        Console.WriteLine($"Perim. = {2 * (w + h)}");

    // Generic business method that accepts any callback
    public static void Process(double w, double h, RectDelegate callback)
    {
        callback?.Invoke(w, h);
    }

    public static void Main()
    {
        var rect = new Rectangle();

        // Multicast: += chains delegates (FIFO order)
        RectDelegate calc = rect.Area;
        calc += rect.Perimeter;

        // Fire once  executes both methods
        calc(4, 3);

        // Passing delegate as callback
        Process(10, 2, calc);
    }
}
```

---
## 2. Lambda Expressions – Captured Variables & Expression Trees
```csharp
using System;
using System.Linq.Expressions;

class LambdaDemo
{
    static void Main()
    {
        int factor = 5;                 // captured variable (closure)
        Func<int, int> multiply = x => x * factor;

        Console.WriteLine(multiply(3)); // 15

        // Change captured variable after lambda creation
        factor = 10;
        Console.WriteLine(multiply(3)); // 30

        // Build the same lambda as an expression tree
        ParameterExpression p = Expression.Parameter(typeof(int), "x");
        ConstantExpression f = Expression.Constant(10); // uses latest value
        var body = Expression.Multiply(p, f);
        var exp  = Expression.Lambda<Func<int,int>>(body, p);

        Console.WriteLine(exp.Compile().Invoke(3));     // 30
    }
}
```

---
## 3. LINQ – GroupJoin and Deferred Execution
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class LinqDemo
{
    record Student(string Name, int ClassId);
    record Class  (int Id,    string Title);

    static void Main()
    {
        var students = new[]
        {
            new Student("Alice", 1), new Student("Bob", 1),
            new Student("Cathy", 2), new Student("Dan", 3)
        };
        var classes = new[]
        {
            new Class(1, "Math"), new Class(2, "Physics"), new Class(3, "History")
        };

        // GROUP JOIN → each class with its students
        var query = classes.GroupJoin(
                        students,
                        c => c.Id,
                        s => s.ClassId,
                        (c, studs) => new { c.Title, Students = studs });

        // Deferred: nothing executed yet
        foreach (var grp in query)
        {
            Console.WriteLine($"{grp.Title}: {string.Join(", ", grp.Students.Select(s=>s.Name))}");
        }
    }
}
```

---
## 4. Indexers – Overload & Read-Only
```csharp
using System;
using System.Collections.Generic;

class Contacts
{
    private readonly Dictionary<string,string> _book = new();

    // string key indexer (read-write)
    public string this[string name]
    {
        get => _book.TryGetValue(name, out var v) ? v : "<unknown>";
        set => _book[name] = value;
    }

    // int indexer (read-only) – returns entry at position
    public (string Name,string Phone) this[int index]
        => (_book.Keys.ElementAt(index), _book.Values.ElementAt(index));

    static void Main()
    {
        var c = new Contacts { ["Alice"] = "555-1234", ["Bob"] = "555-9876" };
        Console.WriteLine(c["Alice"]);          // 555-1234
        Console.WriteLine(c[1]);                // (Bob, 555-9876)
    }
}
```

---
## 5. Multithreading – ThreadPool, Race & Lock
```csharp
using System;
using System.Threading;

class ThreadDemo
{
    static int _counter = 0;
    static readonly object _lock = new();

    static void Work()
    {
        for (int i = 0; i < 1_000_000; i++)
        {
            // Comment ‘lock’ to observe race condition
            lock (_lock)
            {
                _counter++;
            }
        }
    }

    static void Main()
    {
        WaitCallback cb = _ => Work();
        ThreadPool.QueueUserWorkItem(cb);
        ThreadPool.QueueUserWorkItem(cb);

        Thread.Sleep(1000);
        Console.WriteLine($"Counter = {_counter}"); // should be 2,000,000
    }
}
```

---
## 6. Preprocessor Directives – Conditional Compilation
```csharp
#define EXPERIMENTAL
using System;

class PreprocDemo
{
    static void Main()
    {
#if EXPERIMENTAL && DEBUG
        Console.WriteLine("Experimental debug build");
#elif EXPERIMENTAL
        Console.WriteLine("Experimental release build");
#else
        Console.WriteLine("Stable build");
#endif
    }
}
```

---
## 7. Assemblies & Metadata – Reading Manifest
```csharp
using System;
using System.Reflection;

class AssemblyInfoDemo
{
    static void Main()
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        Console.WriteLine($"Full name: {asm.FullName}\nLocation : {asm.Location}\n");

        foreach (var attr in asm.GetCustomAttributes())
        {
            Console.WriteLine($"{attr.GetType().Name}: {attr}");
        }
    }
}
```

---
## 8. Reflection – Dynamic Plugin Loader
```csharp
using System;
using System.IO;
using System.Linq;
using System.Reflection;

interface IPlugin { void Run(); }

class Loader
{
    static void Main(string[] args)
    {
        string pluginDir = Path.Combine(AppContext.BaseDirectory, "plugins");
        foreach (var dll in Directory.EnumerateFiles(pluginDir, "*.dll"))
        {
            Assembly asm = Assembly.LoadFrom(dll);
            var types = asm.GetTypes()
                           .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface);
            foreach (var t in types)
                ((IPlugin)Activator.CreateInstance(t)!).Run();
        }
    }
}
```

---
## 9. Collections & Generics – Custom Generic Repository
```csharp
using System;
using System.Collections.Generic;

interface IRepository<T>
{
    void Add(T item);
    IEnumerable<T> All { get; }
}

class InMemoryRepository<T> : IRepository<T>
{
    private readonly List<T> _store = new();
    public void Add(T item) => _store.Add(item);
    public IEnumerable<T> All => _store.AsReadOnly();
}

class RepoDemo
{
    static void Main()
    {
        IRepository<string> repo = new InMemoryRepository<string>();
        repo.Add("foo");
        repo.Add("bar");
        foreach (var s in repo.All) Console.WriteLine(s);
    }
}
```

---
## 10. Task Parallel Library (TPL) – Parallel.ForEach with Cancellation & Exception Handling
```csharp
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class TplDemo
{
    static async Task Main()
    {
        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_,e) => { e.Cancel = true; cts.Cancel(); };

        var urls = Enumerable.Range(1, 20)
                             .Select(i => $"https://example.com/api/{i}");

        var bag = new ConcurrentBag<string>();

        try
        {
            await Parallel.ForEachAsync(urls, cts.Token, async (url, token) =>
            {
                await Task.Delay(200, token);      // simulate I/O
                if (url.EndsWith("/13")) throw new InvalidOperationException("Bad luck!");
                bag.Add(url);
                Console.WriteLine($"Fetched {url}");
            });
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled by user.");
        }
        catch (AggregateException ae)
        {
            foreach (var ex in ae.Flatten().InnerExceptions)
                Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine($"Successful calls: {bag.Count}");
    }
}
```

---
## by Sandip.Kalsait

*Happy coding!*