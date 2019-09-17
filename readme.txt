Basic MBTA Schedule Predictor
version 3
written by Simon Yip

Basic License:
You can use this program however you wish. You can make any modifications.
However, you must give credit to me if you use any or all parts of the code.

Purpose:
This program was written for the Quincy College 2018 Code-A-Thon.
It is written in Visual Basic.
The code easily breaks if the format suddenly changes.
It grabs the JSON file from the MBTA's website thru the V3 API provided.
Information is gathered from "prediction" side where things can changes depending on events/traffic.
mbta_alias.txt is required for the program to function.
It is a mapping of all Red Line stations to their key MBTA website counterparts.

Program shows both Commuter Rail and Subway activity as provided by the MBTA.
This includes the Silver Line, which includes the newly added SL3 Chelsea connection.


Requirements:
Visual Studio 2015 SP3 (Community Edition) to compile source
MSVC C++ 2015/.NET 4.5+ Redistributable
