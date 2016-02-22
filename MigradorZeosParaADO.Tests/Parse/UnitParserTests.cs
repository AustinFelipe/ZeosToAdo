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
            var parser = new UnitDeclarationParser();
            var unitParsed = parser.GetParsed(unit).ToList();

            // Assert
            Assert.AreEqual(3, unitParsed.Count);

            Assert.AreEqual("Uni1", unitParsed[0].Name);
            Assert.AreEqual(UsesPosition.Top, unitParsed[0].Position);

            Assert.AreEqual("Teste2", unitParsed[1].Name);
            Assert.AreEqual(UsesPosition.Bottom, unitParsed[1].Position);

            Assert.AreEqual("Teste3", unitParsed[2].Name);
            Assert.AreEqual(UsesPosition.Bottom, unitParsed[2].Position);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldReturnAnErrorIfMoreThan3Declarations()
        {
            // Arrange
            var unit = "unit Uni1, Unit2, Unit3; teste unit Uni4; test unit Uni5;";

            // Act
            var parser = new UnitDeclarationParser();

            // Assert (Exception)
            parser.GetParsed(unit).ToList();
        }

        [TestMethod]
        public void ShouldConvertAUnitListToStringDeclaration()
        {
            // Arrange
            var unitDeclaration = "unit TesteUnit, TesteUnit2, TesteUnit3;";
            var listOfUnit = new List<UsesDeclaration>
            {
                new UsesDeclaration { Name = "TesteUnit", Position = UsesPosition.Top },
                new UsesDeclaration { Name = "TesteUnit2", Position = UsesPosition.Top },
                new UsesDeclaration { Name = "TesteUnit3", Position = UsesPosition.Bottom },
            };

            // Act
            var parser = new UnitDeclarationParser();
            var result = parser.ToString(listOfUnit);

            // Assert
            Assert.AreEqual(unitDeclaration, result);
        }

        [TestMethod]
        public void ShouldReturnATopDeclaration()
        {
            // Arrange
            var unitDeclaration = "unit TesteUnit, TesteUnit2;";
            var listOfUnit = new List<UsesDeclaration>
            {
                new UsesDeclaration { Name = "TesteUnit", Position = UsesPosition.Top },
                new UsesDeclaration { Name = "TesteUnit2", Position = UsesPosition.Top },
                new UsesDeclaration { Name = "TesteUnit3", Position = UsesPosition.Bottom },
            };

            // Act
            var parser = new UnitDeclarationParser();
            var result = parser.ToTopUnitString(listOfUnit);

            // Assert
            Assert.AreEqual(unitDeclaration, result);
        }

        [TestMethod]
        public void ShouldReturnABottomDeclaration()
        {
            // Arrange
            var unitDeclaration = "unit TesteUnit3;";
            var listOfUnit = new List<UsesDeclaration>
            {
                new UsesDeclaration { Name = "TesteUnit", Position = UsesPosition.Top },
                new UsesDeclaration { Name = "TesteUnit2", Position = UsesPosition.Top },
                new UsesDeclaration { Name = "TesteUnit3", Position = UsesPosition.Bottom },
            };

            // Act
            var parser = new UnitDeclarationParser();
            var result = parser.ToBottomUnitString(listOfUnit);

            // Assert
            Assert.AreEqual(unitDeclaration, result);
        }
    }
}
