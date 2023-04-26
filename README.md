# UWP_GUI

Contains code for a UWP Windows app that can connect to an arduino and control a robot arm.

.Ino file for arduino: https://github.com/Robot-Builders-Team-University-of-Idaho/Robot_UWP/tree/PortFix/RobotUI_UWP/ArduinoCode/RobotUI

To build this project, Visual Studio with the "Universal Windows Platform Development" package is required.

# User's Guide

Home Page:
<img width="485" alt="image" src="https://user-images.githubusercontent.com/78044374/234441615-b5fbc13b-8cf0-4e0d-96be-eb892c57fdf3.png">

On the home page their are several buttons, "Documentation" will take you to the github page containing the user's guide and project files, "Claw Effector" "Suction Effector" and "Coffee Grounds" will take you to pages with specific controlls for each end effector. Finaly the "Debug Page" is a page that enables manual control over six seperate servos.

End Effector Pages:
<img width="323" alt="image" src="https://user-images.githubusercontent.com/78044374/234442240-74ab08cb-c298-48cb-84fa-687921cad01e.png">

Each page will have the same basic layout, with the "Controller Connect" button enableling the app to get inputs from a controller that is connected via bluetooth to your computer. The dropdown is for selecting the com port that the computer has designated for your Arduino microcontroller, this can be found either in device manager
<img width="387" alt="image" src="https://user-images.githubusercontent.com/78044374/234442760-902201de-159a-4a95-a61d-86fc0f394e09.png">
or in the Arduino IDE. Additionaly there are connect and disconnect buttons that will open and close communication between the app and the selected com port. The return button automaticaly closes the port.

Debug Page:
<img width="486" alt="image" src="https://user-images.githubusercontent.com/78044374/234443045-d59738cd-8149-4d1e-8a3c-03847bbfef4c.png">

The debug page has a slider that can control 6 servos independently. This page is most usefull for assembly and to zero out each servo before they are installed. The record and play buttons currently do not work.
