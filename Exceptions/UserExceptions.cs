namespace Exceptions
{
    public class UserNotFoundException(Guid? id)
        : Exception(
            id is null ? "Usuário não encontrado." : $"Usuário com ID {id} não encontrado."
        ),
            IHasHttpCode
    {
        public int Code => 404;
    }

    public class UserConflictException(string emailOrName)
        : Exception($"Usuário com email ou nome '{emailOrName}' já existe."),
            IHasHttpCode
    {
        public int Code => 409;
    }

    public class InvalidUserCredentialsException()
        : Exception("Email ou Senha Incorretos"),
            IHasHttpCode
    {
        public int Code => 401;
    }

    public class InvalidUserTokenException() : Exception("Token invalido"), IHasHttpCode
    {
        public int Code => 401;
    }

    public class InternalUserException()
        : Exception("Erro interno ao processar usuário."),
            IHasHttpCode
    {
        public int Code => 500;
    }

    public class PermissionForbiddenUserExcepion()
        : Exception("Você não tem permissão para definir esse nível de acesso."),
            IHasHttpCode
    {
        public int Code => 403;
    }
}
