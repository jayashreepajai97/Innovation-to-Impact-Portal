using IdeaDatabase.Enums;
using System;
using System.Linq;

namespace IdeaDatabase.Utils
{
    public class ConvertSize
    {
        public static double ConvertSizeToBytes(double? originalSize, string unit)
        {
            double not_converted = -1.0;

            if (string.IsNullOrWhiteSpace(unit))
            {
                return not_converted;
            }

            unit = unit.Trim().ToUpper();

            /* validate size */
            if (originalSize == null || originalSize < 0.0)
            {
                return not_converted;
            }
            
            /* validate size unit */
            if (string.IsNullOrEmpty(unit))
            {
                return not_converted;
            }

            if (!Enum.GetNames(typeof(SizeUnits)).Select(x => x.ToUpper()).Contains(unit))
            {
                return not_converted;
            }

            /* convert size */
            int enum_position = (int)Enum.Parse(typeof(SizeUnits), unit, true);
            if (enum_position >= 0)
            {
                return ((double)originalSize * Math.Pow(1024, enum_position));
            }

            return not_converted;
        }

        public static double GetUsedSpacePercentage(double? Space, string SpaceUnit, double? FreeSpace, 
            string FreeSpaceUnit)
        {
            double usedSpacePercentage = 0;
            double? _space = Space;

            /* validate size */
            if (Space == null || Space <= 0.0 || FreeSpace == null || FreeSpace <= 0.0)
            {
                return usedSpacePercentage;
            }
            /* validate size unit */
            if (string.IsNullOrEmpty(SpaceUnit) || string.IsNullOrEmpty(FreeSpaceUnit))
            {
                return usedSpacePercentage;
            }

            double freeSize = ConvertSizeToBytes(FreeSpace, FreeSpaceUnit);
            double totalSize = ConvertSizeToBytes(Space, SpaceUnit);

            if (freeSize > totalSize)
            {
                usedSpacePercentage = Convert.ToDouble(_space);
            }
            else
            {
                usedSpacePercentage = (totalSize - freeSize) * 100 / totalSize;
            }

            return Math.Round(usedSpacePercentage, 2);
        }
    }
}