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

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using AForge.Imaging.Filters;
using AForge.Vision.Motion;

namespace VCamDeskApp
{
	// class that exposes needed win32 gdi functions.
	class Win32
	{
		public enum Bool
		{
			False = 0,
			True
		};


		[StructLayout(LayoutKind.Sequential)]
		public struct Point
		{
			public Int32 x;
			public Int32 y;

			public Point(Int32 x, Int32 y) { this.x = x; this.y = y; }
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct Size
		{
			public Int32 cx;
			public Int32 cy;

			public Size(Int32 cx, Int32 cy) { this.cx = cx; this.cy = cy; }
		}


		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		struct ARGB
		{
			public byte Blue;
			public byte Green;
			public byte Red;
			public byte Alpha;
		}


		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct BLENDFUNCTION
		{
			public byte BlendOp;
			public byte BlendFlags;
			public byte SourceConstantAlpha;
			public byte AlphaFormat;
		}


		public const Int32 ULW_COLORKEY = 0x00000001;
		public const Int32 ULW_ALPHA = 0x00000002;
		public const Int32 ULW_OPAQUE = 0x00000004;

		public const byte AC_SRC_OVER = 0x00;
		public const byte AC_SRC_ALPHA = 0x01;

		public enum VirtualKeyStates : int
		{
			// see https://www.pinvoke.net/default.aspx/user32.getkeystate
			// better: use System.Windows.Forms.Keys
			VK_LBUTTON = 0x01,
			VK_RBUTTON = 0x02,
			VK_CANCEL = 0x03,
			VK_MBUTTON = 0x04,
			VK_OEM_PLUS = 0xBB, //For any country / region, the '+' key
			VK_OEM_COMMA = 0xBC,  //For any country / region, the ',' key
			VK_OEM_MINUS = 0xBD,  //For any country / region, the '-' key
			VK_OEM_PERIOD = 0xBE  //For any country / region, the '.' key
		}




		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern Bool DeleteDC(IntPtr hdc);

		[DllImport("gdi32.dll", ExactSpelling = true)]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern Bool DeleteObject(IntPtr hObject);


		[DllImport("USER32.dll")]
		public static extern short GetKeyState(VirtualKeyStates nVirtKey);


		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd,
						 int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();
	}



	/// <para>Your PerPixel form should inherit this class</para>
	class VcdPerPixelAlphaForm : Form
	{
		protected Bitmap localCacheBitmap;
		protected Size frameSize;
		protected Size sourceFrameSize;
		private ResizeNearestNeighbor resizeFilter; //used to resize the image, based on target image size
		private Crop cropFilter; //used to crop the image, based on source image size
		//int cropRect.Y = 0;
		//int cropRect.X = 0;
		Rectangle cropRect;



		public VcdPerPixelAlphaForm()
		{
			InitializeComponent();
			// This form should not have a border or else Windows will clip it.
			parentForm = null;
			localCacheBitmap = null;
			//this.StartPosition = FormStartPosition.Manual;


			//SetTargetFrameSizeAndCrop(new Size(320, 200));
			myDelegate = new UpdateBitmap(UpdateBitmapMethod);
			//autoZoom = new AutoZoom();


			Bitmap startBmp = new Bitmap(1, 1);
			using (Graphics gfx = Graphics.FromImage(startBmp))
			using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0)))
			{
				gfx.FillRectangle(brush, 0, 0, this.frameSize.Width, this.frameSize.Height);
			}
			SetBitmap(startBmp);
			UpdateFilters();
			UpdateBitmapMethod();

		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			//FormBorderStyle = FormBorderStyle.None;
			this.TopMost = true;
			this.ShowInTaskbar = false;
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			this.Size = new Size(0, 0);
			this.ResumeLayout(false);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VcdPerPixelAlphaForm_KeyDown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VcdPerPixelAlphaForm_MouseDown);
			this.Resize += new System.EventHandler(this.VcdPerPixelAlphaForm_Resize);

		}

		public void setParentForm(ParentForm parentForm)
		{
			this.parentForm = parentForm;
		}

		private ParentForm parentForm;

		/// <summary>
		/// Frees our bitmap.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && localCacheBitmap != null)
				{
					localCacheBitmap.Dispose();
					localCacheBitmap = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}


		/// <summary>
		/// replaces the local bitmap (including dispose of old bitmap)
		/// </summary>
		/// <param name="bitmap"></param>
		public void SetBitmap(Bitmap bitmap)
		{
			if (localCacheBitmap != null)
				localCacheBitmap.Dispose();
			localCacheBitmap = bitmap;
			sourceFrameSize = localCacheBitmap.Size;
			//SetBitmap(bitmap, 255);
		}


		/// <summary>
		/// Change target framesize to resize to and update crop information
		/// </summary>
		/// <param name="frameSize"></param>
		public void SetTargetFrameSizeAndCrop(Size frameSize)
		{
			this.frameSize = frameSize;
			cropRect = new Rectangle(new Point(0, 0), sourceFrameSize); //no initial crop
			UpdateFilters();
		}

		/// <summary>
		/// Change target framesize to resize to and update crop information
		/// </summary>
		/// <param name="borderRect"></param>
		public void SetSourceFrameSizeAndCrop(Rectangle borderRect)
		{
			cropRect = borderRect;

			//add some space
			int space = 10;
			if ((cropRect.X - space) > 0)
				cropRect.X -= space;
			if ((cropRect.Y - space) > 0)
				cropRect.Y -= space;
			if (cropRect.Right < sourceFrameSize.Width + space)
				cropRect.Width += space;
			if (cropRect.Bottom < sourceFrameSize.Height + space)
				cropRect.Height += space;

			//correct aspect ratio 
			float sourceAspectRatio = (float)sourceFrameSize.Width / (float)sourceFrameSize.Height;
			float cropAspectRatio = (float)cropRect.Width / (float)cropRect.Height;

			if (sourceAspectRatio > cropAspectRatio) {
				cropRect.Height = (int)(cropRect.Width / sourceAspectRatio);
				if (cropRect.Bottom > sourceFrameSize.Height)
                {
					cropRect.Y = 0;
					cropRect.Height = sourceFrameSize.Height;
				}
			}
			else if (sourceAspectRatio < cropAspectRatio)
			{
				cropRect.Width= (int)(cropRect.Height * sourceAspectRatio);
				if (cropRect.Right > sourceFrameSize.Width)
				{
					cropRect.X = 0;
					cropRect.Width = sourceFrameSize.Width;
				}
			}
			
			UpdateFilters();
		}

		/// <summary>
		/// create and update filters for resize and crop
		/// </summary>
		private void UpdateFilters()
		{

			//cropFilter = new Crop(new Rectangle(cropRect.X, cropRect.Y, sourceFrameSize.Width - (2 * cropRect.X), sourceFrameSize.Height - (2 * cropRect.Y)));
			cropFilter = new Crop(cropRect);
			resizeFilter = new ResizeNearestNeighbor(frameSize.Width, frameSize.Height);
			
		}

		/// <summary>
		/// enlarge target framesize
		/// </summary>
		public void Enlarge()
		{
			frameSize.Width = (int)(frameSize.Width * 1.1);
			frameSize.Height = (int)(frameSize.Height * 1.1);

			UpdateFilters();

		}

		/// <summary>
		/// shrink target framesize
		/// </summary>
		public void Shrink()
		{
			if (frameSize.Width > 100)
			{
				frameSize.Width = (int)(frameSize.Width / 1.1);
				frameSize.Height = (int)(frameSize.Height / 1.1);

				UpdateFilters();
			}
		}


		/// <summary>
		/// crop target framesize more
		/// </summary>
		public void CropMore()
		{
			//TODO: broken
			int aspectRatio = (int)(sourceFrameSize.Width / sourceFrameSize.Height);

			if (cropRect.Y < ((int)((float)sourceFrameSize.Height / 3)))
			{
				cropRect.Y = cropRect.Y + (int)((float)sourceFrameSize.Height * 0.1);
				cropRect.X = cropRect.X + (int)((float)sourceFrameSize.Width * 0.1);

				UpdateFilters();
			}
		}

		/// <summary>
		/// crop target framesize less
		/// </summary>
		public void CropLess()
		{
			//TODO: broken
			cropRect.Y = cropRect.Y - (int)((float)sourceFrameSize.Height * 0.1);
			cropRect.X = cropRect.X - (int)((float)sourceFrameSize.Width * 0.1);

			//you can and should not negative crop :)
			if (cropRect.Y < 0 || cropRect.X < 0)
			{
				cropRect.Y = 0;
				cropRect.X = 0;
			}

			UpdateFilters();
		}


		public void UpdateBitmapMethod()
		{
			try
			{
				
				//if image should be cropped apply filter, otherwise just set it to resized image
				if (cropRect.Y > 0)
				{
					Bitmap croppedImage = cropFilter.Apply(localCacheBitmap);
					Bitmap resizedImageAfterCrop = resizeFilter.Apply(croppedImage);
					frameSize = resizedImageAfterCrop.Size;
					//update image
					UpdateBitmapMethod(resizedImageAfterCrop, 255);


					resizedImageAfterCrop.Dispose();
					croppedImage.Dispose();
				}
				else
				{
					// resize image
					Bitmap resizedImage = resizeFilter.Apply(localCacheBitmap);
					frameSize = resizedImage.Size;
					//update image
					UpdateBitmapMethod(resizedImage, 255);
					//free temp bitmap
					resizedImage.Dispose();

				}

				//update form size
				//this.Size = new Size(frameSize.Width + 10, frameSize.Height + 10);
				

			}
			catch (System.ArgumentException e)
			{
				//this happens if localCacheBitmap is disposed?
			}
		}


		/// <para>Changes the current bitmap with a custom opacity level.  Here is where all happens!</para>
		public void UpdateBitmapMethod(Bitmap bitmap, byte opacity)
		{
			//if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
			//throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

			// The ideia of this is very simple,
			// 1. Create a compatible DC with screen;
			// 2. Select the bitmap with 32bpp with alpha-channel in the compatible DC;
			// 3. Call the UpdateLayeredWindow.

			IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
			IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
			IntPtr hBitmap = IntPtr.Zero;
			IntPtr oldBitmap = IntPtr.Zero;

			try
			{
				this.Size = bitmap.Size;
				hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  // grab a GDI handle from this GDI+ bitmap
				oldBitmap = Win32.SelectObject(memDc, hBitmap);

				Win32.Size size = new Win32.Size(bitmap.Width, bitmap.Height);
				Win32.Point pointSource = new Win32.Point(0, 0);
				Win32.Point topPos = new Win32.Point(Left, Top);
				Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
				blend.BlendOp = Win32.AC_SRC_OVER;
				blend.BlendFlags = 0;
				blend.SourceConstantAlpha = opacity;
				blend.AlphaFormat = Win32.AC_SRC_ALPHA;

				Win32.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
			}
			finally
			{
				Win32.ReleaseDC(IntPtr.Zero, screenDc);
				if (hBitmap != IntPtr.Zero)
				{
					Win32.SelectObject(memDc, oldBitmap);
					//Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
					Win32.DeleteObject(hBitmap);
				}
				Win32.DeleteDC(memDc);
			}
		}


		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x00080000; // This form has to have the WS_EX_LAYERED extended style
				return cp;
			}
		}

		public delegate void UpdateBitmap();
		public UpdateBitmap myDelegate;

		private void VcdPerPixelAlphaForm_KeyDown(object sender, KeyEventArgs e)
		{
			//TODO
			// https://www.codeproject.com/Articles/11114/Move-window-form-without-Titlebar-in-C
			//DO SOMETHING
			switch (e.KeyCode)
			{
				case Keys.Oemplus:
					//Console.WriteLine("+");
					Enlarge();
					break;
				case Keys.OemMinus:
					//Console.WriteLine("-");
					Shrink();
					break;
				case Keys.Oemcomma:
					//Console.WriteLine(",");
					CropMore();
					break;
				case Keys.OemPeriod:
					//Console.WriteLine(".");
					CropLess();
					break;

					/*default:
						//Console.WriteLine("Other key");
						break;*/
			}

		}

		private void VcdPerPixelAlphaForm_MouseDown(object sender, MouseEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButtons.Left:
					Win32.ReleaseCapture();
					Win32.SendMessage(Handle, Win32.WM_NCLBUTTONDOWN, Win32.HT_CAPTION, 0);
					break;
				case MouseButtons.Right:
					//Console.WriteLine("Right Click detected, bye!");
					parentForm.Invoke(parentForm.quitProgramDelegate);
					break;
			}
		}


		/// <summary>
		/// on resize event (also see WndProc override)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void VcdPerPixelAlphaForm_Resize(object sender, EventArgs e)
		{
			frameSize.Width = this.Width;
			frameSize.Height = this.Height;
			UpdateFilters();
		}

		/// <summary>
		/// override on resize to be able to maintain aspect ratio
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if ( m.Msg == 0x214) 
			{ // WM_SIZING
			  // Keep the window square
				RECT rc = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
				int w = rc.Right - rc.Left;
				int h = rc.Bottom - rc.Top;
				
				//correct aspect ratio 
				float sourceAspectRatio = (float)sourceFrameSize.Width / (float)sourceFrameSize.Height;
				float rcAspectRatio = (float)w / (float)h;

				if (sourceAspectRatio > rcAspectRatio)
				{
					h = (int)(w / sourceAspectRatio);
				}
				else if (sourceAspectRatio < rcAspectRatio)
				{
					w = (int)(h * sourceAspectRatio);
				}

				//int z = w > h ? w : h;
				rc.Bottom = rc.Top + h;
				rc.Right = rc.Left + w;
				Marshal.StructureToPtr(rc, m.LParam, false);
				m.Result = (IntPtr)1;
				return;
			}
			base.WndProc(ref m);
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
	}
}