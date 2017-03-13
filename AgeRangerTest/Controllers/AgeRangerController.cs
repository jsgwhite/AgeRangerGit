using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgeRangerTest.Controllers
{
    public class AgeRangerController : ApiController
    {
        private AgeRangerRepository.Repo GetRepo()
        {
            var repo = new AgeRangerRepository.Repo();
            repo.DbFile = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/AppData"), "AgeRanger.db");
            return repo;
        }


        // GET: api/AgeRanger
        public IEnumerable<AgeRangerEntities.Person> Get()
        {
            return GetRepo().GetAllPeople();
        }
        
        // GET: api/AgeRanger?first_name={}
        public AgeRangerEntities.Person GetByFirst(string first_name)
        {
            return GetRepo().GetPersonByFirstName(first_name);
        }

        // GET: api/AgeRanger?last_name={}
        public AgeRangerEntities.Person GetByLast(string last_name)
        {
            return GetRepo().GetPersonByLastName(last_name);
        }
        
        // PUT: api/AgeRanger/5
        public void Put(int id, [FromBody]Models.PersonModel value)
        {
            // Wouldn't normally do mapping here.
            var p = new AgeRangerEntities.Person() { Id = value.Id, First = value.FirstName, Last = value.LastName, Age = value.Age };
            GetRepo().AddPerson(p);
        }

        // DELETE: api/AgeRanger/5
        public void Delete(int id)
        {
        }
    }
}
