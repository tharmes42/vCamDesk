/*
MIT License

Copyright(c) 2020 Tobias Harmes

Portions Copyright � 2002-2004 Rui Godinho Lopes
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
using System;
using System.Drawing;
using System.Windows.Forms;




/// <para>Our test form for this sample application.  The bitmap will be displayed in this window.</para>
class AlphaForm : PerPixelAlphaForm
{
	public AlphaForm()
	{
		InitializeComponent();
		parentForm = null;
	}

	private void InitializeComponent()
	{
		this.SuspendLayout();
		this.TopMost = true;
		this.ShowInTaskbar = false;
		this.ResumeLayout(false);

	}

	// Let Windows drag this form for us
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	protected override void WndProc(ref Message m)
	{
		int keyDown = Win32.GetKeyState(Win32.VirtualKeyStates.VK_RBUTTON) >> 8; // if right mouse button pressed, then value -1 in high byte
		//high five to https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getkeystate

		if (m.Msg == 0x0084 /*WM_NCHITTEST*/) {
			if (keyDown!=0){
				//parentForm.Invoke(parentForm.myDelegate);			
				if (parentForm != null)
				{
					//Console.WriteLine("Right Click detected, bye!");
					parentForm.Invoke(parentForm.quitProgramDelegate);
				}
			}
			m.Result= (IntPtr)2;	// HTCLIENT
			return;
		}
		base.WndProc(ref m);
	}

	public void setParentForm(ParentForm parentForm)
	{
		this.parentForm = parentForm;
	}

	private ParentForm parentForm;

}



///<para>The "controller" dialog box.</para>
class ParentForm : Form
{
	public ParentForm()
	{
		//Font= new Font("tahoma", 8);
		Text= "vCamDesk";
		FormBorderStyle = FormBorderStyle.FixedDialog;
		MinimizeBox = false;
		MaximizeBox = false;
		ClientSize = new Size(350, 160);
		StartPosition = FormStartPosition.CenterScreen;
		frameSize = new Size(300, 180);
		noTransparencyCounter = 0;
		
		InitializeComponent();

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
			for (int i = 1, n = videoDevices.Count; i <= n; i++)
			{
				string cameraName = i + " : " + videoDevices[i - 1].Name;

				camera1Combo.Items.Add(cameraName);
				//my preferred webcam is XSplit VCam
				if (videoDevices[i - 1].Name.Equals("XSplit VCam"))
                {
					preferredCam = i-1;
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
	}


	///<para>Constructs and initializes all child controls of this dialog box.</para>
	private void InitializeComponent()
	{

		this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MyForm_FormClosing);


		// 
		// camera1Combo
		// 
		camera1Combo = new ComboBox();
		camera1Combo.FormattingEnabled = true;
		camera1Combo.Location = new System.Drawing.Point(10, 20);
		camera1Combo.Name = "camera1Combo";
		camera1Combo.Size = new System.Drawing.Size(322, 21);
		camera1Combo.TabIndex = 3;
		Controls.Add(camera1Combo);

		// 
		// startButton
		// 
		startButton = new Button();
		startButton.Location = new System.Drawing.Point(10, 50);
		startButton.Name = "startButton";
		startButton.Size = new System.Drawing.Size(75, 23);
		startButton.TabIndex = 4;
		startButton.Text = "&Start";
		startButton.UseVisualStyleBackColor = true;
		startButton.Click += new System.EventHandler(startButton_Click);
		Controls.Add(startButton);
		// 
		// stopButton
		// 
		stopButton = new Button();
		stopButton.Enabled = false;
		stopButton.Location = new System.Drawing.Point(10, 80);
		stopButton.Name = "stopButton";
		stopButton.Size = new System.Drawing.Size(75, 23);
		stopButton.TabIndex = 5;
		stopButton.Text = "S&top";
		stopButton.UseVisualStyleBackColor = true;
		stopButton.Click += new System.EventHandler(this.stopButton_Click);
		Controls.Add(stopButton);

		//
		// horizontal flip yes/no (yes is default)
		//
		flipHCheckBox = new CheckBox();
		flipHCheckBox.Location = new System.Drawing.Point(10, 110);
		flipHCheckBox.Text = "Flip horizontal";
		flipHCheckBox.Checked = true;
		Controls.Add(flipHCheckBox);

		//
		// use transparency yes/no (yes is default)
		//
		useTransparencyCheckBox = new CheckBox();
		useTransparencyCheckBox.Location = new System.Drawing.Point(10, 130);
		useTransparencyCheckBox.Text = "Transparency";
		useTransparencyCheckBox.Checked = true;
		Controls.Add(useTransparencyCheckBox);

		//
		// resolution selected
		//
		resolutionLabel = new Label();
		resolutionLabel.Location = new System.Drawing.Point(10, 170);
		resolutionLabel.Text = "";
		resolutionLabel.AutoSize = true;
		Controls.Add(resolutionLabel);


		// 
		// videoSourcePlayer1
		// 
		videoSourcePlayer1 = new VideoSourcePlayer();
		//videoSourcePlayer1.BackColor = System.Drawing.SystemColors.ControlDark;
		//videoSourcePlayer1.BackColor = System.Drawing.Color.White;
		videoSourcePlayer1.BorderColor = System.Drawing.Color.Transparent;
		//videoSourcePlayer1.ForeColor = System.Drawing.Color.Transparent;
		videoSourcePlayer1.Location = new System.Drawing.Point(10, 31);
		videoSourcePlayer1.Name = "videoSourcePlayer1";
		videoSourcePlayer1.Size = new System.Drawing.Size(177, 100);
		videoSourcePlayer1.TabIndex = 6;
		videoSourcePlayer1.VideoSource = null;
		videoSourcePlayer1.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(videoSourcePlayer1_NewFrame);
		Controls.Add(videoSourcePlayer1);


		// alphaForm will contain the per-pixel-alpha dib
		alphaForm = new AlphaForm();
		alphaForm.setFrameSize(frameSize);
		alphaForm.setParentForm(this);
		alphaForm.Show();
	}


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
					int alphaPixel = sourceBitmap.GetPixel(1, 1).A;
					if (alphaPixel > 0)
                    {

						//if transparency is checked, but the picture has no transparency alpha channel top left 
						//check limit and count up
						if (noTransparencyCounter > 60)  //which is about 1-3 seconds
                        {
							//disable transparency feature since source has no transparent borders to avoid flickering
							useTransparency = false;
                        }
						else { 
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
				alphaForm.setBitmap(sourceBitmap);
				//ping alphaForm to update bitmap
				alphaForm.Invoke(alphaForm.myDelegate);
			}
			catch (Exception e)
			{
				MessageBox.Show(this, e.Message, "Error updating frame. Sorry :/", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


	// On form closing
	private void MyForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		StopCameras();
	}

	// On "Start" button click
	private void startButton_Click(object sender, EventArgs e)
	{
		StartCameras();
		startButton.Enabled = false;
		stopButton.Enabled = true;
		#if RELEASE
		 global::ParentForm.ActiveForm.Hide();
		#endif
	}

	// On "Stop" button click
	private void stopButton_Click(object sender, EventArgs e)
	{
		StopCameras();

		startButton.Enabled = true;
		stopButton.Enabled = false;

		#if DEBUG
			sourceBitmap.Save("snapshot.png");
		#endif
		this.Dispose();

	}

	// Start cameras
	private void StartCameras()
	{
		//default index is 0
		int resolutionIndex = 0;
		// create first video source
		videoSource1 = new VideoCaptureDevice(videoDevices[camera1Combo.SelectedIndex].MonikerString);
		
		//should the webcam feed be flipped?
		if (flipHCheckBox.Checked) { 
			//eventhandler to flip the picture
			videoSource1.NewFrame += new NewFrameEventHandler(videoSource1_NewFrame);
		}

		//you tranparency be used?
		useTransparency = useTransparencyCheckBox.Checked;

		if (videoSource1.VideoCapabilities.Length > 0) {
			videoSource1.VideoResolution = videoSource1.VideoCapabilities[resolutionIndex]; //It selects the default size
			resolutionLabel.Text = videoSource1.VideoCapabilities[resolutionIndex].FrameSize.ToString();
			float aspectRatio = (float)videoSource1.VideoCapabilities[resolutionIndex].FrameSize.Height / (float)videoSource1.VideoCapabilities[resolutionIndex].FrameSize.Width;
			frameSize.Height = (int)(frameSize.Width * aspectRatio);

			
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
		alphaForm.setFrameSize(frameSize);
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

	private int noTransparencyCounter; // every frame without transparency is counted, if limit of successively frames is exceeded turn of tranparency feature
	private AlphaForm alphaForm;	// form to display videofeed with alpha transparency
	private Bitmap sourceBitmap; // bitmap froum video source player
	private Bitmap localCacheBitmap; // bitmap used in transformations
	private VideoSourcePlayer videoSourcePlayer1; // Video Source Player
	private VideoCaptureDevice videoSource1; // selected video source
	private Size frameSize;
	private ComboBox camera1Combo; // Combobox for source webcam
	private Button startButton;
	private Button stopButton;
	private CheckBox flipHCheckBox; // flip horizontal yes/no
	private CheckBox useTransparencyCheckBox; // use transparency yes/no checkbox
	private bool useTransparency; // use transparency yes/no
	private Label resolutionLabel; //displays selection resolution
	FilterInfoCollection videoDevices; // list of video devices

	public delegate void QuitProgramDelegate();
	public QuitProgramDelegate quitProgramDelegate;

}



// application main
class VCamDeskApp
{
	[STAThread]
	public static void Main()
	{
		Application.Run(new ParentForm());
	}
}
