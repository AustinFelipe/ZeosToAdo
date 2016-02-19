using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MigradorZeosParaADO.Parse;
using System.Collections.Generic;
using MigradorZeosParaADO.DelphiParts;
using System.Linq;

namespace MigradorZeosParaADO.Tests.Parse
{
    [TestClass]
    public class UnitParserTests
    {
        [TestMethod]
        public void ShouldReturnAListOfTopUnits()
        {
            // Arrange
            var unit = "unit Uni1, Unit2, Unit3;";
            List<Unit> expectation = new List<Unit>
            {
                new Unit { Name = "Unit1", Position = UnitPosition.Top },
                new Unit { Name = "Unit2", Position = UnitPosition.Top },
                new Unit { Name = "Unit3", Position = UnitPosition.Top }
            };
            List<Unit> result;

            // Act
            var parser = new UnitParser();
            result = parser.GetParsed(unit).ToList();

            // Assert
            Assert.AreEqual(expectation.Count, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldReturnAnErrorIfMoreThan3Declarations()
        {
            // Arrange
            var unit = "unit Uni1, Unit2, Unit3; unit Uni4; unit Uni5;";

            // Act
            var parser = new UnitParser();

            // Assert (Exception)
            parser.GetParsed(unit);
        }
    }
}
