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
        private IDataManager<Hobbit> _hobbitDataManager;

        public HobbitController()
        {
            _hobbitDataManager = new HobbitDataManager();
        }

        internal HobbitController(IDataManager<Hobbit> hobbitManager)
        {
            _hobbitDataManager = hobbitManager;
        }

        // GET: api/Hobbit
        public IEnumerable<Hobbit> Get()
        {
            return _hobbitDataManager.GetAll();
        }

        // GET: api/Hobbit/5
        public Hobbit Get(long id)
        {
            Hobbit hobbit;
            if (!_hobbitDataManager.TryGet(id, out hobbit))
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
            if (_hobbitDataManager.TryGet(hobbit, out hobbitId))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The Hobbit provided already exists.")
                });
            }

            Hobbit insertedHobbit = _hobbitDataManager.Insert(hobbit);

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
            if (!_hobbitDataManager.TryGet(hobbit, out hobbitId))
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

            Hobbit updatedHobbit = _hobbitDataManager.Update(hobbit, id);

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
            if (!_hobbitDataManager.TryGet(id, out hobbit))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The hobbit identifier provided is not valid.")
                });
            }

            if(!_hobbitDataManager.Delete(id))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("The hobbit identifier provided could not be deleted.")
                });
            }
        }
    }
}
