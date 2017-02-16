using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage
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

        public Cell(int value)
        {
            content = value.ToString();
            DataType = CellDataType.Integer;
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

        public int GetInteger()
        {
            return int.Parse(content);
        }

        public void SetInteger(int value)
        {
            content = value.ToString();
            DataType = CellDataType.Integer;
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

        public static Cell Parse(string value)
        {
            int leftBracketCount = 0;

            for(int i = 0; i < value.Length; i++)
            {
                if(value[i] == '[')
                {
                    leftBracketCount++;
                }
                else
                {
                    break;
                }
            }

            string v = value.Substring(leftBracketCount);
            v = v.Substring(0, v.Length - leftBracketCount);

            if(v.Length == 0)
            {
                return new Cell();
            }

            if(v[0] == '$')
            {
                return new Cell(decimal.Parse(v.Substring(1)));
            }

            if(v[0] == '#')
            {
                return new Cell(int.Parse(v.Substring(1)));
            }

            if(v[0] == ':')
            {
                return new Cell(v.Substring(1));
            }

            if(v[0] == '=')
            {
                return new Cell(new Formula(v.Substring(1)));
            }

            throw new Exception("Unknown entry in the table.");
        }

        public override string ToString()
        {
            switch(DataType)
            {
                case CellDataType.Decimal:
                    return "$" + content;
                case CellDataType.Empty:
                    return "";
                case CellDataType.Formula:
                    return "=" + content;
                case CellDataType.Integer:
                    return "#" + content;
                case CellDataType.String:
                    return ":" + content;
                default:
                    throw new Exception("Unknown data type.");
            }
        }
    }
}
