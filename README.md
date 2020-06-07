# vCamDesk
Webcam on Desktop with support for 3rd-Party virtual background removal (= no greenscreen required)

vCamDesk brings your webcam on your desktop as a free floating small window. 
It stays always on top of any other window or your desktop.

It also supports chromakey (alphachannel), so webcam tools like XSplit vCam or personify chromacam 
which provide virtual background removal without greenscreen are supported and the
result even takes lesser space on your screen / demo / recordings. :)

## Usage:
1) Start the program
2) Select your webcam & hit start

![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen1_startscreen.png "Initial Screen")

3) Wait for the cam to appear and drag your webcam free on top of your windows
- Use left mouse button to drag and move
- Use right mouse button to exit 

### Background removal with XSplit VCam Example
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen3_xsplit.png "XSplit VCam Example")

If you want to leverage background removal with XSplit vCam, you can support me (at no additional cost for you) by using this affiliate link: https://link.xsolla.com/uLZFgSKG

Hint: you should add a grey background, which I find best fit for all kind of demos and presentations.

### Background removal with personify chromacam Example
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen2_chromacam.png "personify chromacam Example")

You can find personify Chromacam here (which is a little more expensive than XSplit VCam):
https://personifyinc.com/products/chromacam
If you want the all in one solution, you should have a look at Personify Presenter.
https://personifyinc.com/products/presenter
It also provides a free placable chromacam on your desktop too with additional features.

### Just the webcam source Example
![alt text](https://github.com/tharmes42/vCamDesk/blob/master/pagecontent/vDeskCam_screen4_without_greenscreen.png "Just the webcam without greenscreen removal :)")


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
