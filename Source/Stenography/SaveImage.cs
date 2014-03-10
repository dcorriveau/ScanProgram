/*!*************************************************************
 *                     SaveImage.cs                            *
 *                    Dylan Corriveau                          *
 *                   February 6, 2012                          *
 * This program builds upon the scanning program, by adding in *
 * the ability to save the bitmap image given. It does this by *
 * taking the bytes found in the file, and rewriting the file, *
 * using the header information and the bitmap data given.     *
 ***************************************************************/
//!These various using statements are used to include the necessary features needed to run

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
//!\namespace Stenography
//!This namespace is used to link together all of the stenography parts together
using System.Windows.Forms;

namespace Stenography
{ 
    //!\class SaveImage
    //!This class is used to define the saving capabilities of the scanning program\n
    //!It defines all bitmap image manipulation used to save to a file
    public class SaveImage
    {
        //!This Double is used as the conversion rate to define how the horizontal and vertical resolution is converted to meters
        public const Double ConversionRate = 39.3700787;
        /*!static Int32 SaveBMPFile(String filename, Int32 width, Int32 height, Image im)\n
         *!This method is used to prompt for the saving of the bitmap files,
         *! by taking in a filename, a width, a height, and an image file\n
         *!\return This function either returns 0 for success, and -1 for error*/
        public static Int32 SaveBMPFile(String filename, Bitmap im)
        {
            //we use this return value to say if it passed or failed
            Int32 returnValue = 0;
            //we first get the bitmap for the image
            var bm = im;
            //lock bits into memory
            BitmapData bitmapData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height),
                                                         ImageLockMode.ReadWrite,bm.PixelFormat);
            //get size of bitmap
            Int32 sizeBitmap = Math.Abs(bitmapData.Stride)*bm.Height;
            //make byte array
            var rgBvalues = new byte[sizeBitmap];
            //find beginning of memory
            IntPtr bitPtr = bitmapData.Scan0;
            //copy into byte array
            Marshal.Copy(bitPtr, rgBvalues, 0, sizeBitmap);
            //we get the stride value in order to get the spot to start from
            Int32 stride = ((im.Width*32) + 7)/8;
            //as well as declare a binary writer to write out the data
            BinaryWriter vWriter = null;
            //we then try to...
            try
            {
                unsafe
                {
                    //we get a byte pointer to the byte values gathered
                    fixed (byte* ptr = rgBvalues)
                    {
                        //then using a new bitmap, consisting of the width,height,stride, 
                        //pixel format, and a new pointer based on the rgb pointer...
                        var bitmap = new Bitmap(bm.Width, bm.Height, stride, PixelFormat.Format32bppArgb,
                                                new IntPtr(ptr));
                        //and open the file for writing
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }
                        //we then create it
                        vWriter = new BinaryWriter(File.Create(filename));
                        //we then declare the header information about the image
                        var x = new Headinfo
                                    {
                                        BfType = 0x4D42,
                                        BfSize = (UInt32) ((bitmap.Size.Width*bitmap.Size.Height*4) + 54),
                                        BfReserved1 = 0,
                                        BfReserved2 = 0,
                                        BfOffBits = 54
                                    };
                        //and sleep for 30 seconds (to avoid a .NET GDI+ error)
                        Thread.Sleep(30);
                        //we then save the differing parts of the header
                        vWriter.Write(x.BfType);
                        vWriter.Write(x.BfSize);
                        vWriter.Write(x.BfReserved1);
                        vWriter.Write(x.BfReserved2);
                        vWriter.Write(x.BfOffBits);
                        //we then create a second part of the header
                        var infoBitmapinfoheader = new BitmapInfoHead
                                                       {
                                                           BiSize = 40,
                                                           BiHeight =
                                                               (UInt32) (bitmap.Size.Height),
                                                           BiWidth =
                                                               (UInt32) (bitmap.Size.Width),
                                                           BiPlanes = 1,
                                                           BiBitCount = 32,
                                                           BiSizeImage = 0,
                                                           BiCompression = 0,
                                                           BiXPelsPerMeter =
                                                               (uint) Math.Round(bitmap.HorizontalResolution*ConversionRate),
                                                           BiYPelsPerMeter =
                                                               (uint) Math.Round(bitmap.VerticalResolution*ConversionRate),
                                                           BiClrUsed = 0,
                                                           BiClrImportant = 0
                                                       };

                        //and save all parts of the information header into the file
                        vWriter.Write(infoBitmapinfoheader.BiSize);
                        vWriter.Write(infoBitmapinfoheader.BiWidth);
                        vWriter.Write(infoBitmapinfoheader.BiHeight);
                        vWriter.Write(infoBitmapinfoheader.BiPlanes);
                        vWriter.Write(infoBitmapinfoheader.BiBitCount);
                        vWriter.Write(infoBitmapinfoheader.BiSizeImage);
                        vWriter.Write(infoBitmapinfoheader.BiCompression);
                        vWriter.Write(infoBitmapinfoheader.BiXPelsPerMeter);
                        vWriter.Write(infoBitmapinfoheader.BiYPelsPerMeter);
                        vWriter.Write(infoBitmapinfoheader.BiClrUsed);
                        vWriter.Write(infoBitmapinfoheader.BiClrImportant);
                        //if the bitmap height is positive (meaning right side up)
                        if (bitmap.Height > 0)
                        {
                            //we iterate backwards so we write the image right side up
                            for (int yValue = bitmap.Height - 1; yValue > -1; yValue--)
                            {
                                //we then get a pointer to the starting scan line, plus our 
                                //offset times the stride (row length)
                                byte* ptr1 = (byte*) bitmapData.Scan0 + (yValue*bitmapData.Stride);
                                //then for the current row
                                for (int xValue = 0; xValue < bitmap.Width; xValue++)
                                {
                                    //we first get the colors from the image, and add them to our color palette struct
                                    var color = new ColorPalette
                                                    {
                                                        Blue = ptr1[xValue*4],
                                                        Green = ptr1[(xValue*4) + 1],
                                                        Red = ptr1[(xValue*4) + 2],
                                                        Alpha = ptr1[(xValue*4) + 3]
                                                    };
                                    //then write out its respected values
                                    vWriter.Write(color.Blue);
                                    vWriter.Write(color.Green);
                                    vWriter.Write(color.Red);
                                    vWriter.Write(color.Alpha);
                                }
                            }
                        }
                            //if the bitmap height is negative (meaning upside down)
                        else
                        {
                            //we then go forwards, so the image is written right side up
                            for (int yValue = 0; yValue < bitmap.Height; yValue++)
                            {
                                //we then get a pointer to the starting scan line, plus our 
                                //offset times the stride (row length)
                                byte* ptr1 = (byte*) bitmapData.Scan0 + (yValue*bitmapData.Stride);
                                //then for the current row
                                for (int xValue = 0; xValue < bitmap.Width; xValue++)
                                {
                                    //we first get the colors from the image, and add them to our color palette struct
                                    var color = new ColorPalette
                                                    {
                                                        Blue = ptr1[xValue*4],
                                                        Green = ptr1[(xValue*4) + 1],
                                                        Red = ptr1[(xValue*4) + 2],
                                                        Alpha = ptr1[(xValue*4) + 3]
                                                    };
                                    //then write out its respected values
                                    vWriter.Write(color.Blue);
                                    vWriter.Write(color.Green);
                                    vWriter.Write(color.Red);
                                    vWriter.Write(color.Alpha);
                                }
                            }
                        }
                        //unlock bits 
                        bm.UnlockBits(bitmapData);
                    }
                }
            }
                //or if an error occured, we display a message and leave
            catch (Exception e)
            {
                MessageBox.Show("An Error Occured Within Saving!\n" + e.Message);
                returnValue = -1;
                //we try to unlock the bits if they are lost
                try
                {
                    bm.UnlockBits(bitmapData);
                }
                catch
                {
                }
            }
            //if the file was opened, we close it
            if (vWriter != null)
            {
                vWriter.Close();
            }
            //we then return the return value
            return returnValue;
        }
        //!\struct BitmapInfoHead
        //!This struct is used to define the header information used to write the bitmap
        internal struct BitmapInfoHead
        {
            //! This UInt32 is used to define the Number of bits per pixel (either 1, 4, 8, 16, 24, or 32)
            public UInt32 BiBitCount;
            //! This UInt32 is used to define the amount of important colors (default is 0)
            public UInt32 BiClrImportant;
            //! This UInt32 is used to define the amount of colors used (default is 0)
            public UInt32 BiClrUsed;
            //! This UInt16 is used to define the compression rate (default is 0)
            public UInt16 BiCompression;
            //! This UInt32 is used to specify the height of the bitmap, in pixels
            public UInt32 BiHeight;
            //! This UInt16 is used to define the planes used (default is 1)
            public UInt16 BiPlanes;
            //! This UInt32 is used to define the size of the header
            public UInt32 BiSize;
            //! This UInt32 is used to define the size of the image, not including header
            public UInt32 BiSizeImage;
            //! This UInt32 is used to specify the width of the bitmap, in pixels
            public UInt32 BiWidth;
            //! This UInt32 is used to define the number of x bits per meter (default is 0
            public UInt32 BiXPelsPerMeter;
            //! This UInt32 is used to define the number of x bits per meter (default is 0)
            public UInt32 BiYPelsPerMeter; 
        }
        //!\struct Headinfo
        //!This struct is used to define the main header information used to write to the file
        internal struct Headinfo
        {
            //! This UInt32 is used to define the starting position of image data in bytes 
            public UInt32 BfOffBits;
            //! This UInt16 is used to define the first reserved space (it is always 0)
            public UInt16 BfReserved1;
            //! This UInt16 is used to define the second reserved space (it is always 0)
            public UInt16 BfReserved2;
            //! This UInt32 is used to define the size of the file in bytes
            public UInt32 BfSize;
            //! This UInt16 is used to define the file type. It is always 4D42h ("BM") 
            public UInt16 BfType;
        }
        //!\struct ColorPalette
        //!This struct is used to define the colors as they are written to the file
        internal class ColorPalette
        {
            //!this byte defines the blue found in the colour
            public Byte Blue;
            //!this byte defines the green found in the colour
            public Byte Green;
            //!this byte defines the red found in the colour
            public Byte Red;
            //!this byte defines the Alpha found in the colour
            public Byte Alpha;
        }
    }
}