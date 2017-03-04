using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Formulas
{
    public class Value
    {
        public enum ValueType
        {
            StringValue,
            DecimalValue,
            TableValue,
            ViewValue,
            MissingValue,
            CompileError
        }

        public ValueType TypeOfValue { get; private set; }
        public string StringValue { get; private set; }
        public decimal DecimalValue { get; private set; }
        public CompileError CompileError { get; private set; }
        public MissingVariables MissingVariables { get; private set; }
        public Table TableValue { get; private set; }


        public Value(Value value)
        {
            TypeOfValue = value.TypeOfValue;
            StringValue = value.StringValue;
            DecimalValue = value.DecimalValue;

            MissingVariables = value.MissingVariables;
            CompileError = value.CompileError;
        }

        public Value(CompileError error)
        {
            TypeOfValue = ValueType.CompileError;
            CompileError = error;
        }

        public Value(string value)
        {
            TypeOfValue = ValueType.StringValue;
            StringValue = value;
        }

        public Value(decimal value)
        {
            DecimalValue = value;
            TypeOfValue = ValueType.DecimalValue;
        }

        public Value(MissingVariables missingVariables)
        {
            this.MissingVariables = missingVariables;
            TypeOfValue = ValueType.MissingValue;
        }

        public override string ToString()
        {
            switch(TypeOfValue)
            {
                case ValueType.DecimalValue:
                    return DecimalValue.ToString(CultureInfo.InvariantCulture);
                case ValueType.MissingValue:
                    return MissingVariables.ToString();
                case ValueType.StringValue:
                    return StringValue;
                case ValueType.CompileError:
                    return CompileError.ToString();
                default:
                    return "";
            }
        }

        public static Value operator+(Value a, Value b)
        {
            if(a.TypeOfValue == ValueType.CompileError
                && b.TypeOfValue == ValueType.CompileError)
            {
                return new Value(a.CompileError + b.CompileError);
            }

            if(a.TypeOfValue == ValueType.CompileError)
            {
                return a;
            }

            if(b.TypeOfValue == ValueType.CompileError)
            {
                return b;
            }

            if(a.TypeOfValue == ValueType.MissingValue 
                && b.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(new MissingVariables(a.MissingVariables, b.MissingVariables));
            }

            if(a.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(a.MissingVariables);
            }

            if(b.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(b.MissingVariables);
            }

            if(a.TypeOfValue == ValueType.DecimalValue
                && b.TypeOfValue == ValueType.DecimalValue)
            {
                return new Value(a.DecimalValue + b.DecimalValue);
            }

            return new Value(a.ToString() + b.ToString());
        }

        public static Value operator -(Value a, Value b)
        {
            if (a.TypeOfValue == ValueType.CompileError
                && b.TypeOfValue == ValueType.CompileError)
            {
                return new Value(a.CompileError + b.CompileError);
            }

            if (a.TypeOfValue == ValueType.CompileError)
            {
                return a;
            }

            if (b.TypeOfValue == ValueType.CompileError)
            {
                return b;
            }

            if (a.TypeOfValue == ValueType.MissingValue
                && b.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(new MissingVariables(a.MissingVariables, b.MissingVariables));
            }

            if (a.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(a.MissingVariables);
            }

            if (b.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(b.MissingVariables);
            }

            if (a.TypeOfValue == ValueType.DecimalValue
                && b.TypeOfValue == ValueType.DecimalValue)
            {
                return new Value(a.DecimalValue - b.DecimalValue);
            }

            throw new InvalidOperationException("Cannot subtract values that are not numbers.");
        }

        public static Value operator *(Value a, Value b)
        {
            if (a.TypeOfValue == ValueType.CompileError
                && b.TypeOfValue == ValueType.CompileError)
            {
                return new Value(a.CompileError + b.CompileError);
            }

            if (a.TypeOfValue == ValueType.CompileError)
            {
                return a;
            }

            if (b.TypeOfValue == ValueType.CompileError)
            {
                return b;
            }

            if (a.TypeOfValue == ValueType.MissingValue
                && b.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(new MissingVariables(a.MissingVariables, b.MissingVariables));
            }

            if (a.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(a.MissingVariables);
            }

            if (b.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(b.MissingVariables);
            }

            if (a.TypeOfValue == ValueType.DecimalValue
                && b.TypeOfValue == ValueType.DecimalValue)
            {
                return new Value(a.DecimalValue * b.DecimalValue);
            }

            throw new InvalidOperationException("Cannot multiply values that are not numbers.");
        }

        public static Value operator /(Value a, Value b)
        {
            if (a.TypeOfValue == ValueType.CompileError
                && b.TypeOfValue == ValueType.CompileError)
            {
                return new Value(a.CompileError + b.CompileError);
            }

            if (a.TypeOfValue == ValueType.CompileError)
            {
                return a;
            }

            if (b.TypeOfValue == ValueType.CompileError)
            {
                return b;
            }

            if (a.TypeOfValue == ValueType.MissingValue
                && b.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(new MissingVariables(a.MissingVariables, b.MissingVariables));
            }

            if (a.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(a.MissingVariables);
            }

            if (b.TypeOfValue == ValueType.MissingValue)
            {
                return new Value(b.MissingVariables);
            }

            if (a.TypeOfValue == ValueType.DecimalValue
                && b.TypeOfValue == ValueType.DecimalValue)
            {
                return new Value(a.DecimalValue / b.DecimalValue);
            }

            throw new InvalidOperationException("Cannot divide values that are not numbers.");
        }
    }
}
