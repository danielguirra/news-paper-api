using Data;

public abstract class BaseService
{
    protected readonly AppDbContext context;

    protected BaseService(AppDbContext context)
    {
        this.context = context;
    }

    protected async Task SaveAsync()
    {
        var saved = await context.SaveChangesAsync();
        if (saved == 0)
            throw new Exception("Nada foi salvo no banco.");
    }
}
