# Coding Guidelines

This guide is derived from the existing `JMAP.Net` codebase and is intended to keep new areas such as `JMAP.Net.Hosting` visually and structurally aligned with the current project.

## Goals

- Keep the public API as readable and predictable as the existing JMAP models.
- Prioritize JSON contracts and RFC terminology over internal abstractions.
- Make new hosting and server components feel consistent with the existing types, tests, and converters.

## Project Structure

- Use file-scoped namespaces that mirror the folder path.
- The file name must match the primary type name.
- Keep public DTOs, protocol types, and options types in flat, easy-to-scan folders.
- Put shared infrastructure in a dedicated area such as `Common`, `Infrastructure`, or `Hosting`.
- Place ASP.NET Core registration extensions in the `Microsoft.Extensions.DependencyInjection` namespace.

## File Organization

- A file contains exactly one top-level production type.
- Do not mix multiple classes, records, structs, or enums in the same file.
- Only allow exceptions for tightly coupled private helper types or intentionally grouped generated types.
- Apply this rule strictly for new code in `JMAP.Net.Hosting`, even if a few older files in the existing codebase still combine a generic base type and a thin wrapper type.
- File names must exactly match the type name, for example `JmapServerOptions.cs`, `JmapAspNetCoreBuilder.cs`, or `MapJmapEndpointRouteBuilderExtensions.cs`.
- Extension methods, builders, options, handlers, contexts, and interfaces each get their own file.
- Test files also contain exactly one test class per file.

## Style

- Use 4 spaces for indentation in C# files.
- Place opening braces on a new line.
- Keep `using` directives at the top of the file, with `System.*` first and project namespaces after that.
- Separate `using` groups with a blank line.
- Prefer file-scoped namespaces over block namespaces.
- Do not add unnecessary `#region` blocks.

## Public API

- Public types must have XML documentation.
- Public model properties should prefer `required` and `init`.
- JSON properties must be named explicitly with `[JsonPropertyName(...)]`.
- Optional properties should use an appropriate `[JsonIgnore(Condition = ...)]`.
- Prefer concrete collection types such as `List<T>` and `Dictionary<TKey, TValue>` in JSON models.
- Names should follow RFC and JMAP terminology, not internal implementation details.

## Modeling

- Simple contract objects stay as `class` unless there is a clear reason to use something else.
- Small value objects with validation logic may be `readonly struct`, as in the existing JMAP core value types.
- Wrapper types for concrete methods may inherit from generic base types when that pattern already exists in the codebase.
- Additional properties on derived requests or responses should be modeled close to the RFC contract, not hidden inside loose metadata containers.

## Nullability and Validation

- Keep `nullable` enabled.
- Avoid ambiguous null semantics in public APIs.
- Prefer `ArgumentNullException.ThrowIfNull(...)` in service, builder, and infrastructure code.
- Use focused `ArgumentException` errors for validated value types such as `JmapId`, `JmapDate`, and `JmapUtcDate`.
- Validation failures should clearly describe the domain-level cause.

## JSON and Serialization

- Serialization must follow the existing contract strictly and should not rely on implicit naming conventions.
- Custom converters should stay small, direct, and focused on a single type.
- If behavior intentionally differs from normal `System.Text.Json` defaults, mention that briefly in the XML comments.
- New HTTP or hosting components may register shared `JsonSerializerOptions`, but they should continue to use the existing converters.

## Hosting and DI Code

- Use `TryAdd` for default registrations so host applications can override defaults.
- Keep builder APIs fluent and small, similar to `AddServer(...).UseAspNetCore()`.
- `Add*()` registers the generic core.
- `UseAspNetCore()` registers only the ASP.NET Core integration.
- Map endpoints separately, for example via `MapJmap()`.
- Validate package-wide or host-wide option relationships with `IConfigureOptions<T>` and `IPostConfigureOptions<T>`.
- Request-scoped state belongs in a dedicated transaction or context type, not in static helpers.

## Tests

- The project uses TUnit.
- Test methods should follow the `Action_ShouldExpectedBehavior` naming pattern.
- Prefer fixture-based contract tests when validating JSON payloads or RFC shapes.
- Compare JSON semantically instead of by raw string value, unless the exact compact output is itself part of the contract.
- New hosting components should start with focused unit and contract tests rather than heavy end-to-end suites.

## What To Carry Into `JMAP.Net.Hosting`

- Public builder and options types with complete XML documentation.
- Small, clearly named interfaces such as `IJmapSessionProvider`, `IJmapRequestDispatcher`, and `IJmapMethodHandler`.
- ASP.NET Core extensions separated from the generic server core.
- No hidden global state; request-specific information should flow through a context or transaction object.
- Error responses should use the existing JMAP error types instead of host-specific ad hoc formats whenever RFC-compatible.

## What To Avoid

- Broad utility classes without a clear domain focus.
- Untyped `object` containers when a dedicated contract type is appropriate.
- ASP.NET-specific types leaking deep into the generic core.
- Hidden serialization behavior without tests.
