Basic MBTA Schedule Predictor
version 3
written by Simon Yip

Uses MIT License

Purpose:
This program was written for the Quincy College 2018 Code-A-Thon
in the month of March 2018.
It is written in Visual Basic 2015, last tested on VS2017/2019.
The code easily breaks if the JSON format suddenly changes.
It grabs the JSON file from the MBTA's website thru the v3 API provided.
Information is gathered from "prediction" side where things can change
depending on traffic events.
mbta_alias.txt is required for the program to function (persistence).
It maps all Red Line stations to their key MBTA website counterparts.

Program shows both Commuter Rail and Subway activity as provided by the MBTA.
This includes the Silver Line, which includes the newly added
SL3 Chelsea connection.

Requirements:
Visual Studio 2015 SP3 (Community Edition) to compile source
MSVC C++ 2015 Redistributable or later
.NET 4.5.2 or later
