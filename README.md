<h4>About the project</h4>
<p>
A Text-To-Speach reader that reads from the clipboard. Reads any text you copy to the clipboard. 
</p>

<h4>Getting Started</h4>
<p>Compile the solution and run it. There is a ".sln" file that can be opened with Visual Studio. It may have to be converted to work on new version of Visual Studio. You can also run the executable found in  ".\ClipRead\bin\Debug\ClipRead.exe" </p>

<h4>Prerequisites</h4>
<p>All you need is the Microsoft .NET framework for Windows and a compiler. I used Visual Studio, but you can also get the build tools separately. The latest build tools can be found here: https://www.visualstudio.com/thank-you-downloading-visual-studio/?sku=BuildTools&rel=15<p>

<p>I compiled this on version 4.0, but as far as I know, it should work fine in version 3.0. It will not work in Mono and I have not tested it on Microsoft .NET for Linux. I think they lack a speech synthesizer.</p>

<h4>Unsupported Platforms</h4>
I only support operating systems that Microsoft supports (is actively patching). This may run on older systems, but I don't test it regularly on those systems and won't investigate bugs on those systems. 

<h4>Installing</h4>
<h3>For Visual Studio:</h3>
<ol>
  <li>Double click the "ClipRead.suo".</li>

<li>Hit the "run" button (looks like a green "play" button). This will make and run a copy of the program. You can also build one with the build menu. Select "debug" or "release" to make a debug or release build. The difference between debug or release builds is outside the scope of this readme. </li>

<p>You can find both builds in the "ClipRead\bin" folder.</p>

<h3>For Non-Visual Studio:</h3>
<p>Read about this here: https://superuser.com/questions/604953/how-can-i-compile-a-net-project-without-having-visual-studio-installed.</p>

<p>I did not use "NuGet" so don't worry about that part.</p>

<h4>Automated Tests</h4>
There are no "Automated Tests" for this project. 

<h4>Deployment</h4>
<p>There is no installer for this project. For the program to run, you will need: 
"cr.ico" - the reader's icon 
"No image.png" - needed for the menu 
"ClipRead.exe" - the program itself
"License.txt" - please keep this in the folder the program resides in. 
</p>

<p>To deploy, just give the end user a folder or zip with those files in it. You can optionally give them a copy of your "pronunciation.ini". Yours is found in "My Documents" (or documents in newer systems). It's under the "ClipReader" subfolder. 
</p>

<h4>Contributing</h4>
<p>Please contact me to contribute. To reduce the chances of spammer "bots" reading my email, I will not provide it outright. The first part of it is "contributetoclipreader". There is then an "@" as usual and it ends with my domain name (jamiemarchant.info).</p>

<p>In your email, please send the source code you wish to contribute. Please wait patiently for a response as I do not check this email frequently.</p>

<h4>License</h4>
<p>The program code and "No image" icon are licensed under a "FreeBSD" style license. The other icons have their own licenses. You can read more in "License.txt".</p>




