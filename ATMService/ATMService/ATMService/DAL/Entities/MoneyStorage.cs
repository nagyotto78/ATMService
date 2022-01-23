using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATMService.DAL.Entities
{

    /// <summary>
    /// Count of denomination in the ATM
    /// </summary>
    [Table("MoneyStorage", Schema="dbo")]
    public class MoneyStorage : IModelId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Demonination PK
        /// </summary>
        public int MoneyDenominationId { get; set; }

        /// <summary>
        /// Denomination navigation
        /// </summary>
        public virtual MoneyDenomination MoneyDenomination { get; set; }

        /// <summary>
        /// Count of denomination
        /// </summary>
        public int Count { get; set; }

    }
}
