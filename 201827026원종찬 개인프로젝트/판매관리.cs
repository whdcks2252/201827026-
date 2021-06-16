using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WindowsFormsApp1
{
    public partial class 판매관리 : Form
    {
        int selectedRow;

        MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable table = new DataTable();
        public 판매관리()
        {
            InitializeComponent();
        }

        private void 판매관리_Load(object sender, EventArgs e)
        {
            searchData("");
        }
        //뒤로가기

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //DB에서 데이터 불러온 후 텍스트 박스 초기화 (데이터 갱신 함수)
        public void LoadData()
        {
            string sql = "Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713";
            MySqlConnection con = new MySqlConnection(sql);
            MySqlCommand cmd_db = new MySqlCommand("SELECT * FROM producttable;", con);

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmd_db;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();

                bSource.DataSource = dbdataset;
                dataGridView1.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //텍스트 박스 초기화
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }
        
        public void searchData(string valueToSearch)
        {
            string query = "SELECT * FROM ProductTable WHERE CONCAT(`product`, `price`, `count`, `total`) like '%" + valueToSearch + "%'";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }
        //DB 불러오기(새로고침)
        private void button4_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        

        //검색기능
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("검색 정보를 입력해주세요");
            }
            else
            {
                string valueToSearch = textBox1.Text.ToString();
                searchData(valueToSearch);
                //텍스트 박스 초기화
                textBox1.Text = "";
            }
        }

        //셀 클릭시 해당 행의 정보를 텍스트박스에 채움
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRow];
            textBox2.Text = row.Cells[0].Value.ToString();
            textBox3.Text = row.Cells[1].Value.ToString();
            textBox4.Text = row.Cells[2].Value.ToString();
            textBox5.Text = row.Cells[3].Value.ToString();
            textBox6.Text = row.Cells[4].Value.ToString();
        }
        
      
       //삭제
        private void button3_Click(object sender, EventArgs e)
        {
            string constring = "Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713";
            if (textBox2.Text == "")
            {
                MessageBox.Show("삭제 할 항목을 찾지 못했습니다.");
            }
            else
            {
                //delete를 통해 DB로 삭제된 데이터 전송 - 기본키 기준으로 수정위치 탐색
                string Query = "delete from posdata.producttable where no ='" + this.textBox2.Text + "';"+"update posdata.stocktable set product='" + this.textBox3.Text + "',price='" + this.textBox4.Text + "'," +
    "count=count+'" + this.textBox5.Text + "' WHERE product='" + this.textBox3.Text + "'"; 
            
                MySqlConnection conDataBase = new MySqlConnection(constring);
        
                MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
                MySqlDataReader myReader;

                try
                {
                    conDataBase.Open();
                    
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("삭제완료");

                    while (myReader.Read())
                    {

                myReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                LoadData();
            }
        }

    }
}
