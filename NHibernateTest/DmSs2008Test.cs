using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Domain;
using NHibernate;
using NHibernate.Criterion;
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

            Assert.That(aircrafts.Count, Is.EqualTo(15), "number of aircrafts");
            AssertAttributeValuesForOneAircraftOf(aircrafts);
        }

        [Test]
        public void SelectNameModelManufacturerAndWingspanOfAllAircrafts_UsingICriteria()
        {
            TranactionContext.Execute(sessionFactory, session =>
            {
                IList aircrafts =
                    session.CreateCriteria<Aircraft>().SetProjection(
                        Projections.ProjectionList()
                            .Add(Projections.Property("Name"))
                            .Add(Projections.Property("Model"))
                            .Add(Projections.Property("Manufacturer"))
                            .Add(Projections.Property("Wingspan"))).List();

                Assert.That(aircrafts.Count, Is.EqualTo(15), "number of aircrafts");
                AssertAttributeValuesForOneAircraftOf(aircrafts);
            });
        }

        [Test]
        public void SelectNameModelManufacturerAndPowerOfAllMotorizedAircrafts()
        {
            IList motorizedAircrafts = TranactionContext.Execute(sessionFactory, session => session.CreateQuery(

                "select Aircraft.Name, Aircraft.Manufacturer, EnginePower " +
                "from MotorizedAircraft"

            ).List());

            Assert.That(motorizedAircrafts.Count, Is.EqualTo(7), "number of motorized aircrafts");
            AssertAttributeValuesForOneMotorizedAircraft(motorizedAircrafts);
        }


        [Test]
        public void SelectNameModelManufacturerAndPowerOfAllMotorizedAircrafts_UsingICriteria()
        {
            IList motorizedAircrafts = TranactionContext.Execute(sessionFactory, session =>
            {
                return session.CreateCriteria<MotorizedAircraft>("ma")
                    .CreateAlias("Aircraft", "a")
                    .SetProjection(Projections.ProjectionList()
                        .Add(Projections.Property("a.Name"))
                        .Add(Projections.Property("a.Manufacturer"))
                        .Add(Projections.Property("ma.EnginePower")))
                    .List();
            });

            Assert.That(motorizedAircrafts.Count, Is.EqualTo(7), "number of motorized aircrafts");
            AssertAttributeValuesForOneMotorizedAircraft(motorizedAircrafts);
        }

        [Test]
        public void SelectDistinctManufacturerOfMotorizedAircrafts_HavingEnginePowerHigherThan100()
        {
            IList motorizedAircrafts = TranactionContext.Execute(sessionFactory, session => session.CreateQuery(

                "select distinct Aircraft.Manufacturer " +
                "from MotorizedAircraft where EnginePower > 100"

            ).List());

            Assert.That(motorizedAircrafts.Count, Is.EqualTo(2), "number of motorized aircrafts");
            Assert.That(motorizedAircrafts[0], Is.EqualTo("AirJet"), "aircraft manufacturer");
            Assert.That(motorizedAircrafts[1], Is.EqualTo("Flug und Trug"), "aircraft manufacturer");
        }

        [Test]
        public void SelectDistinctManufacturerOfMotorizedAircrafts_HavingEnginePowerHigherThan100_UsingICriteria()
        {
            IList motorizedAircrafts = TranactionContext.Execute(sessionFactory, session =>
            {
                return session.CreateCriteria<MotorizedAircraft>("ma")
                    .CreateAlias("Aircraft", "a")
                    .SetProjection(Projections.ProjectionList()
                        .Add(Projections.Distinct(Projections.Property("a.Manufacturer"))))
                        .Add(Restrictions.Gt("ma.EnginePower", 100))
                    .List();
            });

            Assert.That(motorizedAircrafts.Count, Is.EqualTo(2), "number of motorized aircrafts");
            Assert.That(motorizedAircrafts[0], Is.EqualTo("AirJet"), "aircraft manufacturer");
            Assert.That(motorizedAircrafts[1], Is.EqualTo("Flug und Trug"), "aircraft manufacturer");
        }

        [Test]
        public void SelectNumberOfFlightUnits_Since_01_01_2008()
        {
            IList flightUnits = TranactionContext.Execute(sessionFactory, session => session.CreateQuery(

                "select count(*) " +
                "from FlightUnit where Date > :p"

            ).SetDateTime("p", new DateTime(2008, 1, 1))
            .List());

            Assert.That(flightUnits[0], Is.EqualTo(30), "number of flight units");
        }

        [Test]
        public void SelectNumberOfFlightUnits_Since_01_01_2008_UsingICriteria()
        {
            Int32 flightUnits = TranactionContext.Execute(sessionFactory, session =>
            {
                return session.CreateCriteria<FlightUnit>()
                    .SetProjection(Projections.RowCount())
                    .Add(Restrictions.Gt("Date", new DateTime(2008, 1, 1)))
                    .UniqueResult<Int32>();
            });

            Assert.That(flightUnits, Is.EqualTo(30), "number of flight units");
        }

        [Test]
        public void SelectNumbersOfAllMechanicsWhichLookAfterAircraft_WhereAircraftHasAnIdOfOne()
        {
            IList<Mechanic> mechanics = TranactionContext.Execute(sessionFactory, session =>
            {
                IList<Mechanic> ms = session.CreateQuery("from Mechanic").List<Mechanic>();
                return (from m in ms
                        from h in
                            (from h in m.Hangars
                             from a in h.Aircrafts.Where(a => a.Id == 1)
                             select h)
                        select m).ToList();
            });

            Assert.That(mechanics.Count, Is.EqualTo(4), "number of mechanic numbers");
            AssertMechanicNumbersOf(mechanics);
        }

        private static void AssertAttributeValuesForOneAircraftOf(IList aircrafts)
        {
            Assert.That(((object[])aircrafts[0])[0], Is.EqualTo("Tornado"), "aircraft name");
            Assert.That(((object[])aircrafts[0])[1], Is.EqualTo("ES 3"), "aircraft model");
            Assert.That(((object[])aircrafts[0])[2], Is.EqualTo("Flug und Trug"), "aircraft manufacturer");
            Assert.That((double)((object[])aircrafts[0])[3], Is.EqualTo(26.58).Within(0.001), "aircraft wingspan");
        }

        private static void AssertAttributeValuesForOneMotorizedAircraft(IList motorizedAircrafts)
        {
            Assert.That(((object[])motorizedAircrafts[0])[0], Is.EqualTo("Tornado"), "aircraft name");
            Assert.That(((object[])motorizedAircrafts[0])[1], Is.EqualTo("Flug und Trug"), "aircraft manufacturer");
            Assert.That(((object[])motorizedAircrafts[0])[2], Is.EqualTo(120), "aircraft engine power");
        }

        private static void AssertMechanicNumbersOf(IList<Mechanic> mechanics)
        {
            Assert.That(mechanics[0].Number, Is.EqualTo(10), "mechanic number");
            Assert.That(mechanics[1].Number, Is.EqualTo(12), "mechanic number");
            Assert.That(mechanics[2].Number, Is.EqualTo(13), "mechanic number");
            Assert.That(mechanics[3].Number, Is.EqualTo(14), "mechanic number");
        }
    }
}