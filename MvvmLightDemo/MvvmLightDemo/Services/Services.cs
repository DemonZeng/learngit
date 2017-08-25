using MvvmLightDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLightDemo.Services
{
    public interface IDataAccessService
    {
        ObservableCollection<EmployeeInfo> GetEmployees();
        int CreateEmployee(EmployeeInfo Emp);
    }

    public class DataAccessService : IDataAccessService
    {
        CompanyEntties context;
        public DataAccessService()
        {
            context = new CompanyEntties();
        }
        public int CreateEmployee(EmployeeInfo Emp)
        {
            context.EmployeeInfo.Add(Emp);
            context.SaveChanges();
            return Emp.EmpNo;
        }

        public ObservableCollection<EmployeeInfo> GetEmployees()
        {
            ObservableCollection<EmployeeInfo> Employees = new
                ObservableCollection<EmployeeInfo>();
            foreach(var item in context.EmployeeInfo)
            {
                Employees.Add(item);
            }
            return Employees;
        }
    }
}
