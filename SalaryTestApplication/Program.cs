using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Collections;

using SalaryCalculator.Service;
using SalaryCalculator.Engine;

using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Configuration;

namespace SalaryTestApplication
{
    
    class Program
    {
        delegate double CalculateItem();

        static void Main(string[] args)
        {
            //SalaryTest();

            SerializationTest();
            Console.In.ReadLine();

        }

        static void SerializationTest()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<SingleKeyEntry>));

            List<SingleKeyEntry> myList = new List<SingleKeyEntry>();
            myList.Add(new SingleKeyEntry("11", 1.0));
            myList.Add(new SingleKeyEntry("22", 2.0));
            myList.Add(new SingleKeyEntry("33", 3.0));

            StringWriter sw = new StringWriter();

            serializer.Serialize(sw, myList);

            string ss = sw.GetStringBuilder().ToString();
            //Console.Out.WriteLine(ss);

            string json = JsonConvert.SerializeObject(myList);
            Console.Out.WriteLine(json);


            //List<SingleKeyEntry> newList = (List<SingleKeyEntry>)serializer.Deserialize(new StringReader(ss));

            string newJson = ConfigurationManager.AppSettings["MyInitData"];

            List<SingleKeyEntry> newList = JsonConvert.DeserializeObject<List<SingleKeyEntry>>(newJson);


            Console.Out.WriteLine("\n---- \n" + newList.Count);

        }

        static void SalaryTest()
        {
            SalaryCalculator.Service.SalaryCalculationService engine = new SalaryCalculationService();
            Hashtable inputParams = new Hashtable();
            inputParams.Add("Level", "1st");
            inputParams.Add("Post", "Director");
            inputParams.Add("JobTitle", "Junior");
            inputParams.Add("AttendanceDays", 30);
            inputParams.Add("WorkAge", 1);
            inputParams.Add("LiveOnFieldDays", 1);
            inputParams.Add("LiveHomeDays", 1);
            inputParams.Add("SubsidyDays", 10);
            inputParams.Add("CompensatoryLeave", 0);
            string[] ss = new string[] { "注册咨询师", "注册监理工程师" };
            inputParams.Add("RegistrationList", ss);
            inputParams.Add("PartTimeAllowance", 30.0);
            inputParams.Add("LocalAllowance", 40.0);
            inputParams.Add("SocialInsurance", 50.0);
            inputParams.Add("Reward", 60.0);
            inputParams.Add("OTHoursWeekday", 1 * 8 * 21.75);
            inputParams.Add("OTHoursWeekend", 2 * 8 * 21.75);
            inputParams.Add("OTHoursHoliday", 1 * 8 * 21.75);

            inputParams.Add("Backpay", 321.0);
            inputParams.Add("Withhold", 654.0);


            inputParams.Add("Riskfee", 999.0);
            inputParams.Add("WithholdOther", 888.0);




            System.Collections.Hashtable ht = engine.GetSalaryForPerson("", "", inputParams);

            foreach (string key in ht.Keys)
                if ((double)ht[key] > 0)
                    Console.Out.WriteLine(key + ":" + ht[key]);
            Console.In.ReadLine();

            inputParams["Level"] = "Director";
            inputParams["Post"] = "DirectorDeputy";
            inputParams["JobTitle"] = "Senior";
            inputParams["AttendanceDays"] = 15;
            inputParams["WorkAge"] = 2;
            inputParams["LiveOnFieldDays"] = 2;
            inputParams["LiveHomeDays"] = 2;
            inputParams["SubsidyDays"] = 20;
            inputParams["CompensatoryLeave"] = 20;
            inputParams["RegistrationList"] = null;
            inputParams["PartTimeAllowance"] = 111.0;
            inputParams["LocalAllowance"] = 222.0;
            inputParams["SocialInsurance"] = 333.0;
            inputParams["Reward"] = 444.0;

            inputParams["OTHoursWeekday"] = 0 * 8 * 21.75;
            inputParams["OTHoursWeekend"] = 1 * 8 * 21.75;
            inputParams["OTHoursHoliday"] = 1 * 8 * 21.75;

            inputParams["Backpay"] = 123.0;
            inputParams["Withhold"] = 456.0;

            inputParams["Riskfee"] = 1999.0;
            inputParams["WithholdOther"] = 1888.0;


            ht = engine.GetSalaryForPerson("", "", inputParams);

            foreach (string key in ht.Keys)
                if ((double)ht[key] >= 0)
                    Console.Out.WriteLine(key + ":" + ht[key]);
            Console.In.ReadLine();
        }


    }


}
