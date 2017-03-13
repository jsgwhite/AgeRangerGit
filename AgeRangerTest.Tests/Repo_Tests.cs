using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgeRangerTest.Tests
{
    [TestClass]
    public class Repo_Tests
    {
        [TestMethod]
        public void CanGetConnection()
        {
            var repo = new AgeRangerRepository.Repo();
            Assert.IsNotNull(repo.GetAndOpenConnection ());

            // No ex thrown is pass.
        }

        [TestMethod]
        public void AddPerson()
        {
            var repo = new AgeRangerRepository.Repo();
            var testPerson = new AgeRangerEntities.Person { First = "test_first", Last = "test_last", Age = 23 };
            repo.AddPerson(testPerson);
            
            // No ex thrown is pass.
        }

        [TestMethod]
        public void AddPerson_GetPersonByFirstNameReturnsAddedRecord()
        {
            string randomName = Guid.NewGuid().ToString();

            var repo = new AgeRangerRepository.Repo();
            var testPerson = new AgeRangerEntities.Person { First = randomName, Last = "test_last", Age = 2 };
            var addResult = repo.AddPerson(testPerson);
            var person = repo.GetPersonByFirstName(randomName);

            Assert.AreEqual(person.First, testPerson.First);

            // While I'm here verify extra, that description of matched group is as expected.
            Assert.AreEqual(person.Group.Description, "Child");
        }

        [TestMethod]
        public void AddPerson_GetPersonByLastNameReturnsAddedRecord()
        {
            string randomName = Guid.NewGuid().ToString();

            var repo = new AgeRangerRepository.Repo();
            var testPerson = new AgeRangerEntities.Person { First = "first_name", Last = randomName, Age = 100 };
            var addResult = repo.AddPerson(testPerson);
            var person = repo.GetPersonByLastName(randomName);

            Assert.AreEqual(person.First, testPerson.First);

            // While I'm here verify extra, that age range was calc properly.
            Assert.IsTrue(person.Group.MaxAge > testPerson.Age);
            Assert.IsTrue(person.Group.MinAge <= testPerson.Age);
        }

        [TestMethod]
        public void GetAllPeople()
        {
            var repo = new AgeRangerRepository.Repo();
            System.Collections.Generic.List<AgeRangerEntities.Person> persons = repo.GetAllPeople();

            Assert.IsNotNull(persons);
            Assert.AreNotEqual(persons.Count, 0);
        }

        [TestMethod]
        public void GetAllPeople_CountIncreasesAfterAddingPerson()
        {
            var repo = new AgeRangerRepository.Repo();
            int beforeCount = repo.GetAllPeople().Count;

            var testPerson = new AgeRangerEntities.Person { First = "test_first", Last = "test_last", Age = 23 };
            repo.AddPerson(testPerson);
            int afterCount = repo.GetAllPeople().Count;

            Assert.AreEqual(beforeCount, afterCount-1);
        }

        [TestMethod]
        public void GetAgeGroup_EarlyTwenties()
        {
            var repo = new AgeRangerRepository.Repo();
            var ageGroup = repo.GetAgeGroupForThisAge(23);

            Assert.AreEqual(ageGroup.Description, "Early twenties");
        }
        
        [TestMethod]
        public void GetAgeGroup_HandlesNegatives()
        {
            var repo = new AgeRangerRepository.Repo();
            var ageGroup = repo.GetAgeGroupForThisAge(-100);

            Assert.IsNull(ageGroup);

        }

        [TestMethod]
        public void GetAgeGroup_HandlesBoundaryBetweenAgeGroupds()
        {
            var repo = new AgeRangerRepository.Repo();
            var ageGroup = repo.GetAgeGroupForThisAge(30);

            Assert.AreEqual(ageGroup.Description, "Very adult");

        }
    }
}
