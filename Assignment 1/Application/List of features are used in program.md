# 📘 C# Features Used in the Project – *SemantixEngine*

This project demonstrates the use of core and advanced **C# features** to build a semantic **text SemantixEngine** in a modular, maintainable way.

---

##  1. Object-Oriented Programming (OOP)

- Utilizes custom classes like `VectorDocument`, `VectorIndex`, and `FileManager`.
- Promotes **encapsulation**, **inheritance**, and **abstraction** for modular design.
- Logical grouping of functionality for readability and maintainability.

---

##  2. Interfaces & Abstraction

- Implements interfaces like `IVectorStore`, `ISearchable`, or `IPersistable`.
- Encourages **loose coupling** and **dependency inversion** for testability and extension.
- Allows the index logic to be swapped with alternate implementations (e.g., DB-backed or API-based).

---

##  3. Collections & Generics

- Efficient use of:
  - `List<T>` for ordered document storage.
  - `Dictionary<TKey, TValue>` for fast lookups.
  - `ConcurrentDictionary` (if multithreading is introduced).
- Generics provide **type safety** and **code reusability**.

---

##  4. LINQ (Language Integrated Query)

- Powerful querying using:
  ```csharp
  results.OrderByDescending(x => x.Score).Take(10)
  ```
- Enables **filtering**, **sorting**, **mapping**, and **aggregation** in a clean, expressive syntax.

---

##  5. File I/O (Text/JSON/CSV)

- Supports data persistence and import/export via:
  - `StreamReader`, `StreamWriter`
  - `System.Text.Json` for JSON serialization
- Enables reading and writing document vectors, logs, or cached indexes.

---

##  6. Math Operations & Algorithms

- Implements text vector similarity using:
  - **Cosine similarity**
  - **Dot product**
- Core math operations using `float[]` or `double[]` arrays.

---

##  7. Exception Handling & Validation

- Robust error control with:
  ```csharp
  try { ... } catch (Exception ex) { ... }
  ```
- User input is validated to avoid runtime crashes.
- Helpful error messages are shown to guide the user.

---

##  8. Console-Based UI (CLI Programming)

- Simple and intuitive command-line interface using:
  - `Console.WriteLine()`, `Console.ReadLine()`
- Menu-driven flow for document input, search, deletion, and export.
- Great for learning how to build interactive terminal apps.

---

##  9. Modular Architecture (Class Library + Console App)

- Clean separation of concerns:
  - **Console App** handles user interaction.
  - **Class Library** encapsulates logic for vector storage, similarity search, and file operations.
- Improves **code reuse**, **unit testing**, and **scalability**.

---

##  10. Extensibility via Dependency Injection

- Future-proof design allowing:
  - Pluggable similarity strategies (e.g., Euclidean, Jaccard).
  - Backend replacement (e.g., Redis, Elastic, Faiss).
- Can be extended with `.NET Core`’s native dependency injection framework.

---