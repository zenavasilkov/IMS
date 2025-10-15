namespace IMS.BLL.Models;

public abstract class ModelBase
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}
