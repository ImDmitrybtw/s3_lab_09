using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static s3_lab_09_2.Forecast;

namespace s3_lab_09_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void  button1_Click(object sender, EventArgs e)
        {
            Forecast forecast = new Forecast();
            Weather[] results = await Task.Run(async() => { return await forecast.getForecastAsync(); });
            RichTextBox tBox = richTextBox1;
            foreach (Weather wthr in results)
            {

                tBox.Text += String.Format("{0,4} {1, 70} {2, 70} {3, 45} \n", wthr.Country, wthr.Name, wthr.Description, wthr.Temperature);
                await Task.Delay(500);
            }
        }
    }
}
