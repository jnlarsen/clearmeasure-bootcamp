## Why

Work order titles are entered with inconsistent casing. Some users type lowercase, some mixed case. This makes work order lists look inconsistent and complicates search and display. Forcing titles to uppercase on save ensures uniform formatting across the system.

## What Changes

- The `WorkOrder.Title` property setter in the domain model is modified to automatically convert any assigned value to uppercase
- This enforces the rule at the domain layer, ensuring all persistence paths (SaveDraftCommand and any future commands) produce uppercase titles
- Unit tests verify the uppercase transformation
- Integration tests verify the uppercase value is persisted to the database

## Capabilities

### Modified Capabilities
- `work-order-title`: WorkOrder.Title is now always stored in uppercase, regardless of the casing provided by the caller

## Impact

- **Core**: `WorkOrder.cs` — Title property gains a backing field with uppercase conversion in the setter
- **Tests**: New unit tests for the uppercase behavior; existing tests updated to expect uppercase titles
- **Database**: No schema changes — Title column already supports uppercase strings
- **UI**: No changes — the uppercase conversion happens transparently in the domain model
