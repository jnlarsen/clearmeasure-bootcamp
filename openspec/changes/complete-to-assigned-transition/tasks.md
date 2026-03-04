## 1. State Command Implementation

- [x] 1.1 Create `src/Core/Model/StateCommands/CompleteToAssignedCommand.cs` extending `StateCommandBase` with begin status `Complete`, end status `Assigned`, verb `Reassign`/`Reassigned`, creator authorization check, and Execute override that sets `AssignedDate` and clears `CompletedDate`
- [x] 1.2 Register `CompleteToAssignedCommand` in `src/Core/Services/Impl/StateCommandList.cs` by adding it to the `GetAllStateCommands()` method after `AssignedToCancelledCommand`

## 2. Unit Tests

- [x] 2.1 Create `src/UnitTests/Core/Model/StateCommands/CompleteToAssignedCommandTests.cs` with tests:
  - `IsValid_CreatorWithCompleteStatus_ReturnsTrue`
  - `IsValid_NonCreatorWithCompleteStatus_ReturnsFalse`
  - `IsValid_CreatorWithNonCompleteStatus_ReturnsFalse`
  - `Execute_SetsAssignedDateToCurrentDateTime`
  - `Execute_ClearsCompletedDate`
  - `Execute_ChangesStatusToAssigned`
- [x] 2.2 Update `src/UnitTests/Core/Services/StateCommandListTests.cs` to expect 7 commands and include `CompleteToAssignedCommand` at index 6

## 3. Documentation

- [x] 3.1 Update `arch/arch-state-workorder.md` to include the Complete → Assigned (Reassign) transition in the state diagram

## 4. Acceptance Test Fix

- [x] 4.1 Fix `ShouldShowSpeakButtonsOnReadOnlyWorkOrder` in `WorkOrderSpeechTests.cs` — completed work orders are no longer read-only for the creator (who can Reassign); updated test to switch to a non-creator user before asserting read-only state
- [x] 4.2 Make `CreateTestUser` protected in `AcceptanceTestBase.cs` so subclass tests can create additional users for multi-user scenarios
