namespace Exceptions
{
    public class CommentNotFoundException(Guid? id)
        : Exception(
            id == null
                ? "Nenhuma Comentario foi encontrada."
                : $"Comentario com ID {id} não encontrado."
        ),
            IHasHttpCode
    {
        public int Code => 404;
    }

    public class InternalCommentException()
        : Exception("Erro interno ao processar Comentario."),
            IHasHttpCode
    {
        public int Code => 500;
    }

    public class BadRequestComments(string? message)
        : Exception(message == null ? "Erro Na requisicao de Comentario" : $"{message}"),
            IHasHttpCode
    {
        public int Code => 400;
    }

    public class AlreadyLikedException()
        : Exception("Isso já recebeu um like/dislike "),
            IHasHttpCode
    {
        public int Code => 409;
    }
}
