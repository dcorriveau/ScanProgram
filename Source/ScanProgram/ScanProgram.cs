/*!*************************************************************
  *                     ScanProgram.cs                         *
  *                     Dylan Corriveau                        *
  *                     March 12, 2012                         *
  * This program acts a scanning program. It can take in an    *
  * image scanned in from a scanning device, as well as a BMP  *
  * file. From the image recieved, it can inverse the colours, *
  * apply grayscale, vary the level of grayscale, and print the*
  * the image. In terms of printing the image, the program also*
  * allows a customizable page type(layout, size, etc), it also*
  * a print preview to be viewed by the user before they print.*
  **************************************************************/
//!These various using statements are used to include the necessary features needed to run
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WIA;
//!\namespace ScanProgram
//!This namespace is used to link together all of the implementations and functionality found inside of the Scanning Program
namespace ScanProgram
{
    //!\class FrmScanProgram
    //!This class is used to define what is found in the form FrmScanProgram.\n
    //!It defines all menu capabilities, as well as the picture box used to store the picture.
    public partial class FrmScanProgram : Form
    {
        /*!This PageSettings variable is used to set sections of the page 
          *before it is printed (such as margins, and landscape/portrait layout)*/
        private readonly PageSettings _pgSettings = new PageSettings();
        /*!This PrintDocument variable is used to set the document that is printed*/
        private readonly PrintDocument _printDoc = new PrintDocument();
        /*!This PrinterSettings variable is used to set the printer, and any settings needed for the printer*/
        private readonly PrinterSettings _prtSettings = new PrinterSettings();
        /*!This string is used to set how the image is displayed on the page*/
        private String _imageLayout = "Actual Size";
        /*!This colour matrix is used to set the threshold for the greyscale*/
        private ColorMatrix _matrix;
        /*!This foat is used to set how the image is displayed on the page height wise when chosen*/
        private float _newHeight;
        /*!This string is used to set how the image is displayed on the page, in terms of units of measurement*/
        private GraphicsUnit _newUnits;
        /*!This foat is used to set how the image is displayed on the page width wise when chosen*/
        private float _newWidth;
        /*!this image is used to store the image in question so it can be reset*/
        private Image _original;

        /*!This constructor is used to initalize the form, 
          *as well as set the handler that is called on the prInt32 page event.\n
          *It also defines the colour matrix and sets its value to default (gray)*/
        public FrmScanProgram()
        {
            InitializeComponent();
            _printDoc.PrintPage += PrinterSetting;
            _matrix = new ColorMatrix(
                new[]
                    {
                        new[] {.3f, .3f, .3f, 0, 0},
                        new[] {.59f, .59f, .59f, 0, 0},
                        new[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    }
                );
        }
        /*!void FilePrintPreviewMenuItemClick(Object sender, EventArgs e)\n
          *This method is used to show the prInt32 preview dialog box.\n
          *It sets the document to prInt32 to the current document (which is the image given).\n
          *It also error checks the page appropriatly*/
        private void FilePrintPreviewMenuItemClick(Object sender, EventArgs e)
        {
            var dlg = new PrintPreviewDialog {Document = _printDoc};
            if (picImage.Image != null)
            {
                dlg.ShowDialog();
            }
            else
            {
                MessageBox.Show(DefaultandErrorStrings.NoImage);
            }
        }
        /*!void FilePageSetupMenuItemClick(Object sender, EventArgs e)\n
          *This method is used to show the page setup dialog box.\n 
          *It sets the page settings and printer settings, as well 
          *as allowing orientation and margins for the user*/
        private void FilePageSetupMenuItemClick(Object sender, EventArgs e)
        {
            var pageSetupDialog = new PageSetupDialog
                                      {
                                          PageSettings = _pgSettings,
                                          PrinterSettings = _prtSettings,
                                          AllowOrientation = true,
                                          AllowMargins = true
                                      };
            pageSetupDialog.ShowDialog();
        }
        /*!void PrintDocPrintPage(Object sender, EventArgs e)\n
          *This method is used to show the page setup dialog box.\n
          *It sets the page settings and printer settings, as well 
          *as allowing orientation and margins for the user*/
        private void PrintDocPrintPage(Object sender, EventArgs e)
        {
            _printDoc.DefaultPageSettings = _pgSettings;
            var printr = new PrintDialog {Document = _printDoc};
            if (picImage.Image != null)
            {
                if (printr.ShowDialog() == DialogResult.OK)
                {
                    _printDoc.Print();
                }
            }
            else
            {
                MessageBox.Show(DefaultandErrorStrings.NoImage);
            }
        }
        /*!void OpenToolStripItemClick(Object sender, EventArgs e)\n
          *This method is used to show the open file dialog box.\n 
          *It also sets the filter to bitmaps, and to check for existing files.\n
          *When it gets a file, it saves the bitmap, and places it in the picture box*/
        private void OpenToolStripItemClick(Object sender, EventArgs e)
        {
            FileDialog xDialog = new OpenFileDialog
                                     {
                                         CheckFileExists = true,
                                         Filter = DefaultandErrorStrings.Bitmaps
                                     };
            if (xDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var bmpTemp = new Bitmap(xDialog.FileName))
                    {
                        Image img = new Bitmap(bmpTemp);
                        picImage.Image = img;
                        _original = picImage.Image;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was a problem loading the image!\n" + ex.Message);
                }
            }
            xDialog.Dispose();
        }
        /*!void PrinterSetting(Object sender, PrintPageEventArgs e)\n
          *This method is used to set up the image for printing. If an image was found, 
          *it checks how the image should be laid out.\n If the image is actual size, it displays it like normal.\n 
          *If it is the size of the page, it prints it to the size of the page.\n 
          *If it is a specified size, we prompt the user for a width, height, and a unit of measurement.\n 
          *If it works, we get the proper width and height of the new image, and create an empty bitmap 
          *to contain the new image of the new size.\n Once it has been created, we create a graphic object to draw to.\n
          *we then set the graphics unit of measurement to the one recieved, and the interpolation mode to HighQualityBicubic.\n
          *In this mode, it is best used when shrinking images down, while maintaining a clear image.\n 
          *We then draw the new image and place it on the page.\n In any case, we set the page to the setting.\n*/
        private void PrinterSetting(Object sender, PrintPageEventArgs e)
        {
            if (picImage.Image != null)
            {
                switch (_imageLayout)
                {
                    case ("Actual Size"):
                        {
                            e.Graphics.DrawImage(picImage.Image, 0, 0);
                            break;
                        }
                    case ("Fit to Sheet"):
                        {
                            e.Graphics.DrawImage(picImage.Image, 0, 0, _pgSettings.PaperSize.Width,
                                                 _pgSettings.PaperSize.Height);
                            break;
                        }
                    case ("Custom Size"):
                        {
                            if (Math.Abs(_newWidth - 0) > float.Epsilon)
                            {
                                //get width and height
                                float nWidth = _newWidth/picImage.Image.Width;
                                float nHeight = _newHeight/picImage.Image.Height;
                                //find which is bigger, and get that value
                                float lrg = nHeight < nWidth ? nHeight : nWidth;
                                //find true width and height
                                var fnWidth = (int) (picImage.Image.Width*lrg);
                                var fnHeight = (int) (picImage.Image.Height*lrg);
                                //make bitmap of that size
                                var xBitmap = new Bitmap(fnWidth, fnHeight);
                                //draw on graphics
                                Graphics xGraphics = Graphics.FromImage(xBitmap);
                                //set units specified to this
                                xGraphics.PageUnit = _newUnits;
                                //set interpolation to best setting for situation
                                xGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                //draw image in graphics
                                xGraphics.DrawImage(picImage.Image, 0, 0, fnWidth, fnHeight);
                                xGraphics.Dispose();
                                //draw image on prInt32 page
                                e.Graphics.DrawImage(xBitmap, 0, 0);
                                //save image
                                picImage.Image = xBitmap;
                            }
                            break;
                        }
                }
            }
        }
        /*!void ScalingPage(Object sender, EventArgs e)\n
          *This method is used to set the prInt32 page layout for the image.\n 
          *Depending on what button was hit, it sets the image to either normal size, 
          *the size of the page, or a specified size upon printing.\n If they specify they want to use a custom size,
          *we prompt them for a size, and save it.\n*/
        private void ScalingPage(Object sender, EventArgs e)
        {
            if (picImage.Image != null)
            {
                _imageLayout = sender.ToString();
                if (_imageLayout == "Custom Size")
                {
                    var input = new InputBox();
                    input.ShowDialog();
                    if (input.Done())
                    {
                        _newWidth = input.GetWidth();
                        _newHeight = input.GetHeight();
                        _newUnits = input.GetUnits();
                    }
                }
            }
            else
            {
                MessageBox.Show(DefaultandErrorStrings.NoImage);
            }
        }
        /*!void InvertColoursToolStripMenuItemClick(Object sender, EventArgs e) \n
          *This method is used to invert the colours found in the image (if an image is found)\n
          *It does this by locking the bitmap into memory, then when it is safely stored, it gets all of the bytes,
          *and stores them in a byte array.\n Since the memory is unmanaged (we had to allocate the memory), 
          *We then use the marshall class to help move, starting at the first byte in memory till the end, 
          *to the new byte array.\n After the bytes have been moved, we take each value, and minus 255 off of it.\n 
          *Since each byte is unsigned, it will loop around to the inverted value.\n Once it is inverted, 
          *we use the marshall to put the memory back to the bitmap, unlock the memory, and set the image.\n*/
        private void InvertColoursToolStripMenuItemClick(Object sender, EventArgs e)
        {
            if (picImage.Image != null)
            {
                //get image from the picture box
                var bitmap = (Bitmap) picImage.Image;
                //create a rectangle of size bitmap
                var rectBitmap = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                //lock bits into memory
                var bitmapData = bitmap.LockBits(rectBitmap, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                //find beginning of memory
                IntPtr bitPtr = bitmapData.Scan0;
                //get size of bitmap
                Int32 sizeBitmap = Math.Abs(bitmapData.Stride)*bitmap.Height;
                //make byte array
                var rgBvalues = new byte[sizeBitmap];
                //copy into byte array
                Marshal.Copy(bitPtr, rgBvalues, 0, sizeBitmap);
                //minus 255 from rgb value (only red green and blue
                for (Int32 i = 0; i != rgBvalues.Length; i+=3)
                {
                    rgBvalues[i+0] = Convert.ToByte(255 - rgBvalues[i+0]);
                    rgBvalues[i+1] = Convert.ToByte(255 - rgBvalues[i+1]);
                    rgBvalues[i+2] = Convert.ToByte(255 - rgBvalues[i+2]);
                }
                //copy back to the bitmap
                Marshal.Copy(rgBvalues, 0, bitPtr, sizeBitmap);
                //unlock bits 
                bitmap.UnlockBits(bitmapData);
                //save bitmap
                picImage.Image = bitmap;
            }
            else
            {
                MessageBox.Show(DefaultandErrorStrings.NoImage);
            }
        }
        /*!void GreyscaleToolStripMenuItemClick(Object sender, EventArgs e)\n
          *This method is used to "greyscale" the colours(if an image is found).\n It does this by getting 
          *the graphics object, and setting the colour adjustment matrix found in the attributes of the 
          *image to the matrix described above.\n When it has set the colour matrix, it draws 
          *the new image on top of the old bitmap.\n It then disposes of the graphics used 
          *to do this, and sets the picture box to the new image.\n*/
        private void GreyscaleToolStripMenuItemClick(Object sender, EventArgs e)
        {
            if (picImage.Image != null)
            {
                //we get the image
                var bitmap = (Bitmap) picImage.Image;
                //get said image and place it in graphics
                Graphics g = Graphics.FromImage(bitmap);
                //create an image attribute
                var imgAttribs = new ImageAttributes();
                //set them to the matrix
                imgAttribs.SetColorMatrix(_matrix);
                //draw new image on new rectangle, to location bitmap, with attributes matrix
                g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                            0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imgAttribs);
                //dispose graphics
                g.Dispose();
                //assign new image
                picImage.Image = bitmap;
            }
            else
            {
                MessageBox.Show(DefaultandErrorStrings.NoImage);
            }
        }
        /*!void FindDevicesToolStripMenuItemClick(Object sender, EventArgs e)\n
          *This method is used to find any WIA devices that can be used to scan from.\n It does this by making 
          *use of the WIA class library, most notibly the device manager class.\n It prompts the user to 
          *select a device of type scanning (if a device is found).\n if a device hasn't been found, 
          *we display an error message.\n*/
        private void FindDevicesToolStripMenuItemClick(Object sender, EventArgs e)
        {
            var dialog = new CommonDialogClass();
            var wiaDM = new DeviceManagerClass();
            if (wiaDM.DeviceInfos == null || wiaDM.DeviceInfos.Count == 0)
            {
                MessageBox.Show(DefaultandErrorStrings.NoDevices);
            }
            else
            {
                dialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, true);
            }
        }
        /*!void GetImageFromDeviceToolStripMenuItemClick(Object sender, EventArgs e)\n
          *This method is used to get any scanned images from the WIA device found above.\n It does this by making 
          *use of the WIA class library, most notibly the common dialog class.\n If there are devices that can be used, 
          *we allow the user to scan an image from the device.\n once the scanning is completed, we get the image 
          *(as bytes) and place it into memory.\n We then place said memory bytes into an image, 
          *and set the picture box to the new image.\n*/
        private void GetImageFromDeviceToolStripMenuItemClick(Object sender, EventArgs e)
        {
            try
            {
                var dialog = new CommonDialogClass();
                var wiaDM = new DeviceManagerClass();
                if (wiaDM.DeviceInfos == null || wiaDM.DeviceInfos.Count == 0)
                {
                    MessageBox.Show(DefaultandErrorStrings.NoDevices);
                }
                else
                {
                    //See method comment for more information
                    ImageFile img = dialog.ShowAcquireImage(
                        WiaDeviceType.ScannerDeviceType,
                        WiaImageIntent.ColorIntent,
                        WiaImageBias.MaximizeQuality,
                        FormatID.wiaFormatJPEG);
                    if (img != null)
                    {
                        Byte[] imageBytes = (byte[]) img.FileData.get_BinaryData();
                        var ms = new MemoryStream(imageBytes);
                        picImage.Image = Image.FromStream(ms);
                        if (picImage.Image != null)
                        {
                            _original = new Bitmap(picImage.Image);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(DefaultandErrorStrings.ErrorScan + exception.Message);
            }
        }
        /*!void SetGrayThreshold(Object sender, EventArgs e)\n
          *This method is used to set the threshold for the greyscale.\n Using the colour matrix system, we set the matrix to 
          *different values for each of the buttons (lower for darker images, and higher for lighter images).\n*/
        private void SetGrayThreshold(Object sender, EventArgs e)
        {
            switch (sender.ToString())
            {
                case ("1 (dark)"):
                    {
                        //darkest
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.1f, .1f, .1f, 0, 0},
                                    new[] {.19f, .19f, .19f, 0, 0},
                                    new[] {.02f, .02f, .02f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
                case ("2"):
                    {
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.15f, .15f, .15f, 0, 0},
                                    new[] {.29f, .29f, .29f, 0, 0},
                                    new[] {.04f, .04f, .04f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
                case ("3"):
                    {
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.2f, .2f, .2f, 0, 0},
                                    new[] {.39f, .39f, .39f, 0, 0},
                                    new[] {.06f, .06f, .06f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
                case ("4"):
                    {
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.25f, .25f, .25f, 0, 0},
                                    new[] {.49f, .49f, .49f, 0, 0},
                                    new[] {.08f, .08f, .08f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
                case ("5 (normal)"):
                    {
                        //Pure Grey
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.3f, .3f, .3f, 0, 0},
                                    new[] {.59f, .59f, .59f, 0, 0},
                                    new[] {.11f, .11f, .11f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
                case ("6"):
                    {
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.4f, .4f, .4f, 0, 0},
                                    new[] {.69f, .69f, .69f, 0, 0},
                                    new[] {.21f, .21f, .21f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
                case ("7"):
                    {
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.5f, .5f, .5f, 0, 0},
                                    new[] {.79f, .79f, .79f, 0, 0},
                                    new[] {.31f, .31f, .31f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
                case ("8"):
                    {
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.6f, .6f, .6f, 0, 0},
                                    new[] {.89f, .89f, .89f, 0, 0},
                                    new[] {.41f, .41f, .41f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
                case ("9"):
                    {
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.7f, .7f, .7f, 0, 0},
                                    new[] {.99f, .99f, .99f, 0, 0},
                                    new[] {.51f, .51f, .51f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
                case ("10 (light)"):
                    {
                        //lightest
                        _matrix = new ColorMatrix(
                            new[]
                                {
                                    new[] {.8f, .8f, .8f, 0, 0},
                                    new float[] {1, 1, 1, 0, 0},
                                    new[] {.61f, .61f, .61f, 0, 0},
                                    new float[] {0, 0, 0, 1, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                }
                            );
                        break;
                    }
            }
        }
        /*!void ResetImageToolStripMenuItemClick(Object sender, EventArgs e)\n
          *This method is used to reset the image to its original state (if an image is available).\n
          *This is done by using the original image gotten at the time of scanning (or opening)*/
        private void ResetImageToolStripMenuItemClick(Object sender, EventArgs e)
        {
            if (_original != null)
            {
                picImage.Image = _original;
                _original = new Bitmap(picImage.Image);
            }
            else
            {
                MessageBox.Show(DefaultandErrorStrings.NoImage);
            }
        }
        /*!void ExitToolStripItem_Click(object sender, EventArgs e)\n
          *This method is prompts the user if they wish to leave. If yes, we close the program*/
        private void ExitToolStripItemClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(DefaultandErrorStrings.Leave,"Leave?", MessageBoxButtons.YesNo, MessageBoxIcon.Stop,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Close();
            }
        }
        /*!void SaveToolStripMenuItemClick(object sender, EventArgs e)\n
          *This method is used to prompt for the saving of the bitmap files*/
        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            //if theres an image,
            if (picImage.Image != null)
            {
                //we prompt for a save dialog, with a default extension of .bmp, and a filter to only showing bmp files
                var x = new SaveFileDialog
                                       {
                                           AddExtension = true,
                                           DefaultExt = ".bmp",
                                           Filter = DefaultandErrorStrings.Bitmaps
                                       };
                //and store the result
                DialogResult q = x.ShowDialog();
                //if it worked
                if (q == DialogResult.OK)
                {
                    //we switch the result of the save BMP file
                    if ((SaveImage.SaveBMPFile(x.FileName, (Bitmap)picImage.Image)) == 0)
                    {
                        MessageBox.Show(DefaultandErrorStrings.Save);
                    }
                }
            }
            //otherwise, say there is no image
            else
            {
                MessageBox.Show(DefaultandErrorStrings.NoImage);
            }
        }
        /*!void EncodeMessageToolStripMenuItemClick(object sender, EventArgs e)\n
          *This method is used to prompt for an encoding of a message in a bitmap*/
        private void EncodeMessageToolStripMenuItemClick(object sender, EventArgs e)
        {
            //if theres an image,
            if (picImage.Image != null)
            {
                //we prompt for a save dialog, with a default extension of .bmp, and a filter to only showing bmp files
                var x = new SaveFileDialog
                                       {
                                           AddExtension = true,
                                           DefaultExt = ".bmp",
                                           Filter = DefaultandErrorStrings.Bitmaps
                                       };
                //and store the result
                DialogResult q = x.ShowDialog();
                //if it worked
                if (q == DialogResult.OK)
                {
                    //if this succeeds, we assign the image
                    Bitmap bmp = Encoder.EncodeMessage((Bitmap) picImage.Image,x.FileName);
                    if (bmp != null)
                    {
                        MessageBox.Show("Sucessfully encoded the message!");
                        picImage.Image = bmp;
                    }
                }
            }
            //otherwise, say there is no image
            else
            {
                MessageBox.Show(DefaultandErrorStrings.NoImage);
            }
        }

        /*!void DecodeMessageToolStripMenuItemClick(object sender, EventArgs e)\n
          *This method is used to check if a message is in the bitmap, and if there is one, display it*/
        private void DecodeMessageToolStripMenuItemClick(object sender, EventArgs e)
        {
            //if there is an image, decode it
            if (picImage.Image != null)
            {
                //if a string came back...
                String[] decoded = Decoder.DecodeMessage((Bitmap) picImage.Image);
                if (decoded!=null)
                {
                    //if the message was just saying there was no message to decode, we display it
                    if(decoded.Length==1)
                    {
                        MessageBox.Show(decoded[0]);
                    }
                    //otherwise, display what was extracted
                    else
                    {
                        MessageBox.Show("Copywrite= " + decoded[0] + "\nName Entered= " + decoded[1] + "\nText Entered= " +
                                        decoded[2] + "\nSucessfully decoded the message!");
                    }
                }
            }
            //otherwise, say there is no image
            else
            {
                MessageBox.Show(DefaultandErrorStrings.NoImage);
            }
        }
    }
    /*!\class DefaultandErrorStrings
      *This public class is used to contain the strings that describe any error messages, 
      *or default text strings used in the program*/
    public class DefaultandErrorStrings
    {
        //!this string is used to set the filter used for the opening of files
        public const String Bitmaps = "Bitmap File (*.bmp)|*.bmp";
        //!this string is used in conjunction with a scanning error
        public const String ErrorScan = "An Error occured when scanning! ";
        //!this string is used to say that no device has been found
        public const String NoDevices = "No Devices Found! Please check your devices and try again";
        //!this string is used to say that no device has been selected
        public const String NoDeviceSelected = "No Device Selected! Please select a device and try again";
        //!this string is used to say that no image has been found
        public const String NoImage = "No Image Found! Please Select an Image";
        //!this string is used to ask if the user wants to leave
        public const String Leave = "Are you sure you want to leave?";
        //!this string is used to inform the user when they do not have a proper unit amount
        public const String Unit = "Invalid Units, Please Enter a proper unit";
        //!this string is used to inform the user if the save was successful
        public const String Save = "The save was successful!";
    }
}