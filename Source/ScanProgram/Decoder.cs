/*!*************************************************************
 *!                       Decoder.cs                           *
 *!                     Dylan Corriveau                        *
 *!                     March 12, 2012                         *
 *! This class is used to get a name and text field embedded in*
 *! the bitmap, then display it to the user.                   *
 *!*************************************************************/
//!these using statements are used to include the necessary features needed to run
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//!\namespace ScanProgram
//!This namespace is used to link together all of the implementations and functionality found inside of the Scanning Program

namespace ScanProgram
{
    //!\class Decoder
    //!This class is used get embedded text from a bitmap.\n
    //!It does this by reading the Alpha value within the image\n
    internal class Decoder
    {
        /*!static String[] DecodeMessage(Bitmap bmp)\n
         *!This method is used to take a bitmap, and decrypt a message from the bitmap\n
         *!\return This function either returns null for error, or the information read*/

        internal static String[] DecodeMessage(Bitmap bmp)
        {
            String[] returnValue = null;
            //lock bits into memory
            BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                                 ImageLockMode.ReadWrite,
                                                 bmp.PixelFormat);
            Stream passStream = new MemoryStream(Encoder.ByteKey);
            //length of the message
            Int32 messageLength = 0;
            //get size of bitmap
            Int32 sizeBitmap = Math.Abs(bitmapData.Stride)*bmp.Height;
            //make byte array
            var rgBvalues = new byte[sizeBitmap];
            //find beginning of memory
            IntPtr bitPtr = bitmapData.Scan0;
            //copy into byte array
            Marshal.Copy(bitPtr, rgBvalues, 0, sizeBitmap);
            //we get the stride value in order to get the spot to start from
            Int32 stride = ((bmp.Width*32) + 7)/8;
            //we also make an array list to store the bytes
            var xList = new ArrayList();
            //this is used to count the bytes in the message
            int byteCount = 0;
            //this is the current color we are talking about
            int currentColorComponent = 0;
            //as well as declare a binary writer to write out the data
            var vWriter = new BinaryWriter(new MemoryStream());
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
                        var bitmap = new Bitmap(bmp.Width, bmp.Height, stride, PixelFormat.Format32bppArgb,
                                                new IntPtr(ptr));
                        var x = new SaveImage.Headinfo
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
                        var infoBitmapinfoheader = new SaveImage.BitmapInfoHead
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
                                                               (uint)
                                                               Math.Round(bitmap.HorizontalResolution*
                                                                          SaveImage.ConversionRate),
                                                           BiYPelsPerMeter =
                                                               (uint)
                                                               Math.Round(bitmap.VerticalResolution*
                                                                          SaveImage.ConversionRate),
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
                        //this long is used to get the encryption password location
                        long passPosition;
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
                                    var color = new SaveImage.ColorPalette
                                                    {
                                                        Blue = ptr1[(xValue*4)],
                                                        Green = ptr1[(xValue*4) + 1],
                                                        Red = (ptr1[(xValue*4) + 2]),
                                                        Alpha = ptr1[(xValue*4) + 3]
                                                    };
                                    //if we are at position 0, we get the first value, and extract the message using the noise bit fashion
                                    if (xValue == 0 && yValue == bitmap.Height-1)
                                    {
                                        Color pxColor = Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
                                        messageLength = (pxColor.R << 2) + (pxColor.G << 1) + pxColor.B;
                                    }
                                        //if not,
                                    else
                                    {
                                        //if we are past the message, just write the bytes as normal
                                        if (byteCount >= messageLength)
                                        {
                                            vWriter.Write(color.Blue);
                                            vWriter.Write(color.Green);
                                            vWriter.Write(color.Red);
                                            vWriter.Write(color.Alpha);
                                        }
                                            //otherwise,
                                        else
                                        {
                                            //if the password position is at the length, we reset the position
                                            if (passStream.Position == passStream.Length)
                                            {
                                                passStream.Seek(0, SeekOrigin.Begin);

                                            }
                                            //we first get the positino
                                            passPosition = passStream.Position;
                                            //seek to the element ^before the end
                                            passStream.Seek(-passPosition, SeekOrigin.End);
                                            //then get that key
                                            var reverKey = (byte) passStream.ReadByte();
                                            //and reset the position
                                            passStream.Seek(passPosition, SeekOrigin.Begin);
                                            //we then build a colour with the values we got
                                            Color clrColor = Color.FromArgb(color.Alpha, color.Red,
                                                                            color.Green,
                                                                            color.Blue);
                                            //Extract the hidden message-byte from the color
                                            var foundByte =
                                                (byte) (reverKey ^ GetColorComponent(clrColor, currentColorComponent));
                                            //and add it to our list
                                            xList.Add(foundByte);
                                            //we then rotate through the color types
                                            currentColorComponent = (currentColorComponent == 2)
                                                                        ? 0
                                                                        : (currentColorComponent + 1);
                                            //write out the values
                                            vWriter.Write(clrColor.B);
                                            vWriter.Write(clrColor.G);
                                            vWriter.Write(clrColor.R);
                                            vWriter.Write(clrColor.A);
                                            //and add to the byte count
                                            byteCount++;
                                        }
                                    }

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
                                    var color = new SaveImage.ColorPalette
                                                    {
                                                        Blue = ptr1[(xValue*4)],
                                                        Green = ptr1[(xValue*4) + 1],
                                                        Red = (ptr1[(xValue*4) + 2]),
                                                        Alpha = ptr1[(xValue*4) + 3]
                                                    };
                                    //if we are at position 0, we get the first value, and extract the message using the noise bit fashion
                                    if (xValue == 0 && yValue == 0)
                                    {
                                        Color pxColor = Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
                                        messageLength = (pxColor.R << 2) + (pxColor.G << 1) + pxColor.B;
                                    }
                                        //if not,
                                    else
                                    {
                                        //if we are past the message, just write the bytes as normal
                                        if (byteCount >= messageLength)
                                        {
                                            vWriter.Write(color.Blue);
                                            vWriter.Write(color.Green);
                                            vWriter.Write(color.Red);
                                            vWriter.Write(color.Alpha);
                                        }
                                            //otherwise,
                                        else
                                        {
                                            //if the password position is at the length, we reset the position
                                            if (passStream.Position == passStream.Length)
                                            {
                                                passStream.Seek(0, SeekOrigin.Begin);

                                            }
                                            //we first get the positino
                                            passPosition = passStream.Position;
                                            //seek to the element ^before the end
                                            passStream.Seek(-passPosition, SeekOrigin.End);
                                            //then get that key
                                            var reverKey = (byte) passStream.ReadByte();
                                            //and reset the position
                                            passStream.Seek(passPosition, SeekOrigin.Begin);
                                            //we then build a colour with the values we got
                                            Color clrColor = Color.FromArgb(color.Alpha, color.Red,
                                                                            color.Green,
                                                                            color.Blue);
                                            //Extract the hidden message-byte from the color
                                            var foundByte =
                                                (byte) (reverKey ^ GetColorComponent(clrColor, currentColorComponent));
                                            //and add it to our list
                                            xList.Add(foundByte);
                                            //we then rotate through the color types
                                            currentColorComponent = (currentColorComponent == 2)
                                                                        ? 0
                                                                        : (currentColorComponent + 1);
                                            //write out the values
                                            vWriter.Write(clrColor.B);
                                            vWriter.Write(clrColor.G);
                                            vWriter.Write(clrColor.R);
                                            vWriter.Write(clrColor.A);
                                            //and add to the byte count
                                            byteCount++;
                                        }
                                    }

                                }
                            }
                        }
                        //unlock bits 
                        bmp.UnlockBits(bitmapData);
                        //we then check if count is not 0. if it is, we say there was no data, otherwise, we decrypt what we have
                        returnValue = xList.Count != 0
                                          ? DecryptStringFromBytes((byte[]) xList.ToArray(typeof (byte)))
                                          : new[] {"There is no data encrypted in this bitmap!"};
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An Error Occured in Encoding! " + e.Message);
                //we try to unlock the bits if they are lost
                try
                {
                    bmp.UnlockBits(bitmapData);
                }
                catch
                {
                }
            }
            return returnValue;

        }

        /*!static string[] DecryptStringFromBytes(byte[] textToDecrypt)\n
         *!This method is used to take a byte array, and return a split up string\n
         *!\return This function either throws an exception if null, or the decrypted string*/

        private static string[] DecryptStringFromBytes(byte[] textToDecrypt)
        {
            string[] returnValue;
            try
            {
                //we first turn the bytes into a string
                String str = Encoding.ASCII.GetString(textToDecrypt);
                //if the string is empty, we throw an exception
                if (String.IsNullOrEmpty(str))
                {
                    throw new ArgumentNullException("textToDecrypt");
                }
                //we then make a TripleDES crypto service provider 
                var cryptoProvider = new TripleDESCryptoServiceProvider();
                //and a memory stream to read from
                var memoryStream = new MemoryStream(Convert.FromBase64String(str));
                //and a new crypto stream based on the key
                var cryptoStream = new CryptoStream(memoryStream,
                                                    cryptoProvider.CreateDecryptor(Encoder.ByteKey, Encoder.ByteKey),
                                                    CryptoStreamMode.Read);
                var reader = new StreamReader(cryptoStream);
                //and read in the full string
                var fullString = reader.ReadToEnd();
                //we then split the array on the \0
                var array = fullString.Split('\0');
                //we then return the string if the special characters are found, or invalid string if they are not.
                if ((fullString.IndexOf("S-", StringComparison.Ordinal) != -1) &&
                    (fullString.IndexOf("E\a", StringComparison.Ordinal) != -1))
                {
                    returnValue = new[] {array[0].Remove(0, 4), array[1].TrimStart('\n'), array[2].TrimStart('\n')};
                }
                else
                {
                    returnValue = new[] {"This data is not using ours!"};
                }
            }
                //if an exception occured, we say that it is not our data
            catch (Exception)
            {
                returnValue = new[] {"This data is not using ours!"};
            }
            return returnValue;
        }
        /*!byte GetColorComponent(Color pixelColor, int colorComponent))\n
         *!This method is used to return a specific color value from a pixel\n
         *!\return This function returns the color specified (either R, G or B)*/
        private static byte GetColorComponent(Color pixelColor, int colorComponent)
        {
            //this byte is used to store the color
            byte returnValue = 0;
            //we switch the colors
            switch (colorComponent)
            {
                    //we then either get the Red
                case (0):
                    {
                        returnValue = pixelColor.R;
                        break;
                    }
                    //Green
                case (1):
                    {
                        returnValue = pixelColor.G;
                        break;
                    }
                    //or Blue value 
                case (2):
                    {
                        returnValue = pixelColor.B;
                        break;
                    }
            }
            //and return it
            return returnValue;
        }
    }
}