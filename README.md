# Decibels - Real-world E-commerce Application

**Decibels** is a real-world e-commerce application for musical instruments, deployed on Azure. It showcases the use of a wide range of modern technologies and development practices. **(To complete a test purchase, use card number `4242424242424242` with any valid future expiry date and CVC)**

---

## Technologies & Architecture

### .NET Stack
- **.NET 8 (ASP.NET Core MVC)**
- **Entity Framework Core (Code-First)**: For seamless database interaction and schema management.
- **ASP.NET Core Identity**: Robust user authentication and authorization system.
- **Role Management**: Implemented for granular access control (e.g., Customer, Admin, Employee, Company).
- **Areas**: For better organization and modularity of the application.
- **Session & Cookie Management**: For handling user sessions and preferences.
- **LINQ**: For efficient data querying.
- **Repository and Unit of Work Patterns**: Applied for a clean, testable, and maintainable data access layer.
- **N-Tier Architecture**: Ensuring clear separation of concerns within the application layers.

### Azure Services & Cloud Practices
- **Azure App Service**: Hosting the web application with a focus on scalability and reliability.
  - **Deployment Slots (Staging and Production)**: For zero-downtime deployments and testing in a pre-production environment.
  - **Secure Configuration with Application Settings**: Managing sensitive data (e.g., connection strings, API keys) securely in the cloud, separate from source code.
- **Azure SQL Server & SQL Database**: Reliable and scalable relational database for storing application data.
- **Azure Blob Storage**: Utilized for persistent storage and serving of product images, enhancing performance and scalability.
- **Azure Active Directory Workload Identity**: For secure, automated authentication of CI/CD pipelines to Azure resources, eliminating the need for secrets in GitHub Actions.
- **GitHub Actions CI/CD**: Automated pipelines for Continuous Integration and Continuous Deployment to both staging and production environments.

### Core Features
- **Product Catalog**: Users can view and browse a wide range of musical instruments.
- **User Registration & Authentication**: Secure registration and login functionalities.
- **Shopping Cart Management**: Comprehensive CRUD operations for customers to manage their cart items.
- **Order Management**: Full CRUD capabilities for order processing, accessible by authorized roles (Customer/Admin/Employee/Company).
- **Secure Payments (Stripe)**: Integration of Stripe for seamless and secure payment processing. 
- **Social Login**: Enhanced user experience with Facebook registration/login.
- **Responsive Design**: Ensuring optimal viewing experience across various devices (desktop, tablet, mobile).
- **REST API**: Underlying API design for data interaction.

### Libraries & Tools
- **Bootstrap v5**: Modern and responsive frontend framework for UI components.
- **DataTables**: For interactive and efficient display of tabular data.
- **Toastr**: For elegant and non-intrusive notification messages.
- **SSMS (SQL Server Management Studio)**: For database management and querying.
- **TinyMCE**: Rich text editing