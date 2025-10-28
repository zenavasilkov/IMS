namespace IMS.BLL.Logging;

public static class LoggingConstants
{
    public const string RESOURCE_CREATED = "{Entity} with id {EntityId} has been created";
    public const string RESOURCE_UPDATED = "{Entity} with id {EntityId} has been updated";
    public const string RESOURCE_DELETED = "{Entity} with id {EntityId} has been deleted";
    public const string RESOURCE_NOT_FOUND = "{Entity} with id {EntityId} not found";
    public const string RESOURCE_TO_UPDATE_NOT_FOUND = "{Entity} to update not found";
}
