using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diamond;
using System.IO;
using System.Text;
using FluentAssertions;

namespace DiamondTest
{
    [TestClass]
    public class StorageTests
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    using (Repository r = new Repository(@"C:\DiamondData", "Shaun Bogan", "smbogan@gmail.com"))
        //    {
        //        string sample = "this is text for the file !!";

        //        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(sample)))
        //        {
        //            r.WriteFile(new ResourceIdentifier(ResourceType.Text, "samples", "sample"), ms);
        //        }

        //        r.Commit();

        //        using (var fs = r.ReadFile(new ResourceIdentifier(ResourceType.Text, "samples", "sample")))
        //        {
        //            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
        //            {
        //                sr.ReadToEnd().Should().Be(sample);
        //            }
        //        }
        //        //r.Undo();
        //    }
        //}

        //[TestMethod]
        //public void TableTest()
        //{
        //    using (Repository r = new Repository(@"C:\DiamondData", "Shaun Bogan", "smbogan@gmail.com"))
        //    {
        //        Table table = new Table("A", "B", "Wallaby", "Sandoval");

        //        table.AddRow();

        //        table[0, 0].SetString("Revenue");
        //        table[0, 1].SetDecimal(10.5m);
        //        table[0, 3].SetFormula(new Formula());

        //        using (var ms = new MemoryStream())
        //        {
        //            table.Write(ms);

        //            ms.Position = 0;

        //            r.WriteFile(new ResourceIdentifier(ResourceType.Table, "fake"), ms);
        //        }

        //        r.Commit();

        //        using (var stream = r.ReadFile(new ResourceIdentifier(ResourceType.Table, "fake")))
        //        {
        //            var table2 = new Table(stream);
        //        }

                
        //        //r.Undo();
        //    }
        //}
    }
}
