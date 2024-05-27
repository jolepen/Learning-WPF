using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WpfExampleForToolkit.Models
{
    /// <summary>
    /// Customer
    /// </summary>
    public partial class Customer : ObservableValidator
    {
        private string _customerID;
        [Required]
        [MaxLength(5)]
        public string CustomerID
        {
            get { return _customerID; }
            set { SetProperty(ref _customerID, value, true); }
        }
        private string _companyName;
        [Required]
        [MaxLength(40)]
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(ref _companyName, value, true); }
        }
        [ObservableProperty]
        [MaxLength(30)]
        private string _contactName;
        [ObservableProperty]
        [MaxLength(30)]
        private string _contactTitle;
        [ObservableProperty]
        [MaxLength(60)]
        private string _address;
        [ObservableProperty]
        [MaxLength(15)]
        private string _city;
        [ObservableProperty]
        [MaxLength(15)]
        private string _region;
        [ObservableProperty]
        [MaxLength(10)]
        private string _postalCode;
        [ObservableProperty]
        [MaxLength(15)]
        private string _country;

        private string _phone;
        [MaxLength(24)]
        [Phone]
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref this._phone, value, true); }
        }
        [ObservableProperty]
        [MaxLength(24)]
        [Phone]
        [NotifyDataErrorInfo] //SetProperty함수의 valid 를 설정하지 않고도 해당 속성을 추가해서 Validation을 작동 가능하다.
        private string _fax;
    }
}