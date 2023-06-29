# HoloLens-Eye-Tracking

This package is meant to test the eye tracking capabilities of a HoloLens 2.

## Specs
Unity version: 2019.4.30f1

MRTK version: 2.7.0

Visual Studio version: 2019

## Deployment Instructions
Unity build settings can be found [here](https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/build-and-deploy-to-hololens).

Build instructions in Unity and Microsoft Visual Studio can be found [here](https://learn.microsoft.com/en-us/training/modules/learn-mrtk-tutorials/1-7-exercise-hand-interaction-with-objectmanipulator?ns-enrollment-type=learningpath&ns-enrollment-id=learn.azure.beginner-hololens-2-tutorials).

Instructions for deploying an application to the HoloLens 2 via the device portal can be found [here](https://learn.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-the-windows-device-portal).

### Build the App in Unity

1. Click on File -> Build Settings to open the Build Settings window.
2. Make sure the Target Device is HoloLens, the Architecture is ARM64, and Build Configuration is set to Release.
3. Add Sample Scene to the Scenes in Build list. This is the only scene needed - the other scenes are not functional.
4. Hit Build and specify your build directory.

### Build the .appx file in Visual Studio

1. After building the app in Unity, a Windows Explorer window will open. Navigate to your build directory, where there should be a Visual Studio solution file (.sln). Click on this file to open it in Visual Studio.
2. Right-click on the project in the solution explorer and click Publish -> Create App Packages.
3. Click through the prompts (for the most part, the default options are ok). Make sure that the build version is Release, and the architecture is ARM64.
4. Click Build. After a few minutes, this should create a .appx file within your specified directory.

### Deploy to the HoloLens

1. Connect to the HoloLens 2 device portal via web browser.
2. Navigate to Views -> Apps and select the Local Storage tab.
3. Click Choose File and select the .appx file you generated in Visual Studio.
4. Click Install. The application should now be installed on your HoloLens 2.

