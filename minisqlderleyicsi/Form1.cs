using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace minisqlderleyicsi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
            veritabanliste();
            listBox1.Items.Clear();
        }
        //LOAD BOŞ GELMESİN DİYE HAZIRDA OLAN BİR VERİTABANIYLA TABLOSUNU ÇAĞIRIYORUM. 
        //PROGRAM BU METODU SADECE İLK AÇILIŞTA KULLANMIŞ OLUYOR. YANİ OLMASA DA OLURDU.
        void listele()
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=SerhatDemir\SQLEXPRESS;Initial Catalog=DbNotKayıt;Integrated Security=True");
            SqlDataAdapter da = new SqlDataAdapter("select * from TBLDERS", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        //DİNAMİK OLARAK ÜRETİLEN BAĞLANTI NESNESİNDE VERİTABANI ADINDAKİ DEĞİŞKENLE BİRLİKTE
        //SİSTEMDEKİ TÜM VERİTABANLARI ÇAĞIRILIP COMBOBOX DATASOURCE UNA AKTARILIYOR.
        void veritabanliste()
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=SerhatDemir\SQLEXPRESS;Initial Catalog=;Integrated Security=True");

            SqlDataAdapter da = new SqlDataAdapter("SELECT NAME FROM sys.databases", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "NAME";
            comboBox1.DataSource = dt;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //COMBOBOXDAN VERİTABANI SEÇİMİ YAPILDIĞINDA SEÇİLEN VERİTABANI ADINI
            //YENİ OLUŞTURULAN BAĞLANTI NESNESİNE AKTARARAK SEÇİLEN VERİTABANINI ÇAĞIRIP
            //ÇAĞIRILAN VERİTABANINA AİT TABLOLARI GETİRİYORUZ 
            //VE BUNLARI LISTBOXITEMS OLARAK YAZDIRIYORUZ.
            SqlConnection baglanti = new SqlConnection(@"Data Source=SerhatDemir\SQLEXPRESS;Initial Catalog=" + comboBox1.Text + ";Integrated Security=True");
            baglanti.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT NAME FROM sys.tables", baglanti);
            SqlDataReader dr = cmd2.ExecuteReader();
            while (dr.Read())
            {
                listBox1.Items.Add(dr[0]);
            }
            baglanti.Close();
            //veritabani = comboBox1.Text;
            label1.Text = comboBox1.Text;
        }
        string listboxitemsec = "";

        //SORGU BUTONU
        string sorgu;
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection sqlbaglanti = new SqlConnection(@"Data Source=SerhatDemir\SQLEXPRESS;Initial Catalog=" + comboBox1.Text + ";Integrated Security=True");

            void liste()
            {
                //LISTBOXITEMSEC ADINDA BİR DEĞİŞKEN TANIMLAYIP LİSTBOXDAN
                //SEÇİLEN İTEMA GÖRE LİSTELEME YAPAN SORGU.
                SqlDataAdapter listeleadapter = new SqlDataAdapter("select * from " + listboxitemsec, sqlbaglanti);
                DataTable listeletable = new DataTable();
                listeleadapter.Fill(listeletable);
                dataGridView1.DataSource = listeletable;
            }
            sorgu = richTextBox1.Text;
            //BURADA DA SORGU İÇİNDE DE DML KOMUTLARI VAR MI YOK MU BAKTIRIP ONA GÖRE İŞLEM YAPTIRIYORUZ.
            if (sorgu.Contains("select"))
            {
               
                SqlDataAdapter da = new SqlDataAdapter(sorgu, sqlbaglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }

            if (sorgu.Contains("insert"))
            {
                try
                {
                    sqlbaglanti.Open();
                    SqlCommand cmd = new SqlCommand(sorgu, sqlbaglanti);
                    cmd.ExecuteNonQuery();
                    sqlbaglanti.Close();
                    liste();
                    MessageBox.Show("Insert sorgunuz başarılı şekilde gerçekleştirildi.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert sorgunuzu kontrol edin.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (sorgu.Contains("update"))
            {
                try
                {
                    sqlbaglanti.Open();
                    SqlCommand cmd = new SqlCommand(sorgu, sqlbaglanti);
                    cmd.ExecuteNonQuery();
                    sqlbaglanti.Close();
                    liste();
                    MessageBox.Show("Update sorgunuz başarılı şekilde gerçekleştirildi.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Update sorgunuzu kontrol edin.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (sorgu.Contains("delete"))
            {
                try
                {
                    sqlbaglanti.Open();
                    SqlCommand cmd = new SqlCommand(sorgu, sqlbaglanti);
                    cmd.ExecuteNonQuery();
                    sqlbaglanti.Close();
                    liste();
                    MessageBox.Show("Delete sorgunuz başarılı şekilde gerçekleştirildi.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Delete sorgunuzu kontrol edin.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection sqlbaglanti = new SqlConnection(@"Data Source=SerhatDemir\SQLEXPRESS;Initial Catalog=" + comboBox1.Text + ";Integrated Security=True");
            listboxitemsec = listBox1.SelectedItems[0].ToString();
            SqlDataAdapter listeleadapter = new SqlDataAdapter("select * from " + listboxitemsec, sqlbaglanti);
            DataTable listeletable = new DataTable();
            listeleadapter.Fill(listeletable);
            dataGridView1.DataSource = listeletable;
        }

    }
}
