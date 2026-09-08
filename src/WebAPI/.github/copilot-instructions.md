# Copilot Instructions

## Project Guidelines
- For this project, treat field length limits as database/API constraints rather than domain business rules; keep DTO validation and EF/database constraints, but do not duplicate length checks in domain entities unless the limit is a true business invariant.
- For book cover images, prefer storing a separate small size thumbnail with a fixed width of 160 pixels, preserving the original image aspect ratio for adaptive height, and returning thumbnails for book lists/summaries while using the original image for book details.
