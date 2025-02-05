using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Air_Ticket_Management_System
    {
    public partial class Booking : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;
        public Booking()
        {
            InitializeComponent();
            this.FormClosing += (sender, e) => Application.Exit();
        }

        //Refresh Datagridview table
        private void Booking_Load_1(object sender, EventArgs e)
        {
            LoadFlightRecords();
            LoadBookingRecords();
        }
        private void LoadFlightRecords()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM FlightRecords", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvFlightRecords.DataSource = dt;
            }
        }

        private void LoadBookingRecords()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Booking", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvBooking.DataSource = dt;
            }
        }

        ///Booking Button
        private void btnBook_Click(object sender, EventArgs e)
        {
            if (dgvFlightRecords.SelectedRows.Count > 0)
            {
                // Get selected flight information
                var selectedRow = dgvFlightRecords.SelectedRows[0];
                int flightCode = Convert.ToInt32(selectedRow.Cells["FlightCode"].Value);
                int passengerid = int.Parse(PassangerIDField.Text);
                string name = NameField.Text;
                string email = EmailField.Text;
                string passportNumber = PassportNumField.Text;
                string nationality = NationalityField.Text;
                double amount = double.Parse(AmountField.Text);

                // Insert booking into the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Booking (FlightCode, PassengerID, Name, Email, PassportNum, Nationality, Amount) VALUES (@FlightCode, @PassengerID, @Name, @Email, @PassportNum, @Nationality, @Amount)", conn);
                    cmd.Parameters.AddWithValue("@FlightCode", flightCode);
                    cmd.Parameters.AddWithValue("@PassengerID", passengerid);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PassportNum", passportNumber);
                    cmd.Parameters.AddWithValue("@Nationality", nationality);
                    cmd.Parameters.AddWithValue("@Amount", amount);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // After booking, decrease the number of available seats
                    SqlCommand updateSeatsCmd = new SqlCommand("UPDATE FlightRecords SET NumOfSeat = NumOfSeat - 1 WHERE Flightcode = @FlightCode AND NumOfSeat > 0", conn);
                    updateSeatsCmd.Parameters.AddWithValue("@FlightCode", flightCode);

                    int rowsAffected = updateSeatsCmd.ExecuteNonQuery();

                    // If no rows were affected, it means there are no seats left, so show an error
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("No available seats left for this flight.");
                    }
                    else
                    {
                        MessageBox.Show("Flight booked successfully!");
                    }

                    conn.Close();
                }

                // Refresh the Booking DataGridView
                LoadBookingRecords();
            }
            else
            {
                MessageBox.Show("Please select a flight to book.");
            }
        }

        // Cancel Button
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            if (dgvBooking.SelectedRows.Count > 0)
            {
                var selectedRow = dgvBooking.SelectedRows[0];
                int ticketId = Convert.ToInt32(selectedRow.Cells["TicketID"].Value);
                int flightCode = Convert.ToInt32(selectedRow.Cells["FlightCode"].Value); // Get the associated FlightCode for the booking

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Delete booking from the database
                    SqlCommand cmd = new SqlCommand("DELETE FROM Booking WHERE TicketID = @TicketID", conn);
                    cmd.Parameters.AddWithValue("@TicketID", ticketId);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // After cancellation, increase the number of available seats
                    SqlCommand updateSeatsCmd = new SqlCommand("UPDATE FlightRecords SET NumOfSeat = NumOfSeat + 1 WHERE FlightCode = @FlightCode", conn);
                    updateSeatsCmd.Parameters.AddWithValue("@FlightCode", flightCode);

                    updateSeatsCmd.ExecuteNonQuery();

                    conn.Close();
                }

                // Refresh the Booking DataGridView and FlightRecords DataGridView to reflect changes
                LoadBookingRecords();
                LoadFlightRecords();

                MessageBox.Show("Booking cancelled and seat availability updated!");
            }
            else
            {
                MessageBox.Show("Please select a booking to cancel.");
            }

        }


        //Login Button
        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }


    }
    
}