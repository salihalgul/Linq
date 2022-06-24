using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectedArchitecture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con=new SqlConnection(@"Data Source=DESKTOP-QO5MM8V\SQLEXPRESS;Initial Catalog=NORTHWND;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlCommand komut=new SqlCommand("SELECT * FROM Categories",con);
            con.Open();
            SqlDataReader okuyucu = komut.ExecuteReader();
            comboBox1.DisplayMember = "CategoryName";
            comboBox1.ValueMember = "CategoryID";

            List<Category> liste=new List<Category>();
            while (okuyucu.Read())
            {
                Category can=new Category();
                can.CategoryID = (int) okuyucu["CategoryID"];
                can.CategoryName = okuyucu["CategoryName"].ToString();
                liste.Add(can);
            }
            con.Close();
            comboBox1.DataSource = liste;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            var secilenID = comboBox1.SelectedValue;
            con.Open();
            SqlCommand cmd=new SqlCommand("SELECT * FROM Products WHERE CategoryID="+secilenID,con);
            SqlDataReader okuyucu = cmd.ExecuteReader();

            while (okuyucu.Read())
            {
                Button b=new Button();
                b.Name = "b" + okuyucu["ProductID"];
                b.Height = 50;
                b.AutoSize = true;
                b.Text = okuyucu["ProductName"].ToString();
                flowLayoutPanel1.Controls.Add(b);
                b.Click += UrunDetayGetir;
            }
            con.Close();
        }

        private void UrunDetayGetir(object tiklanilan, EventArgs tikdetay)
        {
            Button tiklanilanButton = (Button) tiklanilan;
            var IDtxt = tiklanilanButton.Name.Remove(0, 1);

            con.Open();
            SqlCommand komut=new SqlCommand("SELECT * FROM Products WHERE ProductID="+IDtxt,con);
            SqlDataReader okuyucu = komut.ExecuteReader();
            okuyucu.Read();
            UrunDetay detay=new UrunDetay();
            detay.lbl_UrunAdi.Text = okuyucu["ProductName"].ToString();
            detay.lbl_qpu.Text = okuyucu["QuantityPerUnit"].ToString();
            detay.Show();
            con.Close();
        }
    }
}
