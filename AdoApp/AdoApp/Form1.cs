using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoApp
{
    //ADO.Net (ActiveXDataObject.Net)Aktivex tabanlı çalışan microsoft veri erişim teknolojisidir.
    //Bu veri erişim sadece mssql değil diğer databaseler içinde uygulanabilir.
    //Connection:Çalıştırılacak olan sorgununhangi bağlantı üzerinde işlem yapacağı belirtilmektedir.
    //CommandType: Çalıştırılacak olan sql cümlesinin tipini belirlemek için kullanılır.
    //CommandText:Çalıştırılacak olan sorgu cümlesi yazılmaktadır.
    //Transection:Çalıştırılacak olan sorguların kontrollü bir şekilde işlenmesini sağlamaktadır. Genellikle Insert,Update ve Delete işlemleri için kullanılmaktadır.
    //Parameters: T-Sql cümlesi yada Stored Procedure içerisinde kullanılan parametreleri setlemek için kullanılır.

    //Execute Metodları
    //-->ExecuteReader:Çalıştırılan sql sorgusundan geri birden fazla değer dönecekse bu metod kullanılır.
    //-->ExecuteScalar:Çalıştırılan sorgudan tek değer dönecekse bu komut kullanılır.
    //-->ExecuteNonQuery:sorgudan geriye veri dönmediğinde bu metod kullanılır. Update/Delete/Insert işlemlerin kullanılır.
    public partial class Form1 : Form
    {
        SqlCommand uygula;
        SqlDataReader oku;
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CON"].ToString());
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Combo();
            Listele();
        }

        private void Listele()
        {
            Liste.Rows.Clear();
            int i = 0;
            conn.Open();
            uygula = new SqlCommand("select * from tblDoktors", conn);
            oku = uygula.ExecuteReader();
            foreach(var k in oku)
            {
                Liste.Rows.Add();
                Liste.Rows[i].Cells[0].Value =oku[0];
                Liste.Rows[i].Cells[1].Value =oku[1];
                Liste.Rows[i].Cells[2].Value =oku[2];
                Liste.Rows[i].Cells[3].Value =oku[3];

                i++;
            }
            Liste.AllowUserToAddRows = false;
            oku.Close();
            conn.Close();
        }

        private void Combo()
        {
            string query;
            query = "Select * from tblUnvan";
            uygula = new SqlCommand(query, conn);
            uygula.Connection.Open();
            oku = uygula.ExecuteReader(CommandBehavior.CloseConnection);
            foreach(var k in oku)
            {
                txtUnvan.Items.Add(oku.GetString(1));
            }
            conn.Close();
            oku.Close();

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            uygula = new SqlCommand();
            uygula.CommandType = CommandType.StoredProcedure;
            uygula.CommandText = "[S_Doktors]";
            uygula.Connection = conn;
            uygula.Connection.Open();

            uygula.Parameters.Add("@unvan", SqlDbType.NVarChar, 50).Value = txtUnvan.Text;
            uygula.Parameters.Add("@doktoradi", SqlDbType.NVarChar, 50).Value = txtDoktor.Text;
            uygula.Parameters.Add("@tel", SqlDbType.NVarChar, 50).Value = txtTel.Text;

            uygula.ExecuteNonQuery();
            uygula.Connection.Close();
            conn.Close();
            MessageBox.Show("Kayıt işlemi yapıldı");
        }
    }
}
