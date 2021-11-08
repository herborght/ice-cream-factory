# Pasteurization Module

This module models a pasteurization module in an ice cream factory where it heats and cools down the ice cream mix to kill bacteria and fungi present. 

It consists of two parts, one heater and one cooler that interoperate to rapidly heat and cool down the ice cream. This module is based on the tank module and calculates the change in temperature based on conduction.  

The temperature is controlled using a TemperatureSetPoint that uses the heater or cooler to achieve the desired temperature. 

For demonstration purposes we let the mix heat to 70 C for 3 minutes which gives approximately 20 pasteurization units, which are calculated live and changes each time depending on the disturbances in temperature.
