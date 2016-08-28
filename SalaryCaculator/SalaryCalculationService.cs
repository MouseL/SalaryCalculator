using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

using SalaryCalculator.Engine;

namespace SalaryCalculator.Service
{
    interface iSalaryService
    {
        Hashtable GetSalaryForPerson(string algorithmDefID, string ruleBaseID, Hashtable inputParams );

    }

    
    public class SalaryCalculationService : iSalaryService
    {
        iSalaryAlgorithmDef _algorithmDef;
        iSalaryBaseDef _baseDef;

        void InitAlgorithmDef()
        {
            _algorithmDef = new NeimengSalaryCalculationEngine();
        }

        void InitBaseDef()
        {
            _baseDef = new NeimengSalaryCalculationFactorBase();
        }

        iSalaryAlgorithmDef GetAlgorithmDefByID(string algorithmDefID)
        {
            return _algorithmDef;
        }

        iSalaryBaseDef GetBaseDefByID(string ruleBaseID)
        {
            return _baseDef;
        }

        public Hashtable GetSalaryForPerson(string algorithmDefID, string ruleBaseID, Hashtable inputParams)
        {
            Hashtable payrollResult = new Hashtable();
            foreach (string payrollItem in _algorithmDef.GetPayrollItemList())
            {
                payrollResult.Add(payrollItem, _algorithmDef.GetPayrollItemValue(payrollItem, _baseDef, inputParams));
            }
            return payrollResult;
        }

        public SalaryCalculationService()
        {
            InitBaseDef();
            InitAlgorithmDef();
        }
    }

    

    
}
