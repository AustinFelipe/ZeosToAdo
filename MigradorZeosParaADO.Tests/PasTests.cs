using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MigradorZeosParaADO.Tests
{
    [TestClass]
    public class PasTests
    {
        #region Teste no uses

        [TestMethod]
        public void DeveSubstituirUses()
        {
            // Arrange
            var toReplace = "uses Windows,Messages,ZAbstractRODataset,ZDataset,ZAbstractDataset;";
            var expected = "uses Windows,Messages,ADODB;";

            // Act
            var result = ZeosToAdo.PasUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveSubstituirUsesComSpace()
        {
            // Arrange
            var toReplace = "uses Windows,Messages, ZAbstractRODataset , ZAbstractDataset, ZDataset;";
            var expected = "uses Windows,Messages,ADODB;";

            // Act
            var result = ZeosToAdo.PasUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveSubstituirUsesComSaltoLinha()
        {
            // Arrange
            var toReplace = @"uses Windows,Messages, ZAbstractRODataset , 

                ZAbstractDataset, ZDataset;";
            var expected = "uses Windows,Messages,ADODB;";

            // Act
            var result = ZeosToAdo.PasUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveManterSomenteUmADODB()
        {
            // Arrange
            var toReplace = "uses Windows,Messages, ZAbstractRODataset , ZAbstractDataset, ZDataset, ADODB;";
            var expected = "uses Windows,Messages,ADODB;";

            // Act
            var result = ZeosToAdo.PasUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Teste no corpo do código

        [TestMethod]
        public void DeveSubstituirOCorpoDaUnit()
        {
            // Arrange
            var toReplace = @"uses Windows,Messages, ZAbstractRODataset , ZAbstractDataset, ZDataset;

                type
                    AlgumaQuery: TZQuery;
                    TesteQuery: TZQuery;
                    BotaoQualquer: TButton

                    procedure ClickAlguma;
                    var
                        FTeste: TZQuery;
                    begin
                        FTeste.Open();
                    end;
                end;";
            var expected = @"uses Windows,Messages,ADODB;

                type
                    AlgumaQuery: TADOQuery;
                    TesteQuery: TADOQuery;
                    BotaoQualquer: TButton

                    procedure ClickAlguma;
                    var
                        FTeste: TADOQuery;
                    begin
                        FTeste.Open();
                    end;
                end;";

            // Act
            var result = ZeosToAdo.PasUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveSubstituirParamByName()
        {
            // Arrange
            var toReplace = @"qrySumQtdPedido.ParamByName('IdPedido').AsInteger := IdPedido;
                              qrySumQtdPedido.ParamByName('IdPedido').AsFloat := IdPedido;
                              qrySumQtdPedido.ParamByName('IdPedido').AsString := IdPedido;
                              qrySumQtdPedido.ParamByName('IdPedido').AsDateTime := IdPedido;";
            var expected = @"qrySumQtdPedido.Parameters.ParamByName('IdPedido').Value := IdPedido;
                              qrySumQtdPedido.Parameters.ParamByName('IdPedido').Value := IdPedido;
                              qrySumQtdPedido.Parameters.ParamByName('IdPedido').Value := IdPedido;
                              qrySumQtdPedido.Parameters.ParamByName('IdPedido').Value := IdPedido;";

            // Act
            var result = ZeosToAdo.PasUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveIgnorarParamByNameSeCasoJaExistaParameters()
        {
            // Arrange
            var toReplace = @"qrySumQtdPedido.Parameters.ParamByName('IdPedido').Value := IdPedido;
                              qrySumQtdPedido.Parameters.ParamByName('IdPedido').Value := IdPedido;";
            var expected = @"qrySumQtdPedido.Parameters.ParamByName('IdPedido').Value := IdPedido;
                              qrySumQtdPedido.Parameters.ParamByName('IdPedido').Value := IdPedido;";

            // Act
            var result = ZeosToAdo.PasUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveSubstituirDbZeosPorDbMain()
        {
            // Arrange
            var toReplace = "Q.Connection := dmZeos.dbZeos;";
            var expected = "Q.Connection := dmZeos.dbMain;";

            // Act
            var result = ZeosToAdo.PasUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        #endregion
    }
}
