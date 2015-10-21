using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Transactions;

namespace PhotoLibraryApp
{
    public partial class Form1 : Form
    {
        FileDialog openFileDialog1;
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFileDlg.FileName);
                FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                BinaryReader rdr = new BinaryReader(fs);
                byte[] fileData = rdr.ReadBytes((int)fs.Length);
                rdr.Close();
                fs.Close();

                string cs = @"Data Source=LAPTOP\SQL2014;Initial Catalog=MyFsDb;Integrated Security=TRUE";
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string sql = "INSERT INTO MyFsTable VALUES (@fData, @fName, default)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.Add("@fData", SqlDbType.Image, fileData.Length).Value = fileData;
                    cmd.Parameters.Add("@fName", SqlDbType.NVarChar).Value = fi.Name;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                MessageBox.Show(fi.FullName, "File Inserted!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
               
            
        }
    }
}
