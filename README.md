Inventory Management System (IMS)
This Inventory Management System (IMS) is designed to efficiently manage inventory, categories, and suppliers.
It follows Clean Architecture for a well-structured and scalable system. 
The project integrates Serilog for logging, In-Memory Caching for performance optimization, 
JWT Authentication for secure access, Exception Middleware for error handling, and a Role-Based Access System to control user permissions. 
It also includes comprehensive unit testing to ensure high code quality and system reliability. 
Swagger UI is implemented for easy API testing.
How to Set Up and Run the Project
Clone the Repository
git clone https://github.com/SheriKhan01/Inventory-Management-System.git
cd Inventory-Management-System
Update the Database Connection String
Open appsettings.json in the IMS.API project and update your database connection details.
Apply Migrations
Run the following commands to set up the database
cd IMS.Infrastructure
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run --project IMS.API
Here are the Swagger endpoints for your API controllers:
Category Controller
/api/Category/GetAllCategories [GET]
Description: Fetch all categories.

/api/Category/GetCategoryById?id={id} [GET]
Description: Fetch a specific category by ID.

/api/Category/AddCategory [POST]
Body: { "name": "Category Name", "description": "Category Description" }
Description: Add a new category.

/api/Category/UpdateCategory?id={id} [PUT]
Body: { "name": "Updated Name", "description": "Updated Description" }
Description: Update an existing category.

/api/Category/DeleteCategory?id={id} [DELETE]
Description: Delete a category by ID.
Inventory Item Controller
/api/InventoryItem/GetAllInventories [GET]
Description: Fetch all inventory items.

/api/InventoryItem/GetInventoryDetail?id={id} [GET]
Description: Fetch an inventory item by ID.

/api/InventoryItem/AddInventory [POST]
Body: { "name": "Item Name", "quantity": 10, "categoryId": "GUID" }
Description: Add a new inventory item.

/api/InventoryItem/UpdateInventory?id={id} [PUT]
Body: { "name": "Updated Item Name", "quantity": 15 }
Description: Update an existing inventory item.

/api/InventoryItem/DeleteInventory?id={id} [DELETE]
Description: Delete an inventory item by ID.
Supplier Controller
/api/Supplier/GetAllSupplier [GET]
Description: Fetch all suppliers.

/api/Supplier/GetSupplierById?id={id} [GET]
Description: Fetch a supplier by ID.

/api/Supplier/AddSupplier [POST]
Body: { "name": "Supplier Name", "contact": "Supplier Contact" }
Description: Add a new supplier.

/api/Supplier/UpdateSupplier?id={id} [PUT]
Body: { "name": "Updated Supplier Name", "contact": "Updated Contact" }
Description: Update an existing supplier.

/api/Supplier/DeleteSupplier?id={id} [DELETE]
Description: Delete a supplier by ID.
Account Controller
/api/Account/register [POST]
Body: { "email": "user@example.com", "password": "P@ssword1", "firstName": "John", "lastName": "Doe" }
Description: Register a new user.

/api/Account/login [POST]
Body: { "email": "user@example.com", "password": "P@ssword1" }
Description: Log in and receive a JWT token.

/api/Account/user/{id} [GET]
Authorization: Bearer Token
Description: Get user details by ID.

/api/Account/users [GET]
Authorization: Bearer Token (Admin Only)
Description: Get all users.

/api/Account/role/create [POST]
Body: { "roleName": "Admin" }
Authorization: Bearer Token (Admin Only)
Description: Create a new role.

/api/Account/roles [GET]
Authorization: Bearer Token (Admin Only)
Description: Get all roles.
Deploy Using Azure CLI (Command Line Interface)
If you prefer command-line deployment, open PowerShell or Command Prompt and execute:
Login to Azure
az login
Set Subscription (If you have multiple Azure accounts)
az account set --subscription "YOUR_SUBSCRIPTION_ID"
Create a Resource Group
az group create --name IMSResourceGroup --location eastus
Create an App Service Plan
az appservice plan create --name IMSAppPlan --resource-group IMSResourceGroup --sku B1 --is-linux
Create a Web App
az webapp create --resource-group IMSResourceGroup --plan IMSAppPlan --name IMSInventoryAPI --runtime "DOTNETCORE:8.0"
Deploy API to Azure
az webapp up --name IMSInventoryAPI --resource-group IMSResourceGroup --runtime "DOTNETCORE:8.0"
After running this, your API will be live at:
 https://imsinventoryapi.azurewebsites.net/swagger/index.html
