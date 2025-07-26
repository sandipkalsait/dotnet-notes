
# C\#” Assignment Questions

## Question 1: List and Explain Your Ten Favorite C\# Features 

1. **Language-Integrated Query (LINQ)**
LINQ embeds SQL-style querying into C\# collections and external data sources. It unifies in-memory and database queries but their performance and execution models differ—`List<int>.Where(...)` runs in-process, whereas `DbSet<int>.Where(...)` is translated to SQL, affecting latency, indexing, and caching.
2. **Async/Await for Asynchronous Programming**
The `async` keyword and `await` operator simplify non-blocking code. `await Task.Delay(1000)` frees up the thread while delaying, unlike `Thread.Sleep(1000)` which blocks the thread entirely, impacting responsiveness under load.
3. **Extension Methods**
Static methods declared in static classes let you “attach” new behavior to existing types. For example, `string.IsPalindrome()` cannot access private fields (unlike subclassing), but it avoids inheritance overhead and preserves encapsulation.
4. **Generics**
Type parameters (`List<T>`, `Dictionary<TKey, TValue>`) enable compile-time type safety and reuse. In contrast, non-generic `ArrayList` stores `object` and requires casts at runtime, risking `InvalidCastException`.
5. **Nullable Reference Types**
Annotating references as nullable (`string?`) versus non-nullable (`string`) triggers compiler warnings, preventing unintended null-dereferences. Legacy APIs without annotations may hide null risks, requiring judicious use of `!` and null checks.
6. **Pattern Matching**
Modern `switch` and `is` patterns let you concisely inspect types and properties.

```csharp
switch (obj)
{
  case Person { Age: > 18 } adult: …
  case null: …
}
```

Versus nested `if`-`else`, pattern matching can be more expressive but may require familiarity to avoid obscure branching.
7. **Records**
Immutable reference types with built-in value equality:

```csharp
public record Person(string Name, int Age);
```

They eliminate boilerplate for `Equals`, `GetHashCode`, and `ToString`, but restrict mutation unless you use `with` expressions to clone with modifications.
8. **Dependency Injection (DI)**
Using `Microsoft.Extensions.DependencyInjection`, you configure services’ lifetimes (`Singleton`, `Transient`, `Scoped`) to manage dependencies. A singleton service reused across threads can introduce state-sharing issues, whereas transient services avoid global state but incur creation overhead.
9. **Nullable Value Types**
Wrapping value types (`int?`, `DateTime?`) signals explicit absence of value rather than sentinel defaults (`-1`), reducing error-prone magic numbers but requiring `HasValue` and `Value` checks or the null-coalescing operator (`??`).
10. **Expression-Bodied Members**
Single-line methods and properties use `=>` syntax for brevity:

```csharp
public override string ToString() => $"{Name} ({Age})";
```

This reduces boilerplate but can hamper debugging when more logic is needed.

## Question 2: Console Application Demonstrating All Ten Features 

Below is a complete `Program.cs` illustrating all features within a single console application named **FeatureDemoApp**:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

// 1. Record for immutable data model
public record Person(string Name, int Age);

// 2. Extension method for palindrome check
public static class StringExtensions
{
    public static bool IsPalindrome(this string s)
        => new string(s.Reverse().ToArray()).Equals(s, StringComparison.OrdinalIgnoreCase);
}

public interface IPersonService { Task<IEnumerable<Person>> GetPeopleAsync(); }

public class PersonService : IPersonService
{
    public async Task<IEnumerable<Person>> GetPeopleAsync()
    {
        await Task.Delay(500); // simulate asynchronous I/O
        return new List<Person>
        {
            new("Anna", 28), new("Bob", 34), new("Eve", 22)
        };
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        // 3. Configure DI container
        var services = new ServiceCollection();
        services.AddSingleton<IPersonService, PersonService>();
        var provider = services.BuildServiceProvider();

        // 4. Use async/await to fetch data
        var service = provider.GetRequiredService<IPersonService>();
        var people = await service.GetPeopleAsync();

        // 5. LINQ to filter adults, and 6. pattern matching via 'with'
        var adults = people
            .Where(p => p.Age >= 18)
            .Select(p => p with { })  // record with expression
            .ToList();

        // 7. Nullable reference and 8. nullable value types demonstration
        Console.Write("Enter a number: ");
        string? input = Console.ReadLine();
        int? parsed = int.TryParse(input, out var n) ? n : null;
        Console.WriteLine(parsed.HasValue
            ? $"You entered: {parsed}"
            : "Invalid number");

        // 9. Generics: display list of adults
        DisplayList(adults);

        // 10. Expression-bodied members & extension method
        foreach (var person in adults)
        {
            Console.WriteLine(person.Name.IsPalindrome()
                ? $"{person.Name} is palindrome!"
                : $"{person.Name} is not palindrome.");
        }
    }

    // Generic method with constraint
    static void DisplayList<T>(List<T> items) where T : notnull
    {
        foreach (var item in items)
            Console.WriteLine(item);
    }
}
```

**Feature-to-Code Mapping:**

- **Record:** `Person`
- **Extension Method:** `IsPalindrome`
- **DI:** `ServiceCollection` \& `AddSingleton`
- **Async/Await:** `GetPeopleAsync`, `Main`
- **LINQ \& Pattern Matching:** `Where`, `with`
- **Nullable Reference \& Value Types:** `string? input`, `int? parsed`
- **Generics:** `DisplayList<T>`
- **Expression-Bodied Members:** extension method and `ToString` inherited


## Question 3: Peer Demonstration and Brainstorm Session 

### 1. Structured Presentation

- Each student runs **FeatureDemoApp**, selects two features (e.g., **LINQ** performance vs. **records** immutability), and explains real-world trade-offs.


### 2. Live Enhancement Exercise

- Modify code on the spot: swap `Task.Delay` for actual file I/O, convert `DisplayList<T>` to yield `IEnumerable<T>`, or introduce `CancellationToken`.


### 3. Critical Comparative Discussion

- Debate async patterns: `Task.Run` vs. genuine `async` I/O.
- Examine DI lifetimes: singleton state issues vs. transient overhead.
- Evaluate pattern matching readability in nested scenarios.


### 4. Roadmap for Advanced Topics

- Introduce unit testing with mock DI containers.
- Explore C\# 11 features (raw string literals, list patterns).
- Migrate console app to a minimal web API or .NET MAUI mobile app.


### 5. Reflection Documentation

- Each peer documents one key insight (e.g., null safety improvements) and one open question (e.g., optimizing LINQ-to-SQL translations).

