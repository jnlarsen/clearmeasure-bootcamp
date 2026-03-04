## Context

The ChurchBulletin system uses a Command Pattern for work order state transitions. Each transition is a record inheriting from `StateCommandBase`, which provides `IsValid()` validation (checking begin status and user authorization) and an `Execute()` method that calls `WorkOrder.ChangeStatus()`. Existing commands follow a consistent pattern: `InProgressToCompleteCommand` sets `CompletedDate` during execution, and `InProgressToAssignedCommand` validates the current user is the assignee.

## Goals / Non-Goals

**Goals:**
- Add a Complete → InProgress state transition following the existing `StateCommandBase` pattern
- Validate that only the assignee can reopen a completed work order
- Clear `CompletedDate` when reopening
- Provide unit test coverage for valid and invalid scenarios

**Non-Goals:**
- UI changes (buttons, pages) for the reopen action — this is a Core-layer change only
- Modifying the DataAccess layer or adding new handlers — the existing `StateCommandHandler` already processes all `IStateCommand` implementations
- Adding new WorkOrderStatus values

## Decisions

### Decision 1: Follow the InProgressToCompleteCommand pattern (mirror/inverse)

**Rationale:** `CompleteToInProgressCommand` is the logical inverse of `InProgressToCompleteCommand`. It should mirror that command's structure: override `Execute()` to clear `CompletedDate` (the inverse of setting it), validate the assignee, and use `GetBeginStatus()` = Complete, `GetEndStatus()` = InProgress.

### Decision 2: Name the transition verb "Reopen"

**Rationale:** "Reopen" clearly communicates the intent of moving a completed work order back to active work. Past tense: "Reopened". This is consistent with the naming pattern of other commands (Assign/Assigned, Complete/Completed, Shelve/Shelved, Cancel/Cancelled).

### Decision 3: Only the assignee can reopen

**Rationale:** The issue specifies the assignee should perform this transition, consistent with `InProgressToCompleteCommand` where only the assignee can complete. The person doing the work should decide if more work is needed.

## Risks / Trade-offs

- **[State diagram complexity]** Adding a backward transition (Complete → InProgress) creates a cycle in the state machine. This is intentional and mirrors real-world workflows where completed work occasionally needs to be reopened.
- **[No UI]** Without corresponding UI changes, this transition is only accessible programmatically (via MCP tools, API, or tests). Future work can add a "Reopen" button to the work order detail page.

## Open Questions

None — the issue requirements are clear and the implementation pattern is well-established.
