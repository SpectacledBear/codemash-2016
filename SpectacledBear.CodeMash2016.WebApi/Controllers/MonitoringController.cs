using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SpectacledBear.CodeMash2016.WebApi.Data;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.Controllers
{
    public class MonitoringController : ApiController
    {
        SqliteDataManager _sqliteData = new SqliteDataManager();

        // GET: api/Monitoring
        public SqliteModel Get()
        {
            SqliteModel model = _sqliteData.GetModels();

            return model;
        }

        // GET: api/Monitoring/5
        public string Get(int id)
        {
            throw new NotImplementedException("This method has not been written yet.");
        }

        // POST: api/Monitoring
        public void Post([FromBody]string value)
        {
            throw new NotImplementedException("This method has not been written yet.");
        }

        // PUT: api/Monitoring/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException("This method has not been written yet.");
        }

        // DELETE: api/Monitoring/5
        public void Delete(int id)
        {
            throw new NotImplementedException("This method has not been written yet.");
        }
    }
}
