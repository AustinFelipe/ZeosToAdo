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
        public void ShouldReadTopAndBottom()
        {
            // Arrange
            var unit = "unit Uni1; random object that shouldn't be parsed unit Teste2, Teste3;";

            // Act
            var parser = new UnitParser();
            var unitParsed = parser.GetParsed(unit).ToList();

            // Assert
            Assert.AreEqual("Uni1", unitParsed[0].Name);
            Assert.AreEqual(UnitPosition.Top, unitParsed[0].Position);

            Assert.AreEqual("Teste2", unitParsed[1].Name);
            Assert.AreEqual(UnitPosition.Bottom, unitParsed[1].Position);

            Assert.AreEqual("Teste3", unitParsed[2].Name);
            Assert.AreEqual(UnitPosition.Bottom, unitParsed[2].Position);
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
