# McGurkEffectVR
This repo contains all the code, videos, and Unity prefabs used on the McGurk Effect VR project

1.Download the Unity package from Google Drive using the link in the text file titled Unity Package
2.Import the package into Unity
3.In Unity, go File -> Build Settings, add open scenes to build and switch the target platorm to Android, then click Switch Platform
4.In Unity, go Edit -> Project Settings -> Audio, set Spatializer Plugin and Ambisonic Decoer Plugin to Resonance Audio
5.In Unity, go Edit -> Project Settings -> Player -> Other Settings, Delete Vulkan from the list of Graphics APIs and then check the box next to Auto Graphics API. Change the Package Name to com.DefaultCompany.ProductName. Change Minimum API Level to 19.
6.In Unity, go Edit -> Project Settings -> Player -> XR Settings, ensure the box next to Virtual Reality Supported is checked, then add Oculus to the list of Virtual Reality SDKs.

Unity should be ready to build the project and upload it to the Quest headset now.
