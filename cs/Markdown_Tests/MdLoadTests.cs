using System.Diagnostics;
using System.Reflection;
using System.IO;
using NUnit.Framework;
using Markdown;

namespace Markdown_Tests
{
    [TestFixture]
    class MdLoadTests
    {
        private string testsDirectoryName = "GeneratedTests";

        [TestCase("high_depth_test7000.txt", "high_depth_ans7000.txt", 
            TestName = "test high depth with 7000 symbols")]
        [TestCase("high_depth_test70000.txt", "high_depth_ans70000.txt",
            TestName = "test high depth with 70000 symbols")]
        [TestCase("many_rendering_test3500.txt", "many_rendering_ans3500.txt",
            TestName = "test many rendering with 3500 symbols")]
        [TestCase("many_rendering_test35000.txt", "many_rendering_ans35000.txt",
            TestName = "test many rendering with 35000 symbols")]
        public void TestPerformance(string testFileName, string answerFileName)
        {
            var testContent = LoadContent(testFileName);
            var expectedAnswer = LoadContent(answerFileName);

            var stopwatch = Stopwatch.StartNew();
            var md = new Md();
            var actualAnswer = md.Render(testContent);
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            TestContext.Out.WriteLine($"done in {elapsedMilliseconds} milliseconds");
            Assert.AreEqual(expectedAnswer, actualAnswer);
        }

        private string LoadContent(string fileName)
        {
            var assemblyDirectoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var pathToFile = Path.Combine(assemblyDirectoryName, testsDirectoryName, fileName);

            using (var readingStream = File.OpenText(pathToFile))
                return readingStream.ReadToEnd();
        }
    }
}
