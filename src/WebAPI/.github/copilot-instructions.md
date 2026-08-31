# Copilot Instructions

## Project Guidelines
- For this project, treat field length limits as database/API constraints rather than domain business rules; keep DTO validation and EF/database constraints, but do not duplicate length checks in domain entities unless the limit is a true business invariant.
