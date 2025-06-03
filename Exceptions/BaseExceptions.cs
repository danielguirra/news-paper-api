using Exceptions;

public class BadRequestTakeSkip()
    : Exception("Parâmetros 'take' deve ser > 0 e 'skip' >= 0."),
        IHasHttpCode
{
    public int Code => 400;
}
