# Customer/Lead Image Upload Feature

## Task
Implement a feature that allows users to upload images to customer or lead profiles. The backend should enforce a maximum of 10 images per customer/lead, with images stored as Base64-encoded strings in the database. The frontend carousel/gallery is a bonus task.

Estimated time to complete: 1 hours

### Requirements

#### Backend (C#)
- Create database model to allow storing up to 10 images per customer/lead. (Also create model for customer/lead).
- Limit of 10 images per customer/lead profile (cannot add the 11th. Think of a cleaver way to handle this).
- API endpoints for:
  - Uploading one or more images to a customer/lead.
  - Listing all images for a customer/lead.
  - Deleting an image from a customer/lead.

---

### Additional Notes
- Focus on code clarity, maintainability, and user experience.
- The feature should be easy to understand and use for end users.
- If you use any frameworks or libraries, list them and explain your choices.
- You may provide mockups or screenshots if it helps demonstrate your solution.
- If you have suggestions for improvements or better ideas for the implementation, please include them. Creative or optimized solutions are very welcome.

---

### Bonus task, if time is still time left.
#### Frontend (Any framework of your choice)
- Implement an image upload UI for customer/lead profiles.
- Show uploaded images in a responsive carousel/gallery component.
- Users should be able to:
  - Add (upload) images (with limit enforced).
  - View images in a carousel.
  - Delete images.
- The UI/UX should be intuitive, clean, and integrate naturally into the existing application.

---

### Acceptance Criteria
- No customer/lead can have more than 10 images.
- Images are stored as Base64 strings.
- The API endpoints must work as described.
