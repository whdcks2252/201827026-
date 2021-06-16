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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Join dlg = new Join();
            dlg.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string myConnection = "Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713";
                MySqlConnection myConn = new MySqlConnection(myConnection);
                //각각의 항목을 db와 대조
                MySqlCommand selectCommand = new MySqlCommand("select * from posdata.pos_login where id='" + this.textBox1.Text + "' and password='" + this.textBox2.Text + "'", myConn);
                MySqlDataReader myReader;

                myConn.Open();
                myReader = selectCommand.ExecuteReader();
                int count = 0;
                while (myReader.Read())
                {
                    count = count + 1;
                }

                if (count == 1)
                {
                    MessageBox.Show("로그인 되었습니다.");
                    this.Visible = false;
                    main showForm1 = new main();
                    showForm1.ShowDialog();
                }

                else if (count > 1)
                {
                    MessageBox.Show("중복된 유저가 존재합니다.");
                }
                else
                {
                    MessageBox.Show("아이디, 비밀번호나 직책이 일치하지 않습니다.");
                }
                myConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Login_Load_1(object sender, EventArgs e)
        {

        }
    }

    }

