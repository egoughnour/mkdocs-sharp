using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace MkDocsSharp.XmlCommentMarkDownGenerator.Test
{
    /// <summary>
    /// Summary description for ElementTests
    /// </summary>
    [TestClass]
    public class ElementTests
    {
        public TestContext? TestContext { get; set; }

        [TestMethod]
        public void ElementPara()
        {
            var inputResourceName = "MkDocsSharp.XmlCommentMarkDownGenerator.Test.ElementPara_input.xml";
            Regex normalizeSpace = new Regex(@"\s+", RegexOptions.Compiled);
            var testInput = TestUtil.FetchResourceAsString(inputResourceName);

            var testOutput = normalizeSpace.Replace(testInput.ToMarkDown(), " ");
            //TODO: better test here
            Assert.IsNotNull(testOutput);
        }

        [TestMethod]
        public void ElementC()
        {
            var inputResourceName = "MkDocsSharp.XmlCommentMarkDownGenerator.Test.ElementC_input.xml";
            Regex normalizeSpace = new Regex(@"\s+", RegexOptions.Compiled);
            var testInput = TestUtil.FetchResourceAsString(inputResourceName);

            var testOutput = normalizeSpace.Replace(testInput.ToMarkDown(), " ");
            Assert.IsTrue(testOutput.Contains("`code tag c`"));
        }

        [TestMethod]
        public void ElementParam()
        {
            var inputResourceName = "MkDocsSharp.XmlCommentMarkDownGenerator.Test.ElementParam_input.xml";
            Regex normalizeSpace = new Regex(@"\s+", RegexOptions.Compiled);
            var testInput = TestUtil.FetchResourceAsString(inputResourceName);

            var testOutput = normalizeSpace.Replace(testInput.ToMarkDown(), " ");
            Assert.IsNotNull(testOutput);
        }
    }
}
