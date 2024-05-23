# WPF Clock and Weather Application

This is a WPF (Windows Presentation Foundation) application that displays the current date, time, and weather information. The application allows the user to toggle between different date and time formats and switch between English and Danish languages.

## Features

- Display current date and time.
- Display current weather information including temperature, wind direction, wind speed, and humidity.
- Toggle between 24-hour and 12-hour time formats.
- Toggle between European (dd-MM-yyyy) and American (MM-dd-yyyy) date formats.
- Toggle between numeric and spelled out month formats in the date.
- Switch between English and Danish languages.

## How to Use

The application has a menu with two main options: Format and Language.

- Under the Format menu, there are three options:
  - Toggle date format MM-dd-yyyy / dd-MM-yyyy: This option allows you to switch between American and European date formats.
  - Toggle Month format: This option allows you to switch between numeric and spelled out month formats.
  - Toggle 24/12 format: This option allows you to switch between 24-hour and 12-hour time formats.

- Under the Language menu, there is one option:
  - Switch language / Skift sprog: This option allows you to switch between English and Danish languages.

## Code Structure

The main code for the application is in the `MainWindow.xaml` and `MainWindow.xaml.cs` files. The `MainWindow.xaml` file contains the XAML markup for the user interface, and the `MainWindow.xaml.cs` file contains the C# code-behind for the application logic.

## Requirements

- .NET Framework 4.7.2 or later
- Windows operating system

## License

This project is licensed under the MIT License - see the LICENSE.md file for details.