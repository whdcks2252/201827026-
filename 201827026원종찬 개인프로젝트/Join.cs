using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Join : Form
    {
        public Join()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                    string constring = "Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713";
                    MySqlConnection conDataBase = new MySqlConnection(constring);
            conDataBase.Open();
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해주세요");
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                String id = this.textBox1.Text;
                MySqlCommand selectCommand = new MySqlCommand("select * from posdata.pos_login where id='" + @id +"'", conDataBase);
                MySqlDataReader myReader;
                int count = 0;
                myReader = selectCommand.ExecuteReader();
                while (myReader.Read())
                {
                    count++;
                }
                myReader.Close();
                if (count > 0)
                    MessageBox.Show("아이디가 중복됩니다");
                else
                {
                    string Query = "INSERT INTO posdata.pos_login (id,password,name) value ('" + this.textBox1.Text + "','" + this.textBox2.Text + "','" + this.textBox3.Text + "')";
                    MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);

                    try
                    {
                        myReader = cmdDatabase.ExecuteReader();
                        MessageBox.Show("계정 생성 완료");

                        while (myReader.Read())
                        {

                        }
                        this.Visible = false;
                        Login showForm1 = new Login();
                        showForm1.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("항목을 정확히 입력해주세요");
                        MessageBox.Show(ex.Message);
                    }
                }           
             }
        }
        //계정생성
        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login showForm1 = new Login();
            showForm1.ShowDialog();
        }

        private void Join_Load(object sender, EventArgs e)
        {

        }
    }
}
