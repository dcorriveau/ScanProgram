/*!*************************************************************
  *                       InputBox.cs                          *
  *                     Dylan Corriveau                        *
  *                     March 12, 2012                         *
  * This program acts in conjunction with the scanning program * 
  * It gets a new width, height, and graphical unit from the   *
  * user if they specified they wanted a custom size.          * 
  **************************************************************/
//!These various using statements are used to include the necessary features needed to run

using System;
using System.Drawing;
using System.Windows.Forms;

//!\namespace ScanProgram
//!This namespace is used to link together all of the implementations and functionality found inside of the Scanning Program
namespace Stenography
{
    //!\class InputBox
    //!This class is used to define what is found in the form InputBox. 
    //!It defines all of the button code found to get the width, height, and units for the new image.
    public partial class InputBox : Form
    {
        /*!This float is used to store the height of the new image from the user*/
        private float _heightImage;
        /*!This boolean is used to say if the user entered in all fields sucessfully */
        private Boolean _success;
        /*!This GraphicsUnit is used to store the unit of measurement from the user*/
        private GraphicsUnit _unitofMeasurement;
        /*!This float is used to store the width of the new image from the user*/
        private float _widthImage;
        /*!This constructor is used to initalize the form, 
          *as well as set the sucess variable to false (to say we havn't finished)*/
        public InputBox()
        {
            InitializeComponent();
            _success = false;
        }
        /*!GraphicsUnit GetUnits()\n
          *This method is used to get the graphical units from this dialog box*/
        public GraphicsUnit GetUnits()
        {
            return _unitofMeasurement;
        }
        /*!float GetHeight()\n
          *This method is used to get the hight of the new image from this dialog box*/
        public float GetHeight()
        {
            return _heightImage;
        }
        /*!float GetWidth()\n
          *This method is used to get the width of the new image from this dialog box*/
        public float GetWidth()
        {
            return _widthImage;
        }
        /*!Boolean Done()\n
          *This method is used to say if the input box was sucessful or not*/
        public Boolean Done()
        {
            return _success;
        }
        /*!void CheckifDone(Object sender, EventArgs e)\n
          *This method is used to check if all fields are correct.\n It first checks if any fields are null, 
          *if not, it tries to parse in the fields.\n If this passes, we set the success variable to true and leave.\n 
          *If anything failed, we inform the user*/
        private void CheckifDone(object sender, EventArgs e)
        {
            if ((txtHeight.Text != null) && (txtWidth.Text != null) && (cmbUnits.SelectedItem != null))
            {
                try
                {
                    _heightImage = float.Parse(txtHeight.Text);
                    _widthImage = float.Parse(txtWidth.Text);
                    if (Enum.TryParse(cmbUnits.SelectedItem.ToString(), true, out _unitofMeasurement))
                    {
                        _success = true;
                        Close();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(DefaultandErrorStrings.Unit);
                }
            }
        }
    }
}