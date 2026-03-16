## Overview
Add a Payments domain to an existing ASP.NET Core 9 Minimal API solution following Clean Architecture with CQRS, Repository pattern, and EF Core split-context migrations.

## Solution Structure
Four existing projects: `Ai.Courses.Api`, `Ai.Courses.Logic`, `Ai.Courses.Data`, `Ai.Courses.Migrations`.

## Domain Model

### Entities
- **TypeEntity** — a spending category scoped per user (`UserId`, `Name`). Unique constraint on `(UserId, Name)`.
- **ItemEntity** — a day's entry for a user (`UserId`, `Date`). Unique constraint on `(UserId, Date)`. Has a collection of Payments. Cascade-deletes its Payments.
- **PaymentEntity** — a single payment within an Item (`ItemId`, `TypeId`, `PlannedAmount?`, `SpentAmount?`). FK to Type with Restrict delete. At least one of `PlannedAmount` or `SpentAmount` must be provided.

## Business Rules
- `UserId` is always taken from the Bearer token claims — never from the request body.
- POST upserts by date: find or create the `ItemEntity` for the given date, then add a new `PaymentEntity`.
- If the `TypeName` from the request does not exist for the user, create it automatically.
- Deleting a payment that leaves its parent Item with no remaining payments also deletes the Item.
- `PlannedAmount` is nullable. `SpentAmount` is nullable. At least one must be provided and have a valid positive value.

## API Endpoints (`/api/v1/items`)
- `GET /` — list all items for the current user, ordered by date, with payments and type name.
- `GET /{id}` — get a single item by id for the current user.
- `POST /` — add a payment to a day (upsert item by date). Body: `{ date, typeName, plannedAmount?, spentAmount? }`.
- `PUT /{id}/payments/{paymentId}` — update a payment. Body: `{ typeName, plannedAmount?, spentAmount? }`.
- `DELETE /{id}/payments/{paymentId}` — delete a payment (and item if last payment).
- `DELETE /{id}` — delete an item and all its payments.

All endpoints require authorization.

## Databases
Two separate SQL Server databases:
- `users` — connection string key `UsersConnection`.
- `payments` — connection string key `PaymentsConnection`.

Use the existing EF Core split-context migration pattern (migration-only subcontexts in the Migrations project).

## Validation (FluentValidation)
- Add: `Date` required, `TypeName` required max 100 chars, at least one of `PlannedAmount`/`SpentAmount` provided with valid values.
- Update: same as Add plus `PaymentId` and `ItemId` required.
- Validation is invoked manually inside endpoint handlers — not via pipeline behavior.
