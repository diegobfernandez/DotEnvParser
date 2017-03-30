namespace DotEnvParser
{
    public class Result<S, F>
    {
        public S Success { get; private set; }
        public F Failure { get; private set; }
        public bool IsSuccess => Failure == null;

        public static Result<S, F> WithSuccess(S data)
        {
            return new Result<S, F>
            {
                Success = data,
            };
        }

        public static Result<S, F> WithFailure(F data)
        {
            return new Result<S, F>
            {
                Failure = data,
            };
        }
    }
}
