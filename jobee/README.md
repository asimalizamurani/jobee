<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - OnlineJobs</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/OnlineJobs.styles.css" asp-append-version="true" />
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-lg navbar-dark shadow-lg">
            <div class="container">
                <a class="navbar-brand" href="/Home/Index">Jobee</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse position-relative" id="navbarNav">
                    <button class="close-btn d-lg-none" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-label="Close">&times;</button>
                    <ul class="navbar-nav me-auto mb-2 mb-lg-0">

                        @if (User.IsInRole("Applicant"))
                        {
                            <li class="nav-item">
                                <a class="nav-link" href="/Dashboard/ViewDashboard">Dashboard</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Vacancy/Index">Vacancies</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Applicant/Apply">Apply for Job</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Interview/Index">Interviews</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Reports/ApplicantReport">Reports</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Home/Help">Help</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Home/About">About</a>
                            </li>
                        }

                        @if (User.IsInRole("HR"))
                        {
                            <li class="nav-item">
                                <a class="nav-link" href="/Applicant/Index">Manage Applicants</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Vacancy/Index">Vacancies</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Interview/Index">Interviews</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Reports/HRReport">Reports</a>
                            </li>
                        }

                        @if (User.IsInRole("Admin"))
                        {
                            <li class="nav-item">
                                <a class="nav-link" href="/Applicant/Index">Manage Applicants</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Vacancy/Index">Manage Vacancies</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Vacancy/Create">Create Vacancy</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Reports/AdminReport">Reports</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/User/Manage">User Management</a>
                            </li>
                        }

                    </ul>
                    <div>
                        <a href="/User/Login" class="btn btn-primary" style="color: inherit; text-decoration: none;">Login</a>
                        <a href="/User/Logout" class="btn btn-primary" style="color: inherit; text-decoration: none;">Logout</a>
                    </div>
                </div>
            </div>
        </nav>
    </header>

    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2024 - OnlineJobs - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
        </div>
    </footer>
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
