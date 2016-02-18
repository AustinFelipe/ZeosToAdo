using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MigradorZeosParaADO
{
    public static class ZeosToAdo
    {
        /// <summary>
        /// Substitui os componentes do Zeos para o ADO dentro do Pas
        /// </summary>
        /// <param name="fileText">Conteúdo do arquio</param>
        /// <returns>Arquivo convertido</returns>
        public static string PasUpdate(string fileText)
        {
            var convertedText = fileText;
            var expressions = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("TZQuery", "TADOQuery"),
                new KeyValuePair<string, string>(@"[\s\n]*ZAbstractRODataset[, ]*", ""),
                new KeyValuePair<string, string>(@"[\s\n]*ZAbstractDataset[, ]*", ""),
                new KeyValuePair<string, string>(@"(\.Parameters)?\.ParamByName\(", ".Parameters.ParamByName("),
                new KeyValuePair<string, string>(@".AsInteger", ".Value"),
                new KeyValuePair<string, string>(@".AsFloat", ".Value"),
                new KeyValuePair<string, string>(@".AsString", ".Value"),
                new KeyValuePair<string, string>(@".AsDateTime", ".Value"),
                new KeyValuePair<string, string>(@"Connection := dmZeos.dbZeos;", "Connection := dmZeos.dbMain;"),
                new KeyValuePair<string, string>(@",;", ";")
            };

            if (!Regex.IsMatch(convertedText, "ADODB"))
                expressions.Add(new KeyValuePair<string, string>("ZDataset", "ADODB"));
            else
                expressions.Add(new KeyValuePair<string, string>(@"ZDataset[, ]*", ""));

            foreach (var expre in expressions)
            {
                convertedText = Regex.Replace(convertedText, @expre.Key, @expre.Value, RegexOptions.IgnoreCase);
            }

            return convertedText;
        }

        /// <summary>
        /// Converte os componentes do Zeos para ADO dentro do DFM
        /// </summary>
        /// <param name="fileText"></param>
        /// <returns></returns>
        public static string DfmUpdate(string fileText, bool removeParams = true)
        {
            var convertedText = fileText;
            var expressions = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("TZQuery", "TADOQuery"),
                new KeyValuePair<string, string>(@"Connection = dmZeos.dbZeos", "Connection = dmZeos.dbMain"),
                new KeyValuePair<string, string>(@"Params = <", "Parameters = <"),
                new KeyValuePair<string, string>(@"\s*ParamType = ptInput", ""),
                new KeyValuePair<string, string>(@"\s*ParamType = ptUnknown", ""),
                new KeyValuePair<string, string>(@"\s*DataType = ftUnknown", ""),
                new KeyValuePair<string, string>(@"[\s\n]*ParamData = <[\n\s\w\W]*?end>", "")
            };

            if (removeParams)
                expressions.Add(new KeyValuePair<string, string>(@"Parameters = <[\n\s\w\W]*?(end>|>)", "Parameters = <>"));

            foreach (var expre in expressions)
            {
                convertedText = Regex.Replace(convertedText, @expre.Key, @expre.Value, RegexOptions.IgnoreCase);
            }

            return convertedText;
        }
    }
}
