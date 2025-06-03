namespace Exceptions
{
    public class CategoryNotFoundException(Guid? id)
        : Exception(
            id == null
                ? "Nenhuma categoria foi encontrada."
                : $"Categoria com ID {id} não encontrada."
        ),
            IHasHttpCode
    {
        public int Code => 404;
    }

    public class ConflictCategoryException(string name)
        : Exception($"Já existe uma categoria com o nome '{name}'"),
            IHasHttpCode
    {
        public int Code => 409;
    }

    public class InternalCategoryException()
        : Exception("Erro interno ao processar categoria."),
            IHasHttpCode
    {
        public int Code => 500;
    }

    public class BadRequestCategory() : Exception("Erro na requisicao categoria."), IHasHttpCode
    {
        public int Code => 401;
    }
}
