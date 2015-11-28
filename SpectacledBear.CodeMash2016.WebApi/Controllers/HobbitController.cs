using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpectacledBear.CodeMash2016.WebApi.Data;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.Controllers
{
    public class HobbitController : ApiController
    {
        private HobbitDataManager _hobbitManager = new HobbitDataManager();

        // GET: api/Hobbit
        public IEnumerable<Hobbit> Get()
        {
            return _hobbitManager.GetAllHobbits();
        }

        // GET: api/Hobbit/5
        public Hobbit Get(long id)
        {
            Hobbit hobbit;
            if (!_hobbitManager.TryGetHobbit(id, out hobbit))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The hobbit identifier provided is not valid.")
                });
            }

            return hobbit;
        }

        // POST: api/Hobbit
        public Hobbit Post(Hobbit hobbit)
        {
            long hobbitId;
            if (_hobbitManager.TryGetHobbitId(hobbit, out hobbitId))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The Hobbit provided already exists.")
                });
            }

            Hobbit insertedHobbit = _hobbitManager.InsertHobbit(hobbit);

            if (insertedHobbit == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The body did not contain a valid Hobbit data type.")
                });
            }

            return insertedHobbit;
        }

        // PUT: api/Hobbit/5
        public Hobbit Put(long id, Hobbit hobbit)
        {
            long hobbitId;
            if (!_hobbitManager.TryGetHobbitId(hobbit, out hobbitId))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The Hobbit provided does not already exist.")
                });
            }

            if(hobbitId != id)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The Hobbit provided already exists as another Hobbit.")
                });
            }

            Hobbit updatedHobbit = _hobbitManager.UpdateHobbit(hobbit, id);

            if (updatedHobbit == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The body did not contain a valid Hobbit data type.")
                });
            }

            return updatedHobbit;
        }

        // DELETE: api/Hobbit/5
        public void Delete(long id)
        {
            Hobbit hobbit;
            if (!_hobbitManager.TryGetHobbit(id, out hobbit))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The hobbit identifier provided is not valid.")
                });
            }

            if(!_hobbitManager.DeleteHobbit(id))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("The hobbit identifier provided could not be deleted.")
                });
            }
        }
    }
}
