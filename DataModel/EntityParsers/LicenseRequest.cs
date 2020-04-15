using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class LicenseRequest: IEntityWithId, IParsable
    {
        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            DateCreate = DataTypeParser.DateTimeNull(formData["DateCreate"]);
            ClinicName = DataTypeParser.String(formData["ClinicName"]);
            Address = DataTypeParser.String(formData["Address"]);
            Phone1 = DataTypeParser.String(formData["Phone1"]);
            Phone2 = DataTypeParser.String(formData["Phone2"]);
            Email = DataTypeParser.String(formData["Email"]);
            Id_Company = DataTypeParser.Int(formData["Id_Company_AddLicenseRequest_VI"]);
            Id_AdminUnit = DataTypeParser.Int(formData["Id_AdminUnit"]);
            Id_RequestType = DataTypeParser.Int(formData["Id_RequestType_AddLicenseRequest_VI"]);
            Id_User = DataTypeParser.Int(formData["Id_User"]);
            return this;
        }

        public int Id_CurrentState
        {
            get
            {
                return RequestStateHistory.OrderByDescending(x => x.DateStatusChange).Select(x => x.Id_State).FirstOrDefault();
            }
        }
        public RequestState CurrentState
        {
            get
            {
                return RequestStateHistory.OrderByDescending(x => x.DateStatusChange).Select(x => x.RequestState).FirstOrDefault();
            }
        }

        public string AllActivityTypes_ru
        {
            get
            {
                var types = LicenseRequestActivityType.Select(x => x.ActivityType).OrderBy(x => x.Id)
                    .Select(x => x.Name_ru).ToList();
                return string.Join(", ", types);
            }
        }

        public string strRegisterTime
        {
            get { return DateCreate.Value.ToMainDateTimeFormat(); }
        }

        public string FullAddress
        {
            get
            {
                var data = new List<string>();

                var au = AdminUnits;
                while (au.ParentId != null)
                {
                    data.Add(au.Name_ru);
                    au = au.AdminUnits2;
                }

                data.Reverse();
                data.Add(Address);
                return string.Join(", ", data);
            }
        }
    }
}

