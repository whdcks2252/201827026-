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
    public partial class 재고관리 : Form
    {
        int selectedRow;

        MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable table = new DataTable();
        public 재고관리()
        {
            InitializeComponent();
        }

        //DB에서 데이터 불러온 후 텍스트 박스 초기화 (데이터 갱신 함수)
        public void LoadData()
        {
            string sql = "Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713";
            MySqlConnection con = new MySqlConnection(sql);
            MySqlCommand cmd_db = new MySqlCommand("SELECT * FROM stocktable;", con);

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
        }
        //새로고침
        private void button6_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        public void searchData(string valueToSearch)
        {
            string query = "SELECT * FROM stocktable WHERE CONCAT(`product`, `price`, `count`) like '%" + valueToSearch + "%'";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }
        private void 재고관리_Load_1(object sender, EventArgs e)
        {

            searchData("");
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
        }

        
        //뒤로가기
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //검색
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
        //재고추가
        private void button2_Click(object sender, EventArgs e)
        {
            string constring = "Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713";
            string Query = "INSERT INTO posdata.stocktable (product,price,count) value ('" + this.textBox3.Text + "','" + this.textBox4.Text + "','" + this.textBox5.Text + "')";
            MySqlConnection conDataBase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
            MySqlDataReader myReader;

            try
            {
                if (textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
                {
                    MessageBox.Show("항목을 정확히 입력해주세요");
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                }
                else
                {
                    conDataBase.Open();
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("재고 추가 완료");

                    while (myReader.Read())
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("항목을 정확히 입력해주세요");
                MessageBox.Show(ex.Message);
            }

            LoadData();
        }
        //물품수정
        private void button3_Click(object sender, EventArgs e)
        {
            string constring = "Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713";

            if (textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해주세요");
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
            }
            else
            {
                decimal price = decimal.Parse(textBox4.Text);
                decimal count = decimal.Parse(textBox5.Text);




                //업데이트를 통해 DB로 수정된 데이터 전송 - 기본키 기준으로 수정위치 탐색
                string Query = "update posdata.stocktable set idstocktable ='" + this.textBox2.Text + "',product='" + this.textBox3.Text + "',price='" + this.textBox4.Text + "'," +
                    "count='" + this.textBox5.Text + "' where idstocktable ='" + this.textBox2.Text + "'";
                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
                MySqlDataReader myReader;

                try
                {
                    conDataBase.Open();
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("수정완료");

                    while (myReader.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            LoadData();
        }
        //물품삭제
        private void button4_Click(object sender, EventArgs e)
        {
            string constring = "Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713";
            if (textBox2.Text == "")
            {
                MessageBox.Show("삭제 할 항목을 찾지 못했습니다.");
            }
            else
            {
                //delete를 통해 DB로 삭제된 데이터 전송 - 기본키 기준으로 수정위치 탐색
                string Query = "delete from posdata.stocktable where idstocktable ='" + this.textBox2.Text + "';";
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

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                LoadData();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox3.Text = "cheetos";
                textBox4.Text = "1200";
            }
            if (comboBox1.SelectedIndex == 1)
            {
                textBox3.Text = "HoneyButterChip";
                textBox4.Text = "1500";
            }
            if (comboBox1.SelectedIndex == 2)
            {
                textBox3.Text = "Pocachip";
                textBox4.Text = "1500";
            }
            if (comboBox1.SelectedIndex == 3)
            {
                textBox3.Text = "onionring";
                textBox4.Text = "1200";
            }
            if (comboBox1.SelectedIndex == 4)
            {
                textBox3.Text = "Churros";
                textBox4.Text = "1500";
            }
            if (comboBox1.SelectedIndex == 5)
            {
                textBox3.Text = "KkobukchipChoco";
                textBox4.Text = "1500";
            }
            if (comboBox1.SelectedIndex == 6)
            {
                textBox3.Text = "Kkobukchipinjeolmi";
                textBox4.Text = "1500";
            }
            if (comboBox1.SelectedIndex == 7)
            {
                textBox3.Text = "swingchip";
                textBox4.Text = "1500";
            }
            if (comboBox1.SelectedIndex == 8)
            {
                textBox3.Text = "KkoKahlcone";
                textBox4.Text = "1200";
            }
            if (comboBox1.SelectedIndex == 9)
            {
                textBox3.Text = "Soju ";
                textBox4.Text = "1800";
            }
            if (comboBox1.SelectedIndex == 10)
            {
                textBox3.Text = "Powerade";
                textBox4.Text = "2000";
            }
            if (comboBox1.SelectedIndex == 11)
            {
                textBox3.Text = "CocaCola";
                textBox4.Text = "2500";
            }
            if (comboBox1.SelectedIndex == 12)
            {
                textBox3.Text = "Sprite ";
                textBox4.Text = "1800";
            }
            if (comboBox1.SelectedIndex == 13)
            {
                textBox3.Text = "Fanta ";
                textBox4.Text = "2000";
            }
            if (comboBox1.SelectedIndex == 14)
            {
                textBox3.Text = "Hotsix";
                textBox4.Text = "1000";
            }
            if (comboBox1.SelectedIndex == 15)
            {
                textBox3.Text = "PocariSweat";
                textBox4.Text = "2500";
            }
            if (comboBox1.SelectedIndex == 16)
            {
                textBox3.Text = "ShinRamen";
                textBox4.Text = "850";
            }
            if (comboBox1.SelectedIndex == 17)
            {
                textBox3.Text = "Nuguri";
                textBox4.Text = "750";
            }
            if (comboBox1.SelectedIndex == 18)
            {
                textBox3.Text = "SamyangRamen";
                textBox4.Text = "850";
            }
            if (comboBox1.SelectedIndex == 19)
            {
                textBox3.Text = "JinRamen";
                textBox4.Text = "750";
            }
            if (comboBox1.SelectedIndex == 20)
            {
                textBox3.Text = "YeolRamen";
                textBox4.Text = "750";
            }
            if (comboBox1.SelectedIndex == 21)
            {
                textBox3.Text = "KimchiRamen";
                textBox4.Text = "650";
            }
            if (comboBox1.SelectedIndex == 22)
            {
                textBox3.Text = "Snackmen";
                textBox4.Text = "650";
            }
            if (comboBox1.SelectedIndex == 23)
            {
                textBox3.Text = "ShinRamenBlack";
                textBox4.Text = "1000";
            }
            if (comboBox1.SelectedIndex == 24)
            {
                textBox3.Text = "Gamjamen";
                textBox4.Text = "850";
            }
            


        }
    }
}
