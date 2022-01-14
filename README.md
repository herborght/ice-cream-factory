# Ice Cream Factory

![Screenshot](screenshot.png)

### Description

The ice cream factory projects is done in collaboration with ABB and Mäladalen University. The project was to build a UI for an already existing simulation engine. The app is a .NET application. We used WPF to develop the UI, as requested by the customer. The simulation engine has the possibility to several types of modules, which all will visualized in the UI's simulation page. To get a snapshot of all the parameters in the current simulation, the user can switch to a raw data view. The history will be logged in its own file, and the UI allows for downloading of previous (and current) simulations and allow the user to select these by date. 
***

### Features
* Live update of the UI 
* A UI separated from the simulation enige
* Logging and downloading of logging files
* Raw Data view of the current simulation
*** 

### Technologies and languages
* .NET
* C#
* WPF
* XAML
***
### Pictures of the application

The Simulation Page with a simulation running.

![Screenshot](screenshot.png)


The raw data page with a simulation running.

![Screenshot](screenshot.png)

***
### Installation guide
* Install MQTT broker for communication between the modules. It can be downloaded here: https://mosquitto.org/download/ 
  * Run the installer (mosquitto-2.0.13-install-windows-x64.exe)** 
  * Navigate to download location and start the broker with `mosquitto` in the command prompt 
* Install Visual Studio for running the application: https://code.visualstudio.com/download 
* Clone the project into your local directory: `git clone https://github.com/herborght/ice-cream-factory.git` 
* Open the project in Visual Studio 
  * Navigate to “app.config” under SimulatorEnv/app.config 
  * Change the “BrokerAddress” value to localhost IP (127.0.0.1) 
  * Run the project from Visual Studio	 

***

### Source code structure 
The code is structured into four different components, the Common, SimulationTests, SimulatorEnv and SimulatorUI. We have only worked on the SimulationUI, and added some tests to the Test environment, and will therefore not address the structure of the SimulatorEnv and Common. 

The simulator UI is organized into multiple cs classes and three xaml files. The xaml files are the visual components of the UI. The MainWindow is a window type, and holds the sidebar (implemented directly), and contains a Page. The Page is either of the type SimulationPage or RawDataPage. The SimulationPage is the page with the different modules visualized, and the RawDataPage displays the raw data in table-form. By using the switch-view button of the sidebar, you change which page is contained in the MainWindow.  

Each of the xaml files has a .cs files belonging to it, with the same name, this file implements the logic of the windows and pages. Where Simulation page adds its elements programmatically using a Canvas control form and various shapes such as rectangles and ellipses to represent the values.   

The different classes are instantiated by inheriting the attributes and fields of TankModule, and then adds their own unique fields. We use the Main.cs to read and update the val
 
