using DotEnvParser;
using System;
using Xunit;

namespace DotEnvParserTest
{
    public class InjectEnvironmentVariables
    {
        [Fact]
        public void MustInject()
        {
            var result = Parser.Parse(@"A=1
B=2");
            Parser.InjectEnvironmentVariables(result);

            var variableA = Environment.GetEnvironmentVariable("A");
            var variableB = Environment.GetEnvironmentVariable("B");

            Assert.Equal(variableA, result[0].Value);
            Assert.Equal(variableB, result[1].Value);

            Environment.SetEnvironmentVariable("A", null);
            Environment.SetEnvironmentVariable("B", null);
        }

        [Fact]
        public void MustNotInjectVariablesThatAlreadyExists()
        {
            Environment.SetEnvironmentVariable("A", "2");

            var result = Parser.Parse(@"A=1
B=2");
            Parser.InjectEnvironmentVariables(result);

            var variableA = Environment.GetEnvironmentVariable("A");
            var variableB = Environment.GetEnvironmentVariable("B");

            Assert.Equal(variableA, "2");
            Assert.Equal(variableB, result[1].Value);

            Environment.SetEnvironmentVariable("A", null);
            Environment.SetEnvironmentVariable("B", null);
        }
    }
}
