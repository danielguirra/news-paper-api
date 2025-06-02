namespace Exceptions
{
    public class NewsNotFoundException(Guid? newsId)
        : Exception(
            newsId == null
                ? "Nenhuma notícia foi encontrada."
                : $"Notícia com ID {newsId} não foi encontrada."
        ),
            IHasHttpCode
    {
        public int Code => 404;
    }

    public class InvalidNewsCategoryException(Guid categoryId)
        : Exception($"A categoria com ID {categoryId} não é válida para a notícia."),
            IHasHttpCode
    {
        public int Code => 400;
    }

    public class InvalidNewsAuthorException(Guid authorId)
        : Exception($"O autor com ID {authorId} não é válido para a notícia."),
            IHasHttpCode
    {
        public int Code => 400;
    }

    public class ConflictNewsException(string title)
        : Exception($"Já existe uma notícia com o título '{title}'."),
            IHasHttpCode
    {
        public int Code => 409;
    }

    public class InternalNewsException() : Exception("Um erro interno ocorreu"), IHasHttpCode
    {
        public int Code => 500;
    }
}
