namespace Fatura.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando una entidad no se encuentra en la base de datos.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public string EntityName { get; }
        public object? EntityId { get; }

        public EntityNotFoundException(string entityName, object? entityId = null)
            : base($"La entidad '{entityName}' no fue encontrada{(entityId != null ? $" con ID: {entityId}" : "")}.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public EntityNotFoundException(string entityName, object? entityId, Exception innerException)
            : base($"La entidad '{entityName}' no fue encontrada{(entityId != null ? $" con ID: {entityId}" : "")}.", innerException)
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
