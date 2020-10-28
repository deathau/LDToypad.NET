# LDToypad.NET
A .NET Core C# library for communicating with the LEGO Dimensions Toypad

**NOTE:** This does not work with the XBox 360 or XBox One versions of the toypad.

This is a hobby project built in my spare time. I am happy to recieve contributions if I think they will improve the library.

## Instructions
- Download or checkout the source code
- Open the `LDToypad.sln` file
- Plug in your toypad to an available USB port
- Run the Example console app
- The pad should light up and change different colours and placing tags on the pad will trigger events to be logged to the console

## Documentation
Documentation is pretty much non-existent at the moment, but I've made an effort to provide comments in the code which should help you navigate.

The main class is `LDToypad.Toypad`, which has some events to subscribe to and some functions for manipulating the toypad lights.  
I *will* get around to documenting it better later.