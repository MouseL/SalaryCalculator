using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace SalaryCalculator.Engine
{
    public interface iSalaryBaseDef
    {
        double GetPayrollItemBase(string itemID, Hashtable inputParams);

    }


    public class NeimengSalaryCalculationFactorBase : iSalaryBaseDef
    {
        delegate double ItemBaseFinder(string itemID, Hashtable inputParams);
        Hashtable _baseFinderList;
        

        public double GetPayrollItemBase(string itemID, Hashtable inputParams)
        {
            ItemBaseFinder finder = ((ItemBaseFinder)_baseFinderList[itemID]);
            return finder(itemID, inputParams);
        }

        public NeimengSalaryCalculationFactorBase()
        {
            _baseFinderList = new Hashtable();

            _baseFinderList.Add("LevelSalary", new ItemBaseFinder(LevelSalaryBase));

            _baseFinderList.Add("JobTitleAllowance", new ItemBaseFinder(JobTitleAllowanceBase));

            _baseFinderList.Add("WorkAgeAllowance", new ItemBaseFinder(WorkAgeAllowanceBase));

            _baseFinderList.Add("OnFieldBase", new ItemBaseFinder(OnFieldBase));

            _baseFinderList.Add("LiveHomeBase", new ItemBaseFinder(LiveHomeBase));

            _baseFinderList.Add("PerformanceSalaryBase", new ItemBaseFinder(PerformanceSalaryBase));

            _baseFinderList.Add("SubsidyBase", new ItemBaseFinder(SubsidyBase));

            _baseFinderList.Add("RegistrationAllowanceBase", new ItemBaseFinder(RegistrationAllowanceBase));

            _baseFinderList.Add("CookAllowanceBase", new ItemBaseFinder(CookAllowanceBase));

            _baseFinderList.Add("OTWeekdayRate", new ItemBaseFinder(OTRate));
            _baseFinderList.Add("OTWeekendRate", new ItemBaseFinder(OTRate));
            _baseFinderList.Add("OTHolidayRate", new ItemBaseFinder(OTRate));

            _baseFinderList.Add("CommunicationAllowanceBase", new ItemBaseFinder(CommunicationAllowanceBase));

            _baseFinderList.Add("FoodAllowanceBase", new ItemBaseFinder(FoodAllowanceBase));


        }

        static List<SingleKeyEntry> LevelSalaryBaseTable = InitLevelSalaryBaseTable();

        static List<SingleKeyEntry> InitLevelSalaryBaseTable()
        {
            List<SingleKeyEntry> baseTable = new List<SingleKeyEntry>();
            baseTable.Add(new SingleKeyEntry("Intern", 1500.0));
            baseTable.Add(new SingleKeyEntry("1st", 1700.0));
            baseTable.Add(new SingleKeyEntry("2nd", 2100.0));
            baseTable.Add(new SingleKeyEntry("3rd", 2500.0));
            baseTable.Add(new SingleKeyEntry("4th", 3000.0));
            baseTable.Add(new SingleKeyEntry("5th", 3600.0));
            baseTable.Add(new SingleKeyEntry("6th", 4300.0));
            baseTable.Add(new SingleKeyEntry("7th", 5200.0));
            baseTable.Add(new SingleKeyEntry("8thf", 6200.0));
            baseTable.Add(new SingleKeyEntry("Profession", 10000.0));
            baseTable.Add(new SingleKeyEntry("Director", 10200.0));
            // 实习生（1500）、一级（1700）、二级（2100）、三级（2500）、四级（3000）、五级（3600）、六级（4300）、七级（5200）、八级（6200）、专家（10000）、主任（10200）
            return baseTable;
        }

        double LevelSalaryBase(string itemID, Hashtable inputParams)
        {
            string paramKey = "Level";
            string level = (string)inputParams[paramKey];

            return FindValueInTable(LevelSalaryBaseTable, level);            
        }

        static List<SingleKeyEntry> PerformanceSalaryBaseTable = InitPerformanceSalaryBaseTable();

        static List<SingleKeyEntry> InitPerformanceSalaryBaseTable()
        {
            List<SingleKeyEntry> baseTable = new List<SingleKeyEntry>();
            baseTable.Add(new SingleKeyEntry("Chairman", 900.0));
            baseTable.Add(new SingleKeyEntry("Director", 700.0));
            baseTable.Add(new SingleKeyEntry("Inspector", 500.0));
            baseTable.Add(new SingleKeyEntry("Other", 300.0));
            //  Post:项目部主任 900,    总监总监(主管）700      监理师（专责）500， 员级（试用期）300
            return baseTable;
        }

        double PerformanceSalaryBase(string itemID, Hashtable inputParams)
        {
            string Post = (string)inputParams["Post"];

            return FindValueInTable(PerformanceSalaryBaseTable, Post);
        }

        static List<SingleKeyEntry> FoodAllowanceBaseTable = InitFoodAllowanceBaseTable();

        static List<SingleKeyEntry> InitFoodAllowanceBaseTable()
        {
            List<SingleKeyEntry> baseTable = new List<SingleKeyEntry>();
            baseTable.Add(new Engine.SingleKeyEntry("Contractor", 750.0));
            baseTable.Add(new Engine.SingleKeyEntry("Other", 500.0));
            //  PeopleType:合同工每月伙食费为750，劳务派遣及协议的行政人员每月为500
            return baseTable;
        }

        double FoodAllowanceBase(string itemID, Hashtable inputParams)
        {
            string PeopleType = (string)inputParams["PeopleType"];
            return FindValueInTable(FoodAllowanceBaseTable, PeopleType);
        }


        static List<SingleKeyEntry> CommunicationAllowanceBaseTable = InitCommunicationAllowanceBaseTable();

        static List<SingleKeyEntry> InitCommunicationAllowanceBaseTable()
        {
            List<SingleKeyEntry> baseTable = new List<SingleKeyEntry>();
            baseTable.Add(new SingleKeyEntry("Director", 300.0));
            baseTable.Add(new SingleKeyEntry("DirectorDeputy", 200.0));
            baseTable.Add(new Engine.SingleKeyEntry("Other", 60.0));
            //  Post:岗位	总监	总代	总代以下
            //       金额   300      200        60

            return baseTable;
        }

        double CommunicationAllowanceBase(string itemID, Hashtable inputParams)
        {
            string Post = (string)inputParams["Post"];

            return FindValueInTable(CommunicationAllowanceBaseTable, Post);
        }

        //注册津贴  RegistrationType, Post, EmploymentType
        /*
           （一）注册咨询师、注册安全师、注册设备监理师和一级建造师2000元/月；注册造价师4000元/月；注册监理工程师5000元/月；
           （二）除主专业外的其余注册证书1000元/月（即双证及以上的人员，除专业对口的证书外，其余所有证书1000元/月）
           （三）岗位要与执业资格证书专业相对应，不对应的注册津贴执行50%或进行人员调岗。
           （四）公司全职人员按100%津贴计算，挂证人员按50%津贴计算。     有问题
         */

        static List<SingleKeyEntry> RegistrationAllowanceBaseTable = InitRegistrationAllowanceBaseTable();

        static List<SingleKeyEntry> InitRegistrationAllowanceBaseTable()
        {
            List<SingleKeyEntry> baseTable = new List<SingleKeyEntry>();
            baseTable.Add(new Engine.SingleKeyEntry("注册咨询师", 2000.0));
            baseTable.Add(new Engine.SingleKeyEntry("注册安全师", 2000.0));
            baseTable.Add(new Engine.SingleKeyEntry("注册设备监理师", 2000.0));
            baseTable.Add(new Engine.SingleKeyEntry("一级建造师", 2000.0));
            baseTable.Add(new Engine.SingleKeyEntry("注册造价师", 4000.0));
            baseTable.Add(new Engine.SingleKeyEntry("注册监理工程师", 5000.0));

            baseTable.Add(new Engine.SingleKeyEntry("Other", 1000.0));
            //  RegistrationType （一）注册咨询师、注册安全师、注册设备监理师和一级建造师2000元/月；注册造价师4000元/月；注册监理工程师5000元/月；
            return baseTable;
        }

        double RegistrationAllowanceBase(string itemID, Hashtable inputParams)
        {
            string RegistrationType = (string)inputParams["RegistrationType"];

            return FindValueInTable(RegistrationAllowanceBaseTable, RegistrationType);
        }

        static List<SingleKeyEntry> OTRateTable = InitOTRateTableTable();

        static List<SingleKeyEntry> InitOTRateTableTable()
        {
            List<SingleKeyEntry> baseTable = new List<SingleKeyEntry>();
            baseTable.Add(new Engine.SingleKeyEntry("OTWeekdayRate", 1.0));
            baseTable.Add(new Engine.SingleKeyEntry("OTWeekendRate", 2.0));
            baseTable.Add(new Engine.SingleKeyEntry("OTHolidayRate", 3.0));

            return baseTable;
        }

        double OTRate(string itemID, Hashtable inputParams)
        {
            return FindValueInTable(OTRateTable, itemID);
        }


        List<DoubleKeyEntry> JobTitleBaseTable = InitJobTitleBaseTable();

        static List<DoubleKeyEntry> InitJobTitleBaseTable()
        {
            List<DoubleKeyEntry> titleBaseTable = new List<DoubleKeyEntry>();
            titleBaseTable.Add(new DoubleKeyEntry("Director", "Senior", 4000.0));
            titleBaseTable.Add(new DoubleKeyEntry("DirectorDeputy", "Senior", 3000.0));
            titleBaseTable.Add(new DoubleKeyEntry("Other", "Senior", 2000.0));
            titleBaseTable.Add(new DoubleKeyEntry("Director", "Middle", 3000.0));
            titleBaseTable.Add(new DoubleKeyEntry("DirectorDeputy", "Middle", 2000.0));
            titleBaseTable.Add(new DoubleKeyEntry("Other", "Middle", 1000.0));
            titleBaseTable.Add(new DoubleKeyEntry("Director", "Junior", 1000.0));
            titleBaseTable.Add(new DoubleKeyEntry("DirectorDeputy", "Junior", 500.0));
            titleBaseTable.Add(new DoubleKeyEntry("Other", "Junior", 300.0));
            return titleBaseTable;
        }

        double JobTitleAllowanceBase(string itemID, Hashtable inputParams)
        {
            string paramKey1 = "JobTitle";
            string paramKey2 = "Post";
            string title = (string)inputParams[paramKey1];
            string post = (string)inputParams[paramKey2];

            return FindValueInTable(JobTitleBaseTable, post, title);
        }

        static List<SingleKeyEntry> GeneralFactorBaseTable = InitGeneralFactorBaseTable();

        static List<SingleKeyEntry> InitGeneralFactorBaseTable()
        {
            List<SingleKeyEntry> baseTable = new List<SingleKeyEntry>();
            baseTable.Add(new Engine.SingleKeyEntry("WorkAgeAllowanceBase", 60.0));
            baseTable.Add(new Engine.SingleKeyEntry("OnFieldBase", 60.0));
            baseTable.Add(new Engine.SingleKeyEntry("LiveHomeBase", 60.0));
            baseTable.Add(new Engine.SingleKeyEntry("SubsidyBase", 44.0));
            baseTable.Add(new Engine.SingleKeyEntry("CookAllowanceBase", 10.0));

            return baseTable;
        }


        double WorkAgeAllowanceBase(string itemID, Hashtable inputParams)
        {
            return FindValueInTable(GeneralFactorBaseTable, "WorkAgeAllowanceBase");
        }

        double OnFieldBase(string itemID, Hashtable inputParams)
        {
            return FindValueInTable(GeneralFactorBaseTable, "OnFieldBase");
        }

        double LiveHomeBase(string itemID, Hashtable inputParams)
        {
            return FindValueInTable(GeneralFactorBaseTable, "LiveHomeBase");
        }

        double SubsidyBase(string itemID, Hashtable inputParams)
        {
            return FindValueInTable(GeneralFactorBaseTable, "SubsidyBase");
        }

        double CookAllowanceBase(string itemID, Hashtable inputParams)
        {
            return FindValueInTable(GeneralFactorBaseTable, "CookAllowanceBase");
        }


        double FindValueInTable(List<SingleKeyEntry> table, string key)
        {
            if (key != null)
            {
                foreach (SingleKeyEntry entry in table)
                {
                    if (entry.Key.Equals(key))
                        return entry.Value;
                }
            }
            return 0.0;
        }

        double FindValueInTable(List<DoubleKeyEntry> table, string key1, string key2)
        {
            foreach (DoubleKeyEntry entry in table)
            {
                if (entry.Key1.Equals(key1) && entry.Key2.Equals(key2))
                    return entry.Value;
                
            }
            return 0.0;
        }

    }

    [Serializable]
    public class SingleKeyEntry
    {
        string _key;
        double _value;

        public string Key
        {
            get
            {
                return _key;
            }

            set
            {
                _key = value;
            }
        }

        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public SingleKeyEntry(string key, double value)
        {
            _key = key;
            _value = value;
        }

        public SingleKeyEntry()
        {
        }
    }

    [Serializable]
    public class DoubleKeyEntry
    {
        string _key1;
        string _key2;
        double _value;

        public string Key1
        {
            get
            {
                return _key1;
            }

            set
            {
                _key1 = value;
            }
        }

        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public string Key2
        {
            get
            {
                return _key2;
            }

            set
            {
                _key2 = value;
            }
        }

        public DoubleKeyEntry(string key1, string key2, double value)
        {
            _key1 = key1;
            _key2 = key2;
            _value = value;
        }

        public DoubleKeyEntry()
        {
        }
    }

}
