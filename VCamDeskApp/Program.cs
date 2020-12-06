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

using System;
using System.Drawing;
using System.Windows.Forms;
using nucs.JsonSettings;


namespace VCamDeskApp
{


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
		public int cropAuto { get; set; } = 1;
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


	static class Program
	{
		/// <summary>
		/// application main entrypoint
		/// </summary>
		[STAThread]
		static void Main()
		{
			//Application.Run(new OldParentForm());
			Application.Run(new ParentForm());

		}
	}

}