## Context

The ChurchBulletin system uses Onion Architecture with a CQRS pattern via MediatR. State transitions for work orders are modeled as `IStateCommand` implementations that extend `StateCommandBase`. Each command declares a begin status, end status, authorization check, and transition verb. The `StateCommandList` class registers all available commands and provides filtering/matching methods. Existing commands include: `SaveDraftCommand`, `DraftToAssignedCommand`, `AssignedToInProgressCommand`, `InProgressToAssignedCommand`, `InProgressToCompleteCommand`, and `AssignedToCancelledCommand`.

## Goals / Non-Goals

**Goals:**
- Add a Complete → Assigned state transition following the existing `StateCommandBase` pattern
- Restrict execution to the work order creator (matching `DraftToAssignedCommand` authorization pattern)
- Update `AssignedDate` and clear `CompletedDate` during execution
- Register the command in `StateCommandList` so it is discoverable by the UI and MCP tools

**Non-Goals:**
- Modifying the DataAccess layer (the existing `StateCommandHandler` handles persistence)
- Adding new UI components (existing UI dynamically renders available commands)
- Changing the `WorkOrder` model or database schema

## Decisions

### Decision 1: Follow the `DraftToAssignedCommand` pattern for authorization

**Rationale:** The `DraftToAssignedCommand` checks `currentUser == WorkOrder.Creator`. The same authorization rule applies here — only the creator should be able to reassign a completed work order. This maintains consistency.

### Decision 2: Name the transition verb "Reassign"

**Rationale:** "Reassign" clearly communicates the action. The present tense verb is `Reassign` and past tense is `Reassigned`. This follows the naming convention of existing commands (Assign/Assigned, Complete/Completed, Shelve/Shelved).

### Decision 3: Clear `CompletedDate` and set `AssignedDate` during execution

**Rationale:** Moving from Complete back to Assigned means the work order is no longer complete. Clearing `CompletedDate` and setting `AssignedDate` to the current datetime maintains data integrity and matches the behavior of other commands that set dates on transition.

### Decision 4: Register the command at the end of the `StateCommandList`

**Rationale:** The command list order matters for UI rendering. Adding `CompleteToAssignedCommand` after `AssignedToCancelledCommand` keeps the list organized by workflow progression. The existing test verifies count and order, so it must be updated.

## Risks / Trade-offs

- **[State loop]** This creates a cycle in the state machine (Complete → Assigned → InProgress → Complete). This is intentional — it enables rework workflows. No mitigation needed.
- **[Test update]** The `StateCommandListTests.ShouldReturnAllStateCommandsInCorrectOrder` test hardcodes the command count and order. It must be updated to expect 7 commands.

## Open Questions

None — the implementation is well-defined by the existing patterns and the issue specification.
