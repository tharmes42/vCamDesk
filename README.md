# vCamDesk
Shows your webcam on your desktop with support for 3rd-Party virtual background removal (= no greenscreen required)

vCamDesk brings your webcam on your desktop as a free floating small window. 
It stays always on top of any other window or your desktop.

It also supports chromakey (alphachannel), so webcam tools like Snap Camera, XSplit vCam or personify chromacam 
which provide virtual background removal without greenscreen are supported and the
result even takes lesser space on your screen / demo / recordings. :)

## IMPORTANT: PROJECT NOT MAINTAINED -> Use new app WebcamOnDesktop
I have created the program new from scratch as an universal windows app to make is easier and safer to distribute the app. The new app lacks the greenscreen / alpha channel function, because this is not available with the current windows SDK. But the most important things are working.

Have a look at it: [WebcamOnDesktop](https://github.com/tharmes42/WebcamOnDesktop)

## Download
You find the latest releases under https://github.com/tharmes42/vCamDesk/releases

## Usage:
1) Start the program
2) Select your webcam & hit start

![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen1_startscreen.png "Initial Screen")

3) Wait for the cam to appear and drag your webcam free on top of your windows
- Use left mouse button to drag and move
- Use right mouse button to exit 

### Background removal for free with Snap Camera
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/snapchat_filter.png "Snap Camera Example")

You can get Snap Camera at https://snapcamera.snapchat.com/. Be sure you copy&paste this url in snap camera to get my snapchat chromakey filter lense: https://www.snapchat.com/unlock/?type=SNAPCODE&uuid=c62e1aee41734de28841b30bc88ff716&metadata=01
Then you can select Snap Camera and there you go!

### Background removal with XSplit VCam example
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen3_xsplit.png "XSplit VCam Example")

If you want to leverage background removal with XSplit vCam, you can support me (at no additional cost for you) by using this affiliate link: https://link.xsolla.com/uLZFgSKG

Hint: you should add a grey background, which I find fits best for all kind of demos and presentations.

### Background removal with personify chromacam example
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen2_chromacam.png "personify chromacam Example")

You can find personify Chromacam here (which is a little more expensive than XSplit VCam):
https://personifyinc.com/products/chromacam
If you want the all in one solution, you should have a look at Personify Presenter.
https://personifyinc.com/products/presenter
It also provides a free placable chromacam on your desktop too with additional features.

### Just the webcam source example
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/no-autocrop.jpg "Just the webcam without greenscreen removal :)")

Using auto-crop it can remove unneeded background details:

![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/autocrop1.jpg "Auto-detection of relevant areas by moving around")
Auto-detection of relevant areas by moving around

![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/autocrop2.jpg "cropped image with less details like the door in the background")
cropped image with less details like the door in the background

## Building & dependencies
Note: If you want to build this, make sure to get the nuget-packages referenced in this project:
- Package Aforge.* / the latest AForge.NET C# Libraries at http://aforgenet.com/framework/
- Package ILMerge
- Package Newtonsoft.Json and nucs.JsonSettings

## Thanks!
I would not have figured it out to get this alphachannel thing working in c# without the excellent demo by Rui Godinho Lopes, so many thanks to him for his PerPixelAlphaForm example.
And thanks to all the people working on AForge.net-Libraries to provide nice usable DirectShow-Support to c#-newbies. 

## Thoughts
I wrote this program because a lot of tools have some kind of silly webcam display 
if I make demos and screenrecordings. Though I am a big fan of OBS Studio
it is sometimes overkill and sometimes I don't notice when my webcam is on top
of content from my screen. To avoid this, a free movable webcam window was
what I needed :)
