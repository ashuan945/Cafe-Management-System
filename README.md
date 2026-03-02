# Cafe Management System

A **C# Windows Forms application** for managing a cafe, providing user management, item catalog, order processing, and reporting functionalities. The application is connected to an **MS SQL LocalDB database**, making it easy to store and manage data.  


## **Project Overview**

The Cafe Management System is designed to streamline cafe operations for both **guests** and **registered users**. The application features:

- **Login System**:  
  - Users can log in using a valid username and password stored in the database.  
  - Guests can also access the ordering functionality without registering.  

- **Order Processing**:  
  - Guests and users can create new orders for beverages and food items.  
  - Each order records the **order number, date, seller, and total amount**.  
  - Orders can be summarized and printed as required.  

- **Item Management**:  
  - Add, edit, update, and view items available in the cafe.  
  - Filter items based on category (**Food** or **Beverage**) for easy browsing.  

- **User Management**:  
  - Add, edit, delete, and view registered users in the system.  

- **Order History & Reporting**:  
  - View all past orders.  
  - Print order summaries including the date, seller, order number, and total amount.  

- **Intuitive GUI**:  
  - Clean and user-friendly Windows Forms interface for smooth navigation.  


## **Features Summary**

| Feature | Guest | Registered User |
|---------|-------|----------------|
| Login / Authentication | ❌ | ✅ |
| Place Orders | ✅ | ✅ |
| View / Print Orders | ❌ | ✅ |
| Manage Items | ❌ | ✅ |
| Manage Users | ❌ | ✅ |
| Filter Items by Category | ✅ | ✅ |

