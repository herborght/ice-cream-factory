# Homogenization Module
This module models a homogenization module in the ice cream factory. This module takes the incoming ice cream mixes and reduces the globulization and particle size of the mix. 

This module has two different stages, where pressure is applied to mix. In this project the module is simulated by calculating the new  particle size  of the mixture as well as an increase in temperature due to the ineffciency of the homogenizer.

This module processes mix in batches, i.e. only one batch can run at one time in the module. The module is based on the tank module and is only filled and empties when mix enters and exits respectively. This also adds an element of realism as the homogenizer is a very inefficient module that consumes a lot more power than required.

In this module there is also the cooling and ageing submodules that cool down and age the mix respectively. The ageing module is simplified to only be a time delay. The cooling module uses Newton's Law of Cooling to simulate the cooling of the mix. 
