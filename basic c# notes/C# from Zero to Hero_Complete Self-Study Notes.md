# C\# from Zero to Hero: Complete Self-Study Notes

Before you begin: C\# (pronounced “see-sharp”) is a modern, object-oriented language created at Microsoft 2000 and now standardized by ECMA and ISO[^1][^2]. It sits at the heart of the .NET platform, so once you learn the language you automatically gain access to a vast ecosystem of tools, libraries, and deployment targets.

## Module 1 Evolution of C\#

### 1.1 Why C\# Was Created

- Late-1990s Microsoft needed a safer, component-oriented alternative to C++ and an enterprise rival to Java[^3].
- Anders Hejlsberg headed the language team, initially calling the project “Cool” (C-like Object-Oriented Language)[^4][^5].
- Design goals: simplicity, strong typing, garbage collection, cross-platform deployment, and seamless fit with the new Common Language Runtime (CLR)[^2].


### 1.2 Version Timeline (highlights)

| Year | C\# Version | Flagship Features |
| :-- | :-- | :-- |
| 2000 | 1.0 | Assemblies, classes, structs, events[^6] |
| 2005 | 2.0 | Generics, `yield`, anonymous methods[^6] |
| 2007 | 3.0 | LINQ, lambdas, auto-properties[^7] |
| 2012 | 5.0 | `async`/`await` for async code[^6][^7] |
| 2021 | 10.0 | Global `using`, file-scoped namespaces[^7] |
| 2024 | 13.0 | `params` collections, new `lock` semantics[^7] |

### 1.3 Why Keep Updating?

Each release adds expressiveness while preserving backwards compatibility—e.g., pattern matching (C\# 7-9) reduced `switch` boilerplate and nullable reference types (C\# 8) addressed entire classes of runtime bugs[^6][^7].

#### Key Takeaways

- C\# evolved from a Java-like first version into a multi-paradigm language embracing functional, declarative, and data-oriented styles.
- Version upgrades map tightly to .NET releases; upgrading the compiler unlocks new syntax without touching existing code.


## Module 2 Getting Started

### 2.1 What Is .NET?

Think of .NET as the “engine + toolbox” that runs C\# code. The CLR loads assemblies, does JIT compilation, handles garbage collection, and enforces security[^8][^9]. Today the unified, cross-platform `.NET 8` replaces older “.NET Framework” and “.NET Core” divides[^8][^10].

### 2.2 Development Tools

1. **Visual Studio (Windows)** – full IDE with designers, profiler, and templates[^11][^12].
2. **Visual Studio Code (cross-platform)** – lightweight editor; add “C\# Dev Kit” extension for IntelliSense and debugging[^13][^14].
3. **dotnet CLI** – command-line tool to create, build, test, and run projects (`dotnet new console`, `dotnet run`).

> Real-world analogy: .NET is the power grid, Visual Studio is your workshop, and the dotnet CLI is your cordless drill.

### 2.3 First Program (Hello World)

```C#
using System;

class HelloWorldProgram
{
    static void Main()
    {
        Console.WriteLine("Hello, world!");
    }
}
```

Compile \& run:

```bash
dotnet new console -n HelloDemo
cd HelloDemo
dotnet run
```

Output → `Hello, world!`[^15][^16].

#### Key Takeaways

- Install .NET SDK first, then an editor.
- Every C\# app starts in `Main()`.
- `using System;` pulls in the base class library so you can write `Console.WriteLine`.


## Module 3 Basic Syntax

### 3.1 Variables \& Data Types

```C#
int age = 30;          // whole numbers
double temp = 36.6;    // floating point
char grade = 'A';      // single UTF-16 char
bool isAdmin = true;   // true / false
string name = "Ada";   // text, immutable
```

Value vs. Reference types: primitives (`int`, `double`) live on the stack; objects (`string`, custom classes) are allocated on the managed heap[^17][^18].

### 3.2 Operators

Arithmetic `+ - * / %`, Comparison `== != > < >= <=`[^19], Logical `&& || !`[^20][^21].
Truth-table diagram:

```
A  B | A && B | A || B
----+--------+-------
0  0 |   0    |  0
0  1 |   0    |  1
1  0 |   0    |  1
1  1 |   1    |  1
```


### 3.3 Control Statements

```C#
if (score >= 60) { ... } else { ... }

switch (choice)
{
    case 1: … break;
    case 2: … break;
    default: … break;
}

for (int i = 0; i < 10; i++) { ... }
while (condition) { ... }
```

Modern C\# adds switch expressions and pattern matching to simplify branching[^22][^23].

#### Key Takeaways

- Strong static typing catches errors at compile-time.
- Operator precedence same as in C/C++—use parentheses for clarity[^24].
- Control flow keywords mirror everyday English (`if`, `else`, `while`).


## Module 4 OOP Concepts in C\#

### 4.1 Classes \& Objects

```C#
class Car
{
    public string Model { get; }
    public Car(string model) => Model = model;   // constructor
    public void Drive() => Console.WriteLine($"{Model} is driving");
}
```

Instantiate: `var ford = new Car("Mustang"); ford.Drive();`[^25][^26].

### 4.2 Encapsulation

Access modifiers: `public`, `private`, `protected`, `internal`. Hide internal state to create a “black box”.

### 4.3 Inheritance \& Polymorphism

```C#
class Animal { public virtual void Speak() => Console.WriteLine("..."); }
class Dog : Animal { public override void Speak() => Console.WriteLine("Woof"); }

Animal pet = new Dog();   // up-cast
pet.Speak();              // outputs "Woof" via dynamic dispatch
```

Polymorphism = “many forms”—same call, different behavior[^27][^28].

### 4.4 Abstraction

Use `abstract` classes or interfaces to expose *what* an object does, not *how*.

```C#
interface IPayable { decimal Pay(); }
```


#### Key Takeaways

- Constructors initialize new objects[^29][^30].
- `override` + `virtual` enable run-time substitution.
- Interfaces define contracts; a class may implement many.


## Module 5 Methods \& Parameters

```C#
// value parameter
int Square(int n) => n * n;

// ref and out
void Increment(ref int x) => x++;
bool TryDivide(int a, int b, out double result)
{
    if (b == 0) { result = double.NaN; return false; }
    result = (double)a / b; return true;
}

// params for variable arguments
int Sum(params int[] numbers) => numbers.Sum();
```

`ref` requires the variable be initialized; `out` does not but must be assigned inside the method[^31][^32][^33][^34].

#### Key Takeaways

- Pass-by-value by default; use `ref`/`out` for by-reference semantics.
- `params` lets a method accept any number of same-type arguments.
- Expression-bodied `=>` methods make single-line helpers concise.


## Module 6 Error Handling

### 6.1 try-catch-finally

```C#
try
{
    File.ReadAllText("data.txt");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    Console.WriteLine("Always runs");
}
```

`finally` executes whether or not an exception occurs[^35][^36].

### 6.2 Throwing \& Custom Exceptions

```C#
if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
class TooMuchWithdrawalException : Exception
{
    public TooMuchWithdrawalException(string msg) : base(msg) { }
}
```

Use `throw;` (not `throw ex;`) to preserve stack trace[^37][^38].

### 6.3 Best Practices

- Catch the most specific exception first[^39].
- Don’t swallow exceptions—log or re-throw.
- Avoid using exceptions for normal control flow.


#### Key Takeaways

- Exceptions bubble up until caught.
- `using` statement disposes resources even if an exception occurs.
- Custom exceptions should end with the word “Exception”[^40].


## Module 7 Collections

### 7.1 Arrays vs. Lists vs. Dictionaries

| Feature | Array | List<T> | Dictionary<TKey,TValue> |
| :-- | :-- | :-- | :-- |
| Size | Fixed[^41] | Dynamic[^42] | Dynamic |
| Access | O(1) index | O(1) index | O(1) key-lookup (hash) |
| Type-safe | Yes | Yes | Yes |
| When to use | performance, 2-D data | general sequences | key-value mapping[^43] |

### 7.2 Generic Collections

`System.Collections.Generic` provides `Stack<T>`, `Queue<T>`, `HashSet<T>`, `LinkedList<T>` etc.[^44][^45].

### 7.3 Sample

```C#
var fruits = new List<string> { "apple", "banana" };
fruits.Add("cherry");

var phoneBook = new Dictionary<string,string>
{
    ["Alice"] = "555-1234",
    ["Bob"]   = "555-5678"
};
```


#### Key Takeaways

- Prefer generic collections; they eliminate boxing/unboxing and are faster[^44].
- Choose the collection based on access pattern and mutability needs.
- `foreach` works on anything implementing `IEnumerable<T>`.


## Module 8 Introduction to LINQ

### 8.1 Concept

LINQ (Language Integrated Query) lets you query objects, XML, SQL, etc., with SQL-like syntax inside C\#[^46][^47].

### 8.2 Two Flavors

```C#
// Query syntax
var evens = from n in numbers
            where n % 2 == 0
            orderby n
            select n;

// Method syntax
var evens2 = numbers.Where(n => n % 2 == 0)
                    .OrderBy(n => n);
```


### 8.3 Common Operators

`Where`, `Select`, `OrderBy`, `GroupBy`, `Join`, `Sum`, `Average`[^47][^48].

> Analogy: LINQ is like giving collections a “SQL dialect”—you tell **what** you want, not **how** to iterate.

#### Key Takeaways

- Any `IEnumerable<T>` can be queried.
- LINQ uses deferred execution; query runs when enumerated, not when defined[^46].
- Method syntax is more powerful (e.g., `Skip`, `Take`, `Any`).


## Module 9 File Handling (System.IO)

### 9.1 Quick Reads/Writes

```C#
string text = File.ReadAllText("notes.txt");
File.WriteAllText("notes.txt", "Hello File I/O!");
```


### 9.2 Streaming Large Files

```C#
using var reader = new StreamReader("big.csv");
while (!reader.EndOfStream)
{
    var line = await reader.ReadLineAsync();
    ...
}
```

Streams avoid loading the entire file into memory[^49][^50].

### 9.3 Directories

`Directory.CreateDirectory("logs");`, `Directory.GetFiles(".", "*.txt")`[^51].

#### Key Takeaways

- Wrap streams in `using` to ensure file handles close.
- Choose `ReadAllText` for small files; streams for large.
- Exceptions: `FileNotFoundException`, `IOException`.


## Module 10 Asynchronous Programming

### 10.1 Tasks \& async/await

```C#
async Task<string> DownloadAsync(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url);
}

string html = await DownloadAsync("https://example.com");
```

`await` frees the calling thread; execution resumes when the task completes[^52][^53][^54].

### 10.2 When to Use

I/O-bound work (web requests, DB calls), long-running CPU tasks (use `Task.Run`)—keep UI responsive or server threads unblocked[^55][^56].

### 10.3 Cancellation \& Error Handling

Pass a `CancellationToken`; wrap awaits in `try/catch` for `TaskCanceledException`.

#### Key Takeaways

- Mark method `async`; it must return `Task`, `Task<T>`, or `void` (event handlers).
- `await` can be used multiple times; control flows like synchronous code.
- Async ≠ multithreading; it’s about non-blocking waits.


## Module 11 C\# Best Practices

1. **Naming conventions**: PascalCase for types/methods, camelCase for locals/parameters, ALL_CAPS for constants[^57][^58][^59].
2. **One responsibility per method** (SRP)[^60].
3. **Avoid magic numbers**—use constants or enums.
4. **Prefer dependency injection** over hard-coded newing[^61].
5. **Write unit tests** (NUnit, xUnit).
6. **Use `var` only when RHS is obvious**.
7. **Follow DRY \& KISS** principles.
8. **Log exceptions, don’t ignore them**[^62].

#### Key Takeaways

Clean code is about readability first—computers compile either way, but humans maintain.

## Module 12 Project-Based Learning

### 12.1 Console Calculator

Features: infix expression parsing, supports + − × ÷, loops until user types `exit`. Demonstrates control flow and error handling.

### 12.2 Employee Manager (List \& LINQ)

CRUD operations on an in-memory `List<Employee>` with LINQ search (`Where(e => e.Department == "HR")`). Persistence: serialize to JSON file with `File.WriteAllText`.

### 12.3 File Reader Async

Reads a large log file, counts lines containing “ERROR”, shows progress with `async`/`await`.

Full commented code for each is included in companion repository; run with `dotnet run`.

## Module 13 Interview Readiness

1. Explain `ref` vs `out` parameters[^34].
2. What is boxing/unboxing and how does `List<int>` avoid it?
3. Describe the differences between `Task`, `Thread`, and `async`/`await`[^56].
4. How does garbage collection work in .NET?
5. Why prefer `IEnumerable<T>` over `IList<T>` as return type?
6. What are nullable reference types introduced in C\# 8?
7. Compare `abstract class` vs `interface`.

## Module 14 C\# vs. Java (Cross-language Understanding)

| Aspect | C\# | Java |
| :-- | :-- | :-- |
| Runtime | CLR[^63] | JVM[^63] |
| Operator overloading | Supported[^64] | Not supported |
| Checked exceptions | None[^64] | Yes |
| GUI stacks | WPF, WinUI, MAUI[^65] | JavaFX, Swing |
| Mobile | Xamarin/.NET MAUI | Android (native) |
| Typical domains | Windows apps, game dev (Unity)[^66] | Enterprise servers, Android |

Key insight: Syntax is similar; switching languages is mostly about framework differences.

## Module 15 Real-World Applications of C\#

- **Game Development** – Unity uses C\# scripts to target 20+ platforms[^66].
- **Web APIs** – ASP.NET Core with C\# powers high-performance REST services that scale to millions[^67].
- **Desktop Apps** – WPF/WinUI for rich Windows UIs (e.g., Visual Studio itself)[^65].
- **Cloud \& Microservices** – C\# on .NET runs in Docker and Azure Functions.
- **IoT \& Mobile** – .NET MAUI, Xamarin, and .NET nanoFramework.


#### Key Takeaways

Learning C\# gives you entrée into games, web back-ends, cloud functions, and Windows tooling with one language.

## Module 16 Cheat Sheets \& Quick Reference

### 16.1 C\# Keywords

| Category | Examples |
| :-- | :-- |
| Value types | `int`, `double`, `bool`, `char`, `struct` |
| Control | `if`, `else`, `switch`, `for`, `foreach`, `while`, `do`, `break`, `continue` |
| Exception | `try`, `catch`, `finally`, `throw` |
| OOP | `class`, `interface`, `enum`, `abstract`, `virtual`, `override`, `sealed` |
| Async | `async`, `await` |

### 16.2 Common Collection Operations

```C#
list.Add(item);          // append
list.Remove(item);       // delete first match
dict.TryGetValue(key, out var val);  // safe lookup
stack.Push(x); stack.Pop();
```


### 16.3 Quick LINQ Recipes

| Task | Query |
| :-- | :-- |
| Filter even numbers | `numbers.Where(n => n % 2 == 0)` |
| Top 5 by score | `players.OrderByDescending(p=>p.Score).Take(5)` |
| Group by city | `people.GroupBy(p => p.City)` |

### 16.4 Visual Studio Shortcuts

| Action | Shortcut |
| :-- | :-- |
| Build solution | `Ctrl+Shift+B` |
| Quick actions/refactor | `Ctrl+.` |
| Peek definition | `Alt+F12` |
| Go to implementation | `Ctrl+F12` |
| Run debugging | `F5` / `Shift+F5` stop |

#### Final Takeaways

- Master fundamentals: strong typing, OOP pillars, exceptions.
- Practice LINQ early; it changes how you think about data.
- Embrace async/await for any I/O.
- Follow naming and formatting conventions to write code your *future self* (and interviewers) will love.

Happy coding—your C\# journey starts now!
