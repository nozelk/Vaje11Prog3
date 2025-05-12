using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;

namespace Nobel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            checkedListBox1.SetItemChecked(0, true);
        }

        private bool checkYears()
        {
            if (int.TryParse(textBox2.Text, out int inputYear))
            {
                if (1901 <= inputYear && inputYear <= 2008)
                {
                    return true;

                }
                else
                {
                    MessageBox.Show("Leto je veljavno število, vendar ni v območju med 1901 in 2008.");
                    return false;
                }
            }


            return false;
        }


        private bool IsAnyCategorySelected()
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Prosim, izberite vsaj eno kategorijo.", "Opozorilo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkYears() || !IsAnyCategorySelected())
            {
                return;
            }

            string povNiz = @"Data Source=C:\Users\kosmr\Desktop\CSharp\Vaje11\nobelDB.db; Version=3;";
            SQLiteConnection conn = new SQLiteConnection(povNiz);
            conn.Open();

            SQLiteCommand command = new SQLiteCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;

            int inputYear = int.Parse(textBox2.Text);

            List<string> izbraneKategorije = new List<string>();
            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                izbraneKategorije.Add(itemChecked.ToString());
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT winner, yr, subject FROM nobel WHERE yr = @InputYear");
            command.Parameters.AddWithValue("@InputYear", inputYear);

            sqlBuilder.Append(" AND subject IN (");
            for (int i = 0; i < izbraneKategorije.Count; i++)
            {
                string paramName = $"@Kategorija{i}";
                sqlBuilder.Append(paramName);
                command.Parameters.AddWithValue(paramName, izbraneKategorije[i]);
                if (i < izbraneKategorije.Count - 1)
                {
                    sqlBuilder.Append(", ");
                }
            }
            sqlBuilder.Append(")");


            sqlBuilder.Append(" ORDER BY yr, subject;"); 

            command.CommandText = sqlBuilder.ToString();

            SQLiteDataReader rez = command.ExecuteReader();

            textBox1.Text = ""; 

            if (rez.HasRows)
            {

                Dictionary<string, List<string>> dobitnikiPoKategorijah = new Dictionary<string, List<string>>();

                while (rez.Read())
                {
                    string kategorija = rez["subject"].ToString();
                    string dobitnik = rez["winner"].ToString();

                    if (!dobitnikiPoKategorijah.ContainsKey(kategorija))
                    {
                        dobitnikiPoKategorijah[kategorija] = new List<string>();
                    }
                    dobitnikiPoKategorijah[kategorija].Add(dobitnik);
                }


                foreach (KeyValuePair<string, List<string>> par in dobitnikiPoKategorijah)
                {
                    string kategorija = par.Key;
                    List<string> seznamDobitnikov = par.Value;

                    textBox1.AppendText($"Kategorija: {kategorija}{Environment.NewLine}");
                    foreach (string dobitnik in seznamDobitnikov)
                    {
                        textBox1.AppendText($"{dobitnik}{Environment.NewLine}");
                    }
                    textBox1.AppendText("------------------------------------" + Environment.NewLine);
                }
            }
            else
            {
                textBox1.AppendText("Ni najdenih rezultatov za izbrane kriterije." + Environment.NewLine);
            }

            rez.Close();
            conn.Close();
        }
    }
}
