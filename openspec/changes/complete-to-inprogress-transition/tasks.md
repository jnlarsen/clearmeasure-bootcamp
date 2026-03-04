## 1. Create CompleteToInProgressCommand

- [x] 1.1 Create `src/Core/Model/StateCommands/CompleteToInProgressCommand.cs` as a record inheriting from `StateCommandBase(WorkOrder, CurrentUser)`
- [x] 1.2 Set `Name` constant to `"Reopen"`, `TransitionVerbPresentTense` to `Name`, `TransitionVerbPastTense` to `"Reopened"`
- [x] 1.3 Implement `GetBeginStatus()` returning `WorkOrderStatus.Complete`
- [x] 1.4 Implement `GetEndStatus()` returning `WorkOrderStatus.InProgress`
- [x] 1.5 Implement `UserCanExecute()` returning `currentUser == WorkOrder.Assignee`
- [x] 1.6 Override `Execute()` to clear `WorkOrder.CompletedDate = null` before calling `base.Execute(context)`

## 2. Unit Tests

- [x] 2.1 Create `src/UnitTests/Core/Model/StateCommands/CompleteToInProgressCommandTests.cs` using NUnit + Shouldly
- [x] 2.2 Test `IsValid_WhenStatusIsCompleteAndUserIsAssignee_ReturnsTrue`
- [x] 2.3 Test `IsValid_WhenStatusIsCompleteAndUserIsNotAssignee_ReturnsFalse`
- [x] 2.4 Test `IsValid_WhenStatusIsNotComplete_ReturnsFalse`
- [x] 2.5 Test `Execute_ClearsCompletedDate`
- [x] 2.6 Test `Execute_SetsStatusToInProgress`

## 3. Documentation

- [x] 3.1 Update `arch/arch-c4-class-domain-model.md` to add the Complete → InProgress transition arrow and `CompleteToInProgressCommand` class to the diagram

## 4. Acceptance Test Fix

- [ ] 4.1 Update `WorkOrderSpeechTests.ShouldShowSpeakButtonsOnReadOnlyWorkOrder` — with the new Reopen command, a completed work order is no longer read-only for the assignee; update test to verify speak buttons are visible alongside the Reopen command button instead of the ReadOnlyMessage
