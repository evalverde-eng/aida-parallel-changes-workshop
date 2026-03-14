# Facilitation

## Session objective

Help participants evolve API and data contracts with production-grade discipline.

## Facilitator framing

State this explicitly at the start:

- treat the exercise as real engineering work
- preserve accepted behavior while evolving
- keep changes small and verifiable
- keep tests, docs, and executable HTTP contracts synchronized

## Suggested timeline

- 15 min: context and constraints
- 75 min: guided implementation cycles
- 20 min: debrief and architecture review

## Team setup

- 2-3 participants per group
- rotate driver every 15 minutes
- navigator enforces TDD and non-regression constraints

## Recommended facilitation sequence

1. Run baseline stack (`up`, `smoke`, `test`).
2. Show OpenAPI and current v1 contract.
3. Demonstrate one full red-green-refactor cycle.
4. Show production+test refactor in same cycle.
5. Show a small cohesive commit message with clear intent.

## Phase markers in workshop context

For student workshop commits, phase markers can be used to clarify intent:

- `[expand]`
- `[migrate]`
- `[contract]`

For repository maintenance commits outside student exercises, these markers are optional and not required.

## Coaching checkpoints

- Is compatibility preserved?
- Are tests behavior-focused and readable?
- Are boundaries clean between transport, application, domain, and infrastructure?
- Are docs and `.http` files updated together with code?
- Is `to-do.md` updated without mutating accepted AC?

## Debrief prompts

- Which change reduced delivery risk most?
- Where did coupling pressure appear first?
- Which mutant was hardest to kill and why?
- Which documentation improvement removed most confusion?
