---
description: 'Analyze implementation plans to identify and resolve [Needs Clarity] markers through structured decision-making with multiple choice options, pros/cons analysis, and reasoning.'
tools: ['edit', 'search', 'ProjectMinder/*', 'usages', 'vscodeAPI', 'changes', 'fetch', 'githubRepo', 'todos']
---
# Clarify Implementation Plan

## Primary Directive

Your goal is to analyze the implementation plan `${input:PlanDocument}`, identify all `[Needs Clarity]` markers, and present structured resolution options to the user. Each option must include comprehensive pros/cons analysis and clear reasoning to enable informed decision-making.

**Output Requirement**: Generate a markdown file in the `plan/` folder with filename pattern `clarity-resolution-[plan-name]-[YYYYMMDD].md` containing the complete analysis for persistent reference and easier reading.

## Execution Context

This prompt is designed for human-AI collaboration where the AI provides expert analysis and structured options, but the human makes final decisions. All options must be presented in a standardized, machine-parseable format that allows for automated plan updates once decisions are made.

## Core Requirements

- Parse implementation plan and extract all `[Needs Clarity]` markers with full context
- For each marker, generate 3-5 viable resolution options
- Provide detailed pros/cons analysis for each option with technical reasoning
- Present options in a consistent, structured format
- Allow user to select from options OR provide free-form alternative
- Generate plan updates based on user selections

## Clarity Marker Identification

When analyzing the plan:
1. Search for all occurrences of `[Needs Clarity]` or `[Needs Clarity:` patterns
2. Extract the full context including:
   - Section and phase where marker appears
   - Task/requirement identifier (T###, REQ-###, etc.)
   - Description of what needs clarification
   - Dependencies or related items
3. Categorize markers by type:
   - **Technical Decision**: Architecture, design pattern, technology choice
   - **Requirements Gap**: Missing specifications, incomplete requirements
   - **Dependency Uncertainty**: Unknown availability, version, compatibility
   - **Implementation Approach**: Multiple valid solutions, trade-off decisions
   - **Resource/Data Uncertainty**: Missing information about data structures, APIs, etc.

## Option Generation Standards

For each `[Needs Clarity]` marker, generate options that:

### Must Include:
- **Option ID**: Unique identifier (OPT-001-A, OPT-001-B, etc.)
- **Option Title**: Concise, descriptive name (3-7 words)
- **Description**: Detailed explanation of the approach (2-4 sentences)
- **Pros**: List of advantages with reasoning (3-5 points)
- **Cons**: List of disadvantages with reasoning (3-5 points)
- **Implementation Impact**: Effort estimate, affected files, complexity
- **Dependencies**: Required libraries, tools, or prior decisions
- **Risk Level**: Low, Medium, High with justification
- **Recommendation**: AI's suggested option with rationale

### Option Quality Criteria:
- Options must be **mutually exclusive** - selecting one precludes others
- Options must be **technically viable** - can be implemented with available resources
- Options must be **sufficiently different** - not minor variations of same approach
- At least one option should be **conservative/safe** (lower risk)
- At least one option should be **optimal/ideal** (best long-term solution)
- Include **pragmatic/compromise** options where appropriate

## Response Format Structure

Present clarity resolutions in the following standardized format:

```markdown
# Implementation Plan Clarity Resolution

**Plan**: [Plan file name]
**Date**: [Current date]
**Total Clarity Markers Found**: [Count]

---

## Clarity Marker #1: [Category] - [Task/Requirement ID]

**Context**: [Section] > [Phase] > [Task Description]

**Needs Clarity**: [Exact text from marker]

**Background**: [Additional context from surrounding text, dependencies, or related requirements]

### Option A: [Option Title]

**Description**: [Detailed explanation of this approach]

**Pros**:
- ✅ **[Benefit Category]**: [Specific advantage with reasoning]
- ✅ **[Benefit Category]**: [Specific advantage with reasoning]
- ✅ **[Benefit Category]**: [Specific advantage with reasoning]

**Cons**:
- ❌ **[Drawback Category]**: [Specific disadvantage with reasoning]
- ❌ **[Drawback Category]**: [Specific disadvantage with reasoning]

**Implementation Impact**:
- **Effort**: [Low/Medium/High] - [Time estimate]
- **Complexity**: [Low/Medium/High] - [Explanation]
- **Affected Files**: [Count and key files]
- **Breaking Changes**: [Yes/No - Details]

**Dependencies**:
- [Dependency 1]
- [Dependency 2]

**Risk Level**: [Low/Medium/High] - [Justification]

---

### Option B: [Option Title]

[Same structure as Option A]

---

### Option C: [Option Title]

[Same structure as Option A]

---

### 🤖 AI Recommendation

**Recommended Option**: Option [A/B/C] - [Option Title]

**Reasoning**: [2-4 sentences explaining why this option is recommended based on project context, constraints, and requirements]

**Trade-offs Accepted**: [What disadvantages are acceptable and why]

---

### 📝 Your Decision

**Select an option by replying with one of the following:**

- **Option A** - [Option Title]
- **Option B** - [Option Title]
- **Option C** - [Option Title]
- **Custom Solution** - [Provide your alternative approach]

**Response Format**:
```
CLARITY-001: [Option A/B/C or "Custom"]
[If Custom, provide detailed description]
```

---

[Repeat for each clarity marker]

---

## Summary of Decisions

Once all clarity markers are resolved, the AI will generate a summary:

| Marker ID | Category | Decision | Impact |
|-----------|----------|----------|--------|
| CLARITY-001 | Technical Decision | Option B | Medium effort, 5 files |
| CLARITY-002 | Requirements Gap | Option A | Low effort, 2 files |
| CLARITY-003 | Implementation Approach | Custom | High effort, details below |

### Custom Solutions

**CLARITY-003**: [User's custom solution details]

### Next Steps

1. Update implementation plan with resolved decisions
2. Regenerate affected tasks with specific details
3. Update acceptance criteria to reflect decisions
4. Review dependencies and adjust as needed
5. Update effort estimates based on decisions
```

## User Response Handling

When the user provides decisions, parse responses in this format:

```
CLARITY-001: Option B
CLARITY-002: Option A
CLARITY-003: Custom
For CLARITY-003, I want to use a hybrid approach that combines Option A's caching with Option C's API design but implements it as a middleware component.
```

### Response Validation:
- Verify all CLARITY-### IDs match identified markers
- Validate Option selections (A, B, C, etc.) are valid for that marker
- For "Custom" responses, ensure sufficient detail is provided
- Flag any missing or ambiguous responses

## Plan Update Process

After collecting user decisions:

1. **Generate Output File**: Create markdown file at `plan/clarity-resolution-[plan-name]-[YYYYMMDD].md` with complete analysis
2. **Remove Clarity Markers**: Replace `[Needs Clarity: ...]` with concrete decisions
3. **Update Task Descriptions**: Add specific implementation details based on selected options
4. **Update Dependencies**: Add new dependencies from selected options
5. **Update Files**: Add affected files from implementation impact
6. **Update Tests**: Add specific test requirements based on decisions
7. **Update Risks**: Add risks from selected options
8. **Update Acceptance Criteria**: Make criteria more specific based on decisions
9. **Add Decision Records**: Document decisions in a new section for traceability

### Output File Structure:

The generated markdown file should include:
- Complete analysis of all clarity markers
- All options with pros/cons
- User decisions and rationale
- Decision records
- Summary table
- Next steps

Filename example: `plan/clarity-resolution-systemobjectbo-repository-pattern-20251112.md`

### Decision Record Format:
```markdown
## 9. Decision Records

### DR-001: [Task ID] - [Decision Title]

**Date**: [YYYY-MM-DD]
**Decided By**: [User name/role]
**Context**: [Original clarity marker description]

**Options Considered**:
- Option A: [Title]
- Option B: [Title]
- Option C: [Title]

**Decision**: Option [A/B/C] - [Title]

**Rationale**: [User's reasoning if provided, or rationale from option description]

**Consequences**:
- [Positive consequence 1]
- [Negative consequence 1]
- [Mitigation strategy for negative consequence]
```

## Analysis Guidelines

When analyzing options:

### Technical Decisions:
- Consider project architecture patterns (Repository, CQRS, etc.)
- Evaluate alignment with existing codebase patterns
- Assess learning curve for development team
- Consider long-term maintainability

### Performance Considerations:
- Benchmark implications where possible
- Consider scalability requirements
- Evaluate resource usage (memory, CPU, I/O)
- Assess caching strategies

### Security Implications:
- Evaluate authentication/authorization impacts
- Consider data protection requirements
- Assess vulnerability surface area
- Review compliance requirements

### Team/Process Impact:
- Consider team expertise and skill sets
- Evaluate testing complexity
- Assess documentation requirements
- Consider deployment complexity

## Example Pro/Con Reasoning

**Good Examples**:
- ✅ **Type Safety**: Using generic SystemObjectBO<TEntity> provides compile-time type checking, reducing runtime errors by eliminating reflection-based type resolution
- ❌ **Migration Effort**: Requires updating 50+ call sites across codebase; estimated 8-16 hours of developer time

**Poor Examples** (avoid):
- ✅ **Better** - Too vague, doesn't explain why
- ❌ **More work** - Not specific about what work or how much

## Edge Cases and Special Handling

### Multiple Interdependent Clarity Markers:
When markers depend on each other:
1. Present them in dependency order
2. Clearly state dependencies: "This decision depends on CLARITY-003"
3. Offer to resolve dependencies first, then revisit

### Conflicting Options:
When options for different markers conflict:
1. Identify the conflict explicitly
2. Suggest resolving higher-priority marker first
3. Adjust options for dependent markers based on first decision

### Insufficient Context:
If a clarity marker lacks sufficient context to generate options:
1. State what additional information is needed
2. Suggest where to find the information (code search, documentation, etc.)
3. Offer to help gather context before presenting options

## Output Quality Checklist

Before presenting options to user, verify:

- [ ] All `[Needs Clarity]` markers have been identified
- [ ] Each marker has 3-5 distinct, viable options
- [ ] Each option has at least 3 pros and 2 cons with reasoning
- [ ] Implementation impact is quantified (effort, files, complexity)
- [ ] At least one conservative and one optimal option per marker
- [ ] AI recommendation is provided with clear rationale
- [ ] Response format is clearly explained
- [ ] Dependencies between options are identified
- [ ] Risk levels are justified
- [ ] Technical terminology is accurate and consistent with project standards

## Automation Support

Generate machine-readable decision summary for automated plan updates:

```json
{
  "plan_file": "refactor-systemobjectbo-repository-pattern-1.md",
  "analysis_date": "2025-11-06T10:30:00Z",
  "clarity_markers": [
    {
      "id": "CLARITY-001",
      "marker_text": "[Needs Clarity: Entity types determination]",
      "task_id": "T008",
      "section": "Implementation Phase 2",
      "category": "Requirements Gap",
      "options": [
        {
          "id": "OPT-001-A",
          "title": "Entity Type Discovery",
          "selected": true,
          "impact": {
            "effort": "Medium",
            "hours": 4,
            "files_affected": 3,
            "breaking_changes": false
          }
        }
      ],
      "decision": "Option A",
      "rationale": "User selected conservative approach to discover entity types through codebase analysis",
      "resolved_text": "Create repository interfaces for each entity type identified through codebase analysis (T002)"
    }
  ],
  "custom_solutions": [],
  "next_actions": [
    "Update task T008 with specific implementation details",
    "Remove [Needs Clarity] marker from task description",
    "Add decision record DR-001"
  ]
}
```

## Workflow Example

1. **User invokes prompt**: Points to implementation plan file
2. **AI analyzes plan**: Identifies all clarity markers with context
3. **AI creates output file**: Generates `plan/clarity-resolution-[plan-name]-[YYYYMMDD].md` with complete analysis
4. **AI presents options**: Structured format with pros/cons for each marker (both in response and output file)
5. **User reviews options**: Reads markdown file or inline response to consider recommendations and project context
6. **User provides decisions**: Using standardized response format
7. **AI validates responses**: Ensures all markers addressed
8. **AI updates plan**: Removes markers, adds details, creates decision records
9. **AI updates output file**: Adds user decisions and final resolution status
10. **AI presents updated plan**: Shows changes and requests final confirmation
11. **User approves**: Plan is ready for execution

## Success Criteria

A successful clarity resolution session results in:

- All `[Needs Clarity]` markers resolved with concrete decisions
- Markdown output file created in `plan/` folder with complete analysis and decisions
- Implementation plan updated with specific, actionable details
- Decision records created for traceability
- No ambiguity remaining in task descriptions
- Updated effort estimates and dependencies
- Plan ready for autonomous or human execution

---

## Instructions for Use

**To clarify an implementation plan:**

1. Invoke this prompt with the plan document as a parameter
2. Review presented options in the generated markdown file (located in `plan/` folder)
3. Provide decisions in the standardized format
4. Review and approve updated plan

**The prompt will automatically:**
- Read the plan document specified in `${input:PlanDocument}`
- Parse and extract all `[Needs Clarity]` markers
- Analyze context and generate resolution options
- Create markdown output file in `plan/clarity-resolution-[plan-name]-[YYYYMMDD].md`
- Present structured decision choices

**Example invocation:**
```
Use letsclarify prompt with PlanDocument = plan/refactor-systemobjectbo-repository-pattern-1.md
```

**Output file location:**
```
plan/clarity-resolution-refactor-systemobjectbo-repository-pattern-20251112.md
```
