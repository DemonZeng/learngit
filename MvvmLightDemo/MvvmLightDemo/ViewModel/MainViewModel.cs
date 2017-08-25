using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MvvmLightDemo.MessageInfrastructure;
using MvvmLightDemo.Model;
using MvvmLightDemo.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace MvvmLightDemo.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        IDataAccessService _serviceProxy;

        ObservableCollection<EmployeeInfo> _Employees;
        public ObservableCollection<EmployeeInfo> Employees
        {
            get { return _Employees; }
            set
            {
                _Employees = value;
                RaisePropertyChanged("Employees");
            }
        }

        /// <summary>
        /// The declaration of the Employee object for Save and Messanger purpose
        /// </summary>
        EmployeeInfo _EmpInfo;

        public EmployeeInfo EmpInfo
        {
            get { return _EmpInfo; }
            set
            {
                _EmpInfo = value;
                RaisePropertyChanged("EmpInfo");
            }
        }

        /// <summary>
        /// The declaration used in case of search Employee
        /// </summary>
        string _EmpName;

        public string EmpName
        {
            get { return _EmpName; }
            set
            {
                _EmpName = value;
                RaisePropertyChanged("EmpName");
            }
        }
       
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataAccessService servPxy)
        {
            _serviceProxy = servPxy;
            Employees = new ObservableCollection<EmployeeInfo>();
            ReadAllCommand = new RelayCommand(GetEmployees);
            EmpInfo = new EmployeeInfo();
            SaveCommand = new RelayCommand<EmployeeInfo>(SaveEmployee);
            SearchCommand = new RelayCommand(SearchEmployee);
            SendEmployeeCommand = new RelayCommand<EmployeeInfo>(SendEmployeeInfo);
            ReceiveEmployeeInfo();
        }

        void GetEmployees()
        {
            Employees.Clear();
            foreach (var item in _serviceProxy.GetEmployees())
            {
                Employees.Add(item);
            }
        }

        void SaveEmployee(EmployeeInfo emp)
        {
            EmpInfo.EmpNo = _serviceProxy.CreateEmployee(emp);
            if(EmpInfo.EmpNo != 0)
            {
                Employees.Add(EmpInfo);
                RaisePropertyChanged("EmpInfo");
            }
        }

        void SearchEmployee()
        {
            Employees.Clear();
            var Res = from e in _serviceProxy.GetEmployees()
                      where e.EmpName.StartsWith(EmpName)
                      select e;
            foreach (var item in Res)
            {
                Employees.Add(item);
            }
        }

        void SendEmployeeInfo(EmployeeInfo emp)
        {
            if (emp != null)
            {
                Messenger.Default.Send<MessageCommunicator>(new MessageCommunicator()
                {
                    Emp = emp
                });
            }
        }

        void ReceiveEmployeeInfo()
        {
            if(EmpInfo != null)
            {
                Messenger.Default.Register<MessageCommunicator>(this, (emp) =>
                {
                    this.EmpInfo = emp.Emp;
                });
            }
        }

        public RelayCommand ReadAllCommand { get; set; }
        public RelayCommand<EmployeeInfo> SaveCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand<EmployeeInfo> SendEmployeeCommand { get; set; }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}