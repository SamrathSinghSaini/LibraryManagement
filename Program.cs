using FinalProject.Models;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace FinalProject
{
    class Program
    {

        private static string library1 = "KNOWLEDGE LIBRARY, BARRIE, ONTARIO";
        static string constr = @"Server=LAPTOP-NEOULAC6\SQLEXPRESSDatabase=library;Trusted_Connection=true";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello User!\nWelcome to the Knowledge Library");
            Main1();


            string firstName = "firstname";
            string lastName = "lastname";

            Console.WriteLine("Please enter a new first name:");
            firstName = Console.ReadLine();
            Console.WriteLine(firstName);
            Console.WriteLine("Please enter a new last name:");
            lastName = Console.ReadLine();
            Console.WriteLine(lastName);
            Console.WriteLine("Your User name: " + firstName + " " + lastName);
            Console.WriteLine("Please enter your age");
            String userAge = Console.ReadLine();
            int age;
            bool isParsable = Int32.TryParse(userAge, out age);
            if (isParsable == true)
            { Console.WriteLine($"The User Age is {age})"); }
            else
            { Console.WriteLine("try again later"); }

            library_opening();

            endMessage();

            LibraryInfo info = new LibraryInfo();

            info.charges();
            info.bestsellers();
            booklistfiles();
            //Tablerecord();
            insertdata();


            static void menu()
            {
                Console.WriteLine("1. Display Data");
                Console.WriteLine("2. Insert Data");
                Console.WriteLine("3. Update Data");
                Console.WriteLine("4. Delete Data");
                Console.WriteLine("5. Exit");
                Console.WriteLine("Enter your choice");
                int ch = int.Parse(Console.ReadLine());
                int x = ch;
                switch (x)
                {
                    case 1:
                        Console.WriteLine("Display Data");
                        displaydata();
                        menu();
                        break;
                    case 2:
                        Console.WriteLine("Insert Data");
                        insertdata();
                        menu();
                        break;
                    case 3:
                        Console.WriteLine("Update Data");
                        updatedata();
                        menu();
                        break;
                    case 4:

                        Console.WriteLine("Delete Data");
                        deletedata();
                        menu();
                        break;
                    case 5:
                        Console.WriteLine("Exit");
                        Environment.Exit(0);
                        break;

                    case 6:
                        Console.WriteLine("Read All Data");
                        Tablerecord();
                        break;
                    default: Console.WriteLine("Unknoun value"); break;


                }
            }

            static void Main1()
            {
                // write content to the console
                Console.WriteLine("We hope you are having a good day1");
            }


            static void endMessage()
            {
                Console.WriteLine("Address " + Program.library1);
            }

            static void library_opening()
            {
                //this method displays the day of the week user and tells us if the library is open during that time

                DateTime now = DateTime.Now;

                Console.WriteLine("The day of week: {0}", now.DayOfWeek);
                String day = now.DayOfWeek.ToString();


                if (day == "Saturday" || day == "Monday" || day == "Tuesday" || day == "Wednesday" || day == "Friday")
                {
                    Console.WriteLine("The Knowledge Library is open today!");
                    Console.WriteLine("Timings today will be from 9am to 7pm");
                }
                else if (day == "Sunday" || day == "Thursday")
                {
                    Console.WriteLine("Sorry! The Library is closed on Sundays and Thursdays");
                }

            }


            static void booklistfiles()
            {


                //Get the location  of the Desktop folder
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string filename = "records.csv";
                //Build a file path
                string filePath = $"{desktop}\\{filename}";
                //Read the CSV file
                string csvFileContent = FileHelper.Read(filePath);
                //Get all records based on the assumption that each line represents a record
                //"\r\n" represents carriage return and line feed characters
                //This line removes carriage returns first then parses the new line characters
                string[] records = csvFileContent.Replace("\r", "").Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine("List of Book Titles and Their Authors");
                Console.WriteLine("-------------------------");
                //Print out all records to the console
                //Using an Indexed approach
                //Skip the Header row 1
                for (int i = 1; i < records.Length - 1; i++)
                {
                    //Get all properties from the record
                    string[] properties = records[i].Split(',');
                    //Our properties should be at least 5 items long
                    if (properties.Length > 1)
                    {
                        Console.WriteLine($"{properties[1].PadRight(10)}\t{properties[2]}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Details of Books Including Price and Release Date");
                Console.WriteLine("----------------------------");
                //Print out all records to the console
                //Using an Enumerated approach
                foreach (var record in records)
                {
                    //Get all properties from the record
                    string[] properties = record.Split(',');
                    //Our properties should be at least 5 items long
                    if (properties.Length > 1)
                    {
                        foreach (var property in properties)
                        {
                            //Pad the column length so that columns are consistant length of 10 characters
                            string entry = property.PadRight(10);
                            Console.Write($"{entry}");
                            Console.Write("\t");
                        }
                        Console.Write("\n");
                    }
                }



            }
        


            static void Tablerecord()
            {
                Console.WriteLine("fdfdff");
                using SqlConnection conn = new SqlConnection();
                // Create the connectionString
                conn.ConnectionString = constr;
                try
                {
                    Console.WriteLine("Openning Connection ...");
                    //open connection
                    conn.Open();
                    Console.WriteLine("Connection successful!");
                    Console.WriteLine("============================");
                    // Create the command
                    SqlCommand command = new SqlCommand("SELECT TOP 20 firstname, lastname, age, book FROM data", conn);
                    string output = "";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("First Name - Last Name - Age - Book Wanted ");
                        Console.WriteLine("============================");
                        while (reader.Read())
                        {
                            output = output + reader.GetValue(0) + " - " + reader.GetValue(1) + "\n";
                        }
                        Console.WriteLine(output);
                    }
                    Console.WriteLine("============================");
                    Console.WriteLine("Data displayed! ");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            static void insertdata()
            {

                
               using SqlConnection con = new SqlConnection();
                Console.Write("Enter Three Digit User ID");
                String usernumber = Console.ReadLine();
                string constr1 = null;
                SqlConnection con1 = new SqlConnection(constr1);
                SqlCommand cmd = new SqlCommand("insertdata into data(userid) values(usernumber)", con1);
                con1.Open();
                cmd.Parameters.AddWithValue("@usernumber", usernumber);
                cmd.ExecuteNonQuery();
                con1.Close();
                Console.WriteLine("Successfully Entered");
            }

            static void displaydata()
            {
                // Create the command
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("SELECT * FROM data", con);
                con.Open();
                string outputl = "";
                using (SqlDataReader readerl = cmd.ExecuteReader())
                {
                    Console.WriteLine("userid \t First name \t Last Name \t Age \t Book Wanted"); Console.WriteLine(" "); while (readerl.Read())
                    {
                        outputl = outputl + readerl.GetValue(0) + " \t " + readerl.GetValue(1) + "\n";
                    }
                    Console.WriteLine(outputl);
                }
                con.Close();
            }

            static void updatedata()
            {
                displaydata();
                Console.Write("Enter User Id to Update : ");
                int userid = int.Parse(Console.ReadLine());
                Console.Write("Enter new Name to Update : ");
                string firstname = Console.ReadLine();
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("update data SET firstname = @firstname WHERE userid = @userid", con);
                con.Open();
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@firstname", firstname);
                cmd.ExecuteNonQuery(); con.Close();
                Console.WriteLine("Record Updated Successfully"); displaydata();
            }

            static void deletedata()
            {
                displaydata();
                Console.Write("Enter user Id to Delete : ");
                int userid = int.Parse(Console.ReadLine());
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("delete from data WHERE userid = @userid", con);
                con.Open();
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.ExecuteNonQuery();
                con.Close();
                Console.WriteLine("Record Deleted Successfully");
                displaydata();
            }
        }




        class LibraryInfo
        {

            public void charges()
            {
                Console.WriteLine("The FOllowing is information about the membership charges in the library");

                decimal d = 20.33m;
                decimal e = 13.50m;
                int g = 60;
                Console.WriteLine($"The charges for a month pass are ${d}");
                Console.WriteLine($"The charges for a two week pass are ${e}");
                Console.WriteLine($"The charges for a six month pass are ${g}");

            }



            public void bestsellers()
            {
                Console.WriteLine("The following is a list of the bestselling books available in the library");
                string[] books = { "Milk and Honey by Rupi Kaur", "Educated by Tata Westover", "Gone Girl by GIllian Flynn", "Post Office by Charles Bukowski", "The Stand by Stephen King", "Harry Potter by J.K Rowling" };

                for (int x = 1; x < books.Length; x++)
                {
                    string res = books[x];
                    Console.WriteLine(x + res);
                }

            }
        }

    }
}

