## MODIFIED Requirements

### Requirement: System prompt instructs title-case formatting for Title
The system prompt in `WorkOrderReformatAgent.ReformatWorkOrderAsync` (`src/UI/Server/WorkOrderReformatAgent.cs`) SHALL instruct the LLM to format the work order title using proper title-case (capitalize the first letter of each significant word), replacing the current instruction that only capitalizes the first letter.

#### Scenario: Title is formatted as title-case
- **GIVEN** a work order with title `"fix the broken pipe in room 201"`
- **WHEN** the system prompt is sent to the LLM
- **THEN** the prompt SHALL instruct the LLM to format the title using proper title-case (e.g., `"Fix the Broken Pipe in Room 201"`)

### Requirement: System prompt instructs description translation to assignee's preferred language
The system prompt SHALL instruct the LLM to translate the work order description into the assignee's preferred language (BCP 47 tag from `WorkOrder.Assignee.PreferredLanguage`), in addition to correcting grammar and punctuation. If no assignee is set, the description SHALL remain in its original language with only grammar and punctuation corrections.

#### Scenario: Description translated when assignee has preferred language
- **GIVEN** a work order with description `"Fix the broken pipe"` and an assignee with `PreferredLanguage = "es-MX"`
- **WHEN** `ReformatWorkOrderAsync` is called
- **THEN** the system prompt SHALL instruct the LLM to translate the description into `es-MX`
- **AND** the user message SHALL include `Preferred Language: es-MX`

#### Scenario: Description not translated when no assignee
- **GIVEN** a work order with description `"Fix the broken pipe"` and no assignee (`Assignee` is `null`)
- **WHEN** `ReformatWorkOrderAsync` is called
- **THEN** the system prompt SHALL instruct the LLM to correct grammar and punctuation only, without translation

### Requirement: User message includes assignee preferred language
The user message constructed in `ReformatWorkOrderAsync` SHALL include a `Preferred Language:` field when the work order has an assignee. When the assignee is null, the preferred language field SHALL be omitted or indicate that no translation is needed.

#### Scenario: User message includes preferred language
- **GIVEN** a work order with `Assignee.PreferredLanguage = "fr-FR"`
- **WHEN** the user message is constructed
- **THEN** the message SHALL contain `Preferred Language: fr-FR`

#### Scenario: User message omits preferred language when no assignee
- **GIVEN** a work order with `Assignee` as `null`
- **WHEN** the user message is constructed
- **THEN** the message SHALL NOT contain a `Preferred Language:` line

### Requirement: XML documentation updated
The XML doc comment on `ReformatWorkOrderAsync` SHALL be updated to reflect the new behavior: title-case formatting for titles and description translation into the assignee's preferred language.

### Requirement: Unit tests for ParseResponse remain valid
The existing `ParseResponse` unit tests in `src/UnitTests/UI.Server/WorkOrderReformatAgentTests.cs` SHALL continue to pass, as `ParseResponse` itself does not change behavior.

### Constraints
- Changes SHALL be limited to `src/UI/Server/WorkOrderReformatAgent.cs` and `src/UnitTests/UI.Server/WorkOrderReformatAgentTests.cs`
- No new NuGet packages SHALL be added
- No new project references SHALL be added
- The `ParseResponse` method signature and behavior SHALL NOT change
- The `ReformatResult` record SHALL NOT change
- Onion architecture rules SHALL be maintained
