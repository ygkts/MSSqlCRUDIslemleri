﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlClient;

namespace u4_UrunGetirOrnegi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string baglanti = "Server = YASEMINGOKTAS; Database = NORTHWND; Trusted_Connection=true;";
        SqlConnection conn = new SqlConnection(baglanti);
        SqlCommand cmd;
        private void UrunGetir(string sorgu)
        {
            SqlDataAdapter adap = new SqlDataAdapter(sorgu, conn);
            DataTable tablo = new DataTable();
            adap.Fill(tablo);
            dataGridView1.DataSource = tablo;
        }  
        private void buttonUrunGetir_Click(object sender, EventArgs e)
        {
            string sorgu = "select * from Products";
            UrunGetir(sorgu);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBulID.Text != "")
                {
                    SqlCommand cmd = new SqlCommand("select * from Products where ProductID = @pID", conn);
                    cmd.Parameters.AddWithValue("@pID", txtBulID.Text);

                    SqlDataAdapter adap = new SqlDataAdapter(cmd);
                    DataTable tablo = new DataTable();
                    adap.Fill(tablo);

                    dataGridView1.DataSource = tablo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void buttonEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAd.Text != "" )
                {
                    conn.Open();
                    cmd = new SqlCommand("insert into Products (ProductName, UnitsInStock, UnitPrice) values (@ProductName, @UnitsInStock, @UnitPrice)",conn);
                    cmd.Parameters.AddWithValue("@ProductName", txtAd.Text);
                    cmd.Parameters.AddWithValue("@UnitsInStock", txtStok.Text);
                    cmd.Parameters.AddWithValue("@UnitPrice", txtFiyat.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("ÜRÜN EKLENDİ..");


                    UrunGetir("select * from products");
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        int id;
        private void buttonGuncelle_Click(object sender, EventArgs e)
        {
            conn.Open();
            string updateSorgu = "update Products set ProductName = @name, UnitPrice = @uprice, UnitsInStock = @Stock where ProductID = @pID ";
            cmd = new SqlCommand(updateSorgu,conn);
            cmd.Parameters.AddWithValue("@pID", id);
            cmd.Parameters.AddWithValue("@name", txtAd.Text);
            cmd.Parameters.AddWithValue("@uprice", txtFiyat.Text);
            cmd.Parameters.AddWithValue("@Stock", txtStok.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Güncellendi..");
            UrunGetir("select * from products");
            conn.Close();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            txtAd.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtFiyat.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            txtStok.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }

        private void buttonSil_Click(object sender, EventArgs e)
        {
            conn.Open();
            string deleteSorgu = "delete from Products where ProductID = @pID";
            cmd = new SqlCommand(deleteSorgu, conn);
            cmd.Parameters.AddWithValue("pID",id);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Silindi..");
            UrunGetir("Select * from Products");
            conn.Close();
        }
    }
}
