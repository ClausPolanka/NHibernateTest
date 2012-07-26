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
            IList aircrafts = TranactionContext.Execute(sessionFactory, session =>
            {
                return session.CreateCriteria<Aircraft>().SetProjection(
                    Projections.ProjectionList()
                        .Add(Projections.Property("Name"))
                        .Add(Projections.Property("Model"))
                        .Add(Projections.Property("Manufacturer"))
                        .Add(Projections.Property("Wingspan"))).List();

            });

            Assert.That(aircrafts.Count, Is.EqualTo(15), "number of aircrafts");
            AssertAttributeValuesForOneAircraftOf(aircrafts);
        }

        [Test]
        public void SelectNameModelManufacturerAndWingspanOfAllAircrafts_UsingICriteriaWithTypeSafeQueryOver()
        {
            IList<object[]> aircrafts = TranactionContext.Execute(sessionFactory, session =>
            {
                return session.QueryOver<Aircraft>()
                    .Select(a => a.Name, a => a.Model, a => a.Manufacturer, a => a.Wingspan)
                    .List<object[]>();
            });

            Assert.That(aircrafts.Count, Is.EqualTo(15), "number of aircrafts");
            AssertAttributeValuesForOneAircraftOf((IList) aircrafts);

            // Oder

            aircrafts = TranactionContext.Execute(sessionFactory, session =>
            {
                return session.QueryOver<Aircraft>()
                    .Select(Projections.ProjectionList()
                        .Add(Projections.Property<Aircraft>(a => a.Name))
                        .Add(Projections.Property<Aircraft>(a => a.Model))
                        .Add(Projections.Property<Aircraft>(a => a.Manufacturer))
                        .Add(Projections.Property<Aircraft>(a => a.Wingspan)))
                    .List<object[]>();
            });

            Assert.That(aircrafts.Count, Is.EqualTo(15), "number of aircrafts");
            AssertAttributeValuesForOneAircraftOf((IList) aircrafts);

            // Oder

            aircrafts = TranactionContext.Execute(sessionFactory, session =>
            {
                return session.QueryOver<Aircraft>()
                    .SelectList(list => list
                        .Select(a => a.Name)
                        .Select(a => a.Model)
                        .Select(a => a.Manufacturer)
                        .Select(a => a.Wingspan))
                    .List<object[]>();
            });

            Assert.That(aircrafts.Count, Is.EqualTo(15), "number of aircrafts");
            AssertAttributeValuesForOneAircraftOf((IList)aircrafts);

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
        public void SelectNameModelManufacturerAndPowerOfAllMotorizedAircrafts_UsingICriteriaWithTypeSafeQueryOver()
        {
            IList<object[]> motorizedAircrafts = TranactionContext.Execute(sessionFactory, session =>
            {
                MotorizedAircraft maAlias = null;
                Aircraft aAlias = null;

                return session.QueryOver<MotorizedAircraft>(() => maAlias)
                    .JoinAlias(() => maAlias.Aircraft, () => aAlias)
                    .Select(Projections.ProjectionList()
                        .Add(Projections.Property(() => aAlias.Name))
                        .Add(Projections.Property(() => aAlias.Manufacturer))
                        .Add(Projections.Property(() => maAlias.EnginePower)))
                    .List<object[]>();
            });

            Assert.That(motorizedAircrafts.Count, Is.EqualTo(7), "number of motorized aircrafts");
            AssertAttributeValuesForOneMotorizedAircraft((IList) motorizedAircrafts);
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
                    .SetProjection(Projections.Distinct(Projections.Property("a.Manufacturer")))
                    .Add(Restrictions.Gt("ma.EnginePower", 100))
                    .List();
            });

            Assert.That(motorizedAircrafts.Count, Is.EqualTo(2), "number of motorized aircrafts");
            Assert.That(motorizedAircrafts[0], Is.EqualTo("AirJet"), "aircraft manufacturer");
            Assert.That(motorizedAircrafts[1], Is.EqualTo("Flug und Trug"), "aircraft manufacturer");
        }

        [Test] public void 
        SelectDistinctManufacturerOfMotorizedAircrafts_HavingEnginePowerHigherThan100_UsingICriteriaWithTypeSafeQueryOver()
        {
            IList<string> motorizedAircrafts = TranactionContext.Execute(sessionFactory, session =>
            {
                MotorizedAircraft maAlias = null;
                Aircraft aAlias = null;

                return session.QueryOver<MotorizedAircraft>(() => maAlias)
                    .JoinAlias(() => maAlias.Aircraft, () => aAlias)
                    .Where(() => maAlias.EnginePower > 100) 
                    .Select(Projections.Distinct(Projections.Property(() => aAlias.Manufacturer)))
                    .List<string>();
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
        public void SelectNumberOfFlightUnits_Since_01_01_2008_UsingICriteriaWithTypeSafeQueryOver()
        {
            Int32 flightUnits = TranactionContext.Execute(sessionFactory, session =>
            {
                return session.QueryOver<FlightUnit>()
                    .Where(f => f.Date > new DateTime(2008, 1, 1))
                    .RowCount();
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

        [Test]
        public void DifferenceBetweenJoinAndIsIn_NoDmExerice()
        {
            var teachsers_flightunits = TranactionContext.Execute(sessionFactory, session => session.CreateQuery(

                "select count(*) from Teacher as t join t.FlightUnits"

                                                                                                 ).UniqueResult());

            Assert.That(teachsers_flightunits, Is.EqualTo(84), "teachers' flight units");
            
            IList<Teacher> teachersHavingFlightUnits = TranactionContext.Execute(sessionFactory, session =>
            {
                return session.QueryOver<Teacher>()
                    .Where(t => t.Number.IsIn(
                        session.CreateQuery("select distinct Teacher.Number from FlightUnit").List()))
                    .List();  
            });

            Assert.That(teachersHavingFlightUnits.Count, Is.EqualTo(12), "number of teacher who have flight units");
        }

        private static void AssertAttributeValuesForOneAircraftOf(IList aircrafts)
        {
            Assert.That(((object[]) aircrafts[0])[0], Is.EqualTo("Tornado"), "aircraft name");
            Assert.That(((object[]) aircrafts[0])[1], Is.EqualTo("ES 3"), "aircraft model");
            Assert.That(((object[]) aircrafts[0])[2], Is.EqualTo("Flug und Trug"), "aircraft manufacturer");
            Assert.That((double) ((object[]) aircrafts[0])[3], Is.EqualTo(26.58).Within(0.001), "aircraft wingspan");
        }

        private static void AssertAttributeValuesForOneMotorizedAircraft(IList motorizedAircrafts)
        {
            Assert.That(((object[]) motorizedAircrafts[0])[0], Is.EqualTo("Tornado"), "aircraft name");
            Assert.That(((object[]) motorizedAircrafts[0])[1], Is.EqualTo("Flug und Trug"), "aircraft manufacturer");
            Assert.That(((object[]) motorizedAircrafts[0])[2], Is.EqualTo(120), "aircraft engine power");
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