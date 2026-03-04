## 1. Domain Model Change

- [ ] 1.1 In `src/Core/Model/WorkOrder.cs`, add a private backing field `_title` and convert the `Title` auto-property to use the backing field with `ToUpper()` in the setter. Follow the same pattern as `Description`/`_description`. Handle null by returning null (do not call ToUpper on null).

## 2. Unit Tests

- [ ] 2.1 In `src/UnitTests/`, add a test class `WorkOrderTitleUppercaseTests` (or add to existing WorkOrder tests) with tests:
  - `Title_WhenSetWithMixedCase_ReturnsUppercase`: Set Title to "Fix Broken Window", assert it equals "FIX BROKEN WINDOW"
  - `Title_WhenSetWithNull_ReturnsNull`: Set Title to null, assert it is null
  - `Title_WhenSetWithEmptyString_ReturnsEmptyString`: Set Title to "", assert it equals ""
- [ ] 2.2 Update any existing unit tests that set WorkOrder.Title with mixed-case values to expect uppercase results

## 3. Integration Tests

- [ ] 3.1 Verify existing integration tests pass with the uppercase change (no new integration tests needed if existing ones cover WorkOrder persistence)

## 4. Validation

- [ ] 4.1 Run `.\privatebuild.ps1` to verify all unit and integration tests pass
