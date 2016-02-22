using MigradorZeosParaADO.DelphiParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MigradorZeosParaADO.Parse
{
    public class UnitDeclarationParser : IParser<IEnumerable<UsesDeclaration>>
    {
        /// <summary>
        /// Returns a parsed unit object
        /// </summary>
        /// <param name="toParser">Text to parse</param>
        /// <returns>Unit parsed</returns>
        public IEnumerable<UsesDeclaration> GetParsed(string toParse)
        {
            var pattern = @"unit[\n\s\w\W]*?;";
            var matches = Regex.Matches(toParse, pattern, RegexOptions.IgnoreCase);
            var clearDeclaration = string.Empty;
            var isTop = true;

            if (matches.Count > 2)
                throw new ArgumentException("toParser has more than 2 unit declarations");

            foreach (Match match in matches)
            {
                clearDeclaration = Regex.Replace(match.Value, @"unit|[;\s\n]", "", RegexOptions.IgnoreCase);

                foreach (var item in clearDeclaration.Split(','))
                {
                    yield return new UsesDeclaration
                    {
                        Name = item,
                        Position = isTop ? UsesPosition.Top : UsesPosition.Bottom
                    };
                }

                isTop = false;
            }
        }

        /// <summary>
        /// Returns a string containing all units
        /// </summary>
        /// <param name="toConvert">List of unit object</param>
        /// <returns>String unit declation</returns>
        public string ToString(IEnumerable<UsesDeclaration> toConvert)
        {
            var unitFormat = "unit {0};";
            var bufferUnit = string.Empty;

            foreach (var unit in toConvert)
            {
                bufferUnit += unit.Name + ", ";
            }

            if (bufferUnit.Length > 0)
                bufferUnit = string.Format(unitFormat, bufferUnit.Substring(0, bufferUnit.Length - 2));

            return bufferUnit;
        }

        /// <summary>
        /// Returns a top unit declaration filtering the list passed
        /// </summary>
        /// <param name="toConvert">List of units</param>
        /// <returns>A string which contains just a top unit declaration</returns>
        public string ToTopUnitString(List<UsesDeclaration> toConvert)
        {
            return ToString(toConvert.Where(t => t.Position == UsesPosition.Top));
        }

        /// <summary>
        /// Returns a bottom unit declaration filtering the list passed
        /// </summary>
        /// <param name="toConvert">List of units</param>
        /// <returns>A string which contains just a bottom unit declaration</returns>
        public string ToBottomUnitString(List<UsesDeclaration> toConvert)
        {
            return ToString(toConvert.Where(t => t.Position == UsesPosition.Bottom));
        }
    }
}
