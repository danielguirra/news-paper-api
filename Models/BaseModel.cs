namespace Models
{
  public abstract class BaseModel
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool Active { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
  }
}
