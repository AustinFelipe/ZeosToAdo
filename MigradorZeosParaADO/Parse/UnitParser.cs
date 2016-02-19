using MigradorZeosParaADO.DelphiParts;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MigradorZeosParaADO.Parse
{
    public class UnitParser : IParser<IEnumerable<Unit>>
    {
        /// <summary>
        /// Returns a parsed unit object
        /// </summary>
        /// <param name="toParser">Text to parse</param>
        /// <returns>Unit parsed</returns>
        public IEnumerable<Unit> GetParsed(string toParse)
        {
            var pattern = "unit?;";
            var matches = Regex.Matches(toParse, pattern, RegexOptions.IgnoreCase);

            if (matches.Count > 2)
                throw new ArgumentException("toParser has more than 2 unit declarations");

            return new List<Unit>();
        }

        /// <summary>
        /// Return a string containing all units
        /// </summary>
        /// <param name="toConvert">List of unit object</param>
        /// <returns>String unit declation</returns>
        public string ToString(IEnumerable<Unit> toConvert)
        {
            return string.Empty;
        }
    }
}
