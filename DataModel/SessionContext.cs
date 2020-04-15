using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class MedlicenseSessionEntities : MedlicenseEntities
    {
        private string SessionId { get; set; }
        public MedlicenseSessionEntities(string sessionId)
        {
            SessionId = sessionId;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
            if (Database.Connection.State != ConnectionState.Open)
                Database.Connection.Open();

            var command = Database.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "declare @p as VARBINARY(128) set @p = CAST(N'" + SessionId +
                                  "' as VARBINARY(128)) SET CONTEXT_INFO @p";
            command.ExecuteNonQuery();

            return base.SaveChanges();
        }
    }
}
