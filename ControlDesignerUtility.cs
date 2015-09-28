using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.Design;

namespace DaleWare.Web.UI.Controls
{
    public static class ControlDesignerUtility
    {
        public static string UnitString(Unit unit)
        {
            string rv = string.Empty;
            switch (unit.Type)
            {
                case UnitType.Cm:
                case UnitType.Em:
                case UnitType.Ex:
                case UnitType.Mm:
                    rv = string.Format("{0}{1}", unit.Value, unit.Type.ToString().ToLower());
                    break;
                case UnitType.Inch:
                    rv = string.Format("{0}in", unit.Value);
                    break;
                case UnitType.Percentage:
                    rv = string.Format("{0}%", unit.Value);
                    break;
                case UnitType.Pica:
                    rv = string.Format("{0}pc", unit.Value);
                    break;
                case UnitType.Pixel:
                    rv = string.Format("{0}px", unit.Value);
                    break;
                case UnitType.Point:
                    rv = string.Format("{0}pt", unit.Value);
                    break;
                default:
                    //normally will be pixels
                    rv = string.Format("{0}", unit.Value);
                    break;

            }

            return rv;
        }
    }
}
