using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Ticket_Management_System
{
    internal static class DatabaseConfig
    {
        public static readonly string ConnectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\Programming File\\Air_Ticket_Management_System\\DataBase\\Air_Ticket_Management_System.mdf\";Integrated Security=True;Connect Timeout=30;";
    }
}
