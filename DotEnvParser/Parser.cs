using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotEnvParser
{
    public static class Parser
    {
        public const string EMPTY_LINE = "Empty line";
        public const string E_COULD_NOT_PARSE_LINE = "Could not parse line";
        public const string COMMENT_LINE = "Line is comment";

        private static readonly Regex Regex = new Regex(@"^(?<key>[a-zA-Z][a-zA-Z0-9_]*)=(?<value>.*)$");

        public static Result<Variable, string> ParseLine(string line)
        {
            if (String.IsNullOrWhiteSpace(line))
            {
                return ParseLineResult.WithFailure(EMPTY_LINE);
            }

            if (line[0] == '#')
            {
                return ParseLineResult.WithFailure(COMMENT_LINE);
            }

            var match = Regex.Match(line);
            if (match.Success)
            {
                var result = new Variable(match.Groups["key"].Value, match.Groups["value"].Value);
                return ParseLineResult.WithSuccess(result);
            }

            return ParseLineResult.WithFailure(E_COULD_NOT_PARSE_LINE);
        }

        public static IList<Variable> Parse(string text, bool throwOnError = false)
        {
            var lineEndings = new[] { "\r\n", "\n" };
            var lines = text.Split(lineEndings, StringSplitOptions.None);
            var parsedLines = lines.Select(ParseLine);

            var errors = parsedLines.Where(x => !x.IsSuccess && (x.Failure == COMMENT_LINE || x.Failure == EMPTY_LINE));
            var hasErrors = errors.Count() > 0;
            if (throwOnError && hasErrors)
            {
                throw new ParserErrorException<string>(errors.Select(x => x.Failure));
            }

            return parsedLines
                .Where(x => x.IsSuccess)
                .Select(x => x.Success)
                .ToList();
        }

        public static void InjectEnvironmentVariables(IList<Variable> variables)
        {
            foreach (var variable in variables)
            {
                var envvar = Environment.GetEnvironmentVariable(variable.Name);
                if (envvar == null)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value);
                }
            }
        }
    }
}
