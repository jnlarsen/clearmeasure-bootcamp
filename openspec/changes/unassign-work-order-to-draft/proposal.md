## Why

Once a work order is assigned, the creator has no way to return it to Draft status for revision before reassigning. The only available transition from Assigned for the creator is cancellation. This forces creators to cancel and recreate work orders when they simply need to revise the assignment, adding unnecessary friction.

## What Changes

- New state command `AssignedToDraftCommand` in `src/Core/Model/StateCommands/` enabling the Assigned → Draft transition
- The command clears `Assignee` and `AssignedDate` when executed
- Only the **creator** of the work order can execute this transition
- Unit tests covering valid transitions, invalid status, and unauthorized user scenarios

## Capabilities

### New Capabilities
- `assigned-to-draft-command`: State command enabling work order transition from Assigned to Draft status, restricted to the work order creator

### Modified Capabilities
<!-- None — this is an additive change within the existing state command framework -->

## Impact

- **New file**: `src/Core/Model/StateCommands/AssignedToDraftCommand.cs`
- **New file**: `src/UnitTests/Core/Model/StateCommands/AssignedToDraftCommandTests.cs`
- **Dependencies**: None — uses only existing Core types (no new NuGet packages)
- **Database**: No schema changes
- **Architecture**: Stays within Core layer; follows existing `StateCommandBase` pattern
