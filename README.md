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



## **Database Setup** (Using Visual Studio)

Follow the steps below to connect to the LocalDB database using **Visual Studio Server Explorer**.

### Step 1: Open Server Explorer

1. Open the project in **Visual Studio**.
2. Navigate to: **View → Server Explorer**.

### Step 2: Add a New Database Connection

1. In **Server Explorer**, right-click on **Data Connections**.
2. Select **Add Connection...**.

### Step 3: Configure the Connection

1. In the **Add Connection** window:
- Data Source: **Microsoft SQL Server Database File (SqlClient)**
- Data File Name: **Cafedb**
- Logon to the server: **Use Windows Authentication**

2. Click **OK**.

### Step 4: Create Tables

1. Expand **Data Connections → Cafedb.mdf**
2. Right-click **Tables**
3. Select **New Query**
4. Copy and execute the following SQL:

```sql
-- Table: ItemTbl
CREATE TABLE ItemTbl (
  ItemNum INT NOT NULL PRIMARY KEY,
  ItemName VARCHAR(50) NOT NULL,
  ItemCat VARCHAR(50) NOT NULL,
  ItemPrice DECIMAL(10, 2) NOT NULL
);
GO

-- Table: OrderTbl
CREATE TABLE OrderTbl (
  OrderNum INT NOT NULL PRIMARY KEY,
  OrderDate VARCHAR(50) NOT NULL,
  [User] VARCHAR(50) NOT NULL,
  OrderAmount INT NOT NULL
);
GO

-- Table: UserTbl
CREATE TABLE UserTbl (
  Uname VARCHAR(50) NOT NULL PRIMARY KEY,
  Uphone VARCHAR(50) NOT NULL,
  Upassword NCHAR(10) NOT NULL
);
GO
```

5. Click Execute to create the tables.



## 📸 Screenshots

### 🔐 Login Form
The login form allows registered users to enter their credentials or continue as a guest.

<img src="https://github.com/user-attachments/assets/eec46fae-459c-4768-897f-3ee67ae3e29b" width="350"/>

### 🛒 Order Form
Users can select items, add them to the order, and calculate the total amount.

<img src="https://github.com/user-attachments/assets/37f93db2-7fc0-48e8-9b04-e528ce552523" width="500"/>

### 👤 User Management
Admins can add, edit, delete, and view registered users.

<img src="https://github.com/user-attachments/assets/4eb31da5-23a9-42c6-8e57-942d95d410ee" width="500"/>

### 🍽 Item Management
Manage food and beverage items and filter by category.

<img src="https://github.com/user-attachments/assets/841d881e-3454-4aec-a38a-f33b027574b7" width="500"/>

### 🧾 Orders Summary
View past orders and print order summaries.

<p>
  <img src="https://github.com/user-attachments/assets/811847ed-4b24-4942-bba7-672be4d5cfbc" width="350"/>
  <img src="https://github.com/user-attachments/assets/8f73a1d7-ce9d-4970-8358-4813093be25e" width="350"/>
</p>
