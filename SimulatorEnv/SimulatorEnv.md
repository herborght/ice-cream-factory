# Simulator Environment


## Calculations Class

This part of the simulation controls flow properties of the mix such as viscosity and density based on the recipe that is provided to the simulation, which are then altered in each of the modules as needed. This class also provides an initial recipe parsing functionality. 

## ExecEngine Class

This class executes the simulation and its modules with an interval that is defined by the app.config file. 

## Modules

There are currently four different modules: 
1. [Pasteurization Module](Modules/PasteurizationModule.md)
2. [Homogenization, Ageing and Cooling Module](Modules/HomogenizationModule.md)
3. [Dynamic Freezing Module](Modules/DynamicFreezing.md)
4. [Flavoring, Hardening and packaging Module](Modules/FlavoringHardeningPacking.md)

There are also two basic modules that provide some basic functionality to base other modules on as well as module base in the form of an abstract class that provides requirements on how a module should be set up:  
 - Tank Module - A simple tank with inflow and outflow and a controllable SetPoint of the Level. 
 - Complex Module - A system of three interconnected tanks that each have an inflow and outflow, but have a disturbance every 50 seconds. 


## Logging
Logs the events of the simulation, using an EventSource and an Event Listener. There is also an option to use a simpler CSV file log called SimpleFileLog that can be used to visualize the log data easier. 

## Platform
In our demonstration system, the Simulator runs on a Windows 10 node with the x86-64 instruction set. 
