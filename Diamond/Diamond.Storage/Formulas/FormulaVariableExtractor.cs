using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Diamond.Storage.Formulas
{
    public class FormulaVariableExtractor
    {
        private static Parser<string[]> ReplacementDecimalVariable =
            from leading in Parse.WhiteSpace.Many()
            from sign in Parse.Char('#')
            from id in Identifier
            from trailing in Parse.WhiteSpace.Many()
            select new string[] { id };

        private static Parser<string[]> ReplacementStringVariable =
            from leading in Parse.WhiteSpace.Many()
            from sign in Parse.Char('$')
            from id in Identifier
            from trailing in Parse.WhiteSpace.Many()
            select new string[] { id };

        private static Parser<string> Identifier =
            from leading in Parse.WhiteSpace.Many()
            from first in Parse.Letter.Once()
            from rest in Parse.LetterOrDigit.Many()
            from trailing in Parse.WhiteSpace.Many()
            select new string(first.Concat(rest).ToArray());

        //public static Parser<CodeVariableReferenceExpression> Variable =
        //    from variable in Identifier
        //    select new CodeVariableReferenceExpression(variable);

        private static Parser<string> MethodName =
            from name in Identifier
            select name;

        private static Regex replacementRegex = new Regex("(\\$\\([^()]+?\\))", RegexOptions.Compiled);

        private static string[] ReplaceStringVariables(string str)
        {
            var matches = replacementRegex.Matches(str);

            if (matches.Count == 0)
            {
                return new string[] { };
            }

            List<string> parts = new List<string>();

            for (int m = 0; m < matches.Count; m++)
            {
                string variable = matches[m].Value.Substring(2, matches[m].Value.Length - 3);

                parts.Add(variable);
            }

            return parts.ToArray();
        }

        private static Parser<string[]> DoubleString =
            from leading in Parse.WhiteSpace.Many()
            from startString in Parse.Char('"').Once()
            from content in Parse.String("\"\"").Text().Or(Parse.AnyChar.Except(Parse.Char('"')).Many().Text()).Many()
            from endString in Parse.Char('"').Once()
            select ReplaceStringVariables(string.Concat(content));

        private static Parser<string[]> SingleString =
            from leading in Parse.WhiteSpace.Many()
            from startString in Parse.Char('\'').Once()
            from content in Parse.String("''").Text().Or(Parse.AnyChar.Except(Parse.Char('\'')).Many().Text()).Many()
            from endString in Parse.Char('\'').Once()
            select new string[] { };

        private static Parser<string[]> Decimal =
            from leading in Parse.WhiteSpace.Many()
            from n in Parse.DecimalInvariant
            from trailing in Parse.WhiteSpace.Many()
            select new string[] { };

        static Parser<object> Operator(string op)
        {
            return Parse.String(op).Token().Return("o");
        }

        static readonly Parser<object> Add = Operator("+");
        static readonly Parser<object> Subtract = Operator("-");
        static readonly Parser<object> Multiply = Operator("*");
        static readonly Parser<object> Divide = Operator("/");
        static readonly Parser<object> Modulo = Operator("%");
        //static readonly Parser<CodeBinaryOperatorType> Power = Operator("^", CodeBinaryOperatorType.);

        static readonly Parser<string[]> Operand =
            from operand in ReplacementDecimalVariable.Or(ReplacementStringVariable).Or(Decimal).Or(DoubleString).Or(SingleString).Or(Parse.Ref(() => Call))
            select operand;

        static readonly Parser<string[]> ExpressionLevel2 = Parse.ChainOperator(Multiply.Or(Divide).Or(Modulo), Operand, (op, l, r) => l.Concat(r).ToArray());

        static readonly Parser<string[]> Expression = Parse.ChainOperator(Add.Or(Subtract), ExpressionLevel2, (op, l, r) => l.Concat(r).ToArray());

        private static Parser<string[]> ParametersRest =
            from comma in Parse.Char(',')
            from expr in Expression
            select expr;

        private static Parser<string[]> Parameters =
            from first in Expression
            from rest in ParametersRest.Many()
            select first.Concat(rest.Aggregate(new string[] { }, (a, b) => a.Concat(b).ToArray())).ToArray();


        //TODO: make this call a utility object that can provide methods
        private static Parser<string[]> Call =
            from methodName in Identifier
            from lparen in Parse.Char('(').Once()
            from parameters in Parameters.Optional()
            from rparen in Parse.Char(')').Once()
            select parameters.GetOrDefault()?.ToArray() ?? new string[] { };

        public static string[] GetVariables(string formula)
        {
            return Expression.Parse(formula);
        }
    }
}
