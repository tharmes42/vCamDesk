/*
MIT License

Copyright(c) 2020 Tobias Harmes

Portions Copyright © 2002-2004 Rui Godinho Lopes
many thanks to him for the example "PerPixelAlphaForm", this software is using parts from 

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/


using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using nucs.JsonSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VCamDeskApp
{
	public partial class ParentForm : Form
    {
        public ParentForm()
        {

			InitializeComponent();
			InitializeAdditionalComponents();

		}

		//Constructs and initializes all child controls of this dialog box.
		private void InitializeAdditionalComponents()
		{

			//JsonSettings Step 4: Load
			parentFormSettings = JsonSettings.Load<FormSettings>(); //relative path to executing file.


			// horizontal flip yes/no (yes is default)
			flipHCheckBox.Checked = (parentFormSettings.flipH == 1) ? true : false;

			// crop horizontal yes/no (no is default)
			cropAutoCheckBox.Checked = (parentFormSettings.cropAuto == 1) ? true : false;

			

			// use transparency yes/no (yes is default)
			useTransparencyCheckBox.Checked = (parentFormSettings.useTransparency == 1) ? true : false;

			// resolution default
			resolutionLabel.Text = "          n/a        ";


			// 
			// videoSourcePlayer1
			// 
			videoSourcePlayer1 = new VideoSourcePlayer();
			videoSourcePlayer1.BorderColor = System.Drawing.Color.Transparent;
			videoSourcePlayer1.Location = new System.Drawing.Point(10, 31);
			videoSourcePlayer1.Name = "videoSourcePlayer1";
			videoSourcePlayer1.Size = new System.Drawing.Size(177, 100);
			videoSourcePlayer1.TabIndex = 6;
			videoSourcePlayer1.VideoSource = null;
			videoSourcePlayer1.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(videoSourcePlayer1_NewFrame);
			Controls.Add(videoSourcePlayer1);


			// alphaForm will contain the per-pixel-alpha dib
			alphaForm = new AlphaForm();
			// place alphaForm to the last position, if it is not out of screen dimension
			var myScreen = Screen.FromControl(this);
			if (((parentFormSettings.framePositionX + parentFormSettings.frameSizeWidth) > myScreen.Bounds.Width) ||
				((parentFormSettings.framePositionY + parentFormSettings.frameSizeHeight) > myScreen.Bounds.Height))
			{
				//reset location if it is out of screen
				parentFormSettings.framePositionX = 100;
				parentFormSettings.framePositionY = 100;
			}
			alphaForm.SetDesktopLocation(parentFormSettings.framePositionX, parentFormSettings.framePositionY);
			alphaForm.SetTargetFrameSizeAndCrop(frameSize);
			alphaForm.setParentForm(this);
			alphaForm.Show();


			frameSize = new Size(parentFormSettings.frameSizeWidth, parentFormSettings.frameSizeHeight);
			aspectRatio = (float)frameSize.Width / (float)frameSize.Height;

			noTransparencyCounter = 0;

			//InitializeComponent();

			quitProgramDelegate = new QuitProgramDelegate(QuitProgramMethod);


			// show device list
			try
			{
				// enumerate video devices
				videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
				int preferredCam = 0;
				if (videoDevices.Count == 0)
				{
					throw new Exception();
				}
				for (int i = 0, n = videoDevices.Count; i < n; i++)
				{
					//string cameraName = i + " : " + videoDevices[i - 1].Name;

					camera1Combo.Items.Add(videoDevices[i].Name);
					//get last cam by name
					if (videoDevices[i].Name.Equals(parentFormSettings.lastCam))
					{
						preferredCam = i;
					}
				}
				camera1Combo.SelectedIndex = preferredCam;
			}
			catch
			{
				startButton.Enabled = false;
				camera1Combo.Items.Add("No cameras found");
				camera1Combo.SelectedIndex = 0;
				camera1Combo.Enabled = false;
			}

			this.Text = this.Text + " Build: " + Application.ProductVersion; 
		}//initializeComponents2

		//on new frame update alphaForm via delegate 
		private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
		{
			//get frame from running webcam
			sourceBitmap = videoSourcePlayer1.GetCurrentVideoFrame();
			if (sourceBitmap != null)
			{

				try
				{
					if (useTransparency)
					{
						//make transparent to get alpha channel information
						sourceBitmap.MakeTransparent();
						//get top left pixel alpha value
						int alphaPixel = sourceBitmap.GetPixel(1, 1).A;
						//is top left pixel alpha non-transparent?
						if (alphaPixel > 0)
						{

							//if transparency is checked, but the picture has no transparency alpha channel top left 
							//check limit and count up
							if (noTransparencyCounter > 60)  //which is about 1-3 seconds
							{
								//disable transparency feature since source has no transparent borders to avoid flickering
								useTransparency = false;
								//reduce the framesize again, since transparency is not used
								frameSize.Width = (int)(frameSize.Width / 1.2);
								frameSize.Height = (int)(frameSize.Width / aspectRatio);
								alphaForm.SetTargetFrameSizeAndCrop(frameSize);

							}
							else
							{
								noTransparencyCounter++;
								//then drop the frame to avoid flickering
								sourceBitmap.Dispose();
								sourceBitmap = null;
								return;
							}
						}
						else
						{
							//we haven transparency, reset counter :)
							noTransparencyCounter = 0;
						}
						//send sourceBitmap to alphaForm
						//alphaForm.setBitmap(GraphicTools.ResizeImage(sourceBitmap, alphaFormWidth, alphaFormHeight));
						//alphaForm.setBitmap(sourceBitmap);
					}
					else
					{
						//localCacheBitmap = ReplaceTransparency(sourceBitmap, System.Drawing.Color.White);
						//sourceBitmap = localCacheBitmap;					
					}
					alphaForm.SetBitmap(sourceBitmap);
					//ping alphaForm to update bitmap
					alphaForm.Invoke(alphaForm.myDelegate);
				}
				catch (Exception e)
				{
					//MessageBox.Show(this, e.Message, "Error updating frame. Sorry :/", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Console.WriteLine(e.Message + "\n Error updating frame. Sorry :/");
					this.Dispose();
					//return;
				}

			}
		}



		private void videoSource1_NewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			//eventArgs.Frame.RotateFlip(RotateFlipType.Rotate180FlipNone);
			eventArgs.Frame.RotateFlip(RotateFlipType.RotateNoneFlipX);

		}

		// Start cameras
		private void StartCameras()
		{
			//default index is 0
			int resolutionIndex = 0;
			// create first video source
			videoSource1 = new VideoCaptureDevice(videoDevices[camera1Combo.SelectedIndex].MonikerString);

			//should the webcam feed be flipped?
			if (flipHCheckBox.Checked)
			{
				//eventhandler to flip the picture
				videoSource1.NewFrame += new NewFrameEventHandler(videoSource1_NewFrame);
			}

			//you tranparency be used?
			useTransparency = useTransparencyCheckBox.Checked;
			if (useTransparency)
			{
				frameSize.Width = (int)(frameSize.Width * 1.2); //enlarge the framesize by 20% if transparency is used
			}

			if (videoSource1.VideoCapabilities.Length > 0)
			{
				videoSource1.VideoResolution = videoSource1.VideoCapabilities[resolutionIndex]; //It selects the default size
				resolutionLabel.Text = videoSource1.VideoCapabilities[resolutionIndex].FrameSize.ToString();
				aspectRatio = (float)videoSource1.VideoCapabilities[resolutionIndex].FrameSize.Width / (float)videoSource1.VideoCapabilities[resolutionIndex].FrameSize.Height;
				frameSize.Height = (int)(frameSize.Width / aspectRatio);


				/*		for (int i = 0; i < videoSource1.VideoCapabilities.Length; i++)
						{
							string resolution = "Resolution Number " + Convert.ToString(i);
							string resolution_size = videoSource1.VideoCapabilities[i].FrameSize.ToString();
							Console.WriteLine("resolution , resolution_size>> " + resolution + "" + resolution_size);
						}
						resolutionIndex = 6;
						videoSource1.VideoResolution = videoSource1.VideoCapabilities[resolutionIndex];
						float aspectRatio = (float)videoSource1.VideoCapabilities[resolutionIndex].FrameSize.Height / (float)videoSource1.VideoCapabilities[resolutionIndex].FrameSize.Width;
						alphaFormHeight = (int)(alphaFormWidth * aspectRatio);
				*/
			}
			videoSourcePlayer1.VideoSource = videoSource1;
			videoSourcePlayer1.Start();

			//set Crop Settings on alphaForm
			alphaForm.CropAuto = cropAutoCheckBox.Checked;
			//update target size and activate crop
			alphaForm.SetTargetFrameSizeAndCrop(frameSize);

		}

		// Stop cameras
		private void StopCameras()
		{
			videoSourcePlayer1.SignalToStop();
			//videoSourcePlayer1.WaitForStop();
		}

		//delegate method to be invoked from outside
		public void QuitProgramMethod()
		{
			this.Show();
			parentFormSettings.framePositionX = alphaForm.DesktopLocation.X;
			parentFormSettings.framePositionY = alphaForm.DesktopLocation.Y;
			alphaForm.Hide();
			stopButton_Click(this, null);
		}


		public static System.Drawing.Bitmap ReplaceTransparency(System.Drawing.Bitmap bitmap, System.Drawing.Color background)
		{
			/* Important: you have to set the PixelFormat to remove the alpha channel.
			 * Otherwise you'll still have a transparent image - just without transparent areas */
			var result = new System.Drawing.Bitmap(bitmap.Size.Width, bitmap.Size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			var g = System.Drawing.Graphics.FromImage(result);
			g.Clear(background);
			g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
			g.DrawImage(bitmap, 0, 0);
			return result;
		}

		private FormSettings parentFormSettings; //contains the form settings to save them permanent to file
		private int noTransparencyCounter; // every frame without transparency is counted, if limit of successively frames is exceeded turn of tranparency feature
		private VideoSourcePlayer videoSourcePlayer1; // Video Source Player
		private VideoCaptureDevice videoSource1; // selected video source
		private AlphaForm alphaForm;    // form to display videofeed with alpha transparency
		private Bitmap sourceBitmap; // bitmap froum video source player
		private Size frameSize; // target size of frame
		private float aspectRatio; // aspectRation auf frame
		private bool useTransparency; // use transparency yes/no
		FilterInfoCollection videoDevices; // list of video devices

		public delegate void QuitProgramDelegate();
		public QuitProgramDelegate quitProgramDelegate;



		// On "Stop" button click
		private void stopButton_Click(object sender, EventArgs e)
		{

			StopCameras();

			startButton.Enabled = true;
			stopButton.Enabled = false;

			parentFormSettings.Save();

			this.Dispose();

		}

		// On "Start" button click
		private void startButton_Click(object sender, EventArgs e)
        {
			StartCameras();
			startButton.Enabled = false;
			stopButton.Enabled = true;
			ActiveForm.Hide();
		}

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (stopButton.Enabled)
			{
				StopCameras();
			}
		}

        private void camera1Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
			ComboBox comboBox = (ComboBox)sender;

			//save the name of the cam to settings
			parentFormSettings.lastCam = (string)comboBox.SelectedItem;
		}

        private void useTransparencyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			CheckBox checkBox = (CheckBox)sender;

			//save use of transparency flag
			parentFormSettings.useTransparency = checkBox.Checked ? 1 : 0;
		}

        private void flipHCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			CheckBox checkBox = (CheckBox)sender;

			//save use of transparency flag
			parentFormSettings.flipH = checkBox.Checked ? 1 : 0;
		}

        private void cropAutoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			CheckBox checkBox = (CheckBox)sender;

			//save use of crop horizontal flag
			parentFormSettings.cropAuto = checkBox.Checked ? 1 : 0;
		}


    }
}
