---
description: 'Create a new implementation plan file for new features, refactoring existing code or upgrading packages, design, architecture or infrastructure.'
tools: ['edit/createFile', 'edit/createDirectory', 'edit/editFiles', 'search', 'runCommands', 'runTasks', 'Context7/*', 'ProjectMinder/*', 'usages', 'vscodeAPI', 'problems', 'changes', 'testFailure', 'fetch', 'githubRepo', 'extensions', 'todos']

---
# Create Implementation Plan

## Primary Directive

Your goal is to create a new implementation plan file for `${input:PlanPurpose}`. Your output must be machine-readable, deterministic, and structured for autonomous execution by other AI systems or humans.

## Execution Context

This prompt is designed for AI-to-AI communication and automated processing. All instructions must be interpreted literally and executed systematically without human interpretation or clarification.

## Core Requirements

- Generate implementation plans that are fully executable by AI agents or humans
- Use deterministic language with zero ambiguity
- Structure all content for automated parsing and execution
- Ensure complete self-containment with no external dependencies for understanding

## Plan Structure Requirements

Plans must consist of discrete, atomic phases containing executable tasks. Each phase must be independently processable by AI agents or humans without cross-phase dependencies unless explicitly declared.

## Phase Architecture

- Each phase must have measurable completion criteria
- Tasks within phases must be executable in parallel unless dependencies are specified
- All task descriptions must include specific file paths, function names, and exact implementation details
- No task should require human interpretation or decision-making

## AI-Optimized Implementation Standards

- Use explicit, unambiguous language with zero interpretation required
- Structure all content as machine-parseable formats (tables, lists, structured data)
- Include specific file paths, line numbers, and exact code references where applicable
- Define all variables, constants, and configuration values explicitly
- Provide complete context within each task description
- Use standardized prefixes for all identifiers (REQ-, AC-, etc.)
- **Use T### format for task identifiers** (T001, T002, T003, etc.) to enable range-based execution
- Include validation criteria that can be automatically verified
- **Mark all uncertain, ambiguous, or incomplete items with `[Needs Clarity]`** - This signals that human input or additional research is required before execution
- **Include acceptance criteria (AC-) for all phases and critical tasks** - Use Given-When-Then format or testable assertions

## Output File Specifications

- Save implementation plan files in `/plan/` directory
- Use naming convention: `[purpose]-[component]-[version].md`
- Purpose prefixes: `upgrade|refactor|feature|data|infrastructure|process|architecture|design`
- Example: `upgrade-system-command-4.md`, `feature-auth-module-1.md`
- File must be valid Markdown with proper front matter structure

## Mandatory Template Structure

All implementation plans must strictly adhere to the following template. Each section is required and must be populated with specific, actionable content. AI agents must validate template compliance before execution.

## Template Validation Rules

- All front matter fields must be present and properly formatted
- All section headers must match exactly (case-sensitive)
- All identifier prefixes must follow the specified format
- Tables must include all required columns
- No placeholder text may remain in the final output

## Clarity Markers

When generating implementation plans, any item requiring clarification, additional information, or decision-making must be explicitly marked with `[Needs Clarity]`. This includes:

- Tasks with incomplete requirements or specifications
- Dependencies that are uncertain or unavailable
- Technical decisions requiring stakeholder input
- Requirements that conflict or are ambiguous
- Design patterns where multiple approaches are equally valid without clear selection criteria

Example: `TASK-003: Implement authentication flow [Needs Clarity: OAuth provider selection pending]`

## Acceptance Criteria Standards

All phases and critical tasks must include acceptance criteria using industry-standard formats. Acceptance criteria must be:

- **Testable**: Can be verified programmatically or through manual testing
- **Specific**: Clear, measurable outcomes with no ambiguity
- **Complete**: Covers all aspects of the requirement
- **Understandable**: Both humans and AI models can interpret and validate

**Preferred Format**: Given-When-Then (Behavior-Driven Development style)
- **Given**: Initial context or preconditions
- **When**: Action or event that occurs
- **Then**: Expected outcome or result

**Alternative Format**: Direct assertions with validation criteria
- Use verifiable statements (e.g., "Function returns 200 status code")
- Include measurable criteria (e.g., "Response time < 100ms")
- Specify exact expected behaviors

## Status

The status of the implementation plan must be clearly defined in the front matter and must reflect the current state of the plan. The status can be one of the following (status_color in brackets): `Completed` (bright green badge), `In progress` (yellow badge), `Planned` (blue badge), `Deprecated` (red badge), or `On Hold` (orange badge). It should also be displayed as a badge in the introduction section.

```md
---
goal: [Concise Title Describing the Package Implementation Plan's Goal]
version: [Optional: e.g., 1.0, Date]
date_created: [YYYY-MM-DD]
last_updated: [Optional: YYYY-MM-DD]
owner: [Optional: Team/Individual responsible for this spec]
status: 'Completed'|'In progress'|'Planned'|'Deprecated'|'On Hold'
tags: [Optional: List of relevant tags or categories, e.g., `feature`, `upgrade`, `chore`, `architecture`, `migration`, `bug` etc]
---

# Introduction

![Status: <status>](https://img.shields.io/badge/status-<status>-<status_color>)

[A short concise introduction to the plan and the goal it is intended to achieve.]

## 1. Requirements & Constraints

[Explicitly list all requirements & constraints that affect the plan and constrain how it is implemented. Use bullet points or tables for clarity.]

- **REQ-001**: Requirement 1
- **SEC-001**: Security Requirement 1
- **[3 LETTERS]-001**: Other Requirement 1
- **CON-001**: Constraint 1
- **GUD-001**: Guideline 1
- **PAT-001**: Pattern to follow 1

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: [Describe the goal of this phase, e.g., "Implement feature X", "Refactor module Y", etc.]

**Acceptance Criteria:**
- AC-001: **Given** [initial state/context], **When** [action occurs], **Then** [expected outcome]
- AC-002: **Given** [initial state/context], **When** [action occurs], **Then** [expected outcome]

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| T001 | Description of task 1 | ✅ | 2025-04-25 |
| T002 | Description of task 2 | |  |
| T003 | Description of task 3 [Needs Clarity: Specific implementation approach TBD] | |  |

### Implementation Phase 2

- GOAL-002: [Describe the goal of this phase, e.g., "Implement feature X", "Refactor module Y", etc.]

**Acceptance Criteria:**
- AC-003: **Given** [initial state/context], **When** [action occurs], **Then** [expected outcome]
- AC-004: [Alternative format] Function `validateInput()` returns `true` for valid inputs and `false` for invalid inputs with error codes 400-499

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-004 | Description of task 4 | |  |
| TASK-005 | Description of task 5 | |  |
| TASK-006 | Description of task 6 | |  |

## 3. Alternatives

[A bullet point list of any alternative approaches that were considered and why they were not chosen. This helps to provide context and rationale for the chosen approach.]

- **ALT-001**: Alternative approach 1
- **ALT-002**: Alternative approach 2

## 4. Dependencies

[List any dependencies that need to be addressed, such as libraries, frameworks, or other components that the plan relies on.]

- **DEP-001**: Dependency 1
- **DEP-002**: Dependency 2

## 5. Files

[List the files that will be affected by the feature or refactoring task.]

- **FILE-001**: Description of file 1
- **FILE-002**: Description of file 2

## 6. Testing

[List the tests that need to be implemented to verify the feature or refactoring task. Link tests to acceptance criteria where applicable.]

- **TEST-001**: Description of test 1 (Validates AC-001)
- **TEST-002**: Description of test 2 (Validates AC-002, AC-003)
- **TEST-003**: Description of test 3 [Needs Clarity: Test data requirements undefined]

## 7. Risks & Assumptions

[List any risks or assumptions related to the implementation of the plan.]

- **RISK-001**: Risk 1
- **ASSUMPTION-001**: Assumption 1

## 8. Related Specifications / Further Reading

[Link to related spec 1]
[Link to relevant external documentation]
```
