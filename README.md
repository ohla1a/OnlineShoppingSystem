# Online Shopping Management System 🛒

A complete **Point of Sale (POS) & Management System** built with **C# WinForms** and **Microsoft SQL Server**.
Developed as a Computer Science project at **Rangsit International College (RIC)**.

## 🌟 Key Features
* **Interactive Dashboard:** Real-time summary cards showing Total Orders, Revenue, and Customer counts.
* **Order Processing:** "Add to Cart" functionality with automatic subtotal calculation.
* **Reporting Module:** Generates printable PDF invoices/receipts using **Microsoft Report Viewer (RDLC)**.
* **Database Management:** Full CRUD (Create, Read, Update, Delete) support for:
    * Products & Categories
    * Customers & Suppliers
    * Staff & Payments
* **Secure Login:** Role-based access control with fade-in UI effects.

## 🛠️ Technology Stack
* **Language:** C# (.NET Framework)
* **GUI:** Windows Forms (WinForms) with custom rounded buttons and modern dashboard layout.
* **Database:** SQL Server LocalDB (`(localdb)\MSSQLLocalDB`).
* **Reporting:** Microsoft RDLC Report Viewer.

## 🚀 How to Run This Project
1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/ohla1a/OnlineShoppingSystem.git](https://github.com/ohla1a/OnlineShoppingSystem.git)
    ```
2.  **Setup the Database:**
    * Open **Microsoft SQL Server Management Studio (SSMS)**.
    * Open the file `Database.sql` (included in this folder).
    * Execute the script to create the `OnlineShoppingDB` database and dummy data.
3.  **Run the App:**
    * Open `OnlineShoppingManagementSystem.sln` in **Visual Studio**.
    * Press **Start**.
    * *Note: The connection string is pre-configured for LocalDB.*

# 📸 Screenshots

<img width="845" height="404" alt="Screenshot 2026-01-06 205540" src="https://github.com/user-attachments/assets/cae56929-bc64-479b-acbf-ebd46f133107" />
Modern, secure login interface featuring a flat design aesthetic and user authentication against the SQL database.

<img width="1125" height="759" alt="Screenshot 2026-01-06 212325" src="https://github.com/user-attachments/assets/fd5f6c0c-71ad-40ac-9a2f-cb0a89377f97" />
Real-time Admin Dashboard displaying key performance indicators (KPIs) such as Total Revenue, Order Counts, and Inventory Status.

<img width="1123" height="760" alt="Screenshot 2026-01-06 212438" src="https://github.com/user-attachments/assets/710d62e1-c8f5-424f-bd7f-8931da6f1e6e" />
Comprehensive Product Management module allowing for full CRUD operations, stock tracking, and supplier categorization.

<img width="1124" height="760" alt="Screenshot 2026-01-06 212702" src="https://github.com/user-attachments/assets/7efddffa-cb44-4873-b07b-8876df5bd148" />
Automated reporting system using Microsoft Report Viewer (RDLC) to generate and print professional order invoices.
