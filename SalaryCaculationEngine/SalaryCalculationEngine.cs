using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace SalaryCalculator.Engine
{
    public interface iSalaryAlgorithmDef
    {
        List<string> GetPayrollItemList();

        double GetPayrollItemValue(string itemID, iSalaryBaseDef baseDef, Hashtable inputParams);

    }

    public class NeimengSalaryCalculationEngine : iSalaryAlgorithmDef
    {
        delegate double PayrollItemCalculator(iSalaryBaseDef baseDef, Hashtable inputParams);
        Hashtable _calculatorList;

        public List<string> GetPayrollItemList()
        {
            List<string> payRollItemList = new List<string>(itemList);
            
            return payRollItemList;
        }

        string[] itemList = {
            "LevelSalary", //级别工资   Level
             /*
                实习生（1500）、一级（1700）、二级（2100）、三级（2500）、四级（3000）、五级（3600）、六级（4300）、七级（5200）、八级（6200）、专家（10000）、主任（10200）
              */
             "JobTitleAllowance", //职称津贴      JobTitle   Post  AttendanceDays  
             /* JobTitle 高级工程师（高级技师）、工程师（技师）、助理工程师;  Post:总监 总监代表  监理师及以下    有问题*/
             "WorkAgeAllowance", // 工龄津贴  WorkAge Post
             /*WorkAge * 60  if(not 地区主任）  */
             "OnFieldAllowance", // 现场津贴   LiveOnFieldDays LiveHomeDays
             /*    OnFieldDays  *60  +  LiveHomeDays * 45          */
             "Subsidy", // 生活费补助天数    SubsidyDays
             /*     SubsidyDays * 44                  */
             "BaseSalary", // 基础工资
             /*  LevelSalary/30*(AttendanceDays(出勤）+CompensatoryLeave(补休）)  + Subsidy */

             "PerformanceSalary", // 绩效工资  Post  AttendanceDays
             /*  Post:项目部主任 900,    总监总监(主管）700 
                监理师（专责）500， 员级（试用期）300  /30 * AttendanceDays  */ 
 
             "RegistrationAllowance", //注册津贴  RegistrationType, Post, EmploymentType
             /*
                （一）注册咨询师、注册安全师、注册设备监理师和一级建造师2000元/月；注册造价师4000元/月；注册监理工程师5000元/月；
                （二）除主专业外的其余注册证书1000元/月（即双证及以上的人员，除专业对口的证书外，其余所有证书1000元/月）
                （三）岗位要与执业资格证书专业相对应，不对应的注册津贴执行50%或进行人员调岗。
                （四）公司全职人员按100%津贴计算，挂证人员按50%津贴计算。     有问题
              */
            
             "CookAllowance", // 厨师津贴  LiveOnFieldDays
             /*  LiveOnFieldDays * 10    */
             
             "PartTimeAllowance", // 兼职津贴  PartTimeAllowance
             "LocalAllowance", // 地方补贴   LocalAllowance
             "SocialInsurance", //社保   SocialInsurance
             "Reward", // 奖励           Reward

            "OvertimeAllowance", // 加班费     OTHours 实际加班（小时） OTHoursWeekday  OTHoursWeekend  OTHoursHoliday
             /*  BaseSalary /21.75 * OTHours /8 * OTRate
              * 
机关人员：周一至周五（200%）；周六、周日（200%）；法定节假日（300%）;
                 现场人员：法定节假日（300%）;   有问题    
              */
             "CommunicationAllowance", // 通讯费    Post AttendanceDays
             /*  Attendance * base/30  */
             "Backpay", // 补发工资   Backpay
             "Withhold", // 扣款  Withhold
             "SalaryTotal", // 应发工资
             /*  BaseSalary + OnFieldAllowance + WorkAgeAllowance +
PerformanceSalary + JobTitleAllowance + RegistrationAllowance +
PartTimeAllowance + LocalAllowance + Reward + OvertimeAllowance
              *  + CommunicationAllowance + Backpay - Withhold  */
             "FoodAllowance", // 伙食费    PeopleType    有问题
             "TaxBasetotal", // 计税基础
             /* SalaryTotal + FoodAllowance - SocialInsurance  */
             "Riskfee", // 扣风险金
             "Tax", // 扣税  
             "WithholdOther", // 扣款其他  WithholdOther
             "ActualIncome" // 实发工资
              };


        public double GetPayrollItemValue(string itemID, iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            PayrollItemCalculator calculator = ((PayrollItemCalculator)_calculatorList[itemID]);
            if (calculator != null)
                return calculator(baseDef, inputParams);
            else
                return -1;
        }


        public NeimengSalaryCalculationEngine()
        {
            _calculatorList = new Hashtable();

            _calculatorList.Add("LevelSalary", new PayrollItemCalculator(LevelSalaryCompute));

            _calculatorList.Add("JobTitleAllowance", new PayrollItemCalculator(JobTitleAllowanceCompute));

            _calculatorList.Add("WorkAgeAllowance", new PayrollItemCalculator(WorkAgeAllowanceCompute));

            _calculatorList.Add("OnFieldAllowance", new PayrollItemCalculator(OnFieldAllowanceCompute));

            _calculatorList.Add("Subsidy", new PayrollItemCalculator(SubsidyCompute));

            _calculatorList.Add("BaseSalary", new PayrollItemCalculator(BaseSalaryCompute));

            _calculatorList.Add("PerformanceSalary", new PayrollItemCalculator(PerformanceSalaryCompute));

            _calculatorList.Add("RegistrationAllowance", new PayrollItemCalculator(RegistrationAllowanceCompute));

            _calculatorList.Add("CookAllowance", new PayrollItemCalculator(CookAllowanceCompute));


            _calculatorList.Add("PartTimeAllowance", new PayrollItemCalculator(PartTimeAllowanceCompute));
            _calculatorList.Add("LocalAllowance", new PayrollItemCalculator(LocalAllowanceCompute));
            _calculatorList.Add("SocialInsurance", new PayrollItemCalculator(SocialInsuranceCompute));
            _calculatorList.Add("Reward", new PayrollItemCalculator(RewardCompute));

            _calculatorList.Add("OvertimeAllowance", new PayrollItemCalculator(OvertimeAllowanceCompute));

            _calculatorList.Add("CommunicationAllowance", new PayrollItemCalculator(CommunicationAllowanceCompute));

            _calculatorList.Add("Backpay", new PayrollItemCalculator(BackpayCompute));
            _calculatorList.Add("Withhold", new PayrollItemCalculator(WithholdCompute));

            _calculatorList.Add("SalaryTotal", new PayrollItemCalculator(SalaryTotalCompute));

            _calculatorList.Add("FoodAllowance", new PayrollItemCalculator(FoodAllowanceCompute));

            _calculatorList.Add("TaxBaseTotal", new PayrollItemCalculator(TaxBaseTotalCompute));

            _calculatorList.Add("Riskfee", new PayrollItemCalculator(RiskfeeCompute));
            _calculatorList.Add("Tax", new PayrollItemCalculator(TaxCompute));
            _calculatorList.Add("WithholdOther", new PayrollItemCalculator(WithholdOtherCompute));
            _calculatorList.Add("ActualIncome", new PayrollItemCalculator(ActualIncomeCompute));

        }

        double LevelSalaryCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return baseDef.GetPayrollItemBase("LevelSalary", inputParams);
        }

        double JobTitleAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            double titleBase = baseDef.GetPayrollItemBase("JobTitleAllowance", inputParams);
            int AttendanceDays = (int)inputParams["AttendanceDays"];
            if (AttendanceDays >= 30)
                return titleBase;
            else
                return titleBase / 30.0 * AttendanceDays;
        }

        double WorkAgeAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            double ageBase = baseDef.GetPayrollItemBase("WorkAgeAllowance", inputParams);
            int WorkAge = (int)inputParams["WorkAge"];
            string Post = (string)inputParams["Post"];
            if (Post.Equals("LocalDirector"))
                return 0;
            else
                return ageBase * WorkAge;
        }

        double OnFieldAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            double OnFieldBase = baseDef.GetPayrollItemBase("OnFieldBase", inputParams);
            double LiveHomeBase = baseDef.GetPayrollItemBase("LiveHomeBase", inputParams); ;
            int OnFieldDays = (int)inputParams["LiveOnFieldDays"];
            int LiveHomeDays = (int)inputParams["LiveHomeDays"];

            return OnFieldBase * OnFieldDays + LiveHomeBase * LiveHomeDays;
        }

        double SubsidyCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            double SubsidyBase = baseDef.GetPayrollItemBase("SubsidyBase", inputParams);
            int SubsidyDays = (int)inputParams["SubsidyDays"];

            return SubsidyBase * SubsidyDays;
        }

        double BaseSalaryCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            double LevelSalary = LevelSalaryCompute(baseDef, inputParams);
            double Subsidy = SubsidyCompute(baseDef, inputParams);

            int CompensatoryLeave = (int)inputParams["CompensatoryLeave"];
            int AttendanceDays = (int)inputParams["AttendanceDays"];

            return LevelSalary / 30.0 * (CompensatoryLeave + AttendanceDays) + Subsidy;
        }

        double PerformanceSalaryCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            int AttendanceDays = (int)inputParams["AttendanceDays"];
            double PerformanceSalaryBase = baseDef.GetPayrollItemBase("PerformanceSalaryBase", inputParams); ;

            if (AttendanceDays > -30)
                return PerformanceSalaryBase;
            else
                return PerformanceSalaryBase / 30.0 * AttendanceDays;
        }

        double RegistrationAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            //"RegistrationAllowance", //注册津贴  RegistrationType, Post, EmploymentType
            string[] RegistrationList = (string[])inputParams["RegistrationList"];

            if (RegistrationList == null)
                return 0.0;

            if (RegistrationList.Length > 0)
            {
                double maxRegistrationAllowance = 0;
                foreach (string registration in RegistrationList)
                {
                    if (inputParams.ContainsKey("RegistrationType"))
                        inputParams["RegistrationType"] = registration;
                    else
                        inputParams.Add("RegistrationType", registration);
                    double RegistrationBase = baseDef.GetPayrollItemBase("RegistrationAllowanceBase", inputParams);
                    if (RegistrationBase > maxRegistrationAllowance)
                        maxRegistrationAllowance = RegistrationBase;
                }

                if (inputParams.ContainsKey("RegistrationType"))
                    inputParams["RegistrationType"] = "Other";
                else
                    inputParams.Add("RegistrationType", "Other");
                double otherRegistrationBase = baseDef.GetPayrollItemBase("RegistrationAllowanceBase", inputParams);
                return maxRegistrationAllowance + otherRegistrationBase * (RegistrationList.Length - 1);
            }

            return 0.0;
        }

        double CookAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            double CookAllowanceBase = baseDef.GetPayrollItemBase("CookAllowanceBase", inputParams);
            int OnFieldDays = (int)inputParams["LiveOnFieldDays"];

            return CookAllowanceBase * OnFieldDays;
        }

        double PartTimeAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return (double)inputParams["PartTimeAllowance"];
        }

        double LocalAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return (double)inputParams["LocalAllowance"];
        }

        double SocialInsuranceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return (double)inputParams["SocialInsurance"];
        }

        double RewardCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return (double)inputParams["Reward"];
        }


        double BackpayCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return (double)inputParams["Backpay"];
        }

        double WithholdCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return (double)inputParams["Withhold"];
        }

        double OvertimeAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            double OTWeekdayRate = baseDef.GetPayrollItemBase("OTWeekdayRate", inputParams);
            double OTHoursWeekday = (double)inputParams["OTHoursWeekday"];
            double OTWeekendRate = baseDef.GetPayrollItemBase("OTWeekendRate", inputParams);
            double OTHoursWeekend = (double)inputParams["OTHoursWeekend"];
            double OTHolidayRate = baseDef.GetPayrollItemBase("OTHolidayRate", inputParams);
            double OTHoursHoliday = (double)inputParams["OTHoursHoliday"];

            double BaseSalary = BaseSalaryCompute(baseDef, inputParams);

            return BaseSalary / 21.75 * OTHoursWeekday / 8 * OTWeekdayRate
                + BaseSalary / 21.75 * OTHoursWeekend / 8 * OTWeekendRate
                + BaseSalary / 21.75 * OTHoursHoliday / 8 * OTHolidayRate;
        }


        double CommunicationAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            int AttendanceDays = (int)inputParams["AttendanceDays"];
            double CommunicationAllowanceBase = baseDef.GetPayrollItemBase("CommunicationAllowanceBase", inputParams);

            return AttendanceDays * CommunicationAllowanceBase / 30.0;
        }

        double SalaryTotalCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            double BaseSalary = BaseSalaryCompute(baseDef, inputParams);
            double OnFieldAllowance = OnFieldAllowanceCompute(baseDef, inputParams);
            double WorkAgeAllowance = WorkAgeAllowanceCompute(baseDef, inputParams);
            double PerformanceSalary = PerformanceSalaryCompute(baseDef, inputParams);
            double JobTitleAllowance = JobTitleAllowanceCompute(baseDef, inputParams);
            double RegistrationAllowance = RegistrationAllowanceCompute(baseDef, inputParams);
            double PartTimeAllowance = PartTimeAllowanceCompute(baseDef, inputParams);
            double CookAllowance = CookAllowanceCompute(baseDef, inputParams);
            double LocalAllowance = LocalAllowanceCompute(baseDef, inputParams);
            double Reward = RewardCompute(baseDef, inputParams);
            double OvertimeAllowance = OvertimeAllowanceCompute(baseDef, inputParams);
            double CommunicationAllowance = CommunicationAllowanceCompute(baseDef, inputParams);
            double Backpay = BackpayCompute(baseDef, inputParams);
            double Withhold = WithholdCompute(baseDef, inputParams);



            return BaseSalary + OnFieldAllowance + WorkAgeAllowance +
                   PerformanceSalary + JobTitleAllowance + RegistrationAllowance +
                   PartTimeAllowance + CookAllowance + LocalAllowance + Reward + OvertimeAllowance +
                   CommunicationAllowance + Backpay - Withhold;
        }

        double FoodAllowanceCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return baseDef.GetPayrollItemBase("FoodAllowanceBase", inputParams); ;
        }

        double TaxBaseTotalCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {

            double SalaryTotal = SalaryTotalCompute(baseDef, inputParams);
            double FoodAllowance = FoodAllowanceCompute(baseDef, inputParams);
            double SocialInsurance = SocialInsuranceCompute(baseDef, inputParams);

            return SalaryTotal + FoodAllowance - SocialInsurance;
        }

        double RiskfeeCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return (double)inputParams["Riskfee"];
        }

        double WithholdOtherCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {
            return (double)inputParams["WithholdOther"];
        }

        double TaxCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {

            double TaxBaseTotal = TaxBaseTotalCompute(baseDef, inputParams);

            double TaxFreeBase = 3500.00;

            double NoneFreeTaxBase = TaxBaseTotal - TaxFreeBase;
            double TaxRate = 0.0;
            double CalculationFactor = 0.0;

            if (NoneFreeTaxBase > 80000)
            {
                TaxRate = 0.45;
                CalculationFactor = 13505.0;
            }
            else if (NoneFreeTaxBase > 55000)
            {
                TaxRate = 0.35;
                CalculationFactor = 5505.0;
            }
            else if (NoneFreeTaxBase > 35000)
            {
                TaxRate = 0.30;
                CalculationFactor = 2755.0;
            }
            else if (NoneFreeTaxBase > 9000)
            {
                TaxRate = 0.20;
                CalculationFactor = 555.0;
            }
            else if (NoneFreeTaxBase > 4500)
            {
                TaxRate = 0.10;
                CalculationFactor = 105.0;
            }
            else if (NoneFreeTaxBase > 1500)
            {
                TaxRate = 0.03;
            }

            return NoneFreeTaxBase * TaxRate - CalculationFactor;
        }

        double ActualIncomeCompute(iSalaryBaseDef baseDef, Hashtable inputParams)
        {

            double SalaryTotal = SalaryTotalCompute(baseDef, inputParams);
            double Withhold = WithholdCompute(baseDef, inputParams);
            double Riskfee = RiskfeeCompute(baseDef, inputParams);
            double Tax = TaxCompute(baseDef, inputParams);
            double WithholdOther = WithholdOtherCompute(baseDef, inputParams);

            return SalaryTotal - Withhold - Riskfee - Tax - WithholdOther;

        }


    }

}
