using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MigradorZeosParaADO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBox2.Text;

            var result = folderBrowserDialog1.ShowDialog();

            if (result != DialogResult.OK)
                return;

            var path = folderBrowserDialog1.SelectedPath;

            // Working with .pas
            foreach (var targetFile in Directory.EnumerateFiles(path, "*.pas", SearchOption.AllDirectories))
            {
                var fileText = File.ReadAllText(targetFile, Encoding.Default);

                textBox1.AppendText(targetFile + Environment.NewLine);
                
                Application.DoEvents();

                File.WriteAllText(targetFile, ZeosToAdo.PasUpdate(fileText), Encoding.Default);
            }

            // Working with .dfm
            foreach (var targetFile in Directory.EnumerateFiles(path, "*.dfm", SearchOption.AllDirectories))
            {
                var fileText = File.ReadAllText(targetFile, Encoding.Default);

                textBox1.AppendText(targetFile + Environment.NewLine);

                Application.DoEvents();

                File.WriteAllText(targetFile, ZeosToAdo.DfmUpdate(fileText, removeParams: true), Encoding.Default);
            }
        }
    }
}
