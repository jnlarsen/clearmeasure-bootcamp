## 1. State Command Implementation

- [ ] 1.1 Create `src/Core/Model/StateCommands/AssignedToDraftCommand.cs` following the `AssignedToCancelledCommand` pattern
  - Record extending `StateCommandBase(WorkOrder, Employee)`
  - `GetBeginStatus()` returns `WorkOrderStatus.Assigned`
  - `GetEndStatus()` returns `WorkOrderStatus.Draft`
  - `UserCanExecute()` checks `currentUser == WorkOrder.Creator`
  - `Execute()` clears `WorkOrder.AssignedDate` and `WorkOrder.Assignee`, then calls `base.Execute()`
  - `TransitionVerbPresentTense` = "Unassign", `TransitionVerbPastTense` = "Unassigned"

## 2. Unit Tests

- [ ] 2.1 Create `src/UnitTests/Core/Model/StateCommands/AssignedToDraftCommandTests.cs`
  - Extends `StateCommandBaseTests`
  - Test: `ShouldNotBeValidInWrongStatus` — Draft status, expect `IsValid()` false
  - Test: `ShouldNotBeValidWithWrongEmployee` — Assigned status, non-creator, expect `IsValid()` false
  - Test: `ShouldBeValid` — Assigned status, creator, expect `IsValid()` true
  - Test: `ShouldTransitionStateProperly` — Execute command, verify status is Draft, Assignee is null, AssignedDate is null

## 3. Build Verification

- [ ] 3.1 Run `dotnet build src/ChurchBulletin.sln` and verify no errors
- [ ] 3.2 Run unit tests and verify all pass
