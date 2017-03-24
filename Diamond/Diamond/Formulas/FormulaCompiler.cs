using Microsoft.CSharp;
using Sprache;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Diamond.Formulas
{
    public class FormulaCompiler
    {
        IDictionary<string, Value> Variables;
        object MethodSource;

        public FormulaCompiler(IDictionary<string, Value> variables, object methodSource)
        {
            Variables = variables;
            MethodSource = methodSource;
        }

        //private static Parser<CodeExpression> ReplacementDecimalVariable =
        //    from leading in Parse.WhiteSpace.Many()
        //    from sign in Parse.Char('#')
        //    from id in Identifier
        //    from trailing in Parse.WhiteSpace.Many()
        //    select new CodeIndexerExpression(new CodeVariableReferenceExpression("Variables"), new CodePrimitiveExpression(id));

        private static Parser<CodeExpression> ReplacementStringVariable =
            from leading in Parse.WhiteSpace.Many()
            from sign in Parse.Char('$')
            from id in Identifier
            from trailing in Parse.WhiteSpace.Many()
                //select new CodeMethodInvokeExpression(new CodeIndexerExpression(new CodeVariableReferenceExpression("Variables"), new CodePrimitiveExpression(id)), "ToString");
            select new CodeIndexerExpression(new CodeVariableReferenceExpression("Variables"), new CodePrimitiveExpression(id));

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

        private static CodeExpression ReplaceStringVariables(string str)
        {
            var matches = replacementRegex.Matches(str);

            if (matches.Count == 0)
            {
                return new CodeObjectCreateExpression(typeof(Value), new CodePrimitiveExpression(str));
            }

            List<CodeExpression> parts = new List<CodeExpression>();

            string start = str.Substring(0, matches[0].Index);

            if (start.Length > 0)
            {
                parts.Add(new CodeObjectCreateExpression(typeof(Value), new CodePrimitiveExpression(start)));
            }

            for(int m = 0; m < matches.Count; m++)
            {
                string variable = matches[m].Value.Substring(2, matches[m].Value.Length - 3);

                parts.Add(new CodeObjectCreateExpression(typeof(Value), new CodeIndexerExpression(new CodeVariableReferenceExpression("Variables"), new CodePrimitiveExpression(variable))));

                if(m == matches.Count - 1)
                {
                    var sub = str.Substring(matches[m].Index + matches[m].Length);
                    parts.Add(new CodeObjectCreateExpression(typeof(Value), new CodePrimitiveExpression(sub)));
                }
                else
                {
                    var sub = str.Substring(matches[m].Index + matches[m].Length,
                        matches[m + 1].Index - (matches[m].Index + matches[m].Length));
                    parts.Add(new CodeObjectCreateExpression(typeof(Value), new CodePrimitiveExpression(sub)));
                }
            }

            if(parts.Count == 1)
            {
                return parts[0];
            }
            else if(parts.Count == 2)
            {
                return new CodeBinaryOperatorExpression(
                    parts[0], CodeBinaryOperatorType.Add, parts[1]);
            }
            else
            {
                CodeExpression last = new CodeBinaryOperatorExpression(
                    parts[parts.Count - 2], CodeBinaryOperatorType.Add, parts[parts.Count - 1]);

                for (int i = parts.Count - 3; i >= 0; i--)
                {
                    last = new CodeBinaryOperatorExpression(parts[i], CodeBinaryOperatorType.Add, last);
                }

                return last;
            }
        }

        private static Parser<CodeExpression> DoubleString =
            from leading in Parse.WhiteSpace.Many()
            from startString in Parse.Char('"').Once()
            from content in Parse.String("\"\"").Text().Or(Parse.AnyChar.Except(Parse.Char('"')).Many().Text()).Many()
            from endString in Parse.Char('"').Once()
            select ReplaceStringVariables(string.Concat(content).Replace("\"\"", "\""));

        private static Parser<CodeExpression> SingleString =
            from leading in Parse.WhiteSpace.Many()
            from startString in Parse.Char('\'').Once()
            from content in Parse.String("''").Text().Or(Parse.AnyChar.Except(Parse.Char('\'')).Many().Text()).Many()
            from endString in Parse.Char('\'').Once()
            select new CodeObjectCreateExpression(typeof(Value), new CodePrimitiveExpression(string.Concat(content).Replace("''", "'")));

        private static Parser<CodeExpression> Decimal =
            from leading in Parse.WhiteSpace.Many()
            from n in Parse.DecimalInvariant
            from trailing in Parse.WhiteSpace.Many()
            select new CodeObjectCreateExpression(typeof(Value), new CodePrimitiveExpression(decimal.Parse(n)));

        private static Parser<CodeBinaryOperatorType> Operator(string op, CodeBinaryOperatorType opType)
        {
            return Parse.String(op).Token().Return(opType);
        }

        private static readonly Parser<CodeBinaryOperatorType> Add = Operator("+", CodeBinaryOperatorType.Add);
        private static readonly Parser<CodeBinaryOperatorType> Subtract = Operator("-", CodeBinaryOperatorType.Subtract);
        private static readonly Parser<CodeBinaryOperatorType> Multiply = Operator("*", CodeBinaryOperatorType.Multiply);
        private static readonly Parser<CodeBinaryOperatorType> Divide = Operator("/", CodeBinaryOperatorType.Divide);
        private static readonly Parser<CodeBinaryOperatorType> Modulo = Operator("%", CodeBinaryOperatorType.Modulus);
        //static readonly Parser<CodeBinaryOperatorType> Power = Operator("^", CodeBinaryOperatorType.);

        private static readonly Parser<CodeExpression> Operand =
            from operand in ReplacementStringVariable.Or(Decimal).Or(DoubleString).Or(SingleString).Or(Parse.Ref(() => Call)).Or(Parse.Ref(() => Parenth))
            select operand;

        private static readonly Parser<CodeExpression> ExpressionLevel2 = Parse.ChainOperator(Multiply.Or(Divide).Or(Modulo), Operand, (op, l, r) => new CodeBinaryOperatorExpression(l, op, r));

        private static readonly Parser<CodeExpression> Expression = Parse.ChainOperator(Add.Or(Subtract), ExpressionLevel2, (op, l, r) => new CodeBinaryOperatorExpression(l, op, r));

        private static Parser<CodeExpression> ParametersRest =
            from comma in Parse.Char(',')
            from expr in Expression
            select expr;

        private static Parser<IEnumerable<CodeExpression>> Parameters =
            from first in Expression
            from rest in ParametersRest.Many()
            select new CodeExpression[] { first }.Concat(rest);


        //TODO: make this call a utility object that can provide methods
        private static Parser<CodeExpression> Call =
            from methodName in Identifier
            from lparen in Parse.Char('(').Once()
            from parameters in Parameters.Optional()
            from rparen in Parse.Char(')').Once()
            select new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("MethodSource"), methodName, parameters.GetOrDefault()?.ToArray() ?? new CodeExpression[] { });

        private static Parser<CodeExpression> Parenth =
            from lparen in Parse.Char('(')
            from expr in Expression
            from rparen in Parse.Char(')')
            select expr;

        private static Parser<CodeExpression> Formula =
            from formulaMark in Parse.Char('=').Optional()
            from expr in Expression.End()
            select expr;

        private static string GenerateNamespace()
        {
            return "FormulaNamespace";
        }



        public Func<object> Compile(string formula)
        {
            CSharpCodeProvider compiler = new CSharpCodeProvider();

            CodeCompileUnit unit = new CodeCompileUnit();

            CodeTypeDeclaration typedec = new CodeTypeDeclaration("TestClass");

            typedec.IsClass = true;

            typedec.Members.Add(new CodeMemberField(typeof(IDictionary<string, Value>), "Variables"));
            typedec.Members.Add(new CodeMemberField(new CodeTypeReference("dynamic"), "MethodSource"));

            CodeConstructor ctor = new CodeConstructor()
            {
                Attributes = MemberAttributes.Public
            };

            ctor.Statements.Add(new CodeSnippetStatement(@"Variables = variables;"));
            ctor.Statements.Add(new CodeSnippetStatement(@"MethodSource = methodSource;"));

            ctor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IDictionary<string, Value>), "variables"));
            ctor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "methodSource"));

            typedec.Members.Add(ctor);

            CodeMemberMethod method = new CodeMemberMethod()
            {
                Name = "Execute",
                ReturnType = new CodeTypeReference(typeof(object)),
                Attributes = MemberAttributes.Public
            };

            typedec.Members.Add(method);

            CodeExpression code;

            try
            {
                code = Formula.Parse(formula);
            }
            catch(Exception e)
            {
                return () => new Value(new CompileError(new string[] { e.Message }));
            }
            

            method.Statements.Add(new CodeMethodReturnStatement(code));

            var ns = new CodeNamespace(GenerateNamespace());

            ns.Types.Add(typedec);

            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            ns.Imports.Add(new CodeNamespaceImport("System.Text"));
            ns.Imports.Add(new CodeNamespaceImport("Diamond"));

            unit.Namespaces.Add(ns);
            unit.ReferencedAssemblies.Add("System.dll");
            unit.ReferencedAssemblies.Add("System.Core.dll");
            unit.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            unit.ReferencedAssemblies.Add("Diamond.exe");

            var result = compiler.CompileAssemblyFromDom(new System.CodeDom.Compiler.CompilerParameters()
            {

            }, unit);

            if(result.Errors.Count > 0)
            {
                List<string> errors = new List<string>();

                for(int e = 0; e < result.Errors.Count; e++)
                {
                    errors.Add(result.Errors[e].ErrorText);
                }

                return () => new Value(new CompileError(errors));
            }

            var assembly = result.CompiledAssembly;

            var type = assembly.ExportedTypes.First();

            var ctorInstance = type.GetConstructor(new Type[] { typeof(IDictionary<string, Value>), typeof(object) });

            object obj = ctorInstance.Invoke(new object[] { Variables, MethodSource });

            var methodInfo = type.GetMethod("Execute");

            return () =>
            {
                return methodInfo.Invoke(obj, new object[] { });
            };
        }
    }
}
