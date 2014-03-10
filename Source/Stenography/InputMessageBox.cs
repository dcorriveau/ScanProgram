/*!*************************************************************
 *!                    InputMessageBox.cs                      *
 *!                     Dylan Corriveau                        *
 *!                     March 12, 2012                         *
 *! This form acts as an input window for the user to enter in *
 *! a string based on the information given. It is then        *
 *! returned for further processing. I would like to give      *
 *! credit to http://www.csharp-examples.net/inputbox-class/   *
 *! for most of the code.                                      *
 *!*************************************************************/
//!these using statements are used to include the necessary features needed to run
using System;
using System.Drawing;
using System.Windows.Forms;
//! \namespace ScanProgram
//! This namespace is used to link together all of the implementations and functionality found inside of the Scanning Program
namespace Stenography
{
    //! \class InputMessageBox
    //! This class is used to act as an input box for encoding strings
    internal class InputMessageBox
    {
        /*!static DialogResult InputBox(string title, string promptText, ref string value, Int32 length))\n
         *!This method is used to get a string from the user by taking in a title,
         *! a prompt text,a value to refference, and an length of string\n
         *!\return This function either a dialog result based on the option*/
        public static DialogResult InputBox(string title, string promptText, ref string value, Int32 length)
        {
            //we set all of the elements
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            //all of the titles and text
            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;
            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";

            //all of the results...
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            //the positions
            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            //the anchoring
            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            //the special options for the form, and link it all together
            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] {label, textBox, buttonOk, buttonCancel});
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            //then display the dialog box and get the result
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            if(value.Contains("\\0"))
            {
                value=value.Replace("\\0","\\ 0");
            }
            else if (value.Contains("\\a"))
            {
                value = value.Replace("\\a", "\\ a");
            }
            //if the string is too long, we cut it off
            if(value.Length>=length)
            {
                value = value.Substring(0, length - 1);
                value += "\0";
            }
            //if the string is too small, we add a \0 for the decoder, and a \n for input
            else if (value.Length < length)
            {
                value += "\0";

                while (value.Length < length)
                {
                    value += "\n";
                }
            }
            //then return the result
            return dialogResult;
        }
    }
}
