using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Air_Ticket_Management_System
{
    public partial class AddFlightRecords : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;
        public AddFlightRecords()
        {
            InitializeComponent();

            this.FormClosing += (sender, e) =>
            {
                Application.Exit();
            };
        }

        //Record Button
        private void RecordBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FlightCodeField.Text) || string.IsNullOrWhiteSpace(TakeofDate.Text) || string.IsNullOrWhiteSpace(FromField.Text) || string.IsNullOrWhiteSpace(ToField.Text) || string.IsNullOrWhiteSpace(NumOfSeat.Text))
            {
                MessageBox.Show("Please fill in all the required fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into FlightRecords values (@Flightcode,@From,@To,@TakeOfDate,@NumOfSeat)", conn);
            cmd.Parameters.AddWithValue("@Flightcode", int.Parse(FlightCodeField.Text));
            cmd.Parameters.AddWithValue("@From", (FromField.Text));
            cmd.Parameters.AddWithValue("@To", (ToField.Text));
            cmd.Parameters.AddWithValue("@TakeOfDate", (TakeofDate.Text));
            cmd.Parameters.AddWithValue("@NumOfSeat", int.Parse(NumOfSeat.Text));
            cmd.ExecuteNonQuery();

            this.Hide();
            AddFlightRecords newForm = new AddFlightRecords();
            newForm.Show();


            MessageBox.Show("Flight Record Added Successfully");
        }
        
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            FlightCodeField.Clear();
            FromField.Clear();
            ToField.Clear();
            TakeofDate.Clear();
            NumOfSeat.Clear();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            AdminHome adminHome = new AdminHome();
            adminHome.Show();
            this.Hide();
        }
    }
}
