using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Markdown
{
    class Tests
    {
        [TestCase("__w\nw__", TestName = "Md Render Should Not Transform __ Tag In Diferent Lines", ExpectedResult = "__w\nw__")]
        [TestCase("_w\nw_", TestName = "Md Render Should Not Transform _ Tag In Diferent Lines", ExpectedResult = "_w\nw_")]
        [TestCase("__w_w_w__", TestName = "Md Render Should Transform _ Tag Into __ Tag", ExpectedResult = "<strong>w<em>w</em>w</strong>")]
        [TestCase("__w_", TestName = "Md Render Should Not Transform Tags Without Pair", ExpectedResult = "__w_")]
        [TestCase(@"\_w_", TestName = "Md Render Should Not Transform Shielded _ Tag", ExpectedResult = "_w_")]
        [TestCase(@"\__w__", TestName = "Md Render Should Not Transform Shielded __ Tag", ExpectedResult = "__w__")]
        [TestCase("_1_2_", TestName = "Md Render Should Not Transform _ Tag With Numbers", ExpectedResult = "_1_2_")]
        [TestCase("__1__2__", TestName = "Md Render Should Not Transform __ Tag With Numbers", ExpectedResult = "__1__2__")]
        [TestCase("_w", TestName = "Md Render Should Not Transform _ Tag Without Pair", ExpectedResult = "_w")]
        [TestCase("__w", TestName = "Md Render Should Not Transform __ Tag Without Pair", ExpectedResult = "__w")]
        [TestCase("__ w__", TestName = "Md Render Should Not Transform __ Tag With Space After First", ExpectedResult = "__ w__")]
        [TestCase("_wow_", TestName = "Md Render Should Work With \"_\" Tag", ExpectedResult = "<em>wow</em>")]
        [TestCase("__wow__", TestName = "Md Render Should Work With \"__\" Tag", ExpectedResult = "<strong>wow</strong>")]
        public string test(string mdString)
        {
            var actual = Md.Render(mdString);
            return actual;
        }
    }
}
