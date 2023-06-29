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
