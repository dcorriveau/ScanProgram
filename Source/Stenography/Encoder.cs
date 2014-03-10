/*!*************************************************************
 *!                       Encoder.cs                           *
 *!                     Dylan Corriveau                        *
 *!                     March 12, 2012                         *
 *! This class is used to get a name and text field from the   *
 *! user, then embed the text into the bitmap. This is done    *
 *!through altering the alpha field of the bitmap.             *
 *!*************************************************************/
//!these using statements are used to include the necessary features needed to run
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//!\namespace Stenography
//!This namespace is used to link together all of the stenography parts together
namespace Stenography
{
    //!\class Encoder
    //!This class is used to embed text within a bitmap.\n
    //!It does this by altering the Alpha value within the image\n
    public class Encoder
    {
        //this byte[] is used as a key for the cryptography applied
        public static byte[] ByteKey = new byte[] { 0x27, 0x36, 0x68, 0x75, 0x45, 0x46, 0x79, 0x48, 0x31, 
            0x32, 0x33, 0x25, 0x35, 0x26, 0x37, 0x38, 0x27, 0x36, 0x68, 0x75, 0x45, 0x79, 0x47, 0x48 };
        /*!static Bitmap EncodeMessage(Bitmap bmp,String filename)\n
         *!This method is used to take a bitmap, and encrypt/embed a message into the bitmap\n
         *!\return This function either returns null for error, or the new bitmap*/

        public static Bitmap EncodeMessage(Bitmap bmp,String filename)
        {
            Bitmap returnValue=null;
            //we first get the date, and place it in the string
            DateTime today = DateTime.Today;
            String stringToEmbed = "S-"+today.ToString(String.Format("yyyy-MM-dd")) + " Copyright (c) " +
                                 DateTime.Today.Year.ToString(CultureInfo.InvariantCulture) + "\0";
            Stream passStream=new MemoryStream(ByteKey);
            string value = "Name";
            //we then get the name
            if (InputMessageBox.InputBox("BitmapText", "Please Enter the name you wish to save in the bitmap (19 letters max please)", ref value,
                                         20)
                == DialogResult.OK)
            {
                //and the text
                string value2 = "Text";
                //as well as append the name to the string
                stringToEmbed += value;
                if (
                    InputMessageBox.InputBox("BitmapText", "Please Enter the text you wish to save in the bitmap (31 letters max please)",
                                             ref value2, 32) == DialogResult.OK)
                {
                    //we append the text to the string
                    stringToEmbed += value2+"E\a";
                    //we then encrypt the message
                    Byte[] newEntry = EncryptMessage(stringToEmbed);
                    //this is used to count the bytes in ^
                    int byteCount = 0;
                    //lock bits into memory
                    BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                                         ImageLockMode.ReadWrite,
                                                         PixelFormat.Format32bppArgb);
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
                                    BfSize = (UInt32)((bitmap.Size.Width * bitmap.Size.Height * 4) + 54),
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
                                        (UInt32)(bitmap.Size.Height),
                                    BiWidth =
                                        (UInt32)(bitmap.Size.Width),
                                    BiPlanes = 1,
                                    BiBitCount = 32,
                                    BiSizeImage = 0,
                                    BiCompression = 0,
                                    BiXPelsPerMeter =
                                        (uint)Math.Round(bitmap.HorizontalResolution * SaveImage.ConversionRate),
                                    BiYPelsPerMeter =
                                        (uint)Math.Round(bitmap.VerticalResolution * SaveImage.ConversionRate),
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
                                //this value is used to store the new length within the bitmap, so the decoder knows the length
                                int colorValue = newEntry.Length;
                                //and we place it in a color (the same way as the normal bitmaps), by first taking the colorValue, and bitshifting by 2(for the red)
                                int red = colorValue >> 2;
                                //and reset the color value
                                colorValue -= red << 2;
                                //we then place the green in (by bitshifting by 1)
                                int green = colorValue >> 1;
                                //then place the value in the color, minus the green set to the normal color value
                                int blue = colorValue - (green << 1);
                                //we then make a special "color" based on the size ints,
                                Color clrSize = Color.FromArgb(0, red, green, blue);
                                //this long is used to get the encryption password location
                                long passPosition;
                                //this is the current color we are talking about
                                int currentColorComponent = 0;
                                //if the bitmap height is positive (meaning right side up)
                                if (bitmap.Height > 0)
                                {
                                    //we iterate backwards so we write the image right side up
                                    for (int yValue = bitmap.Height - 1; yValue > -1; yValue--)
                                    {
                                        //we then get a pointer to the starting scan line, plus our 
                                        //offset times the stride (row length)
                                        byte* ptr1 = (byte*)bitmapData.Scan0 + (yValue * bitmapData.Stride);
                                        //then for the current row
                                        for (int xValue = 0; xValue < bitmap.Width; xValue++)
                                        {
                                            //we first get the colors from the image, and add them to our color palette struct
                                            var color = new SaveImage.ColorPalette
                                            {
                                                Blue = ptr1[(xValue * 4)],
                                                Green = ptr1[(xValue * 4) + 1],
                                                Red = (ptr1[(xValue * 4) + 2]),
                                                Alpha = ptr1[(xValue * 4) + 3]
                                            };
                                            //if we are at the first byte, we write our new color made above
                                            if (xValue == 0 && yValue == bitmap.Height-1)
                                            {
                                                vWriter.Write(clrSize.B);
                                                vWriter.Write(clrSize.G);
                                                vWriter.Write(clrSize.R);
                                                vWriter.Write(clrSize.A);

                                            }
                                            //if not,
                                            else
                                            {
                                                //if we are past the message, just write the bytes as normal
                                                if (byteCount >= newEntry.Length)
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
                                                    var reverKey = (byte)passStream.ReadByte();
                                                    //and reset the position
                                                    passStream.Seek(passPosition, SeekOrigin.Begin);
                                                    //we then build a colour with the values we got
                                                    Color clrColor = Color.FromArgb(color.Alpha, color.Red,
                                                                                    color.Green,
                                                                                    color.Blue);
                                                    //and set the current byte to the message value, XOR'd by the reverse key
                                                    int currentByte = newEntry[byteCount] ^ reverKey;
                                                    //we then change one part of the color to store the message
                                                    SetColorComponent(ref clrColor, currentColorComponent, currentByte);
                                                    //we then rotate through the color types
                                                    currentColorComponent = (currentColorComponent == 2) ? 0 : (currentColorComponent + 1);
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
                                                Blue = ptr1[(xValue * 4)],
                                                Green = ptr1[(xValue * 4) + 1],
                                                Red = (ptr1[(xValue * 4) + 2]),
                                                Alpha = ptr1[(xValue * 4) + 3]
                                            };
                                            //if we are at the first byte, we write our new color made above
                                            if (xValue == 0 && yValue == 0)
                                            {
                                                vWriter.Write(clrSize.B);
                                                vWriter.Write(clrSize.G);
                                                vWriter.Write(clrSize.R);
                                                vWriter.Write(clrSize.A);

                                            }
                                            //if not,
                                            else
                                            {
                                                //if we are past the message, just write the bytes as normal
                                                if (byteCount >= newEntry.Length)
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
                                                    //we first get the position
                                                    passPosition = passStream.Position;
                                                    //seek to the element ^before the end
                                                    passStream.Seek(-passPosition, SeekOrigin.End);
                                                    //then get that key
                                                    var reverKey = (byte)passStream.ReadByte();
                                                    //and reset the position
                                                    passStream.Seek(passPosition, SeekOrigin.Begin);
                                                    //we then build a colour with the values we got
                                                    Color clrColor = Color.FromArgb(color.Alpha, color.Red,
                                                                                    color.Green,
                                                                                    color.Blue);
                                                    //and set the current byte to the message value, XOR'd by the reverse key
                                                    int currentByte = newEntry[byteCount] ^ reverKey;
                                                    //we then change one part of the color to store the message
                                                    SetColorComponent(ref clrColor, currentColorComponent, currentByte);
                                                    //we then rotate through the color types
                                                    currentColorComponent = (currentColorComponent == 2) ? 0 : (currentColorComponent + 1);
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
                                //we then get a string from the stream
                                returnValue = (Bitmap)Image.FromStream(vWriter.BaseStream);
                                //unlock bits 
                                bmp.UnlockBits(bitmapData);
                                //and close the stream
                                vWriter.Close();
                                //we then try to save the file. If it didn't work, we throw an exception
                                if(SaveImage.SaveBMPFile(filename, returnValue)==-1)
                                {
                                    throw new Exception("Saving Failed!");
                                }
                            }
                        }
                    }
                    catch(Exception e)
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
                        returnValue = null;
                    }
                }
            }
            //and return the bitmap
            return returnValue;
        }
        /*!static  byte[] EncryptMessage(string plainText)\n
         *!This method is used to take a string, and encrypt it\n
         *!\return This function either throws an exception if null, or the encrypted string*/
        static byte[] EncryptMessage(string plainText)
        {
            //if the string is null, we throw an exception
            if (String.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException
                       ("plainText");
            }
            //we then make a TripleDES crypto service provider 
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            //and a memory stream to write from
            var memoryStream = new MemoryStream();
            //and a new crypto stream based on the key
            var cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(ByteKey, ByteKey), CryptoStreamMode.Write);
            var writer = new StreamWriter(cryptoStream);
            //and write our string in to encrypt it
            writer.Write(plainText);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            //we then return the bytes of the encrypted data
            return Encoding.ASCII.GetBytes(Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length));
        }
        /*!void SetColorComponent(ref Color currPixel, int colorComponent,int newValue)\n
         *!This method is used to get a pixel, and change the specified color to a new value\n*/
        public static void SetColorComponent(ref Color currPixel, int colorComponent,int newValue)
        {
            //we switch the colors
            switch(colorComponent)
            {
                    //we then set the Red value, by creating a new pixel with the new value attach
                case(0):
                    {
                        currPixel = Color.FromArgb(currPixel.A, newValue, currPixel.G, currPixel.B);
                        break;
                    }
                //we then set the Green value, by creating a new pixel with the new value attach
                case (1):
                    {
                        currPixel = Color.FromArgb(currPixel.A, currPixel.R,newValue, currPixel.B);
                        break;
                    }
                //we then set the Blue value, by creating a new pixel with the new value attach
                case (2):
                    {
                        currPixel = Color.FromArgb(currPixel.A, currPixel.R, currPixel.G, newValue);
                        break;
                    }
            }
        }
    }
}