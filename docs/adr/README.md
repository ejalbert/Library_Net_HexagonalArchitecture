# Architecture Decision Records

Use this directory to store Architecture Decision Records (ADRs). Each record captures context, decision, and consequences for significant choices across the Library Management System.

## How to Add an ADR

1. Copy the template below into a new file named `NNNN-short-title.md`, where `NNNN` is a zero-padded sequence number (e.g., `0001-initialize-domain.md`).
2. Fill in each section with clear, concise language.
3. Link the ADR from relevant documentation or pull requests.

```
# {title}

- Status: {proposed | accepted | superseded}
- Deciders: {list of people / roles}
- Date: {YYYY-MM-DD}

## Context

{What is motivating the decision? Which forces are at play?}

## Decision

{What is the outcome of the decision?}

## Consequences

{What becomes easier or harder because of this decision for library staff, patrons, and infrastructure?}
```

Supersede old decisions instead of deleting them so the project history remains traceable.
