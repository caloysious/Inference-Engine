using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace InferenceEngine
{
    [TestFixture]
    public class TestClause
    {
      [TestCase]
      public void TestHead()
      {
            Clause clause = new Clause("a1&b2&c3 => d4 ");
            string expected = "d4";
            string actual = clause.Head;
            Assert.IsTrue(true, "Head is false", actual, expected);
      }

        [TestCase]
        public void TestBody()
        {
            Clause clause = new Clause("a1&b2&c3 => d4 ");
            string[] expected = { "a1", "b2", "c3" };
            string[] actual = clause.Body;
            Assert.IsTrue(true, "Body is false", actual, expected);
        }

        [TestCase]
        public void TestEmptyBody()
        {
            Clause clause = new Clause("d4");
            string expectedBody = null;
            string expectedHead = "d4";

            string[] actualBody = clause.Body ;
            string actualHead = clause.Head;

            Assert.IsTrue(true, "Head is false", actualHead, expectedHead);
            Assert.IsTrue(true, "Body is false", actualBody, expectedBody);

        }
    }
}
