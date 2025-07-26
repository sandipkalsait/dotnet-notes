# C# Assignment 1

## Question 1: List and Explain Your Ten Favorite C\# Features 

**Answer:**

1. **LINQ (Language-Integrated Query):**
    - *Explanation:* Integrates query capabilities directly into C\#, enabling powerful, declarative data filtering and transformation.
    - *Contradictive Example:* Filtering an in-memory list versus querying an external database—LINQ unifies both, yet performance characteristics differ significantly depending on data source.
2. **Asynchronous Programming with async/await:**
    - *Explanation:* Simplifies writing non-blocking code by marking methods as `async` and awaiting long-running tasks.
    - *Contradictive Example:* Use `await Task.Delay(1000)` to simulate non-blocking delay versus blocking `Thread.Sleep(1000)`—both pause execution but differ in thread utilization and responsiveness.
3. **Extension Methods:**
    - *Explanation:* Add new methods to existing types without modifying their source or inheriting, by declaring `static` methods in static classes.
    - *Contradictive Example:* Extending `string` to add `IsPalindrome()` contrasts with subclassing—extension methods cannot access private internals but subclassing incurs inheritance complexity.
4. **Generics:**
    - *Explanation:* Define classes, methods, and interfaces with type parameters to achieve compile-time type safety and reuse.
    - *Contradictive Example:* `List<int>` enforces homogeneity at compile time, whereas `ArrayList` accepts mixed types but risks runtime casting errors.
5. **Nullable Reference Types:**
    - *Explanation:* Provides static analysis to distinguish nullable from non-nullable references, reducing null-reference exceptions.
    - *Contradictive Example:* Declaring `string? maybeName` forces null checks; using plain `string` assumes non-null—which can hide potential null-ref issues if legacy code isn’t annotated.
6. **Pattern Matching:**
    - *Explanation:* Enhances `switch` and `is` expressions to match data shapes (e.g., type patterns, property patterns).
    - *Contradictive Example:*

```csharp
if (obj is int i) { /* works for ints */ }  
switch (obj)  
{  
  case string s when s.Length > 0: …  
  case null: …  
}
```

Versus traditional `if-else` chains—pattern matching is more concise but can obscure flow for complex conditions.
7. **Records:**
    - *Explanation:* Immutable reference types with value-based equality, concise syntax for DTOs.
    - *Contradictive Example:*

```csharp
public record Person(string Name, int Age);
```

Versus classic class with manually overridden `Equals`—records save boilerplate but limit mutability.
8. **Dependency Injection (DI) via Microsoft.Extensions.DependencyInjection:**
    - *Explanation:* Standardizes object lifetime management and decoupling through constructor/service injection.
    - *Contradictive Example:* Service registered as `Singleton` shares across requests; `Transient` creates new instances. Misusing lifetimes can cause thread-safety issues or resource leaks.
9. **Nullable Value Types (`int?`, `DateTime?`):**
    - *Explanation:* Wrap value types in `Nullable<T>` to represent missing data without sentinel values.
    - *Contradictive Example:* `int? score = null` versus using `-1` sentinel—`null` is explicit but requires `HasValue` checks; sentinel can slip by unnoticed.
10. **Expression-Bodied Members:**
    - *Explanation:* Simplify single-line methods, properties, and constructors using `=>`.
    - *Contradictive Example:*

```csharp
public override string ToString() => $"{Name} ({Age})";
```

Versus full method block—expression bodies are succinct but can hamper debugging when logic grows.

## Question 2: Create Simple Console Application(s) Demonstrating All Ten Features 

**Answer:**
Below is a high-level design and code snippet for a console application named **FeatureDemoApp** that integrates all ten features:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

// 1. Record type for data model
public record Person(string Name, int Age);

// 2. Extension method to check palindrome
public static class StringExtensions
{
    public static bool IsPalindrome(this string s)
        => s == new string(s.Reverse().ToArray());
}

// 3. Service interface for DI
public interface IPersonService { Task<IEnumerable<Person>> GetPeopleAsync(); }

// 4. Implementation using async/await
public class PersonService : IPersonService
{
    public async Task<IEnumerable<Person>> GetPeopleAsync()
    {
        await Task.Delay(500); // simulate I/O
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
        // 5. Setup DI
        var services = new ServiceCollection();
        services.AddSingleton<IPersonService, PersonService>();
        var provider = services.BuildServiceProvider();

        // 6. Get service
        var service = provider.GetRequiredService<IPersonService>();
        var people = await service.GetPeopleAsync(); // async/await

        // 7. LINQ & Pattern Matching
        var adults = people.Where(p => p.Age >= 18)
                           .Select(p => p with { }) // uses record and expression-bodied clone
                           .ToList();

        // 8. Nullable reference types & values demonstration
        string? input = Console.ReadLine();
        int? parsed = int.TryParse(input, out var num) ? num : null;
        Console.WriteLine(parsed.HasValue
            ? $"You entered: {parsed}"
            : "Invalid number");

        // 9. Generics demonstration
        DisplayList(adults);

        // 10. Pattern matching and extension method
        foreach (var person in adults)
        {
            Console.WriteLine(person.Name.IsPalindrome()
                ? $"{person.Name} is palindrome!"
                : $"{person.Name} is not palindrome.");
        }
    }

    // Generic method
    static void DisplayList<T>(List<T> items)
        where T : notnull
    {
        foreach (var item in items)
            Console.WriteLine(item);
    }
}
```

**Explanation of Feature Usage:**

- Records: `Person` type.
- Extension Methods: `IsPalindrome`.
- Dependency Injection: `IPersonService` \& `ServiceCollection`.
- Async/Await: `GetPeopleAsync()` and `Main`.
- LINQ: Filtering and projection.
- Pattern Matching: `int.TryParse` result and `with` expressions.
- Nullable Reference \& Value Types: `string? input`, `int? parsed`.
- Generics: `DisplayList<T>`.
- Expression-Bodied Member: `IsPalindrome` and `ToString` inherited.
- Nullable Value Types: `int? parsed`.


## Question 3: Demonstrate and Brainstorm with Peers to Deepen Learning 

**Answer:**
After implementing **FeatureDemoApp**, organize a **peer‐review session** that follows this structure:

1. **Presentation (5 minutes per student):**
    - Each student runs the application, highlighting one or two features deeply (e.g., record immutability or LINQ performance pitfalls).
2. **Live Modification Exercise (5 minutes per feature):**
    - Peers suggest an enhancement—such as replacing `Task.Delay` with real I/O, or converting `DisplayList<T>` to yield an `IEnumerable<T>`.
3. **Critical Discussion:**
    - Compare **asynchronous** patterns: `Task.Run` vs. true `async` I/O.
    - Debate **pattern matching** readability in complex scenarios.
    - Evaluate **dependency injection** lifetimes for singleton vs. scoped services in console apps.
4. **Refinement Brainstorm:**
    - Sketch improvements: introducing cancellation tokens, adding unit tests with mock DI containers, or migrating to a minimal API template.
5. **Takeaways Documentation:**
    - Each student writes a brief note on one lesson learned and one remaining question to explore further (e.g., mapping LINQ to SQL optimally).

**Learning Outcomes:**

- Solidify understanding through teaching and listening feedback.
- Identify real-world trade-offs (e.g., verbosity vs. safety in nullable annotations).
- Generate a roadmap for exploring advanced topics such as Blazor or .NET MAUI.
