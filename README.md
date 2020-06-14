# vCamDesk
Webcam on Desktop with support for 3rd-Party virtual background removal (= no greenscreen required)

vCamDesk brings your webcam on your desktop as a free floating small window. 
It stays always on top of any other window or your desktop.

It also supports chromakey (alphachannel), so webcam tools like Snap Camera, XSplit vCam or personify chromacam 
which provide virtual background removal without greenscreen are supported and the
result even takes lesser space on your screen / demo / recordings. :)

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
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen4_without_greenscreen.png "Just the webcam without greenscreen removal :)")


## Building & dependencies
Note: If you want to build this, make sure to get the latest AForge.NET C# Libraries at
http://aforgenet.com/framework/

## Thanks!
I would not have figured it out to get this alphachannel thing working in c# without the excellent demo by Rui Godinho Lopes, so many thanks to him for his PerPixelAlphaForm example.
And thanks to all the people working on AForge.net-Libraries to provide nice usable DirectShow-Support to c#-newbies. 

## Thoughts
I wrote this program because a lot of tools have some kind of silly webcam display 
if I make demos and screenrecordings. Though I am a big fan of OBS Studio
it is sometimes overkill and sometimes I don't notice when my webcam is on top
of content from my screen. To avoid this, a free movable webcam window was
what I needed :)

I even tried to pull this off with Snap Camera from Snapchat, but the green
background does not provide the necessary informations so the result looked, well
extraterrestrial. 
