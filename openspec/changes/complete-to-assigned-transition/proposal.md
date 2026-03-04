## Why

Once a work order reaches Complete status, no further transitions are available. The creator cannot reassign a completed work order to a new or the same assignee. This limits workflow flexibility — completed work orders that need rework or reassignment must be manually recreated.

## What Changes

- New state command `CompleteToAssignedCommand` in `src/Core/Model/StateCommands/` enabling the Complete → Assigned transition
- The command verifies the current user is the **creator** of the work order before allowing the transition
- On execution: sets the new assignee, updates `AssignedDate` to the current date, and clears `CompletedDate`
- Registration of the new command in `StateCommandList.GetAllStateCommands()`
- Unit tests covering valid transitions, invalid user scenarios, and property mutations
- Updated workflow documentation in `arch/`

## Capabilities

### New Capabilities
- `complete-to-assigned`: State transition allowing the creator of a completed work order to reassign it, moving status from Complete back to Assigned

### Modified Capabilities
- `state-command-list`: Updated to include the new `CompleteToAssignedCommand` in the list of all available state commands

## Impact

- **Core project**: New `CompleteToAssignedCommand.cs` file in `src/Core/Model/StateCommands/`
- **Core project**: Updated `StateCommandList` in `src/Core/Services/Impl/` to register the new command
- **Unit tests**: New test class for `CompleteToAssignedCommand` and updated `StateCommandListTests`
- **Architecture docs**: Updated state diagram in `arch/arch-state-workorder.md`
- **Database**: No schema changes
- **Dependencies**: No new NuGet packages
