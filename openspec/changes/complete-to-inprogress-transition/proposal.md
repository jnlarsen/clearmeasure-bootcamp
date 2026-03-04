## Why

Once a work order reaches Complete status, no further transitions are available. If the assignee discovers additional work is needed, there is no way to reopen the work order — a new one must be created. This creates unnecessary overhead and breaks traceability for ongoing work on the same task.

## What Changes

- New state command `CompleteToInProgressCommand` in `src/Core/Model/StateCommands/` enabling the Complete → InProgress transition
- The assignee of a completed work order can reopen it, transitioning status back to InProgress
- The `CompletedDate` is cleared upon reopening
- The assignee remains unchanged
- Unit tests covering valid transition, invalid user, and CompletedDate clearing scenarios

## Capabilities

### New Capabilities
- `reopen-work-order`: Assignee can transition a completed work order back to InProgress status via a new `CompleteToInProgressCommand` state command

### Modified Capabilities
<!-- No existing capabilities are modified. This is a new additive state transition. -->

## Impact

- **New file**: `src/Core/Model/StateCommands/CompleteToInProgressCommand.cs`
- **New file**: `src/UnitTests/Model/StateCommands/CompleteToInProgressCommandTester.cs`
- **Updated file**: `arch/arch-c4-class-domain-model.md` (add Complete → InProgress transition to diagram)
- **Dependencies**: No new NuGet packages required
- **Database**: No schema changes — uses existing WorkOrderStatus values
- **Architecture**: Stays within Core layer; follows existing StateCommandBase pattern
