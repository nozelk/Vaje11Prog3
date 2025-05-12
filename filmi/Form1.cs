using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace filmi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            for (int i = 1921; i <= 2016; i++)
            {
                comboBox1.Items.Add(i);
                comboBox2.Items.Add(i);
            }
        }

        private bool checkYears()
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Prosim, izberite obe letnici.", "Opozorilo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            int startYear = (int)comboBox1.SelectedItem;
            int endYear = (int)comboBox2.SelectedItem;

            return startYear <= endYear;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkYears())
            {
                if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
                {
                    MessageBox.Show("Začetno leto ne sme biti večje od končnega leta.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                string povNiz = @"Data Source=C:\Users\kosmr\Desktop\CSharp\Vaje11\filmi.sqlite; Version=3;";

                SQLiteConnection conn = new SQLiteConnection(povNiz);
                conn.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandType = CommandType.Text;
                command.Connection = conn;

                int startYear = (int)comboBox1.SelectedItem;
                int endYear = (int)comboBox2.SelectedItem;

                string in_name = !string.IsNullOrEmpty(textBox2.Text) ? textBox2.Text : "*";
                string sql;



                command.Parameters.AddWithValue("@StartYear", startYear);
                command.Parameters.AddWithValue("@EndYear", endYear);

                if (in_name != "*"){sql = "SELECT naslov, leto, reziser FROM Filmi WHERE Leto >= @StartYear AND Leto <= @EndYear AND naslov LIKE @InName ORDER BY Leto, Naslov;"; command.Parameters.AddWithValue("@InName", "%" + in_name + "%"); }
                else sql = "SELECT naslov, leto, reziser FROM Filmi WHERE Leto >= @StartYear AND Leto <= @EndYear ORDER BY Leto, Naslov;";
                command.CommandText = sql;


                SQLiteDataReader rez = command.ExecuteReader();
                textBox1.Text = "";

                while (rez.Read())
                { 

                    string naslov = rez["naslov"].ToString();
                    string leto = rez["leto"].ToString();
                    string reziser = rez["reziser"].ToString();

                    textBox1.AppendText($"Naslov: {naslov}{Environment.NewLine}");
                    textBox1.AppendText($"Leto: {leto}{Environment.NewLine}");
                    textBox1.AppendText($"Režiser: {reziser}{Environment.NewLine}");
                    textBox1.AppendText("------------------------------------" + Environment.NewLine);

                    // string naslov = rez[0].ToString();
                    // string leto = rez[1].ToString();
                    // string reziser = rez[2].ToString();
                    // textBox1.AppendText(Environment.NewLine);
                }
                rez.Close();
                conn.Close();



            }
        }
    }
}
