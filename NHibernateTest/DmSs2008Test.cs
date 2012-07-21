using System;
using System.Collections;
using System.Collections.Generic;
using DataAccess;
using Domain;
using NHibernate;
using NUnit.Framework;

namespace NHibernateTest
{
    [TestFixture]
    public class DmSs2008Test
    {
        private ISessionFactory sessionFactory;

        [SetUp]
        public virtual void SetUp()
        {
            sessionFactory = NHibernateBase.BuildSessionFactory();
        }

        [Test]
        public void SelectNameModelManufacturerAndWingspanOfAllAircrafts()
        {
            IList aircrafts = TranactionContext.Execute(sessionFactory, session => session.CreateQuery(
                
                "select Name, Model, Manufacturer, Wingspan " +
                "from Aircraft"

            ).List());

            Assert.That(aircrafts.Count, Is.EqualTo(15), "number of airplanes");
            AssertAttributeValuesForOneAircraftOf(aircrafts);
        }

        [Test]
        public void SelectNameModelManufacturerAndPowerOfAll()
        {
            IList motorizedAircrafts = TranactionContext.Execute(sessionFactory, session => session.CreateQuery(

                "select Aircraft.Name, Aircraft.Manufacturer, EnginePower " +
                "from MotorizedAircraft"

            ).List());
            
            Assert.That(motorizedAircrafts.Count, Is.EqualTo(7), "number of motorized aircrafts");
            AssertAttributeValuesForOneMotorizedAircraft(motorizedAircrafts);
        }

        private static void AssertAttributeValuesForOneAircraftOf(IList aircrafts)
        {
            Assert.That(((object[]) aircrafts[0])[0], Is.EqualTo("Tornado"), "airplane name");
            Assert.That(((object[]) aircrafts[0])[1], Is.EqualTo("ES 3"), "airplane model");
            Assert.That(((object[]) aircrafts[0])[2], Is.EqualTo("Flug und Trug"), "airplane manufacturer");
            Assert.That((double) ((object[]) aircrafts[0])[3], Is.EqualTo(26.58).Within(0.001), "airplane wingspan");
        }

        private static void AssertAttributeValuesForOneMotorizedAircraft(IList motorizedAircrafts)
        {
            Assert.That(((object[]) motorizedAircrafts[0])[0], Is.EqualTo("Tornado"), "aircraft name");
            Assert.That(((object[]) motorizedAircrafts[0])[1], Is.EqualTo("Flug und Trug"), "aircraft manufacturer");
            Assert.That(((object[]) motorizedAircrafts[0])[2], Is.EqualTo(120), "aircraft engine power");
        }
    }
}