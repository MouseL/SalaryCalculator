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

            _baseFinderList.Add("ComunicationAllowanceBase", new ItemBaseFinder(ComunicationAllowanceBase));

            _baseFinderList.Add("FoodAllowanceBase", new ItemBaseFinder(FoodAllowanceBase));


        }

        static Hashtable LevelSalaryBaseTable = InitLevelSalaryBaseTable();

        static Hashtable InitLevelSalaryBaseTable()
        {
            Hashtable baseTable = new Hashtable();
            baseTable.Add("Intern", 1500.0);
            baseTable.Add("1st", 1700.0);
            baseTable.Add("2nd", 2100.0);
            baseTable.Add("3rd", 2500.0);
            baseTable.Add("4th", 3000.0);
            baseTable.Add("5th", 3600.0);
            baseTable.Add("6th", 4300.0);
            baseTable.Add("7th", 5200.0);
            baseTable.Add("8thf", 6200.0);
            baseTable.Add("Profession", 10000.0);
            baseTable.Add("Director", 10200.0);
            // 实习生（1500）、一级（1700）、二级（2100）、三级（2500）、四级（3000）、五级（3600）、六级（4300）、七级（5200）、八级（6200）、专家（10000）、主任（10200）
            return baseTable;
        }

        double LevelSalaryBase(string itemID, Hashtable inputParams)
        {
            string paramKey = "Level";
            string level = (string)inputParams[paramKey];
            if (level != null)
            {
                return (double)LevelSalaryBaseTable[level];
            }
            return 0.0;
        }

        static Hashtable PerformanceSalaryBaseTable = InitPerformanceSalaryBaseTable();

        static Hashtable InitPerformanceSalaryBaseTable()
        {
            Hashtable baseTable = new Hashtable();
            baseTable.Add("Chairman", 900.0);
            baseTable.Add("Director", 700.0);
            baseTable.Add("Inspector", 500.0);
            baseTable.Add("Other", 300.0);
            //  Post:项目部主任 900,    总监总监(主管）700      监理师（专责）500， 员级（试用期）300
            return baseTable;
        }

        double PerformanceSalaryBase(string itemID, Hashtable inputParams)
        {
            string Post = (string)inputParams["Post"];
            if (Post != null)
            {
                if (PerformanceSalaryBaseTable.ContainsKey(Post))
                    return (double)PerformanceSalaryBaseTable[Post];
            }
            return 0.0;
        }

        static Hashtable FoodAllowanceBaseTable = InitFoodAllowanceBaseTable();

        static Hashtable InitFoodAllowanceBaseTable()
        {
            Hashtable baseTable = new Hashtable();
            baseTable.Add("Contractor", 750.0);
            baseTable.Add("Other", 500.0);
            //  PeopleType:合同工每月伙食费为750，劳务派遣及协议的行政人员每月为500
            return baseTable;
        }

        double FoodAllowanceBase(string itemID, Hashtable inputParams)
        {
            string PeopleType = (string)inputParams["PeopleType"];
            if (PeopleType != null)
            {
                if (FoodAllowanceBaseTable.ContainsKey(PeopleType))
                    return (double)FoodAllowanceBaseTable[PeopleType];
            }
            return 0.0;
        }


        static Hashtable ComunicationAllowanceBaseTable = InitComunicationAllowanceBaseTable();

        static Hashtable InitComunicationAllowanceBaseTable()
        {
            Hashtable baseTable = new Hashtable();
            baseTable.Add("Director", 300.0);
            baseTable.Add("DirectorDeputy", 200.0);
            baseTable.Add("Other", 60.0);
            //  Post:岗位	总监	总代	总代以下
            //       金额   300      200        60

            return baseTable;
        }

        double ComunicationAllowanceBase(string itemID, Hashtable inputParams)
        {
            string Post = (string)inputParams["Post"];
            if (Post != null)
            {
                if (ComunicationAllowanceBaseTable.ContainsKey(Post))
                    return (double)ComunicationAllowanceBaseTable[Post];
            }
            return 0.0;
        }

        //注册津贴  RegistrationType, Post, EmploymentType
        /*
           （一）注册咨询师、注册安全师、注册设备监理师和一级建造师2000元/月；注册造价师4000元/月；注册监理工程师5000元/月；
           （二）除主专业外的其余注册证书1000元/月（即双证及以上的人员，除专业对口的证书外，其余所有证书1000元/月）
           （三）岗位要与执业资格证书专业相对应，不对应的注册津贴执行50%或进行人员调岗。
           （四）公司全职人员按100%津贴计算，挂证人员按50%津贴计算。     有问题
         */

        static Hashtable RegistrationAllowanceBaseTable = InitRegistrationAllowanceBaseTable();

        static Hashtable InitRegistrationAllowanceBaseTable()
        {
            Hashtable baseTable = new Hashtable();
            baseTable.Add("注册咨询师", 2000.0);
            baseTable.Add("注册安全师", 2000.0);
            baseTable.Add("注册设备监理师", 2000.0);
            baseTable.Add("一级建造师", 2000.0);
            baseTable.Add("注册造价师", 4000.0);
            baseTable.Add("注册监理工程师", 5000.0);

            baseTable.Add("Other", 1000.0);
            //  RegistrationType （一）注册咨询师、注册安全师、注册设备监理师和一级建造师2000元/月；注册造价师4000元/月；注册监理工程师5000元/月；
            return baseTable;
        }

        double RegistrationAllowanceBase(string itemID, Hashtable inputParams)
        {
            string RegistrationType = (string)inputParams["RegistrationType"];
            if (RegistrationType != null)
            {
                if (RegistrationAllowanceBaseTable.ContainsKey(RegistrationType))
                    return (double)RegistrationAllowanceBaseTable[RegistrationType];
            }
            return 0.0;
        }

        static Hashtable OTRateTable = InitOTRateTableTable();

        static Hashtable InitOTRateTableTable()
        {
            Hashtable baseTable = new Hashtable();
            baseTable.Add("OTWeekdayRate", 2.0);
            baseTable.Add("OTWeekendRate", 2.0);
            baseTable.Add("OTHolidayRate", 3.0);

            return baseTable;
        }

        double OTRate(string itemID, Hashtable inputParams)
        {
            if (itemID != null)
            {
                if (OTRateTable.ContainsKey(itemID))
                    return (double)OTRateTable[itemID];
            }
            return 0.0;
        }


        List<JobTitleBaseInfo> JobTitleBaseTable = InitJobTitleBaseTable();

        static List<JobTitleBaseInfo> InitJobTitleBaseTable()
        {
            List<JobTitleBaseInfo> titleBaseTable = new List<JobTitleBaseInfo>();
            titleBaseTable.Add(new JobTitleBaseInfo("Director", "Senior", 4000.0));
            titleBaseTable.Add(new JobTitleBaseInfo("DirectorDeputy", "Senior", 3000.0));
            titleBaseTable.Add(new JobTitleBaseInfo("Other", "Senior", 2000.0));
            titleBaseTable.Add(new JobTitleBaseInfo("Director", "Middle", 3000.0));
            titleBaseTable.Add(new JobTitleBaseInfo("DirectorDeputy", "Middle", 2000.0));
            titleBaseTable.Add(new JobTitleBaseInfo("Other", "Middle", 1000.0));
            titleBaseTable.Add(new JobTitleBaseInfo("Director", "Junior", 1000.0));
            titleBaseTable.Add(new JobTitleBaseInfo("DirectorDeputy", "Junior", 500.0));
            titleBaseTable.Add(new JobTitleBaseInfo("Other", "Junior", 300.0));
            return titleBaseTable;
        }

        double JobTitleAllowanceBase(string itemID, Hashtable inputParams)
        {
            string paramKey1 = "JobTitle";
            string paramKey2 = "Post";
            string title = (string)inputParams[paramKey1];
            string post = (string)inputParams[paramKey2];
            foreach (JobTitleBaseInfo baseInfo in JobTitleBaseTable)
            {
                if (baseInfo.Post.Equals(post) && baseInfo.Title.Equals(title))
                    return baseInfo.Base;
            }
            return 0.0;
        }

        struct JobTitleBaseInfo
        {
            public string Post;
            public string Title;
            public double Base;

            public JobTitleBaseInfo(string post, string title, double salaryBase)
            {
                this.Post = post;
                this.Title = title;
                this.Base = salaryBase;
            }
        }


        double WorkAgeAllowanceBase(string itemID, Hashtable inputParams)
        {
            return 60.0;
        }

        double OnFieldBase(string itemID, Hashtable inputParams)
        {
            return 60.0;
        }

        double LiveHomeBase(string itemID, Hashtable inputParams)
        {
            return 60.0;
        }

        double SubsidyBase(string itemID, Hashtable inputParams)
        {
            return 44.0;
        }

        double CookAllowanceBase(string itemID, Hashtable inputParams)
        {
            return 10.0;
        }




    }

}
