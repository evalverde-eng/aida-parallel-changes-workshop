# ADR-005: Retire workshop replay scripts from baseline tooling

## Status

Accepted

## Context

The repository now operates with a single long-lived branch (`main`) and strict quality gates executed through `scripts/verify.*`.

`scripts/workshop-replay.*` were historical navigation helpers from a branch-heavy workshop flow. In the current baseline they add maintenance overhead, create documentation noise, and duplicate educational guidance that is already covered through ADRs and commit history review.

## Decision drivers

- Reduce script surface area to operational essentials.
- Keep script policy aligned with the real maintenance workflow.
- Avoid contradictory guidance between branch model and tooling.
- Preserve workshop clarity through documentation and commit narrative instead of replay utilities.

## Decision

Remove `scripts/workshop-replay.sh` and `scripts/workshop-replay.ps1` from the baseline repository.

## Consequences

- Required script set is reduced to operational and quality gates only.
- Documentation and to-do planning must no longer reference replay scripts.
- Workshop storytelling remains supported by commit history, facilitation guidance, and ADR narrative.

## Risks

- Facilitators accustomed to replay scripts lose a familiar navigation shortcut.

## Mitigations

- Keep facilitation docs explicit about phase explanation workflow.
- Keep commits small and intention-revealing to preserve narrative readability.
- Keep ADR-003 marked as superseded historical context.
