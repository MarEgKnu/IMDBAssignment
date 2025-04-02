using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [DataTestMethod]
        [DataRow("[[[]]]", 3, "", '[', ']')]
        [DataRow("[[[]]]]", 3, "]", '[', ']')]
        [DataRow(" s )", 2, "s", ' ', ')')]
        [DataRow("ssss", 2, "ssss", ' ')]
        [DataRow("[ssss)]", 2, "ssss)", ']', '[')]
        [DataRow("[[[ssss)]", 2, "[ssss)", ']', '[')]
        [DataRow(")))", 1, ")", ')')]
        [DataRow(null, 10, null, ' ')]
        [DataRow("", 10, "", ' ')]
        [TestMethod()]
        public void TrimTest_Sucess(string inStr, int trimCount, string expected, params char[] trimChars)
        {
            string actual = inStr.Trim(trimCount, trimChars);
            Assert.AreEqual(expected, actual);
        }
        [DataTestMethod]
        [DataRow("[[[]]]", 7, "", '[', ']')]
        [DataRow("[[[]))", 7, "", ')', '[', ']')]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TrimTest_Fail_CountLongerThanStr(string inStr, int trimCount, string expected, params char[] trimChars)
        {
            string actual = inStr.Trim(trimCount, trimChars);
            Assert.AreEqual(expected, actual);
        }
    }
}