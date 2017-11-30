Getting Started
Compile the solution and run it. There is a ".sln" file that can be opened with Visual Studio. It may have to be converted to work on new version of Visual Studio. You can also run the executable found in  ".\ClipRead\bin\Debug\ClipRead.exe" 

Prerequisites
All you need is the Microsoft .NET framework for Windows and a compiler. I used Visual Studio, but you can also get the build tools separately. The latest build tools can be found here: https://www.visualstudio.com/thank-you-downloading-visual-studio/?sku=BuildTools&rel=15. 

 I compiled this on version 4.0, but as far as I know, it should work fine in version 3.0. It will not work in Mono and I have not tested it on Microsoft .NET for Linux. I think they lack a speech synthesizer. 

Unsupported Platforms 
I only support operating systems that Microsoft supports (is actively patching). This may run on older systems, but I don't test it regularly on those systems and won't investigate bugs on those systems. 

Installing
For Visual Studio: 
1. Double click the "ClipRead.suo".

2.  Hit the "run" button (looks like a green "play" button). This will make and run a copy of the program. You can also build one with the build menu. Select "debug" or "release" to make a debug or release build. The difference between debug or release builds is outside the scope of this readme. 

You can find both builds in the "ClipRead\bin" folder. 

For Non-Visual Studio:
Read about this here: https://superuser.com/questions/604953/how-can-i-compile-a-net-project-without-having-visual-studio-installed.

I did not use "NuGet" so don't worry about that part. 

Automated Tests
There are no "Automated Tests" for this project. 

Deployment
There is no installer for this project. For the program to run, you will need: 
"cr.ico" - the reader's icon 
"No image.png" - needed for the menu 
"ClipRead.exe" - the program itself
"License.txt" - please keep this in the folder the program resides in. 

To deploy, just give the end user a folder or zip with those files in it. You can optionally give them a copy of your "pronunciation.ini". Yours is found in "My Documents" (or documents in newer systems). It's under the "ClipReader" subfolder. 

Contributing
Please contact me to contribute. To reduce the chances of spammer "bots" reading my email, I will not provide it outright. The first part of it is "contributetoclipreader". There is then an "@" as usual and it ends with my domain name (jamiemarchant.info). 

In your email, please send the source code you wish to contribute. Please wait patiently for a response as I do not check this email frequently. 

License
The program code and "No image" icon are licensed under a "FreeBSD" style license. The other icons have their own licenses. You can read more in "License.txt".




