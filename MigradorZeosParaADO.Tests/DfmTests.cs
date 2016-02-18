using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MigradorZeosParaADO.Tests
{
    [TestClass]
    public class DfmTests
    {
        [TestMethod]
        public void DeveTrocarTZQueryParaTADOQuery()
        {
            // Arrange
            var toReplace = @"object PRC_CLIENTE_ANOTA_INCLUIR: TZQuery";
            var expected = @"object PRC_CLIENTE_ANOTA_INCLUIR: TADOQuery";

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveTrocardbZeosPordbMain()
        {
            // Arrange
            var toReplace = @"Connection = dmZeos.dbZeos";
            var expected = @"Connection = dmZeos.dbMain";

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveTrocarParamsPorParameters()
        {
            // Arrange
            var toReplace = @"Params = <";
            var expected = @"Parameters = <";

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveRemoverParamType()
        {
            // Arrange
            var toReplace = @"item
                        DataType = ftInteger
                        Name = 'ID_CLIENTE'
                        ParamType = ptInput
                      end";
            var expected = @"item
                        DataType = ftInteger
                        Name = 'ID_CLIENTE'
                      end";

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveRemoverParamData()
        {
            // Arrange
            var toReplace = @"object PRC_CLIENTE_ANOTA_INCLUIR: TADOQuery
                    ParamData = <
                      item
                        DataType = ftInteger
                        Name = 'ID_CLIENTE'
                        ParamType = ptInput
                      end
                      item
                        DataType = ftString
                        Name = 'ANOTACAO'
                        ParamType = ptInput
                      end>
                  end";
            var expected = @"object PRC_CLIENTE_ANOTA_INCLUIR: TADOQuery
                  end";

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveRemoverSomenteAteOFechamentoDoParamData()
        {
            // Arrange
            var toReplace = @"object PRC_CLIENTE_ANOTA_INCLUIR: TADOQuery
                    ParamData = <
                      item
                        DataType = ftInteger
                        Name = 'ID_CLIENTE'
                        ParamType = ptInput
                      end
                      item
                        DataType = ftString
                        Name = 'ANOTACAO'
                        ParamType = ptInput
                      end>
                  end
                  object Teste: TTeste
                  end>";
            var expected = @"object PRC_CLIENTE_ANOTA_INCLUIR: TADOQuery
                  end
                  object Teste: TTeste
                  end>";

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DevePermitirSomenteUmArrobaNosParametros()
        {
            // Arrange
            var toReplace = @"Params = <
                      item
                        DataType = ftInteger
                        Name = '@ID_CLIENTE'
                        ParamType = ptInput
                      end
                      item
                        DataType = ftString
                        Name = '@ANOTACAO'
                        ParamType = ptInput
                      end>";
            var expected = @"Parameters = <>";

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveRemoverParametrosUnknons()
        {
            // Arrange
            var toReplace = @"Params = <
                      item
                        DataType = ftUnknown
                        Name = '@ID_CLIENTE'
                        ParamType = ptUnknown
                      end
                      item
                        DataType = ftString
                        Name = '@ANOTACAO'
                        ParamType = ptUnknown
                      end>";
            var expected = @"Parameters = <>";

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveRemoverParameters()
        {
            // Arrange
            var toReplace = @"Teste:TTeste
                    Params = <
                      item
                        DataType = ftUnknown
                        Name = '@ID_CLIENTE'
                        ParamType = ptUnknown
                      end
                      item
                        DataType = ftString
                        Name = '@ANOTACAO'
                        ParamType = ptUnknown
                      end>
                    end";
            var expected = @"Teste:TTeste
                    Parameters = <>
                    end";

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace, removeParams: true);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeveManterOsOutrosObjetos()
        {
            #region Arrange
            var toReplace = @"  object QryAnotacao: TZQuery
                    Connection = dmZeos.dbZeos
                    SQL.Strings = (
                      'SELECT A.* FROM ANOTACAO AS A'
                      'where A.ID_CLIENTE=:pID_CLIENTE'
                      ' '
                      ' ')
                    Params = <
                      item
                        DataType = ftUnknown
                        Name = 'pID_CLIENTE'
                        ParamType = ptUnknown
                      end>
                    Left = 404
                    Top = 569
                    ParamData = <
                      item
                        DataType = ftUnknown
                        Name = 'pID_CLIENTE'
                        ParamType = ptUnknown
                      end>
                  end
                  object qryMaxID: TZQuery
                    Connection = dmZeos.dbZeos
                    SQL.Strings = (
                      'SELECT Max(ID_CLIENTE)  AS MaxID'
                      'FROM CLIENTE'
                      ' ')
                    Params = <>
                    Left = 547
                    Top = 297
                  end
                  object QryUF: TZQuery
                    Connection = dmZeos.dbZeos
                    SQL.Strings = (
                      'Select A.UF, B.icm_SUBST from CIDADE AS A'
                      'JOIN UF_ICM B On B.UF = A.UF'
                      'Where ID_CIDADE = :pID_CIDADE')
                    Params = <
                      item
                        DataType = ftInteger
                        Name = 'pID_CIDADE'
                        ParamType = ptInput
                      end>
                    Left = 404
                    Top = 616
                    ParamData = <
                      item
                        DataType = ftInteger
                        Name = 'pID_CIDADE'
                        ParamType = ptInput
                      end>
                  end
                  object qryListaClasse: TADOQuery
                    Connection = dmZeos.dbMain
                    SQL.Strings = (
                      'select * from CLI_CLASSE '
                      'Where ID_CLIENTE= :pID_CLIENTE'
                      ''
                      ' '
                      ' '
                      ' '
                      ' '
                      ' ')
                    Parameters = <>
                    Left = 573
                    Top = 608
                  end";

            var expected = @"  object QryAnotacao: TADOQuery
                    Connection = dmZeos.dbMain
                    SQL.Strings = (
                      'SELECT A.* FROM ANOTACAO AS A'
                      'where A.ID_CLIENTE=:pID_CLIENTE'
                      ' '
                      ' ')
                    Parameters = <>
                    Left = 404
                    Top = 569
                  end
                  object qryMaxID: TADOQuery
                    Connection = dmZeos.dbMain
                    SQL.Strings = (
                      'SELECT Max(ID_CLIENTE)  AS MaxID'
                      'FROM CLIENTE'
                      ' ')
                    Parameters = <>
                    Left = 547
                    Top = 297
                  end
                  object QryUF: TADOQuery
                    Connection = dmZeos.dbMain
                    SQL.Strings = (
                      'Select A.UF, B.icm_SUBST from CIDADE AS A'
                      'JOIN UF_ICM B On B.UF = A.UF'
                      'Where ID_CIDADE = :pID_CIDADE')
                    Parameters = <>
                    Left = 404
                    Top = 616
                  end
                  object qryListaClasse: TADOQuery
                    Connection = dmZeos.dbMain
                    SQL.Strings = (
                      'select * from CLI_CLASSE '
                      'Where ID_CLIENTE= :pID_CLIENTE'
                      ''
                      ' '
                      ' '
                      ' '
                      ' '
                      ' ')
                    Parameters = <>
                    Left = 573
                    Top = 608
                  end";
            #endregion

            // Act
            var result = ZeosToAdo.DfmUpdate(toReplace, removeParams: true);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
