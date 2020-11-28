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
using System;
using System.Drawing;
using System.Windows.Forms;
using nucs.JsonSettings;
using AForge.Imaging.Filters;
using PerPixelAlphaForm;

/// <para>Our test form for this sample application.  The bitmap will be displayed in this window.</para>
class AlphaForm : VcdPerPixelAlphaForm
{
	public AlphaForm()
	{
		InitializeComponent();
		Bitmap startBmp = new Bitmap(this.frameSize.Width, this.frameSize.Height);
		using (Graphics gfx = Graphics.FromImage(startBmp))
		using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 0,0)))
		{
			gfx.FillRectangle(brush, 0, 0, this.frameSize.Width, this.frameSize.Height);
		}
		this.SetBitmap(startBmp);
		this.UpdateBitmapMethod();

	}

	private void InitializeComponent()
	{
		this.SuspendLayout();
		this.TopMost = true;
		this.ShowInTaskbar = false;
		this.ResumeLayout(false);

	}

	/*
	// Let Windows drag this form for us
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	protected override void WndProc(ref Message m)
	{
		int rightMBdown = Win32.GetKeyState(Win32.VirtualKeyStates.VK_RBUTTON) >> 8; // if right mouse button pressed, then value -1 in high byte
		//high five to https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getkeystate

		if (m.Msg == 0x0084 ) {  //WM_NCHITTEST
			if (parentForm != null)
			{
				//switch (MouseButtons)
				if (rightMBdown!=0){
					//Console.WriteLine("Right Click detected, bye!");
					parentForm.Invoke(parentForm.quitProgramDelegate);
				}

			}

			m.Result= (IntPtr)2;	// HTCLIENT
			return;
		}
		base.WndProc(ref m);
	}

*/


}

//
// Load default application setting from a JSON file in the same directory
// see https://github.com/Nucs/JsonSettings
//Step 1: create a class and inherit JsonSettings
class FormSettings : JsonSettings
{
	//Step 2: override a default FileName or keep it empty. Just make sure to specify it when calling Load!
	//This is used for default saving and loading so you won't have to specify the filename/path every time.
	//Putting just a filename without folder will put it inside the executing file's directory.
	public override string FileName { get; set; } = "vCamDesk-config.json"; //for loading and saving.

	#region Settings

	public string lastCam { get; set; } = "";
	public int flipH { get; set; } = 1;
	public int cropH { get; set; } = 1;
	public int cropV { get; set; } = 1;
	public int useTransparency { get; set; } = 0;	
	public int frameSizeWidth { get; set; } = 260;
	public int frameSizeHeight { get; set; } = 146;
	public int framePositionX { get; set; } = 100;
	public int framePositionY { get; set; } = 100;

	//[JsonIgnore] public char ImIgnoredAndIWontBeSaved { get; set; }

	#endregion
	//Step 3: Override parent's constructors
	public FormSettings() { }
	public FormSettings(string fileName) : base(fileName) { }
}




// application main
class VCamDeskApp
{
	[STAThread]
	public static void Main()
	{
		//Application.Run(new OldParentForm());
		Application.Run(new PerPixelAlphaForm.ParentForm());
	}
}
