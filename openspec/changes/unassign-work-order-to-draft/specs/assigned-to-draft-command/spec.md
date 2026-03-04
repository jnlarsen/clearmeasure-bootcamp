## ADDED Requirements

### Requirement: AssignedToDraftCommand transitions work order from Assigned to Draft
The system SHALL provide an `AssignedToDraftCommand` state command that transitions a work order from Assigned status to Draft status.

#### Scenario: Valid transition by creator
- **GIVEN** a work order with status Assigned
- **AND** the current user is the creator of the work order
- **WHEN** the `AssignedToDraftCommand` is executed
- **THEN** the work order status changes to Draft
- **AND** the Assignee is cleared (set to null)
- **AND** the AssignedDate is cleared (set to null)

#### Scenario: Command is valid for creator of assigned work order
- **GIVEN** a work order with status Assigned
- **AND** the current user is the creator of the work order
- **WHEN** `IsValid()` is called on `AssignedToDraftCommand`
- **THEN** it returns true

#### Scenario: Command is invalid for wrong status
- **GIVEN** a work order with status Draft (or any status other than Assigned)
- **WHEN** `IsValid()` is called on `AssignedToDraftCommand`
- **THEN** it returns false

#### Scenario: Command is invalid for non-creator
- **GIVEN** a work order with status Assigned
- **AND** the current user is NOT the creator of the work order
- **WHEN** `IsValid()` is called on `AssignedToDraftCommand`
- **THEN** it returns false

### Requirement: AssignedToDraftCommand uses "Unassign" as transition verb
The system SHALL use "Unassign" as the present-tense transition verb and "Unassigned" as the past-tense verb for UI and logging purposes.
