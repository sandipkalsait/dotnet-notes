# C\# Bootcamp Notes: Learn by Doing

These notes form a full, self-paced path through core C\# and modern .NET. Each module starts with context, drills into hands- examples (three per topic—basic, intermediate, real-world style), flags beginner pitfalls, shares senior-level pro tips, and finishes with a mini challenge plus “What to Learn Next” pointers. Follow the order, practice every snippet, and you will build production-ready C\# muscle memory.

## Module 1  Evolution of C\#

### 1. Why it Matters

C\#’s feature set grows every year; knowing the timeline helps you read legacy code, pick the right language level for projects, and answer interview questions about when `async/await` or nullable reference types appeared[^1][^2].

### 2. Quick Overview

- Born at Microsoft in 2000 (original codename “Cool”) to give the new .NET runtime a modern, type-safe language[^1].
- Standardised by ECMA-334 in 2002, ISO 23270 in 2003[^3].
- Major releases map to .NET releases; even-numbered versions since C\#10 ship with LTS runtimes[^4].


### 3. Detailed Examples

| Year | Version | Big-Picture Feature | Real-World Impact |
| :-- | :-- | :-- | :-- |
| 2005 | C\#2 | Generics | Type-safe collections—no more `ArrayList` boxing[^2] |
| 2012 | C\#5 | `async`/`await` | Non-blocking I/O in ASP.NET—frees server threads[^5] |
| 2024 | C\#13 | `params` collections | Pass `Span<int>` to var-arg APIs in games/high-perf code[^4] |

#### (a) Basic: print the current language version

```csharp
// Requires: <LangVersion>preview</LangVersion> if you target a preview compiler
Console.WriteLine($"C# version: {Environment.Version}");
```

_Output varies; on .NET 9 preview you’ll see “9.0.0”._

#### (b) Intermediate: TOP-LEVEL PROGRAM (C\#9)

```csharp
using System;

// top-level file—no class/namespace ceremony
Console.WriteLine("Top-level programs reduce noise!");
```


#### (c) Real-world: File-scoped namespace (C\#10)

```csharp
namespace Payroll.Api;   // one-liner cleans diff noise

public record Employee(int Id, string FullName);
```


### 4. Common Pitfalls

- Mixing new syntax with old compilers on a build server → CS8652 errors.
- Upgrading to C\#8 without turning on nullable annotations gives a false sense of null-safety.


### 5. Pro Tips

- Add `<LangVersion>latest</LangVersion>` in your .csproj to unlock modern features while remaining multi-target in CI.
- Use `#if NET6_0_OR_GREATER` guards when shipping libraries to consumers on older runtimes.


### 6. Mini Task 🔧

Create a console app that prints “Hello from C\# {version} on .NET {runtime}” using string interpolation and top-level statements.

### 7. What to Learn Next

Dig deeper into how language versions tie to **.NET runtimes and toolchains** (see Module 2).

## Module 2  .NET Runtime vs Framework vs Core vs 6/7/8+

### 1. Why it Matters

Choosing runtime decides OS support, containers, and update cadence for security patches[^6][^7].

### 2. Quick Overview

- **.NET Framework**: Windows-only, mature WinForms/WPF, final feature set frozen at 4.8 LTS (security fixes only).
- **.NET Core 1-3**: Re-engineered, cross-platform, modular CLI.
- **.NET 5–8**: Unified branding; even releases (6, 8) are LTS, odd releases (7) are STS.
- **CLR** (Common Language Runtime): IL → JIT, GC, type safety[^8][^9].


### 3. Detailed Examples

#### Table — pick a runtime

| Scenario | Recommended Runtime | Rationale |
| :-- | :-- | :-- |
| Legacy WinForms LOB app | .NET Framework 4.8 | Has full designer support[^6] |
| Cross-platform microservice in Docker | .NET 8 | Slim Alpine images, native AOT[^10] |
| High-fps Unity game | Mono/Unity CLR fork | Unity engine embeds a tuned runtime[^3] |

#### (a) Basic: detect runtime at run time

```csharp
Console.WriteLine(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
// Sample output: ".NET 8.0.0"
```


#### (b) Intermediate: multi-target library

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net48;net8.0</TargetFrameworks>
  </PropertyGroup>
</Project>
```


#### (c) Real-world: self-contained Linux publish

```bash
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true
```

Creates a stand-alone binary for container distros.

### 4. Common Pitfalls

- Confusing **.NET Standard** (API contract) with **.NET 5+ runtimes**.
- Copying DLLs from Framework 4.8 into a Core app leads to `System.BadImageFormatException`.


### 5. Pro Tips

- Use `dotnet --list-sdks` to audit developer machines.
- Prefer LTS versions in production; update to STS for testbeds to trial features early.


### 6. Mini Task 🔧

Write a script that builds your app for Windows, Linux, and macOS using `dotnet publish` and prints the file sizes—compare trimmed vs. default.

### 7. What to Learn Next

Time to **compile and run a “Hello World”** across runtimes (Module 3).

## Module 3  Hello World Program \& Compilation

### 1. Why it Matters

Seeing the full compile–run cycle cements CLI usage and the relationship between source, IL, and machine code[^11][^12].

### 2. Quick Overview

`dotnet new console`, `dotnet build`, and `dotnet run` wrap MSBuild to turn .cs files → IL in assemblies → JIT’d by CLR.

### 3. Detailed Examples

#### (a) Basic Hello

```bash
dotnet new console -n HelloDemo
cd HelloDemo
dotnet run
```

_Output →_ `Hello, World!`

#### (b) Intermediate: inspect IL with *ILDasm*

```bash
dotnet build -c Release
ildasm bin/Release/net8.0/HelloDemo.dll
```

Observe `IL_0000: ldstr "Hello, World!"`.

#### (c) Real-world: CI build stage (GitHub Actions)

```yaml
- name: Build
  run: dotnet build --configuration Release --no-restore
```


### 4. Common Pitfalls

- Forgetting `namespace` in older compilers pre-C\#10 fails build.
- Mixing Release/Debug artefacts in Docker cache → bloated images.


### 5. Pro Tips

- Use `dotnet watch run` for hot reload cycles (requires .NET 6+)[^13].
- `dotnet publish -c Release /p:PublishTrimmed=true` strips unused IL for tiny containers.


### 6. Mini Task 🔧

Modify HelloDemo to read a `NAME` env var and greet the user; test by passing `NAME=Alice`.

### 7. What to Learn Next

You’re ready for **type fundamentals**—value vs reference memory rules (Module 4).

## Module 4  Data Types: Value, Reference \& Nullable

### 1. Why it Matters

Correctly predicting stack vs heap allocation and nullability stops bugs and avoids premature GC pressure[^14][^15][^16].

### 2. Quick Overview

- **Value types** (`int`, `double`, `struct`) store data inline; copies duplicate the data[^14].
- **Reference types** (`class`, `string`, arrays) store a pointer; copies share the same object[^17].
- **Nullable value types**: `int?`, wraps a `bool HasValue` + `T Value`.
- **Nullable reference types** (C\#8): opt-in compile-time null safety with `string?`[^16].


### 3. Detailed Examples

#### Table — allocation cheat sheet

| Type example | Stack/Heap? | Can be null? | Use when |
| :-- | :-- | :-- | :-- |
| `int` | Stack | No | Math, counters |
| `Point struct` | Inline/Stack | No | High-perf graphics[^18] |
| `Employee class` | Heap | Yes (unless NRT) | Business entities |

#### (a) Basic copy behavior

```csharp
int a = 5;
int b = a;   // copy
b++;
Console.WriteLine($"{a},{b}"); // 5,6
```


#### (b) Intermediate: reference alias

```csharp
var team = new List<string> { "Ada", "Linus" };
var alias = team;
alias.Add("Grace");
Console.WriteLine(team.Count); // 3, same list shared
```


#### (c) Real-world: nullable reference context

```csharp
#nullable enable
string? title = GetBookTitle(id); // might return null
if (title is { Length: >0 })
    Console.WriteLine(title.ToUpper());
```


### 4. Common Pitfalls

- Assuming `struct` is always faster—large structs copied frequently kill cache.
- Forgetting to enable `<Nullable>enable</Nullable>` leads to silent null derefs.


### 5. Pro Tips

- Favor small immutable structs (`readonly struct`) for value equality.
- Use the **null-forgiving operator** `!` sparingly to silence false positives.


### 6. Mini Task 🔧

Implement a `Currency` struct (decimal Amount, string Code) and show copy vs reference behavior when used inside a `List<Currency>`.

### 7. What to Learn Next

With types clear, let’s manipulate them using **operators** (Module 5).

## Module 5  Operators (Arithmetic, Assignment, Comparison, Logical)

### 1. Why it Matters

Operators are glue of expressions; mastery avoids precedence bugs and supports custom operator overloading[^19][^20][^21].

### 2. Quick Overview

C\# groups operators into arithmetic (`+`, `*`), comparison (`==`, `>`), logical (`&&`, `||`), assignment (`+=`), bitwise, and null-handling (`??`)[^20].

### 3. Detailed Examples

#### Table — quick reference

| Category | Examples | Real-world Use |
| :-- | :-- | :-- |
| Arithmetic | `+ - * / %` | Finance calculations |
| Logical | `&& || !` | API input validation |
| Null-coalescing | `??` | Fallback configs |
| Bitwise | `& | ^ << >>` | IoT flag packets |

#### Truth-table diagram (logical `&&`, `||`)

```
A  B | A&&B | A||B
0  0 |  0   |  0
0  1 |  0   |  1
1  0 |  0   |  1
1  1 |  1   |  1
```


#### (a) Basic: compound assignment

```csharp
int hits = 0;
hits += 5;   // same as hits = hits + 5
```


#### (b) Intermediate: null-coalescing assignment (C\#8)

```csharp
List<int>? numbers = null;
(numbers ??= new()).Add(42);
```


#### (c) Real-world: bit flags

```csharp
[Flags]
enum FileAccess { Read=1, Write=2, Delete=4 }

var perms = FileAccess.Read | FileAccess.Write;
bool canWrite = perms.HasFlag(FileAccess.Write);  // true
```


### 4. Common Pitfalls

- Mistaking `=` for `==` inside `if`—compiler catch but watch out in lambdas.
- Using floating-point `==` equality; prefer tolerance comparison.


### 5. Pro Tips

- Overload `+` in value objects (e.g., `Money`) to improve domain readability.
- Use parentheses to clarify precedence in complex LINQ expressions.


### 6. Mini Task 🔧

Write a small function that takes temperature in °C and prints risk level using nested ternary `?:` and null-coalescing `??`.

### 7. What to Learn Next

Ready to branch? Jump into **control flow** structures (Module 6).

## Module 6  Control Flow (`if`, `switch`, loops)

### 1. Why it Matters

Flow statements shape business logic paths and impact performance in tight loops.

### 2. Quick Overview

- Conditional: `if`, pattern-matching `switch`.
- Loops: `for`, `foreach`, `while`, `do…while`.
- Jump: `break`, `continue`, `return`.


### 3. Detailed Examples

#### (a) Basic: pattern switch (C\#9)

```csharp
static string GetGrade(int score) => score switch
{
    >=90 => "A",
    >=75 => "B",
    >=60 => "C",
    _    => "F",
};
```


#### (b) Intermediate: `for` vs `foreach` perf

```csharp
var numbers = Enumerable.Range(0,1_000_000).ToArray();
long sum = 0;
// for faster than foreach on arrays
for (int i=0;i<numbers.Length;i++) sum += numbers[i];
```


#### (c) Real-world: cancellation loop

```csharp
while (!token.IsCancellationRequested)
{
    await ProcessQueueAsync(token);
}
```


### 4. Common Pitfalls

- Using `foreach` on a list you modify → `InvalidOperationException`.
- Forgetting `break` in old `switch` before C\#7 pattern era.


### 5. Pro Tips

- Prefer `foreach` for readability; compiler may optimise.
- Use **`Span<T>`** + `for` when micro-optimising large buffers.


### 6. Mini Task 🔧

Implement a CLI that keeps reading numbers until “exit” and prints running average—demonstrates `while` and exception handling.

### 7. What to Learn Next

Let’s package logic inside **methods** with advanced parameter passing (Module 7).

## Module 7  Methods (Overloading, `ref`, `out`, `params`)

### 1. Why it Matters

Choosing between pass-by-value and pass-by-reference affects API design and performance.

### 2. Quick Overview

- Overload by different parameter list.
- `ref`: caller must init, callee may modify.
- `out`: callee must assign before return.
- `params`: variable-length args, now supports collections (C\#13)[^4].


### 3. Detailed Examples

#### (a) Basic: squared sum overloads

```csharp
int Sum(int a, int b) => a+b;
double Sum(double a, double b) => a+b;
```


#### (b) Intermediate: `TryParse` pattern with `out`

```csharp
if (int.TryParse(input, out int value))
    Console.WriteLine($"Parsed {value}");
else
    Console.WriteLine("Bad number");
```


#### (c) Real-world: `params Span<T>` in game engine

```csharp
static int Sum(params ReadOnlySpan<int> numbers)
{
    int total = 0;
    foreach (var n in numbers) total += n;
    return total;
}
```


### 4. Common Pitfalls

- Forgetting to initialise `ref` param before call → CS816.
- Overusing `out` breaks method composability; prefer tuple returns.


### 5. Pro Tips

- Mark helper methods `static` for JIT inlining.
- Use named arguments to improve readability on long parameter lists.


### 6. Mini Task 🔧

Write `bool TryDivide(int a, int b, out double result)` that avoids divide-by-zero.

### 7. What to Learn Next

Build bigger abstractions: **classes, objects, access modifiers** (Module 8).

## Module 8  Classes, Objects \& Access Modifiers

### 1. Why it Matters

Encapsulation and visibility lay groundwork for maintainable code.

### 2. Quick Overview

- `public`, `private`, `protected`, `internal`, `protected internal`, `private protected`.
- Auto-properties, init-only (`init`) setters (C\#9).


### 3. Detailed Examples

#### (a) Basic encapsulation

```csharp
public class BankAccount
{
    public string Owner { get; }
    private decimal _balance;
    public decimal Balance => _balance;

    public BankAccount(string owner, decimal opening) =>
        (Owner, _balance) = (owner, opening);

    public void Deposit(decimal amount) => _balance += amount;
}
```


#### (b) Intermediate: copy constructor

```csharp
public BankAccount(BankAccount source) =>
    (Owner, _balance) = (source.Owner, source._balance);
```


#### (c) Real-world: record class for DTO

```csharp
public record InvoiceDto(int Id, decimal Total, DateTime IssuedAt);
```


### 4. Common Pitfalls

- Exposing mutable fields as `public` breaks invariants.
- Forgetting parameterless constructor needed by ORMs.


### 5. Pro Tips

- Make classes immutable by default; use `with` expression for copies.
- Seal classes unless designed for inheritance (favor composition).


### 6. Mini Task 🔧

Design a `TodoItem` class with `Id`, `Title`, `IsDone`, and a method `Toggle()`.

### 7. What to Learn Next

Zoom into **constructors** and `this` usage (Module 9).

## Module 9  Constructors \& the `this` Keyword

### 1. Why it Matters

Constructors enforce invariants; `this` clarifies member access and enables chaining.

### 2. Quick Overview

- Parameterless, parameterised, static constructors.
- Constructor chaining `: this(otherArgs)` reduces duplication.


### 3. Detailed Examples

#### (a) Basic chaining

```csharp
public Car(string model) : this(model, 0) {}

public Car(string model, int km) =>
    (Model, Kilometers) = (model, km);
```


#### (b) Intermediate: static constructor

```csharp
static readonly HttpClient Http;
static MyApi()
{
    Http = new HttpClient { BaseAddress = new("https://api") };
}
```


#### (c) Real-world: DI-friendly class

```csharp
public class OrdersService(IRepository<Order> repo, ILogger<OrdersService> log)
{
    private readonly IRepository<Order> _repo = repo;
    private readonly ILogger<OrdersService> _log = log;
}
```


### 4. Common Pitfalls

- Doing heavy I/O in static constructor—blocks first call.
- Calling overridable methods from constructor (unsafe virtual call).


### 5. Pro Tips

- Use `private Car() {}` to block default constructor while allowing EF Core factory usage.
- Prefer minimal records for DTOs to auto-generate equality.


### 6. Mini Task 🔧

Implement a `Stopwatch` wrapper that records start time in constructor and prints elapsed on disposal (`IDisposable`).

### 7. What to Learn Next

Distinguish **static vs instance** members (Module 10).

## Module 10  Static vs Instance Members

### 1. Why it Matters

Global state hidden in statics leads to hard-to-test code; instances enable DI and polymorphism.

### 2. Quick Overview

- `static` field = one per AppDomain.
- Instance members live per object.
- Static classes cannot be instantiated (e.g., `Math`).


### 3. Detailed Examples

#### (a) Basic utility

```csharp
public static class TemperatureConverter
{
    public static double CelsiusToFahrenheit(double c) => c * 9/5 + 32;
}
```


#### (b) Intermediate: singleton pattern

```csharp
public sealed class Config
{
    private Config() { }
    public static Config Instance { get; } = new();
}
```


#### (c) Real-world: static `Guid` cache

```csharp
public static class TraceIds
{
    public static Guid Current { get; } = Guid.NewGuid();
}
```


### 4. Common Pitfalls

- Thread safety: mutate static collections without locks.
- Forgetting to dispose instance resources when hiding everything static.


### 5. Pro Tips

- In ASP.NET Core, prefer scoped services over statics—framework handles lifetime.
- Use static `using` alias `using static System.Console;` to reduce verbosity.


### 6. Mini Task 🔧

Refactor a math helper class to `static` and call its methods without creating objects.

### 7. What to Learn Next

Time to model hierarchies via **inheritance and polymorphism** (Module 11).

## Module 11  Inheritance \& Polymorphism (`virtual`, `override`, `sealed`)

### 1. Why it Matters

Runtime substitution allows open-ended extensibility but also causes brittle bases if misused.

### 2. Quick Overview

- `virtual` lets base method be overridden.
- `override` implements new behavior.
- `sealed` stops further overriding, improving JIT inlining.


### 3. Detailed Examples

#### (a) Basic hierarchy

```csharp
class Animal { public virtual void Speak() => Console.WriteLine("..."); }
class Dog : Animal { public override void Speak() => Console.WriteLine("Woof"); }
```


#### (b) Intermediate: base call

```csharp
class LoggingDog : Dog
{
    public override void Speak()
    {
        Console.WriteLine("Logging before bark");
        base.Speak();
    }
}
```


#### (c) Real-world: template method pattern

```csharp
abstract class DataImporter
{
    public void Import() { Connect(); Read(); }
    protected abstract void Connect();
    protected abstract void Read();
}
```


### 4. Common Pitfalls

- Forgetting `override` keyword—hides method via `new` leading to debug nightmares.
- Overengineering huge hierarchies; prefer composition.


### 5. Pro Tips

- Mark classes `sealed` by default; open explicitly.
- Use pattern matching (`is`, `switch`) to eliminate downcasting.


### 6. Mini Task 🔧

Create `Shape` base class with `Area()`; implement `Circle`, `Rectangle`; calculate areas polymorphically.

### 7. What to Learn Next

Compare **interfaces vs abstract classes** for contracts (Module 12).

## Module 12  Interfaces \& Abstract Classes

### 1. Why it Matters

Interfaces enable multiple inheritance of behavior contracts; abstract classes share base code.

### 2. Quick Overview

- Interface: only signatures (until default interface methods C\#8).
- Abstract class: can hold state and implementation.


### 3. Detailed Examples

#### (a) Basic : interface

```csharp
interface IPlayable { void Play(); }
class Video : IPlayable { public void Play() => Console.WriteLine("Playing video"); }
```


#### (b) Intermediate: abstract common code

```csharp
abstract class Repository<T>
{
    protected readonly DbContext _ctx;
    protected Repository(DbContext ctx) => _ctx = ctx;
    public abstract Task<T?> GetAsync(int id);
}
```


#### (c) Real-world: multiple interface implementation

```csharp
class SmartSpeaker : IPlayable, IDisposable
{
    public void Play() => Console.WriteLine("Music on");
    public void Dispose() => Console.WriteLine("Shutting down");
}
```


### 4. Common Pitfalls

- Adding members to public interfaces breaks consumers; favour versioned facades.
- Using abstract class when you simply need dependency inversion.


### 5. Pro Tips

- Keep interfaces small, single-purpose (ISP).
- Use record interfaces in test doubles for mocking frameworks.


### 6. Mini Task 🔧

Define `IReportFormatter` with `string Format<T>(T data)` and implement JSON \& CSV versions; show polymorphic use.

### 7. What to Learn Next

Time to manage **collections** efficiently (Module 13).

## Module 13  Collections (Array, List, Dictionary, HashSet)

### 1. Why it Matters

Proper collection choice affects big-O performance and memory footprint[^22][^20].

### 2. Quick Overview

- **Array**: fixed length, contiguous memory.
- **List<T>**: dynamic array wrapper.
- **Dictionary<TKey,TValue>**: hash map.
- **HashSet<T>**: unique set membership.


### 3. Detailed Examples

| Operation | Array | List | Dictionary | HashSet |
| :-- | :-- | :-- | :-- | :-- |
| Lookup by index | O(1) | O(1) | N/A | N/A |
| Add item | N/A | Amortised O(1) | O(1) | O(1) |
| Check existence | Linear | Linear | O(1) | O(1) |

#### (a) Basic: List CRUD

```csharp
var fruits = new List<string> { "apple", "banana" };
fruits.Insert(1, "pear");
```


#### (b) Intermediate: Dictionary phone book

```csharp
var phone = new Dictionary<string,string>
{
    ["Alice"]="555-1234"
};
if (phone.TryGetValue("Alice", out var num))
    Console.WriteLine(num);
```


#### (c) Real-world: HashSet for fast dedup

```csharp
var processedIds = new HashSet<Guid>();
if (processedIds.Add(id))
    await HandleAsync(id);    // only once
```


### 4. Common Pitfalls

- Modifying collection during `foreach`.
- Using `List.Contains` in loops (O(n²))—use `HashSet`.


### 5. Pro Tips

- For micro-perf, prefer `Dictionary<,>(capacity)` to avoid resize.
- Use immutable collections (`ImmutableArray<T>`) in multithreaded code.


### 6. Mini Task 🔧

Write a method that receives an `IEnumerable<int>` and returns unique primes using `HashSet<int>`.

### 7. What to Learn Next

Query collections declaratively with **LINQ** (Module 14).

## Module 14  LINQ (Language-Integrated Query)

### 1. Why it Matters

LINQ turns imperative loops into expressive, testable queries[^23][^24][^25].

### 2. Quick Overview

- Two syntaxes: query (`from x in y where ... select ...`) and method (`Where`, `Select`).
- Deferred execution; only runs when enumerated.


### 3. Detailed Examples

#### (a) Basic filter + projection

```csharp
var evens = numbers.Where(n => n%2==0).Select(n => n*n);
```


#### (b) Intermediate: group by

```csharp
var byDept = employees.GroupBy(e => e.Department)
                      .Select(g => new { g.Key, Count = g.Count() });
```


#### (c) Real-world: join \& aggregate

```csharp
var report = orders.Join(customers,
            o => o.CustomerId,
            c => c.Id,
            (o,c) => new { c.Country, o.Total })
        .GroupBy(x => x.Country)
        .Select(g => new { g.Key, Revenue = g.Sum(x => x.Total) });
```


### 4. Common Pitfalls

- Forgetting `.ToList()` before mutating underlying data; query re-executes.
- Heavy queries inside UIs—call `.ToList()` then bind.


### 5. Pro Tips

- Chain `Select` after `Where` to avoid pulling extra columns[^23].
- Use `AsParallel()` sparingly—measure first.


### 6. Mini Task 🔧

Given a CSV of sales (Country, Amount), use LINQ to compute top-3 countries by revenue.

### 7. What to Learn Next

Move from memory to disk via **file I/O** (Module 15).

## Module 15  File I/O with `System.IO`

### 1. Why it Matters

Reading configs, logs, and data feeds is daily work[^12][^26][^27].

### 2. Quick Overview

- **Quick APIs**: `File.ReadAllText`, `File.WriteAllText` for small files[^12][^27][^28].
- **Streaming**: `StreamReader`, `StreamWriter` for large files[^26][^29].
- Async versions end with `Async`.


### 3. Detailed Examples

#### (a) Basic: configuration load

```csharp
string json = File.ReadAllText("settings.json");
```


#### (b) Intermediate: StreamReader line loop

```csharp
using var reader = new StreamReader("big.log");
while (!reader.EndOfStream)
{
    string line = await reader.ReadLineAsync();
    if (line.Contains("ERROR")) errors++;
}
```


#### (c) Real-world: write atomically

```csharp
var temp = Path.GetTempFileName();
File.WriteAllText(temp, data, Encoding.UTF8);
File.Replace(temp, "data.txt", backupFileName: null);
```


### 4. Common Pitfalls

- Loading 4 GB file into memory with `ReadAllText`—OOM crash[^12].
- Forgetting to dispose `StreamReader`—file locked.


### 5. Pro Tips

- Use `File.OpenHandle` (NET8) with `FileOptions.Asynchronous` for high-throughput logs.
- Prefer UTF-8 everywhere; pass `Encoding.UTF8` overload[^30].


### 6. Mini Task 🔧

Write an async method that copies a file line-by-line, only writing lines that exceed 80 characters.

### 7. What to Learn Next

Wire code units together with **delegates, events, lambdas** (Module 16).

## Module 16  Delegates, Events \& Lambdas

### 1. Why it Matters

Delegates enable decoupled callbacks; events underpin GUI and messaging[^31][^32][^33].

### 2. Quick Overview

- Delegate = type-safe method pointer.
- Lambda syntax `() => {}` creates anonymous delegates.
- `event` keyword wraps a multicast delegate for publisher–subscriber.


### 3. Detailed Examples

#### (a) Basic delegate

```csharp
delegate void Log(string msg);          // declaration
Log logger = Console.WriteLine;         // assignment
logger("Hello delegate");
```


#### (b) Intermediate: custom event

```csharp
class Downloader
{
    public event Action<int>? Progress;
    public async Task RunAsync()
    {
        for(int i=0;i<=100;i+=10)
        {
            await Task.Delay(100);
            Progress?.Invoke(i);
        }
    }
}
```


#### (c) Real-world: LINQ lambda combinations

```csharp
Func<int,bool> gt5 = x => x>5;
var filtered = numbers.Where(gt5);
```


### 4. Common Pitfalls

- Capturing loop variable in lambda leads to unexpected late binding.
- Forgetting to unsubscribe events causes memory leaks.


### 5. Pro Tips

- Use `Action`, `Func` built-ins instead of custom delegate types when possible.
- Dispose event sources or use weak events to avoid leaks in WPF.


### 6. Mini Task 🔧

Create a `Timer` class that fires a `Tick` event each second; subscriber prints elapsed seconds.

### 7. What to Learn Next

Handle multiple tasks concurrently with **async/await \& TPL** (Module 17).

## Module 17  Async/Await \& Tasks

### 1. Why it Matters

Async I/O frees threads; TPL simplifies parallel CPU work[^34][^35][^36][^37][^38].

### 2. Quick Overview

- `Task` represents an async operation; `await` unwraps it.
- `async` method must return `Task`, `Task<T>`, or `ValueTask<T>`.
- Cancellation via `CancellationToken`.


### 3. Detailed Examples

#### (a) Basic HTTP fetch

```csharp
async Task<string> FetchAsync(string url)
{
    using var http = new HttpClient();
    return await http.GetStringAsync(url);
}
```


#### (b) Intermediate: parallel CPU with `Task.Run`

```csharp
var tasks = Enumerable.Range(1,4)
    .Select(i => Task.Run(() => HeavyCalc(i)));
await Task.WhenAll(tasks);
```


#### (c) Real-world: producer-consumer with Channel

```csharp
var channel = Channel.CreateUnbounded<int>();
_ = Task.Run(async () => { foreach(var n in data) await channel.Writer.WriteAsync(n); channel.Writer.Complete(); });
await foreach(var n in channel.Reader.ReadAllAsync(token)) Process(n);
```


### 4. Common Pitfalls

- Blocking with `.Result` causing deadlocks on UI threads.
- Missing `ConfigureAwait(false)` in library code—context capture overhead.


### 5. Pro Tips

- Return `ValueTask` for high-frequency methods mostly completing synchronously.
- Use `Parallel.ForEachAsync` (NET6) for data-parallel loops.


### 6. Mini Task 🔧

Build a CLI that downloads 3 URLs concurrently and prints their combined byte size.

### 7. What to Learn Next

Validate code with **unit testing frameworks** (Module 18).

## Module 18  Unit Testing in C\# (xUnit / NUnit)

### 1. Why it Matters

Tests prove behaviour, enable refactoring, and serve as living documentation[^39][^40][^41][^42][^43][^44].

### 2. Quick Overview

- Create test project: `dotnet new xunit -n MyApp.Tests`.
- Attribute `[Fact]` (xUnit) or `[Test]` (NUnit).
- Assertions via `Assert.Equal`, `Assert.Throws`.


### 3. Detailed Examples

#### (a) Basic xUnit fact

```csharp
public class MathTests
{
    [Fact]
    public void Add_ReturnsSum() => Assert.Equal(5, 2+3);
}
```


#### (b) Intermediate theory with data

```csharp
[Theory]
[InlineData(2,2,4)]
[InlineData(-1,1,0)]
public void Add_Works(int a,int b,int expected) =>
    Assert.Equal(expected, a+b);
```


#### (c) Real-world: Arrange-Act-Assert pattern

```csharp
[Fact]
public async Task GetAsync_ReturnsEmployee()
{
    // Arrange
    var repo = new FakeRepo();
    repo.Save(new Employee {Id=1,Name="Ada"});
    var svc = new EmployeeService(repo);

    // Act
    var result = await svc.GetAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Ada", result!.Name);
}
```


### 4. Common Pitfalls

- Tests depending on system clock or external DB—flake.
- Ignoring assert messages; include reason argument.


### 5. Pro Tips

- Keep tests fast (<200 ms) to encourage frequent runs.
- Use continuous test runners (`dotnet watch test`) to get instant feedback.


### 6. Mini Task 🔧

Write an xUnit test that asserts `TryDivide` from Module 7 returns `false` when divisor is 0.

### 7. What to Learn Next

Consolidate skills into a **mini console CRUD project** (Module 19).

## Module 19  Project Example: Console CRUD App

### 1. Why it Matters

Applying concepts end-to-end reinforces retention.

### 2. Quick Overview

We’ll build `ContactManager` storing `Person` objects in a JSON file. Operations: List, Add, Remove, Search.

### 3. Code Walk-through

1. **Model**
```csharp
public record Person(Guid Id, string Name, string Email);
```

2. **Repository (File I/O, LINQ)**
```csharp
class JsonRepo
{
    private const string File="contacts.json";
    private List<Person> _cache = [];

    public JsonRepo() => Load();
    void Load() => _cache = File.Exists(File)
        ? JsonSerializer.Deserialize<List<Person>>(File.ReadAllText(File)) ?? []
        : [];
    void Save() => File.WriteAllText(File, JsonSerializer.Serialize(_cache, new JsonSerializerOptions { WriteIndented=true }));
    public IEnumerable<Person> All() => _cache;
    public void Add(Person p) { _cache.Add(p); Save(); }
    public bool Remove(Guid id) { var removed=_cache.RemoveAll(p=>p.Id==id)>0; if(removed) Save(); return removed; }
}
```

3. **UI (top-level program)**
```csharp
var repo = new JsonRepo();
while(true)
{
    Console.WriteLine("(L)ist (A)dd (R)emove (Q)uit:");
    switch(Console.ReadKey(true).Key)
    {
        case ConsoleKey.L:
            repo.All().ToList().ForEach(p=>Console.WriteLine($"{p.Id} {p.Name} {p.Email}"));
            break;
        case ConsoleKey.A:
            Console.Write("Name: "); var name=Console.ReadLine();
            Console.Write("Email: "); var mail=Console.ReadLine();
            repo.Add(new Person(Guid.NewGuid(), name!, mail!));
            break;
        case ConsoleKey.R:
            Console.Write("Id: "); if(Guid.TryParse(Console.ReadLine(), out var id)) repo.Remove(id);
            break;
        case ConsoleKey.Q: return;
    }
}
```


### 4. Common Pitfalls

- Concurrent edits—solve later with file locking.
- No validation on email; add regex.


### 5. Pro Tips

- Swap repo with in-memory during unit tests (dependency inversion).
- Add async file methods for large datasets.


### 6. Mini Task 🔧

Extend app with search by substring using LINQ `Where`.

### 7. What to Learn Next

Congrats—move on to ASP.NET Core or Unity depending on your interests.

## Cheat-Sheet Section (Quick Reference)

### Keyboard Shortcuts (VS 2022 default)

| Action | Shortcut |
| :-- | :-- |
| Build solution | Ctrl+Shift+B |
| Refactor / Quick actions | Ctrl+. |
| Toggle breakpoint | F9 |
| Step into | F11 |

### Nullable Annotations Symbols

| Symbol | Meaning |
| :-- | :-- |
| `?` | Value may be null (`string?`) |
| `!` | Null-forgiving (`obj!.Prop`) |
| `#nullable enable` | Toggle context in file |

### Delegates vs Events

| Aspect | Delegate | Event |
| :-- | :-- | :-- |
| Can caller overwrite invocations | Yes | No (only add/remove)[^33] |
| Single vs multi target | Both | Multicast by default |
| Common uses | LINQ lambdas, callbacks | UI, pub-sub messaging |

## Final Key Takeaways

- C\#’s 20-plus-year evolution stays backward compatible; keep compiler updated to unlock syntax without breaking old code[^1][^4].
- Unified .NET 6/8 LTS gives you cross-platform, high-performance runtime choices; match to workload needs[^6][^7].
- Deep understanding of value vs reference types, nullable annotations, and memory helps you write faster, safer code with fewer GC pauses[^14][^16].
- LINQ, async/await, and the BCL collections bring declarative, expressive power—learn their performance characteristics and idioms[^24][^35].
- Unit testing with xUnit/NUnit, plus CI, builds confidence and enables fearless refactors[^39][^41].
- Always measure: use `Stopwatch`, benchmarks, and profiling before premature optimisation.
- Practice hands-on: complete every mini-challenge and extend the console app into a REST API when ready.

Happy hacking—your journey from beginner to confident C\# developer is underway!
