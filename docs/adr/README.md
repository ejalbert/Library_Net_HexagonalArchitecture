# Architecture Decision Records

Use this directory to capture significant architectural choices (e.g., persistence technology, hosting model, module conventions). No ADRs have been recorded yet—add one whenever a decision materially impacts the system.

## How to Add an ADR

1. Copy the template below into a new Markdown file.
2. Name the file `NNNN-short-title.md` where `NNNN` is a zero-padded sequence (e.g., `0001-select-mongo-adapter.md`).
3. Fill in the sections and link to the ADR from relevant docs or pull requests.

```
# {title}

- Status: {proposed | accepted | superseded}
- Deciders: {people / roles}
- Date: {YYYY-MM-DD}

## Context

{What is motivating the decision?}

## Decision

{What was decided and why?}

## Consequences

{What becomes easier or harder because of this choice?}
```

Supersede old ADRs instead of deleting them so project history stays traceable.
