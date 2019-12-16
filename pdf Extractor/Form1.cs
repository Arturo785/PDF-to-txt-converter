using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;

namespace pdf_Extractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = openPicker();
            string pathTxt = Path.ChangeExtension(path, ".txt");

            writteToTxt(ReadPdfFile(path),pathTxt);
        }


        public string ReadPdfFile(string fileName)
        {
            StringBuilder text = new StringBuilder();

            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                }
                pdfReader.Close();
            }
            return text.ToString();
        }


        private string openPicker()
        {
            OpenFileDialog myOpener = new OpenFileDialog();
            myOpener.Filter = "All|*.*";
            Stream myStream;
            string path = null;

            try
            {
                if (myOpener.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = myOpener.OpenFile()) != null)
                    {
                        path = myOpener.InitialDirectory + myOpener.FileName;
                        return path;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return path;
        }

        private void writteToTxt(String text,string path)
        {
            System.IO.File.WriteAllText(path, text);
            MessageBox.Show("TXT generado", "Exito",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Exclamation);
        }



    }
}
