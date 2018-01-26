using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

//Karl Burke

namespace Customer_Records
{
    //Class used to store the customers when they have been imported
    public class Customer
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("user_id")]
        public int User_id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }

    public class Program
    {
        //Converts Degrees to Radians
        public static double ConvertDegreesToRadians(double degrees)
        {
            degrees = degrees * Math.PI / 180;

            return degrees;
        }

        //Establishes if the customer is within the specified distance
        public static Boolean FindDistance(double latitude, double longitude)
        {
            double earthCircumference = 40000;
            double distance = 0;
            double maxDistance = 100;
            double intercomLatitude = 52.986375;
            double intercomLongitude = -6.257664;

            double intercomLatitudeRadians = ConvertDegreesToRadians(intercomLatitude);
            double intercomLongitudeRadians = ConvertDegreesToRadians(intercomLongitude);
            double customerLatitudeRadians = ConvertDegreesToRadians(latitude);
            double customerLongitudeRadians = ConvertDegreesToRadians(longitude);

            double longitudeDifference = Math.Abs(intercomLongitudeRadians - customerLongitudeRadians);

            if(longitudeDifference > Math.PI)
            {
                longitudeDifference = 2.0 * Math.PI - longitudeDifference;
            }

            
            double angleCalculation = Math.Acos(Math.Sin(customerLatitudeRadians) * Math.Sin(intercomLatitudeRadians) +
                Math.Cos(customerLatitudeRadians) * Math.Cos(intercomLatitudeRadians) * Math.Cos(longitudeDifference));

            distance = earthCircumference * angleCalculation / (2.0 * Math.PI);

            if (distance < maxDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Reads in the JSON file and adds each item to a list of customers
        public static List<Customer> ReadInCustomers(string filePath)
        {
            List<Customer> Customers = new List<Customer>();
            string Input;

            try
            {
                StreamReader reader = new StreamReader(filePath);

                while ((Input = reader.ReadLine()) != null)
                {
                    var values = JsonConvert.DeserializeObject<Customer>(Input);

                    Customers.Add(values);
                }
                reader.Close();
                
                //Re-organises the list in ascending order by User_id
                Customers = Customers.OrderBy(x => x.User_id).ToList();                
            }
            catch(Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }

            return Customers;
        }

        static void Main()
        {
            //Local location of file
            string filePath = "E:\\customers.json";
            //
            List<Customer> customers = ReadInCustomers(filePath);
            //Used to print the names and ID's of the customers
            string output = "";

            //Checks every customer in the populatedlist
            foreach (Customer customer in customers)
            {
                //Passes customers GPS location
                Boolean withinDistance = FindDistance(customer.Latitude, customer.Longitude);

                //Adds the name and ID of the customer within the specified distance
                if (withinDistance == true)
                {
                    output += customer.User_id + " " + customer.Name + Environment.NewLine;
                }
            }

            //Prints the list of customers
            Console.WriteLine(output);

            Console.ReadLine();
        }

    }
}
