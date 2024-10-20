using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Data.SqlTypes;
using Microsoft.Identity.Client;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;

namespace FinalProject
{

    internal class Class1 : Interface1
    {
        SqlConnection conn;

        public void openconn()
        {
            conn = new SqlConnection("data source=KBRKANNAN\\SQLEXPRESS;" + "database=FinalProject;" + "integrated security=SSPI;");
            Console.WriteLine("Opened");
            
        }
        public void Login()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("-----AB ONLINE SHOPPING-----");
            Console.WriteLine("----CHOOSE YOUR TYPE----");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("1. CUSTOMER");
            Console.WriteLine("2. ADMIN");
            Console.WriteLine("3. NEW CUSTOMER");
            Console.WriteLine("---------------------------------------");
            int choice;
            choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Loginascustomer();
                    break;

                case 2:
                    Loginasadmin();
                    break;
                case 3:
                    Signup();
                    break;
                default:
                    Console.WriteLine("Enter valid details");
                    break;
            }
        }


        public void Signup()
        {
            int id;
            Console.WriteLine("Enter your name");
            string name;
            name = Console.ReadLine();
            Console.WriteLine("Enter your address");
            string address,password;
            address = Console.ReadLine();
            Console.WriteLine("Enter your password");
            password= Console.ReadLine();
            Console.WriteLine("Enter your Phone Number");
            BigInteger Phonenumber;
            Phonenumber = Convert.ToInt64(Console.ReadLine());
            conn.Open();
            SqlCommand command = new SqlCommand($"insert into Customer(Cusname,Cusph,Cusaddress,Cuspassword) values('{name}',{Phonenumber},'{address}','{password}')", conn);
            command.ExecuteNonQuery();
            Console.WriteLine("Successfully Registered");
            command = new SqlCommand($"select Cusid from Customer where Cusph={Phonenumber}", conn);
            SqlDataReader reader= command.ExecuteReader();
            while(reader.Read())
            {
                id = (int)reader[0];
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Your User Id is " + id);
                Console.WriteLine("---------------------------------------");
            }
            
            conn.Close();
            Viewproduct();

        }


        public void Loginascustomer()
        {
            int counter = 0;
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("---Enter your User Id---");
            Console.WriteLine("---------------------------------------");
            int user;
            string pass;
            user = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("---Enter your password---");
            Console.WriteLine("---------------------------------------");
            pass = Console.ReadLine();
            SqlDataAdapter da = new SqlDataAdapter("select Cusid,Cuspassword from Customer", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "newcopy");
            SqlCommandBuilder cmd = new SqlCommandBuilder(da);
            List<string> list = new List<string>();
            foreach (DataRow dr in ds.Tables["newcopy"].Rows)
            {
                list.Add(dr[0].ToString());
                list.Add(dr[1].ToString());
                
            }



            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == Convert.ToString(user) && list[i + 1] == pass)
                {
                    Console.WriteLine("---------------------------------------");
                    Console.WriteLine("---Logged in successfully---");
                    Console.WriteLine("---------------------------------------");
                    Viewproduct();
                    break;
                }
                else
                {
                    counter++;
                }


            }
            if (counter > 0)
            {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("-----Not a valid Customer-----");
                Console.WriteLine("1. Try Again");
                Console.WriteLine("2. Forgot Password");
                Console.WriteLine("---------------------------------------");
                int choice;
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Login();
                        break;
                    case 2:
                        ForgotPassword();
                        break;
                    default:
                        Console.WriteLine("Enter correct choice");
                        break;
                }
            }

        }







        public void Loginasadmin()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Enter your User ID");
            int username;
            username = Convert.ToInt32(Console.ReadLine());
            string password;
            Console.WriteLine("Enter your password");
            password = Console.ReadLine();
            if (username == 143 && password == "boopathi55")
            {
                Console.WriteLine("Logged in successfully");
                Console.WriteLine("Choose your choice");
                Console.WriteLine("1. View all Products");
                Console.WriteLine("2. Add new Products");
                int choice;
                choice=Convert.ToInt32(Console.ReadLine());
                switch(choice)
                {
                    case 1: RandomProduct();
                        break;
                    case 2:
                        Addproducts();
                        break;
                    default:
                        Console.WriteLine("Enter Correct Choice");
                        Login();
                        break;
                }
            }
            else
            {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Invalid Username and Password");
                Console.WriteLine("Try Again");
                Console.WriteLine("---------------------------------------");
                Loginasadmin();
            }
        }


        public void ForgotPassword()
        {
            List<double> list = new List<double>();
            SqlDataAdapter da = new SqlDataAdapter("select Cusph from Customer", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "newcopy");
            SqlCommandBuilder cmd = new SqlCommandBuilder(da);
            foreach (DataRow dr in ds.Tables["newcopy"].Rows)
            {
                double number;
                number = Convert.ToDouble(dr[0].ToString());

                list.Add(number);
            }
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Enter Your Phone Number");
            double ph;
            ph = Convert.ToInt64(Console.ReadLine());
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (ph == list[i])
                {
                    Console.WriteLine("Valid Customer");
                    count++;
                    break;

                }
            }
            if(count==0)
            {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Not a Valid Customer");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Enter Correct Phone Number");
                ForgotPassword();
            }
            Regenerate();
            void Regenerate() { 
            if (count > 0)
            {
                Random r = new Random();
                int Otp, EnterOtp;
                Otp = r.Next(000000, 999999);
               
                Console.WriteLine(Otp);
                
                Console.WriteLine("Enter your otp");
                EnterOtp = Convert.ToInt32(Console.ReadLine());
                    if (EnterOtp == Otp)
                    {
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("OTP Verified");
                        Console.WriteLine("---------------------------------------");
                        string newpassword;
                        Console.WriteLine("Enter new Password");
                        newpassword = Console.ReadLine();

                        using (SqlCommand command = new SqlCommand("UPDATE Customer SET Cuspassword = @Password WHERE Cusph = @Cusph", conn))
                        {
                            conn.Open();
                            command.Parameters.AddWithValue("@Password", newpassword);
                            command.Parameters.AddWithValue("@Cusph", ph);
                            int rowsAffected = command.ExecuteNonQuery();

                            conn.Close();

                        }
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("----------Password Updated----------");
                        Console.WriteLine("---------------------------------------");
                        Viewproduct();
                    }
                    else
                    {
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("Wrong OTP");
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("To Regenerate OTP Press 1 ");
                        Console.WriteLine("To exit Press any Number");
                        int n;
                        n = Convert.ToInt16(Console.ReadLine());
                        if (n == 1)
                            Regenerate();
                        else
                        { Console.WriteLine(); }
                    }
                }
            }



        }

        public void Desiredproduct()
        {
            int count=0;
            Console.WriteLine("Search your products");
            string searching;
            searching = Console.ReadLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Product ID        Product Name      Cost Per Quantity");
            Console.WriteLine("---------------------------------------");
            using (SqlCommand command = new SqlCommand("select Productid, Productname,Costperquantity from Productdetails where Producttype=@search", conn))
            {
                conn.Open();
                command.Parameters.AddWithValue("@search", searching);
                SqlDataReader s = command.ExecuteReader();
                while (s.Read())
                {
                    Console.WriteLine(s[0] + "           " + s[1]+"             " + s[2]);
                    count++;
                }
                conn.Close();

            }
          
                Order();
            
        }
        public void RandomProduct()
        {
            SqlDataAdapter da = new SqlDataAdapter("select Productid,Productname,Costperquantity from Productdetails", conn);
            DataSet ds = new DataSet();

            da.Fill(ds, "newcopy");
            SqlCommandBuilder cmd = new SqlCommandBuilder(da);
            Console.WriteLine("Product Id       Product Name                  Cost");
            foreach (DataRow dr in ds.Tables["newcopy"].Rows)
            {

                Console.WriteLine(dr[0].ToString() + "              " + dr[1].ToString() + "                " + dr[2].ToString());
            }
            Console.WriteLine("To Logout Press 1");
            int n;
            n = Convert.ToInt32(Console.ReadLine());
            Login();
        }
       

        public void Viewproduct()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("-----Search to display the items-----");
            Console.WriteLine("1. Search your desired product");
            Console.WriteLine("2. Randomly show the products available");
            Console.WriteLine("---------------------------------------");
            int choice;
            choice = Convert.ToInt16(Console.ReadLine());
            switch (choice)
            {
                case 1:
                        Desiredproduct();
                        break;
                    
                case 2:
                    RandomProduct1();
                    break;

                default:
                    Console.WriteLine("---------------------------------------");
                    Console.WriteLine("-----Not a valid one-----");
                    Console.WriteLine("---------------------------------------");
                    Viewproduct();
                    break;
            }
        }


        public void Addproducts()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("-----Adding Products-----");
            Console.WriteLine("---------------------------------------");
            string productname, producttype;
            int Quantity;
            SqlMoney costperquantity;
            Console.WriteLine("Enter the Product Name");
            productname= Console.ReadLine();
            Console.WriteLine("Enter the Product Type");
            producttype= Console.ReadLine();
            Console.WriteLine("Enter the Quantity of the Product");
            Quantity=Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the Cost Per Quantity");
            costperquantity=Convert.ToInt64(Console.ReadLine());
            using (SqlCommand command = new SqlCommand("insert into Productdetails(Productname,Producttype,quantity,Costperquantity) values(@proname,@protype,@Quan,@cost)", conn))
            {
                conn.Open();
                command.Parameters.AddWithValue("@proname", productname);
                command.Parameters.AddWithValue("protype", producttype);
                command.Parameters.AddWithValue("@Quan", Quantity);
                command.Parameters.AddWithValue("@cost", costperquantity);
                command.ExecuteNonQuery();
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Product Added");
                Console.WriteLine("---------------------------------------");
                conn.Close();
            }

            Console.WriteLine("To Logout press 1");
            int n;
            n = Convert.ToInt32(Console.ReadLine());
            Login();


        }
        public void RandomProduct1()
        {
            SqlDataAdapter da = new SqlDataAdapter("select Productid,Productname,Costperquantity from Productdetails", conn);
            DataSet ds = new DataSet();

            da.Fill(ds, "newcopy");
            SqlCommandBuilder cmd = new SqlCommandBuilder(da);
            Console.WriteLine("Product Id       Product Name                  Cost");
            foreach (DataRow dr in ds.Tables["newcopy"].Rows)
            {

                Console.WriteLine(dr[0].ToString() + "              " + dr[1].ToString() + "                " + dr[2].ToString());
            }
            Order();
        }
        public void Order()
        {
           
            int Exit = 1;
            decimal TotalAmount = 0,temp=0;
            int cusid;
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Enter the your Customer Id");
            cusid = Convert.ToInt32(Console.ReadLine());
            newlist();
            void newlist()
            {
                if (Exit != 0)
                {
                    Console.WriteLine("---------------------------------------");
                    Console.WriteLine("Enter the Product Id to Order");
                    int Productid;
                    Productid = Convert.ToInt32(Console.ReadLine());





                    using (SqlCommand command = new SqlCommand("insert into Orderdetails(Cusid,Productid) values(@cus,@proid)", conn))
                    {
                        SqlCommand command1;
                        conn.Open();
                        command.Parameters.AddWithValue("@cus", cusid);
                        command.Parameters.AddWithValue("@proid", Productid);
                        command.ExecuteNonQuery();
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("Product added to cart");
                        Console.WriteLine("---------------------------------------");
                        using (SqlCommand cmd = new SqlCommand("select Costperquantity from Productdetails where Productid=@proid", conn))
                        {


                            cmd.Parameters.AddWithValue("@proid", Productid);

                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                temp = (decimal)reader[0];
                            }
                            reader.Close();
                            TotalAmount = temp + TotalAmount;


                        }
                        conn.Close();
                        
                    }

                    /*SqlDataAdapter da = new SqlDataAdapter($"select Costperquantity from Productdetails where Productid={Productid})", conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "newcopy");
                    SqlCommandBuilder cmd = new SqlCommandBuilder(da);
                    foreach (DataRow dr in ds.Tables["newcopy"].Rows)
                    {
                        temp = (double)dr[0];
                    }
                    TotalAmount = temp + TotalAmount;*/


                    Console.WriteLine("---------------------------------------");
                    Console.WriteLine("To Add More Products Press 1, To Exit Press 0 and Continue to Payment");
                    Exit = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("---------------------------------------");
                    newlist();

                }
               

               /* using (SqlCommand command = new SqlCommand("select sum(Costperquantity) as 'Total Amount' from Productdetails join Orderdetails on Productdetails.Productid=Orderdetails.Productid where)", conn))
                {
                    conn.Open();
                    
                }*/
               

            }
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Total Amount to be Paid " + TotalAmount);
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("1. Cash On Delivery");
            Console.WriteLine("2. Debit/Credit Card");
            Console.WriteLine("3. UPI Pay");
            Console.WriteLine("---------------------------------------");
            int choice;
            choice=Convert.ToInt32(Console.ReadLine());
            if (choice != null)
            {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Payment Successfull");
                Console.WriteLine("Thank You Visit Again!");
                Console.WriteLine("---------------------------------------");
            }

            Console.WriteLine("Press 1 to Logout");
            Console.WriteLine("Press any Number to Exit");
            int n;
            n= Convert.ToInt32(Console.ReadLine());
            if (n != 0) 
            Login();
            else { Console.WriteLine(); }


        }



    }
}


