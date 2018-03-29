# HRI-HoloVive
======

HRI-HoloVive is a HRI Environment for simulated and real time control of a robot arm for multiple users. This project is built with part of the HoloViveObserver project by Drew Gottlieb (@dag10) found [here](https://github.com/dag10/HoloViveObserver). Currently it supports a Hololens user and a Vive user through Unity Multiplayer on a shared wifi network.  Both users can interact with the environment using the Vive controllers, eventually controllers will be able to move and control the virtual robot arm. 
 The model for the UR5 robot arm was provided by Long Qian(@qian256) through their UR5 for Unity project found [here](https://github.com/qian256/ur5_unity).

---
## Building
Building this project is largely the same as building the HoloViveObserver project. You must first build for the hololens and then accept the SteamVR settings before building for the Vive. 

Do NOT update the SteamVR provided in the project. It is modified for this project and updating will break the system.

More detailed building directions will be provided when we reach version 1.0, though for the most part the instructions from the HoloViveObserver project are the same. 

---
##Usage

First initiate the scene for the Vive by pressing the play button within unity, then start up the Hololens program. 

---
##To do:

 - Control of robot model using vive controllers
 - Full body models for users
 - Full body avateering for users
 - Full body tracking for Hololens user
 - Control of physical robot.  
 - Robot status information

---
 

