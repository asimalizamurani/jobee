# Jobee - Web Based Online Recruitment System

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## Introduction
Jobee is a web-based online recruitment system designed to streamline the hiring process. It offers functionalities for applicants, HR, and admin users to manage job applications, vacancies, and interviews efficiently. The project aims to simplify recruitment workflows and improve collaboration between stakeholders.

---

## Features
### **Applicants** can:
- View and apply for job vacancies.
- Manage their applications and interviews.
- Access reports and help resources.

### **HR users** can:
- Manage applicants and vacancies.
- Schedule and manage interviews.
- Generate HR-specific reports.

### **Admin users** can:
- Oversee applicants and vacancies.
- Create new job vacancies.
- Manage user roles and permissions.
- Generate administrative reports.

---

## Technologies Used
- **Backend Framework**: ASP.NET MVC
- **Database**: SQL Server Management Studio 2020
- **Frontend**: Razor Views, HTML, CSS, and Bootstrap
- **Development Environment**: Visual Studio

---

## Installation
To run the Jobee system locally, follow these steps:

1. **Clone the repository**:
   ```sh
   git clone https://github.com/asimalizamurani/jobee.git
   cd jobee
   ```

2. **Set up the database**:
   - Create a database in **SQL Server Management Studio 2020**.
   - Update the connection string in `appsettings.json` to match your database configuration.

3. **Apply migrations**:
   ```sh
   dotnet ef database update
   ```

4. **Run the application**:
   ```sh
   dotnet run
   ```

---

## Usage
Once the application is running, you can access the system's functionalities based on your user role:

- **Applicants**: Apply for jobs, view your dashboard, and manage interviews.
- **HR**: Manage applicants, view and create vacancies, and conduct interviews.
- **Admin**: Oversee all operations, manage user roles, and generate reports.

---

## Contributing
We welcome contributions to enhance the Jobee system. To contribute:

1. Fork the repository:
   ```sh
   git fork https://github.com/asimalizamurani/jobee.git
   ```

2. Create a new feature branch:
   ```sh
   git checkout -b feature/YourFeature
   ```

3. Commit your changes:
   ```sh
   git commit -m 'Add a new feature'
   ```

4. Push to the branch:
   ```sh
   git push origin feature/YourFeature
   ```

5. Open a pull request on GitHub.

---

## License
This project is licensed under the [MIT License](LICENSE).

---

## Contact
For questions or support, please reach out to **Asim Alizamurani** at [asim.alizamurani@example.com](mailto:asim.alizamurani@example.com).
