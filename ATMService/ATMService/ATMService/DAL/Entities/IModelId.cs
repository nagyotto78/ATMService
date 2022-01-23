namespace ATMService.DAL.Entities
{

    /// <summary>
    /// Interface for all entity with Id property
    /// </summary>
    public interface IModelId
    {
        /// <summary>
        /// Primary key
        /// </summary>
        int Id { get; set; }

    }
}
