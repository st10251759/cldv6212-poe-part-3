# POE Part 3: ABC Retailers Cloud Solution

## Overview

This repository contains the source code and documentation for **POE Part 3** of my Cloud Development module. The goal of this project is to build upon the work completed in Parts 1 and 2, focusing on improving the existing solution by integrating Azure SQL database functionality, ensuring proper authentication and authorization, and enabling deployment to Azure. 

The final application provides a centralized system for managing products, orders, and customers, with an Azure SQL database backend and CI/CD practices implemented via GitHub.

## Requirements

- **Student Number**: ST10251759
- **Module Code**: CLDV6212
- **POE URL**: https://st10251759-cldv6212-poe-part-3.azurewebsites.net
- **GitHub Link**: https://github.com/st10251759/cldv6212-poe-part-3
- **YouTube Video**: https://youtu.be/zYT3lK6roUM
- **PDF Submission**: ST10251759 - Cameron Chetty - CLDV6212- POE - Part 3.pdf

## Key Features

### 1. **Azure SQL Database**
- Created an **Azure SQL database** to store customer, product, and order information.
- **Replication**: Created a replica of the database in another region for redundancy and improved fault tolerance.
    - **Rationale for Replication**: A replica database ensures high availability, improves disaster recovery, and provides performance improvements for global applications by allowing read access from multiple regions.

### 2. **Web Application Functionalities**
- **User Roles**: 
    - **Admin**: Ability to manage inventory and view all orders.
    - **Client**: Ability to purchase products and view order history.
- **Authentication and Authorization**: Ensured login functionality for both admin and client users.
    - Role-based authentication restricts certain functionalities (e.g., inventory management) to admin users.
- **Order and Cart Functionality**: Clients can add products to their cart, view orders, and proceed with checkout.

### 3. **Deployment to Azure**
- Deployed the web application to **Azure App Service** with an **Azure SQL Server** database configured for use.
    - First, the **Azure SQL Server** was deployed, followed by the **database** deployment.
    - Configured **replication** between regions and provided screenshots.
    - Verified that the local application interacts with the Azure database.
    - Published the **MVC** app to Azure, ensuring proper configuration of the connection string.

### 4. **CI/CD with GitHub**
- Integrated **Continuous Integration/Continuous Deployment (CI/CD)** using GitHub from the start of the project to automate deployment processes.
- The application has at least **5 database entries** to demonstrate functionality.

### 5. **Screenshots**
- Screenshots of the following are included in the documentation:
    - Creating the Azure SQL database.
    - Creating the database replica in a different region.
    - The deployed web application in action.
    - Successful implementation of functionalities (login, inventory management, purchasing products, order confirmation).
    - Azure portal settings for deployment.

## Contact

For any questions or clarifications regarding this project, please feel free to reach out.
- **Student Number**: ST10251759
- **Full Name**: Cameron Chetty
- **Email**: st1025159@vcconnect.edu.za
- **Phone Number**: 0812625090

---
