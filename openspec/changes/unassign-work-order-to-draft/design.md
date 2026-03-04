## Context

The ChurchBulletin system uses a state command pattern for work order status transitions. Each transition is a record extending `StateCommandBase`, which defines the begin/end statuses, authorization rules, and optional side effects. Existing commands like `AssignedToCancelledCommand` demonstrate the pattern for clearing `Assignee` and `AssignedDate` during a transition. The `AssignedToDraftCommand` follows this exact same pattern.

## Goals / Non-Goals

**Goals:**
- Allow the work order creator to unassign an assigned work order, returning it to Draft
- Clear `Assignee` and `AssignedDate` on transition
- Follow the established `StateCommandBase` record pattern
- Comprehensive unit test coverage

**Non-Goals:**
- UI changes (the command will be automatically discovered by the existing state command resolution in DataAccess handlers)
- Database migration (no schema changes needed)
- Modifying any existing state commands

## Decisions

### Decision 1: Follow the `AssignedToCancelledCommand` pattern exactly

**Rationale:** The `AssignedToCancelledCommand` is the closest analog — same begin status (Assigned), same side effects (clear Assignee and AssignedDate), same authorization (creator only). The only difference is the end status (Draft instead of Cancelled).

### Decision 2: Use "Unassign" as the transition verb

**Rationale:** "Unassign" clearly describes the action (reversing an assignment). It is distinct from "Cancel" and aligns with the issue description. `TransitionVerbPresentTense` = "Unassign", `TransitionVerbPastTense` = "Unassigned".

### Decision 3: Creator-only authorization

**Rationale:** Per the issue requirements, only the creator should be able to unassign. This matches the pattern in `AssignedToCancelledCommand` where `UserCanExecute` checks `currentUser == WorkOrder.Creator`.

## Risks / Trade-offs

- **[Minimal risk]** This is a small, additive change following an established pattern. No existing behavior is modified.
- **[UI discovery]** The new command will appear in the UI's state transition options automatically if the UI resolves available commands from the state command collection. This is desired behavior.

## Open Questions

None — the requirements and implementation pattern are clear.
