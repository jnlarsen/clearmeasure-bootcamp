## 1. Create CompleteToInProgressCommand

- [ ] 1.1 Create `src/Core/Model/StateCommands/CompleteToInProgressCommand.cs` as a record inheriting from `StateCommandBase(WorkOrder, CurrentUser)`
- [ ] 1.2 Set `Name` constant to `"Reopen"`, `TransitionVerbPresentTense` to `Name`, `TransitionVerbPastTense` to `"Reopened"`
- [ ] 1.3 Implement `GetBeginStatus()` returning `WorkOrderStatus.Complete`
- [ ] 1.4 Implement `GetEndStatus()` returning `WorkOrderStatus.InProgress`
- [ ] 1.5 Implement `UserCanExecute()` returning `currentUser == WorkOrder.Assignee`
- [ ] 1.6 Override `Execute()` to clear `WorkOrder.CompletedDate = null` before calling `base.Execute(context)`

## 2. Unit Tests

- [ ] 2.1 Create `src/UnitTests/Model/StateCommands/CompleteToInProgressCommandTester.cs` using NUnit + Shouldly
- [ ] 2.2 Test `IsValid_WhenStatusIsCompleteAndUserIsAssignee_ReturnsTrue`
- [ ] 2.3 Test `IsValid_WhenStatusIsCompleteAndUserIsNotAssignee_ReturnsFalse`
- [ ] 2.4 Test `IsValid_WhenStatusIsNotComplete_ReturnsFalse`
- [ ] 2.5 Test `Execute_ClearsCompletedDate`
- [ ] 2.6 Test `Execute_SetsStatusToInProgress`

## 3. Documentation

- [ ] 3.1 Update `arch/arch-c4-class-domain-model.md` to add the Complete → InProgress transition arrow and `CompleteToInProgressCommand` class to the diagram
