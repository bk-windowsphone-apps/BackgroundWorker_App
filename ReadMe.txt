
# Read Me

This app uses BackgrondWorker thread which calls HttpWebRequest service for downloading documents (10 jpgs, 5 pdfs and 5 txt files) from the www.sjsu.edu.

HttpWebRequest is fully Asynchronous in Windows Phone 8 and runs in the background. However, the assignment's requirement was to utilize BackgroundWorker thread. So, I implemented BackgroundWorker which then calls HttpWebRequest. HttpWebRequest makes the connection and webresponse is stream to SaveToFile method for saving the downloaded files to IsolatedStorageFile.

Windows Phone 8 supports 2 API for connection to the web - HttpWebRequest and WebClient. These restricted Win32 version APIs are limited for Async implementation only. 

How to run this program in Emulator.

Requirements
=============
Windows 8 Operating System
Hyper-V Enabled PC for Emulator
Visual Studio 2012-2013

1. Double click on the the visual studio project file in the BackgroundWorker_App directory.  
2. When project is loaded in the Visual Studio, click on the Emulator Play button to run the app.
3. Once the app is installed in the Emulator, click on BackgroundWorker_App to launch the app.
4. Click on appropriate buttons to download files.
5. Once downloading is done, Emulator caches the files, so if you want to run download again, you have to first uninstall the app and reinstall it.
6. If you want to view the downloaded files in IsolatedStorage, you can use IsolatedStorageTool to copy the files from Emulator to local PC's file system.