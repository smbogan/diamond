using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diamond.Storage.Formulas;
using Sprache;
using System.Linq;
using FluentAssertions;
using System.CodeDom;
using Microsoft.CSharp;
using System.Collections.Generic;

namespace DiamondTest
{
    [TestClass]
    public class FormulaTest
    {
        public string M(string variable)
        {
            return string.Format("[{0}]", variable);
        }

        public decimal Z(decimal zk)
        {
            return zk + 3.3m;
        }

        public string Cappy(string name, decimal wallaby)
        {
            return string.Format("({0}, {1})", name, wallaby);
        }

        [TestMethod]
        public void FormulaTest1()
        {
            var t = this;

            var fc = new FormulaCompiler(new Dictionary<string, object>()
            {
                ["X"] = "Y",
                ["A"] = 45.4m
            }, this);
            
            var r = fc.Compile(@"4.05 + 45.01 + ""$(X)"" + M($X) + Z(#A) + "" - "" + Cappy($X,#A)")();



            var sc = new FormulaCompiler(new Dictionary<string, object>()
            {

            }, this);

            var q = sc.Compile(@"5")();

            return;

            CSharpCodeProvider compiler = new CSharpCodeProvider();

            CodeCompileUnit unit = new CodeCompileUnit();

            CodeTypeDeclaration typedec = new CodeTypeDeclaration("TestClass");

            typedec.IsClass = true;

            CodeMemberMethod method = new CodeMemberMethod()
            {
                Name = "Sample",
                ReturnType = new CodeTypeReference(new CodeTypeParameter("System.String")),
                Attributes = MemberAttributes.Public
                
            };

            typedec.Members.Add(method);

            //typedec.Members.Add(new CodeTypeMember())

            var p = FormulaCompiler.ReplaceStringVariables("aasdf$(wallaby)zasd fs df$(xadf)  qqasdf");

            method.Statements.Add(new CodeSnippetStatement(@"Dictionary<string, string> Variables = new Dictionary<string, string>()
                {
                    { ""wallaby"", ""[Wallaby Value]"" },
                    { ""xadf"", ""[XValue]"" },
                };"));

            compiler.CompileAssemblyFromDom(new System.CodeDom.Compiler.CompilerParameters()
            {
                GenerateInMemory = true
            }, unit);

            method.Statements.Add(new CodeMethodReturnStatement(p));

            var ns = new CodeNamespace("TestNamespace");

            ns.Types.Add(typedec);

            //using System;
            //using System.CodeDom;
            //using System.Collections.Generic;
            //using System.Linq;
            //using System.Text;
            //using System.Text.RegularExpressions;
            //using System.Threading.Tasks;

            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            ns.Imports.Add(new CodeNamespaceImport("System.Text"));

            unit.Namespaces.Add(ns);
            unit.ReferencedAssemblies.Add("System.dll");

            var result = compiler.CompileAssemblyFromDom(new System.CodeDom.Compiler.CompilerParameters()
            {

            }, unit);

            var assembly = result.CompiledAssembly;

            var type = assembly.ExportedTypes.First();

            var ctor = type.GetConstructor(new Type[] { });

            object obj = ctor.Invoke(null);

            var methodInfo = type.GetMethod("Sample");

            var resultage = methodInfo.Invoke(obj, new object[] { });

            return;

            //var doubleParseResult = FormulaCompiler.DoubleString.Parse(@"     ""he""""llo""       ");
            //(doubleParseResult.Value as string).Should().Be(@"he""""llo");

            var singleParseResult = FormulaCompiler.SingleString.Parse(@"     'he''llo'       ");
            (singleParseResult.Value as string).Should().Be(@"he''llo");

            var decimalParseResult = FormulaCompiler.Decimal.Parse("  56.34    ");
            ((decimal)decimalParseResult.Value).Should().Be(56.34m);

        }
    }
}
