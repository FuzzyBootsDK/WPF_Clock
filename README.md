# WPF Clock and Weather Application

This is a WPF (Windows Presentation Foundation) application that displays the current date, time, and weather information. The application allows the user to toggle between different date and time formats and switch between English and Danish languages.

## Features

- Display current date and time.
- Display current weather information including temperature, wind direction, wind speed, and humidity.
- Toggle between 24-hour and 12-hour time formats.
- Toggle between European (dd-MM-yyyy) and American (MM-dd-yyyy) date formats.
- Toggle between numeric and spelled out month formats in the date.
- Switch between English and Danish languages.
- Load weather station locations from a SQLite database.
- Update API key through a dialog.

## How to Use

The application has a menu with two main options: Format and Language.

- Under the Format menu, there are three options:
  - Toggle date format MM-dd-yyyy / dd-MM-yyyy: This option allows you to switch between American and European date formats.
  - Toggle Month format: This option allows you to switch between numeric and spelled out month formats.
  - Toggle 24/12 format: This option allows you to switch between 24-hour and 12-hour time formats.

- Under the Language menu, there is one option:
  - Switch language / Skift sprog: This option allows you to switch between English and Danish languages.

- Under the API Key menu, there is one option:
  - Update API Key: This option allows you to update the API key used for fetching weather data.

## Code Structure

The main code for the application is in the `MainWindow.xaml` and `MainWindow.xaml.cs` files. The `MainWindow.xaml` file contains the XAML markup for the user interface, and the `MainWindow.xaml.cs` file contains the C# code-behind for the application logic.

### Main Files

- `MainWindow.xaml`: Contains the XAML markup for the user interface.
- `MainWindow.xaml.cs`: Contains the C# code-behind for the application logic.
- `WeatherDbContext.cs`: Defines the database context for accessing weather station locations stored in a SQLite database.
- `ApiKeyDialog.xaml.cs`: Defines the dialog for updating the API key.
- `Geometry.cs`: Defines the data structures for handling weather data from the API.
- `Location.cs`: Defines the `Location` entity used in the database.

## Requirements

- .NET 8 or later
- Windows operating system

## Setup

1. Ensure you have .NET 8 or later installed on your system.
2. Clone the repository to your local machine.
3. Open the solution in Visual Studio.
4. Build the solution to restore the necessary NuGet packages.
5. Run the application.

## Environment Variables

The application uses a `.env` file to store environment variables. The following variables are required:

- `APIKEY`: The API key for accessing the weather data API.
- `DB_PATH`: The path to the SQLite database file containing weather station locations.

## License

This project is licensed under the MIT License - see the LICENSE.md file for details.
