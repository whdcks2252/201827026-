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
    public partial class main : Form
    {   //상품을 담지 않을시 계산하기 못하게 하기위해
        int sell=0;

        int selectedRow;
        DataTable table = new DataTable();
        public main()
        {
            InitializeComponent();
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Price", typeof(string));
            table.Columns.Add("Count", typeof(string));
            table.Columns.Add("Total", typeof(string));

            dataGridView1.DataSource = table;
            numericUpDown1.Value = 1;
        }

        private void main_Load(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=t2313713;");
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                label25.Text = "Connected";
                label25.ForeColor = Color.Black;
            }
            else
            {
                label25.Text = "DisConnected";
                label25.ForeColor = Color.Red;
            }
        }

        ////프로그램을 종료하는 버튼 이벤트

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("프로그램이 종료되었습니다.");
            this.Close();
        }

        //취소
        private void button2_Click(object sender, EventArgs e)
        {
            //행지우기
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }

            //합계창에 수정된 값 넣기
            decimal all = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                all += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
            }

            textBox4.Text = all.ToString();
            
            if (sell > 0)
            {
                sell--;
            }
        }

        //담기
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해주세요");
                textBox2.Clear();
                textBox3.Clear();
            }
            else
            {
                //합계를 구하기 위해 품목명과 가격을 정의하고 total로 합침
                decimal price = decimal.Parse(textBox3.Text);
                decimal count = numericUpDown1.Value;
                decimal total = price * count;

                //text박스내의 정보를 표에 삽입
                table.Rows.Add(textBox2.Text, textBox3.Text, numericUpDown1.Value, total);
                dataGridView1.DataSource = table;

                //text박스의 정보 초기화
                textBox2.Clear();
                textBox3.Clear();
                numericUpDown1.Value = 1;

                //합계
                decimal all = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    all += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
                }
                textBox4.Text = all.ToString();

               if(sell==0&&sell<2)
                sell++;
            }
        }

        //계산하기
        private void button4_Click(object sender, EventArgs e)
        {
            //DB연결 후 데이터 전송
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Port=3306;Database=posdata;Uid=root;Pwd=t2313713"))
            {
               
                    conn.Open();
                if (sell == 0)
                    MessageBox.Show("물건을 담으세요.");

                else
                {
                    //각 행의 정보를 반복문으로 불러온다
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        String Product = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        String Price = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        String Count = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        String Total = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    //각각의 항목을 db와 대조

                        MySqlCommand selectCommand = new MySqlCommand("select * from posdata.stocktable where product='" + @Product + "' and price='" + @Price + "' and count>='" + @Count + "'", conn);
                        MySqlDataReader myReader;
                int count = 0;
                        myReader = selectCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            count++;
                        }
                        myReader.Close();
                        if (count == 0)
                            MessageBox.Show("재고가 없습니다.");
                        else if(count >0)
                        {
                            //INSERT INTO 쿼리문으로 받아온 정보를 DB에 전송한다. 
                            string sql = string.Format("INSERT INTO producttable(product,price,count,total,c_num) VALUES  ('{0}',{1},{2},{3},{4})", @Product, @Price, @Count, @Total, @i);


                            //INSERT INTO 쿼리문으로 이름으로 찾고 count의 수량에서 판매된 개수만큼 빼준다
                            string sql_count = string.Format("update stocktable set count = count - {0} where product = '{1}'", @Count, @Product);

                            //DB전송을 진행하고 실패시 에러메세지 출력
                            try
                            {
                                MySqlCommand command = new MySqlCommand(sql, conn); 
                                command.ExecuteNonQuery();

                                MySqlCommand c_command = new MySqlCommand(sql_count, conn);
                                c_command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }


                            MessageBox.Show("계산되었습니다.");
                        }
                    }
                }
                if (sell == 1)
                {
                    sell--;
                }
                conn.Close();
            }

            //데이터 그리드뷰 초기화
            int rowCount = dataGridView1.Rows.Count;
            for (int n = 0; n < rowCount; n++)
            {
                if (dataGridView1.Rows[0].IsNewRow == false)
                    dataGridView1.Rows.RemoveAt(0);
            }

            //합계창 초기화
            textBox3.Text = "0";
            

        }

        //새로고침
        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        //버튼 클릭시 상품 보기
        private void button5_Click(object sender, EventArgs e)
        {
            panel3.BringToFront();

        }
        private void button6_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();

        }
        //저장된 데이터 누르면 텍스트입력
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRow];
            textBox2.Text = row.Cells[0].Value.ToString();
            textBox3.Text = row.Cells[1].Value.ToString();
            textBox4.Text = row.Cells[3].Value.ToString();
        }

        private void 재고관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new 재고관리().ShowDialog();

        }

        private void 판매관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new 판매관리().ShowDialog();

        }


        //상품선택시text칸에 상품 금액 입력
        private void pictureBox27_Click(object sender, EventArgs e)
        {
            textBox2.Text = "cheetos";
            textBox3.Text = "1200";
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            textBox2.Text = "HoneyButterChip";
            textBox3.Text = "1500";
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Pocachip";
            textBox3.Text = "1500";
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            textBox2.Text = "onionring";
            textBox3.Text = "1200";
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Churros";
            textBox3.Text = "1500";
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            textBox2.Text = "KkobukchipChoco";
            textBox3.Text = "1500";
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Kkobukchipinjeolmi";
            textBox3.Text = "1500";
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            textBox2.Text = "swingchip";
            textBox3.Text = "1500";
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            textBox2.Text = "KkoKahlcone";
            textBox3.Text = "1200";
        }


        private void pictureBox18_Click(object sender, EventArgs e)
        {
            textBox2.Text = "ShinRamen";
            textBox3.Text = "850";
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Nuguri";
            textBox3.Text = "750";
        }
        private void pictureBox14_Click(object sender, EventArgs e)
        {
            textBox2.Text = "SamyangRamen";
            textBox3.Text = "850"; 

        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            textBox2.Text = "JinRamen";
            textBox3.Text = "750";
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            textBox2.Text = "YeolRamen";
            textBox3.Text = "750";
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            textBox2.Text = "KimchiRamen";
            textBox3.Text = "650";
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Snackmen";
            textBox3.Text = "650";
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            textBox2.Text = "ShinRamenBlack";
            textBox3.Text = "1000";
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Gamjamen";
            textBox3.Text = "850";
        }

       
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
       
           

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Soju ";
            textBox3.Text = "1800";
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Powerade";
            textBox3.Text = "2000";
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            textBox2.Text = "CocaCola";
            textBox3.Text = "2500";
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Sprite ";
            textBox3.Text = "1800";
        } 

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Fanta ";
            textBox3.Text = "2000";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Hotsix";
            textBox3.Text = "1000";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "PocariSweat";
            textBox3.Text = "2500";
        }
    }
}
