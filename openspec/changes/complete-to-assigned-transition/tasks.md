## 1. State Command Implementation

- [ ] 1.1 Create `src/Core/Model/StateCommands/CompleteToAssignedCommand.cs` extending `StateCommandBase` with begin status `Complete`, end status `Assigned`, verb `Reassign`/`Reassigned`, creator authorization check, and Execute override that sets `AssignedDate` and clears `CompletedDate`
- [ ] 1.2 Register `CompleteToAssignedCommand` in `src/Core/Services/Impl/StateCommandList.cs` by adding it to the `GetAllStateCommands()` method after `AssignedToCancelledCommand`

## 2. Unit Tests

- [ ] 2.1 Create `src/UnitTests/Core/Model/StateCommands/CompleteToAssignedCommandTests.cs` with tests:
  - `IsValid_CreatorWithCompleteStatus_ReturnsTrue`
  - `IsValid_NonCreatorWithCompleteStatus_ReturnsFalse`
  - `IsValid_CreatorWithNonCompleteStatus_ReturnsFalse`
  - `Execute_SetsAssignedDateToCurrentDateTime`
  - `Execute_ClearsCompletedDate`
  - `Execute_ChangesStatusToAssigned`
- [ ] 2.2 Update `src/UnitTests/Core/Services/StateCommandListTests.cs` to expect 7 commands and include `CompleteToAssignedCommand` at index 6

## 3. Documentation

- [ ] 3.1 Update `arch/arch-state-workorder.md` to include the Complete → Assigned (Reassign) transition in the state diagram
