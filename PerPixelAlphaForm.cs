//
// Copyright © 2020 by Tobias Harmes (GitHub @tharmes42)
// All rights reserved.
//
// Portions Copyright © 2002-2004 Rui Godinho Lopes
// many thanks to him for the example "PerPixelAlphaForm", this software is using parts from 
//
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED. USE IT AT YOUT OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using AForge.Imaging.Filters;



// class that exposes needed win32 gdi functions.
class Win32
{
	public enum Bool
	{
		False= 0,
		True
	};


	[StructLayout(LayoutKind.Sequential)]
	public struct Point
	{
		public Int32 x;
		public Int32 y;

		public Point(Int32 x, Int32 y) { this.x= x; this.y= y; }
	}


	[StructLayout(LayoutKind.Sequential)]
	public struct Size {
		public Int32 cx;
		public Int32 cy;

		public Size(Int32 cx, Int32 cy) { this.cx= cx; this.cy= cy; }
	}


	[StructLayout(LayoutKind.Sequential, Pack=1)]
	struct ARGB
	{
		public byte Blue;
		public byte Green;
		public byte Red;
		public byte Alpha;
	}


	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct BLENDFUNCTION
	{
		public byte BlendOp;
		public byte BlendFlags;
		public byte SourceConstantAlpha;
		public byte AlphaFormat;
	}


	public const Int32 ULW_COLORKEY = 0x00000001;
	public const Int32 ULW_ALPHA    = 0x00000002;
	public const Int32 ULW_OPAQUE   = 0x00000004;

	public const byte AC_SRC_OVER  = 0x00;
	public const byte AC_SRC_ALPHA = 0x01;

	public enum VirtualKeyStates : int
	{
		// see https://www.pinvoke.net/default.aspx/user32.getkeystate
		VK_LBUTTON = 0x01,
		VK_RBUTTON = 0x02,
		VK_CANCEL = 0x03,
		VK_MBUTTON = 0x04
	}
    

	[DllImport("user32.dll", ExactSpelling=true, SetLastError=true)]
	public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

	[DllImport("user32.dll", ExactSpelling=true, SetLastError=true)]
	public static extern IntPtr GetDC(IntPtr hWnd);

	[DllImport("user32.dll", ExactSpelling=true)]
	public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

	[DllImport("gdi32.dll", ExactSpelling=true, SetLastError=true)]
	public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

	[DllImport("gdi32.dll", ExactSpelling=true, SetLastError=true)]
	public static extern Bool DeleteDC(IntPtr hdc);

	[DllImport("gdi32.dll", ExactSpelling=true)]
	public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

	[DllImport("gdi32.dll", ExactSpelling=true, SetLastError=true)]
	public static extern Bool DeleteObject(IntPtr hObject);


	[DllImport("USER32.dll")]
	public static extern short GetKeyState(VirtualKeyStates nVirtKey);
}



/// <para>Your PerPixel form should inherit this class</para>
/// <author><name>Rui Godinho Lopes</name><email>rui@ruilopes.com</email></author>
class PerPixelAlphaForm : Form
{
	private Bitmap localCacheBitmap;


	public PerPixelAlphaForm()
	{
		InitializeComponent();
		// This form should not have a border or else Windows will clip it.

		localCacheBitmap = null;

		myDelegate = new UpdateBitmap(updateBitmapMethod);

	}

	private void InitializeComponent()
	{
		this.SuspendLayout();
		FormBorderStyle = FormBorderStyle.None;

		this.ResumeLayout(false);

	}

	///<para>Frees our bitmap.</para>
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



	/// <para>Changes the local bitmap.</para>
	public void setBitmap(Bitmap bitmap)
	{
		if (localCacheBitmap != null)
			localCacheBitmap.Dispose();
		localCacheBitmap = bitmap;
		//SetBitmap(bitmap, 255);
	}



	public void updateBitmapMethod()
	{

		// create filter
		ResizeNearestNeighbor filter = new ResizeNearestNeighbor(320, 200);
		// apply the filter
		Bitmap newImage = filter.Apply(localCacheBitmap);
		updateBitmapMethod(newImage, 255);
		newImage.Dispose();
	}

	
	/// <para>Changes the current bitmap with a custom opacity level.  Here is where all happens!</para>
	public void updateBitmapMethod(Bitmap bitmap, byte opacity)
	{
		if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
			throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

		// The ideia of this is very simple,
		// 1. Create a compatible DC with screen;
		// 2. Select the bitmap with 32bpp with alpha-channel in the compatible DC;
		// 3. Call the UpdateLayeredWindow.

		IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
		IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
		IntPtr hBitmap = IntPtr.Zero;
		IntPtr oldBitmap = IntPtr.Zero;

		try {
			hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  // grab a GDI handle from this GDI+ bitmap
			oldBitmap = Win32.SelectObject(memDc, hBitmap);

			Win32.Size size = new Win32.Size(bitmap.Width, bitmap.Height);
			Win32.Point pointSource = new Win32.Point(0, 0);
			Win32.Point topPos = new Win32.Point(Left, Top);
			Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
			blend.BlendOp             = Win32.AC_SRC_OVER;
			blend.BlendFlags          = 0;
			blend.SourceConstantAlpha = opacity;
			blend.AlphaFormat         = Win32.AC_SRC_ALPHA;

			Win32.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
		}
		finally {
			Win32.ReleaseDC(IntPtr.Zero, screenDc);
			if (hBitmap != IntPtr.Zero) {
				Win32.SelectObject(memDc, oldBitmap);
				//Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
				Win32.DeleteObject(hBitmap);
			}
			Win32.DeleteDC(memDc);
		}
	}


	protected override CreateParams CreateParams
	{
		get {
			CreateParams cp = base.CreateParams;
			cp.ExStyle |= 0x00080000; // This form has to have the WS_EX_LAYERED extended style
			return cp;
		}
	}

	public delegate void UpdateBitmap();
	public UpdateBitmap myDelegate;


}
