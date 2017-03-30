using DotEnvParser;
using System;
using System.Linq;
using Xunit;

namespace DotEnvParserTest
{
    public class Parse
    {
        [Theory]
        [InlineData(@"A=1

B=2")]
        [InlineData(@"A=1
#A=2
")]
        [InlineData(@"#A=1
A=2
")]
        [InlineData(@"#A=1
#A=2
")]
        [InlineData(@"#A=1
#A=2
")]
        [InlineData(@"A=1
1=2
")]
        public void MustReturnSuccesfulLines(string text)
        {
            var lineEndings = new[] { "\r\n", "\n" };
            var lines = text.Split(lineEndings, StringSplitOptions.None);

            var result = Parser.Parse(text);

            Assert.True(lines.Length > result.Count());
        }

        [Theory]
        [InlineData(@"A=1

B=2")]
        [InlineData(@"A=1
#A=2
")]
        [InlineData(@"#A=1
A=2
")]
        [InlineData(@"#A=1
#A=2
")]
        [InlineData(@"#A=1
#A=2
")]
        [InlineData(@"A=1
1=2
")]
        public void MustThrowOnFailure(string text)
        {
            var lineEndings = new[] { "\r\n", "\n" };
            var lines = text.Split(lineEndings, StringSplitOptions.None);

            Assert.Throws<ParserErrorException<string>>(() => {
                var result = Parser.Parse(text, true);
            });
        }

        [Fact]
        public void MustPass()
        {
            var text = @"A=1
B=2";
            var result = Parser.Parse(text);

            var line1 = result[0];
            var line2 = result[1];
            Assert.Equal(line1.Name, "A");
            Assert.Equal(line1.Value, "1");
            Assert.Equal(line2.Name, "B");
            Assert.Equal(line2.Value, "2");
        }
    }
}
