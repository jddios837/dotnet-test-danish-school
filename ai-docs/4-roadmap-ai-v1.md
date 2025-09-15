# Customer/Lead Image Upload Feature - Implementation Roadmap

This roadmap provides a step-by-step implementation plan for the Customer/Lead Image Upload feature using Clean Architecture principles. The implementation follows the dependency rule: Domain � Data � API.

## <� Project Overview

**Objective**: Implement a Customer/Lead Image Upload system that allows users to upload, view, and manage up to 10 images per customer/lead profile using Base64 encoding and JSON storage.

**Architecture**: Clean Architecture with .NET 8
**Storage**: JSON files (designed for future database migration)
**Image Limit**: Maximum 10 images per customer/lead
**Image Storage**: Base64-encoded strings

---

## =� Implementation Phases

### Phase 1: Domain Layer Foundation
*Core business logic and entities - no external dependencies*

#### 1.1 Project Structure Setup
- [x] **STEP 1.1.1**: Create Domain project structure
  - [x] Create `src/Malte.Clean.Domain` folder
  - [x] Add `Malte.Clean.Domain.csproj` file
  - [x] Configure project with .NET 8 and nullable reference types
  - [x] Update solution file to include Domain project

#### 1.2 Core Entities
- [x] **STEP 1.2.1**: Create Customer entity
  - [x] Create `Entities/Customer.cs`
  - [x] Implement properties: Id, Name, Email, PhoneNumber, Address, Images, CreatedAt, UpdatedAt
  - [x] Add proper nullable annotations
  - [x] Initialize Images collection in constructor

- [x] **STEP 1.2.2**: Create CustomerImage entity
  - [x] Create `Entities/CustomerImage.cs`
  - [x] Implement properties: Id, CustomerId, Base64Data, FileName, ContentType, SizeInBytes, UploadedAt
  - [x] Add proper nullable annotations
  - [x] Add validation attributes where appropriate

#### 1.3 Business Rules and Validation
- [x] **STEP 1.3.1**: Create validation result classes
  - [x] Create `Common/ValidationResult.cs`
  - [x] Implement Success and Failure static methods
  - [x] Add error message collection support

- [x] **STEP 1.3.2**: Create image validation service
  - [x] Create `Services/ImageValidationService.cs`
  - [x] Implement `CanAddImages` method with 10-image limit validation
  - [x] Add file size validation (5MB limit)
  - [x] Add file type validation for images
  - [x] Add Base64 format validation

#### 1.4 Custom Exceptions
- [x] **STEP 1.4.1**: Create base exceptions
  - [x] Create `Exceptions/ValidationException.cs`
  - [x] Create `Exceptions/NotFoundException.cs`
  - [x] Create `Exceptions/ImageLimitExceededException.cs`
  - [x] Ensure proper inheritance and error message handling

#### 1.5 Repository Interfaces
- [x] **STEP 1.5.1**: Create customer repository interface
  - [x] Create `Repositories/ICustomerRepository.cs`
  - [x] Define CRUD operations for customers
  - [x] Define image-specific operations (GetCustomerImagesAsync, AddImageAsync, RemoveImageAsync)
  - [x] Use async/await patterns throughout

#### 1.6 Use Case Interfaces
- [x] **STEP 1.6.1**: Create use case interfaces
  - [x] Create `UseCases/IUploadImagesUseCase.cs`
  - [x] Create `UseCases/IGetCustomerImagesUseCase.cs`
  - [x] Create `UseCases/IDeleteImageUseCase.cs`
  - [x] Create `UseCases/IGetCustomerUseCase.cs`
  - [x] Define proper input/output parameters for each use case

---

### Phase 2: Data Layer Implementation
*Infrastructure concerns - JSON storage, file operations*

#### 2.1 Data Project Setup
- [x] **STEP 2.1.1**: Create Data project structure
  - [x] Create `src/Malte.Clean.Data` folder
  - [x] Add `Malte.Clean.Data.csproj` file
  - [x] Add reference to Domain project
  - [x] Configure JSON serialization dependencies

#### 2.2 JSON Storage Infrastructure
- [x] **STEP 2.2.1**: Create JSON storage service
  - [x] Create `Storage/JsonStorageService.cs`
  - [x] Implement file read/write operations with proper error handling
  - [x] Add file locking mechanism for concurrent access
  - [x] Create data directory structure (`data/customers.json`)

- [x] **STEP 2.2.2**: Create JSON data models
  - [x] Create `Models/CustomerJsonModel.cs` for JSON serialization
  - [x] Create `Models/CustomerImageJsonModel.cs` for JSON serialization
  - [x] Implement conversion methods between domain entities and JSON models

#### 2.3 Repository Implementation
- [x] **STEP 2.3.1**: Implement customer repository
  - [x] Create `Repositories/CustomerRepository.cs`
  - [x] Implement `ICustomerRepository` interface
  - [x] Add CRUD operations with JSON storage
  - [x] Implement image-specific operations
  - [x] Add proper error handling and logging

#### 2.4 Data Validation and Consistency
- [x] **STEP 2.4.1**: Add data consistency checks
  - [x] Implement customer existence validation
  - [x] Add image count validation in repository
  - [x] Ensure atomic operations for multi-image uploads
  - [x] Add data integrity validation on load

---

### Phase 3: Use Cases Implementation
*Application logic orchestrating domain and data layers*

#### 3.1 Use Case Implementations
- [ ] **STEP 3.1.1**: Implement upload images use case
  - [ ] Create `UseCases/UploadImagesUseCase.cs`
  - [ ] Inject required dependencies (repository, validation service)
  - [ ] Implement business logic with proper validation
  - [ ] Add comprehensive error handling

- [ ] **STEP 3.1.2**: Implement get customer images use case
  - [ ] Create `UseCases/GetCustomerImagesUseCase.cs`
  - [ ] Implement image retrieval logic
  - [ ] Add customer existence validation
  - [ ] Return proper DTOs

- [ ] **STEP 3.1.3**: Implement delete image use case
  - [ ] Create `UseCases/DeleteImageUseCase.cs`
  - [ ] Implement image deletion logic
  - [ ] Add image existence validation
  - [ ] Ensure proper cleanup

- [ ] **STEP 3.1.4**: Implement get customer use case
  - [ ] Create `UseCases/GetCustomerUseCase.cs`
  - [ ] Implement customer retrieval logic
  - [ ] Add proper error handling for non-existent customers

---

### Phase 4: API Layer Implementation
*Controllers, DTOs, middleware, and HTTP concerns*

#### 4.1 API Project Enhancement
- [ ] **STEP 4.1.1**: Configure API project dependencies
  - [ ] Add references to Domain and Data projects
  - [ ] Configure dependency injection for repositories and use cases
  - [ ] Set up JSON serialization options
  - [ ] Configure CORS if needed

#### 4.2 DTOs and Request/Response Models
- [ ] **STEP 4.2.1**: Create request DTOs
  - [ ] Create `DTOs/UploadImageRequest.cs`
  - [ ] Create `DTOs/ImageUploadDto.cs`
  - [ ] Add validation attributes
  - [ ] Implement proper JSON serialization

- [ ] **STEP 4.2.2**: Create response DTOs
  - [ ] Create `DTOs/CustomerImageResponse.cs`
  - [ ] Create `DTOs/CustomerResponse.cs`
  - [ ] Create `DTOs/ApiErrorResponse.cs`
  - [ ] Add proper mapping from domain entities

#### 4.3 Controllers Implementation
- [ ] **STEP 4.3.1**: Implement customer images controller
  - [ ] Create `Controllers/CustomerImagesController.cs`
  - [ ] Implement POST /api/customers/{customerId}/images endpoint
  - [ ] Implement GET /api/customers/{customerId}/images endpoint
  - [ ] Implement DELETE /api/customers/{customerId}/images/{imageId} endpoint
  - [ ] Add proper HTTP status codes and error responses

- [ ] **STEP 4.3.2**: Implement customers controller
  - [ ] Create `Controllers/CustomersController.cs`
  - [ ] Implement GET /api/customers/{customerId} endpoint
  - [ ] Add proper error handling and validation

#### 4.4 Middleware and Error Handling
- [ ] **STEP 4.4.1**: Implement global exception handling
  - [ ] Create `Middleware/GlobalExceptionHandlingMiddleware.cs`
  - [ ] Handle ValidationException, NotFoundException, and generic exceptions
  - [ ] Return consistent error response format
  - [ ] Add proper logging

- [ ] **STEP 4.4.2**: Configure middleware pipeline
  - [ ] Register exception handling middleware in Program.cs
  - [ ] Configure proper middleware order
  - [ ] Add request/response logging if needed

#### 4.5 API Documentation and Testing
- [ ] **STEP 4.5.1**: Configure Swagger documentation
  - [ ] Enhance Swagger configuration for better API documentation
  - [ ] Add XML comments to controllers and DTOs
  - [ ] Configure example requests and responses
  - [ ] Test API endpoints through Swagger UI

---

### Phase 5: Integration and Testing
*End-to-end validation and quality assurance*

#### 5.1 Manual Testing
- [ ] **STEP 5.1.1**: Test core functionality
  - [ ] Test image upload with valid data
  - [ ] Test 10-image limit enforcement
  - [ ] Test image retrieval for existing customer
  - [ ] Test image deletion
  - [ ] Test error scenarios (invalid customer, image not found, etc.)

#### 5.2 Data Validation
- [ ] **STEP 5.2.1**: Validate JSON storage
  - [ ] Verify JSON file creation and structure
  - [ ] Test concurrent access scenarios
  - [ ] Validate data persistence across application restarts
  - [ ] Check file locking mechanism

#### 5.3 Performance Testing
- [ ] **STEP 5.3.1**: Test with large images
  - [ ] Test upload with maximum size images (5MB)
  - [ ] Test upload of 10 images simultaneously
  - [ ] Monitor memory usage during operations
  - [ ] Validate response times

---

### Phase 6: Documentation and Finalization
*Final documentation and project completion*

#### 6.1 Code Documentation
- [ ] **STEP 6.1.1**: Add XML documentation
  - [ ] Document all public classes and methods
  - [ ] Add usage examples where appropriate
  - [ ] Ensure Swagger generates comprehensive API docs

#### 6.2 Setup Documentation
- [ ] **STEP 6.2.1**: Update README (if requested)
  - [ ] Add setup and running instructions
  - [ ] Document API endpoints with examples
  - [ ] Add architecture overview
  - [ ] Include troubleshooting section

---

## =� Progress Tracking

### Current Status: Phase 2 Completed
- **Phase 1 (Domain)**: � **Completed** (6/6 sections)
- **Phase 2 (Data)**: � **Completed** (4/4 sections)
- **Phase 3 (Use Cases)**: � Not Started (0/1 section)
- **Phase 4 (API)**: � Not Started (0/5 sections)
- **Phase 5 (Integration)**: � Not Started (0/3 sections)
- **Phase 6 (Documentation)**: � Not Started (0/2 sections)

### Legend
-  **Completed**: All steps in section finished
- = **In Progress**: Some steps completed, others pending
- � **Not Started**: No steps completed yet
- L **Blocked**: Cannot proceed due to dependencies

---

## = Dependencies Between Phases

1. **Phase 2** depends on **Phase 1** (Domain entities and interfaces)
2. **Phase 3** depends on **Phase 1** and **Phase 2** (Use cases need domain and data layers)
3. **Phase 4** depends on **Phase 3** (Controllers need use cases)
4. **Phase 5** depends on **Phase 4** (Testing needs complete implementation)
5. **Phase 6** can be done in parallel with other phases

---

## <� Success Criteria

### Functional Requirements
- [ ] Users can upload images to customer profiles (max 10 per customer)
- [ ] Images are stored as Base64 strings in JSON files
- [ ] Users can retrieve all images for a customer
- [ ] Users can delete individual images
- [ ] API returns appropriate HTTP status codes and error messages

### Technical Requirements
- [ ] Clean Architecture principles followed
- [ ] Domain layer independent of external concerns
- [ ] Proper error handling and validation
- [ ] JSON storage with file locking for concurrency
- [ ] Comprehensive API documentation via Swagger

### Quality Requirements
- [ ] Code follows SOLID principles
- [ ] Proper separation of concerns
- [ ] Consistent error handling
- [ ] Clear and maintainable code structure
- [ ] Future-ready for database migration

---

## =� Getting Started

To begin implementation, start with **Phase 1: Domain Layer Foundation** and follow the steps sequentially. Each step should be completed and verified before moving to the next one.

**Next Action**: Phase 2 completed! Ready to proceed to Phase 3: Use Cases Implementation.