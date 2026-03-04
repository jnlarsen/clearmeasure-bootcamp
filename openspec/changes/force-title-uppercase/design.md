## Context

The WorkOrder domain model in `src/Core/Model/WorkOrder.cs` has a simple auto-property for Title (`public string? Title { get; set; }`). The Description property already demonstrates the pattern for value transformation via a backing field with custom setter logic (truncation to 4000 chars). The same pattern applies here for uppercase conversion.

All state commands (SaveDraftCommand, DraftToAssignedCommand, etc.) operate on WorkOrder instances and persist via EF Core through StateCommandHandler. The transformation must happen at the domain model level so every code path that sets Title produces an uppercase result.

## Goals / Non-Goals

**Goals:**
- Force WorkOrder.Title to uppercase whenever it is set
- Follow the existing pattern used by Description (backing field + setter logic)
- Maintain null safety — null titles remain null, empty strings remain empty

**Non-Goals:**
- Changing the database schema
- Modifying UI input validation or display logic
- Changing any state command logic

## Decisions

### Decision 1: Transform in the Title property setter using a backing field

**Rationale:** The Description property already uses this exact pattern — a private backing field (`_description`) with transformation logic in the setter (`getTruncatedString`). Applying the same pattern to Title keeps the codebase consistent and ensures the rule is enforced regardless of how Title is set.

**Alternatives considered:**
- Transform in SaveDraftCommand.Execute(): Would only cover one save path, not all state commands or direct property sets
- Transform in StateCommandHandler: Would require checking/transforming in a generic handler, mixing concerns
- Transform in EF Core value converter: Would not enforce the rule in the domain model, only at persistence time

## Risks / Trade-offs

- **[Display impact]** All titles will now appear in uppercase in the UI. This is the desired behavior per the requirement.
- **[Existing data]** Existing database records are not affected — only newly saved work orders will have uppercase titles. A migration script could be added later if needed.
