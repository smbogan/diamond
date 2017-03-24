using Diamond.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class Cell
    {
        string content;

        public CellDataType DataType { get; private set; }

        public Cell()
        {
            content = "";
            DataType = CellDataType.Empty;
        }

        public Cell(Cell other)
        {
            this.content = other.content;
            this.DataType = other.DataType;
        }

        public Cell(string value)
        {
            content = value;
            DataType = CellDataType.String;
        }

        public Cell(decimal value)
        {
            content = value.ToString();
            DataType = CellDataType.Decimal;
        }

        public Cell(Formula formula)
        {
            content = formula.ToString();
            DataType = CellDataType.Formula;
        }

        public Formula GetFormula()
        {
            return new Formula(content);
        }

        public void SetFormula(Formula formula)
        {
            content = formula.ToString();
            DataType = CellDataType.Formula;
        }

        public decimal GetDecimal()
        {
            return decimal.Parse(content);
        }

        public void SetDecimal(decimal value)
        {
            content = value.ToString();
            DataType = CellDataType.Decimal;
        }

        public void SetString(string value)
        {
            content = value;
            DataType = CellDataType.String;
        }

        public string GetString()
        {
            return content;
        }

        public void Clear()
        {
            content = "";
        }

        public Value ToValue()
        {
            switch(DataType)
            {
                case CellDataType.Decimal:
                    return new Value(GetDecimal());
                case CellDataType.Empty:
                    return new Value("");
                case CellDataType.Formula:
                    return new Value(GetFormula().Content);
                case CellDataType.String:
                    return new Value(GetString());
                default:
                    throw new Exception("Unknown cell data type.");
            }
        }

        public static Cell Parse(string value)
        {

            if (string.IsNullOrEmpty(value))
                return new Cell();

            decimal decimalValue;

            if(decimal.TryParse(value, out decimalValue))
            {
                return new Cell(decimalValue);
            }

            if(value.StartsWith("="))
            {
                return new Cell(new Formula(value));
            }

            return new Cell(value);

            //int leftBracketCount = 0;

            //for(int i = 0; i < value.Length; i++)
            //{
            //    if(value[i] == '[')
            //    {
            //        leftBracketCount++;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            //string v = value.Substring(leftBracketCount);
            //v = v.Substring(0, v.Length - leftBracketCount);

            //if(v.Length == 0)
            //{
            //    return new Cell();
            //}

            //if(v[0] == '$')
            //{
            //    return new Cell(decimal.Parse(v.Substring(1)));
            //}

            //if(v[0] == '#')
            //{
            //    return new Cell(int.Parse(v.Substring(1)));
            //}

            //if(v[0] == ':')
            //{
            //    return new Cell(v.Substring(1));
            //}

            //if(v[0] == '=')
            //{
            //    return new Cell(new Formula(v.Substring(1)));
            //}

            //decimal decimalValue;

            //if(decimal.TryParse(v, out decimalValue))
            //{
            //    return new Cell(decimalValue);
            //}

            //int intValue;

            //if(int.TryParse(v, out intValue))
            //{
            //    return new Cell(intValue);
            //}

            //return new Cell(v);
        }

        public override string ToString()
        {
            return content;

            //switch(DataType)
            //{
            //    case CellDataType.Decimal:
            //        return "$" + content;
            //    case CellDataType.Empty:
            //        return "";
            //    case CellDataType.Formula:
            //        return "=" + content;
            //    case CellDataType.Integer:
            //        return "#" + content;
            //    case CellDataType.String:
            //        return ":" + content;
            //    default:
            //        throw new Exception("Unknown data type.");
            //}
        }
    }
}
