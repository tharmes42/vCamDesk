# vCamDesk
Webcam on Desktop with support for XSplit vCam and virtual background removal

vCamDesk brings your webcam on your desktop as a free floating small window. 
It stays ontop of any other window or your desktop.

## Usage:
1) Start the program
2) Select your webcam & hit start
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen1_startscreen.png "Initial Screen")
3) Wait for the cam to appear and drag your webcam free on top of your windows
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen3_xsplit.png "XSplit VCam Example")
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen2_chromacam.png "personify chromacam")
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen4_without_greenscreen.png "Just the webcam without greenscreen removal :)")

## Special virtual background feature: 
If you have the tool XSplit VCam, you can also leverage from the alpha channel support. 
Meaning: you can use XSplit vCam to remove your background and your webcam
output even takes lesser space on your screen / demo / recordings.

Only thing in XSplit VCam: you should add a grey background, which I find best
fit for all kind of demos and presentations. Other colors will always shine through
at the border of your webcam image with removed background. E.g. if you use a green
background you 

If you want to leverage background removal you need XSplit vCam, which you can
get here (affiliate link, no additional cost for you, but your purchase supports this tool):
https://link.xsolla.com/uLZFgSKG

If you want the all in one solution, you should have a look at Personify Presenter.
https://personifyinc.com/products/presenter
It provides a free placable chromacam on your desktop too with additional features.

## Building
Note: If you want to build this, make sure to get the latest AForge.NET C# Libraries at
http://aforgenet.com/framework/

## Thoughts
I wrote this program because a lot of tools have kind of silly webcam display 
if I make demos and screenrecordings. Though I am a big fan of OBS Studio
it is sometimes overkill and sometimes I don't notice when my webcam is on top
of content from my screen. To avoid this, a free movable webcam window was
what I needed :)

I even tried to pull this off with Snap Camera from Snapchat, but the green
background does not provide the necessary informations so the result looked, well
extraterrestrial. 
