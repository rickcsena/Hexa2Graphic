/********************************************************************************
 * HEXA2GRAPHIC
 * may/2015
 * by rickcsena@yahoo.com.br
 * Chart generator from hexadecimal data
 *******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

//-------------------------------------------------------------------------------

namespace HEXA2GRAPHIC
{

    //---------------------------------------------------------------------------

    public partial class frmHexa2Graphic : Form
    {
        /************************************************************************
         * Form1()
         ***********************************************************************/
        public frmHexa2Graphic()
        {
            InitializeComponent();
            this.Text = "Hexa2Graphic V" + Application.ProductVersion.Substring(0, 3);
            chtData.ChartAreas[0].AxisX.Minimum = 0;
            chtData.ChartAreas[0].AxisY.Minimum = 0;
            chtData.Series[0].ChartType = SeriesChartType.Line;
            btnSalvar.Enabled = false;
        }

        /************************************************************************
         * limparData()
         ***********************************************************************/
        private void limparData()
        {
            btnSalvar.Enabled = false;
            chtData.Series[0].Points.Clear();
            lstData.Items.Clear();
            chtData.ChartAreas[0].CursorX.SetCursorPosition(0);
            chtData.ChartAreas[0].CursorY.SetCursorPosition(0);
            chtData.ChartAreas[0].CursorX.LineColor = Color.White;
            chtData.ChartAreas[0].CursorY.LineColor = Color.White;
        }

        /************************************************************************
         * btnConverter_Click()
         ***********************************************************************/
        private void btnConverter_Click(object sender, EventArgs e)
        {
            Int32 i = 0;
            limparData();            

            try
            {
                string[] hexValuesSplit = txtHexData.Text.Split('@', '-');
                foreach (String hex in hexValuesSplit)
                {
                    // Convert the number expressed in base-16 to an integer.
                    int value = Convert.ToInt32(hex, 16);
                    // Get the character corresponding to the integral value.
                    //string stringValue = Char.ConvertFromUtf32(value);
                    //char charValue = (char)value;
                    chtData.Series[0].Points.Add(new DataPoint(i, value));
                    lstData.View = View.Details;
                    lstData.Items.Add(new ListViewItem(new string[] { i.ToString(), hex, value.ToString() }));
                    if ((i & 1) == 1) lstData.Items[i].BackColor = Color.LightBlue;
                    else lstData.Items[i].BackColor = Color.White;
                    i++;
                    //Console.WriteLine("hexadecimal value = {0}, int value = {1}, char value = {2} or {3}", hex, value, stringValue, charValue);
                    Console.WriteLine("hexadecimal value = {0}, int value = {0}", hex, value);
                }
                if (i > 1) btnSalvar.Enabled = true;
                chtData.ChartAreas[0].CursorX.LineColor = Color.Red;
                chtData.ChartAreas[0].CursorY.LineColor = Color.Red;
            }
            catch (Exception erro) 
            {
                Console.WriteLine("No catch: ");
                Console.WriteLine(erro.Message);
                limparData();
            }
        }

        /************************************************************************
         * exportListView2File()
         ***********************************************************************/
        private void exportListView2File(ListView lv, string filename, string splitter)
        {
            if (filename != "")
            {
                using (StreamWriter sw = new StreamWriter(filename + ".txt"))
                {
                    sw.WriteLine("{0}{1}{2}{3}{4}", "Index", splitter, "Hex", splitter, "Dec");
                    foreach (ListViewItem item in lv.Items)
                    {
                        sw.WriteLine("{0}{1}{2}{3}{4}", item.SubItems[0].Text, splitter, item.SubItems[1].Text, splitter, item.SubItems[2].Text);
                    }
                    sw.Close();
                }
            }
        }

        /************************************************************************
         * btnSalvar_Click()
         ***********************************************************************/
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "png files (*.png)|*.png";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                chtData.SaveImage(saveFileDialog1.FileName + ".png", ChartImageFormat.Png);
                exportListView2File(lstData, saveFileDialog1.FileName, "\t");
            }
        }

        /************************************************************************
         * lstData_SelectedIndexChanged()
         ***********************************************************************/
        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {
            chtData.ChartAreas[0].CursorX.SetCursorPosition(Convert.ToInt32(lstData.Items[lstData.FocusedItem.Index].SubItems[0].Text));
            chtData.ChartAreas[0].CursorY.SetCursorPosition(Convert.ToInt32(lstData.Items[lstData.FocusedItem.Index].SubItems[2].Text));
        }

        /********************************************************************************
         * ProcessCmdKey(ref Message msg, Keys keyData)
         * Process information when click on shortcut keys
         ********************************************************************************/

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                frmAboutBox fAboutBox = new frmAboutBox();
                fAboutBox.ShowDialog();

                return true;    // indicate that you handled this keystroke
            }

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
