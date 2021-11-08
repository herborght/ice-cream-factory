# Flavoring, Hardening and Packaging Module

This module models three smaller stages of the simulation. 
First, solid flavoring such as nuts, solid chocolate, and other solids such as maybe cookie dough are added to the mix.

Then, the flavored mix is packed into either High-density polyethylene or waffle style packaging(cone), after which the packed mix is sent into a hardening chamber where the it is frozen again for a certain period of time if required. Hardening is not a dynamic freezing process, meaning that the mix is not agitated or whipped in this submodule. 

For soft ice cream, hardening is not a required step. However, this may be required for cone ice cream or stick ice cream with hard bits.

Finally the ice cream is sent to final packaging and from there on is sent to storage and/or to stores for retail and consumption. Since these parts aren't interesting to us in the use case that focuses on access control on a production facility, these have been simplified to a simple delay. 
