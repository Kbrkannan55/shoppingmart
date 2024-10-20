using FinalProject;

using System.Text;
using Microsoft.Data.SqlClient;

class Final
{


    public static void Main(string[] args)
    {
        Class1 c = new Class1();
        c.openconn();
        c.Login();
        // c.ForgotPassword();
        // c.Viewproduct();
    }
}