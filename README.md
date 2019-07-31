# McGurkEffectVR
This repo contains all the code, videos, and Unity prefabs used on the McGurk Effect VR project

1. Download the Unity package from Google Drive using the link in the text file titled Unity Package
2. Import the package into Unity
3. In Unity, go File -> Build Settings, add open scenes to build and switch the target platorm to Android, then click Switch Platform
4. In Unity, go Edit -> Project Settings -> Audio, set Spatializer Plugin and Ambisonic Decoer Plugin to Resonance Audio
5. In Unity, go Edit -> Project Settings -> Player -> Other Settings, Delete Vulkan from the list of Graphics APIs and then check the box next to Auto Graphics API. Change the Package Name to com.DefaultCompany.ProductName. Change Minimum API Level to 19.
6. In Unity, go Edit -> Project Settings -> Player -> XR Settings, ensure the box next to Virtual Reality Supported is checked, then add Oculus to the list of Virtual Reality SDKs.


Unity should be ready to build the project and upload it to the Quest headset now.

To collect the logs:
1. change the working directory to: C:\Users\\<name>\AppData\Local\Android\Sdk\platform-tools
2. check the debug memory size: adb logcat -g
3. if not 16Mb then set it to 16 Mb by: adb logcat -G 16M
4. collect the logs using: adb logcat -s Unity DEBUG -d
5. to save it: adb logcat -s Unity DEBUG -d>debug.txt
