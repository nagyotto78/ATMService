using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATMService.DAL.Entities
{

    /// <summary>
    /// Available denominations
    /// </summary>
    [Table("MoneyDenomination", Schema="dbo")]
    public class MoneyDenomination : IModelId
    {

        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Inner key for denomination
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value of denomination
        /// </summary>
        public int Value { get; set; }

    }
}
