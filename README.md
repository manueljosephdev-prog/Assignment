<<<<<<< HEAD
# Assignment
=======

# Microservices Web App (.NET 8 + Angular + MSSQL + Azure Blob)

## Run with Docker
docker-compose up --build

Frontend: http://localhost:4200  
Product API: http://localhost:5001/swagger  
Order API: http://localhost:5002/swagger  

## Apply Migrations
cd backend/product-service  
dotnet ef migrations add init  
dotnet ef database update  

cd backend/order-service  
dotnet ef migrations add init  
dotnet ef database update  

## Run Tests
cd backend/order-service/Order.Tests  
dotnet test  

## Assumptions
- Azure Blob uses Azurite locally  
- Each service has its own DB  
- Basic UI only  
>>>>>>> fded34a (Add microservices assignment code)
