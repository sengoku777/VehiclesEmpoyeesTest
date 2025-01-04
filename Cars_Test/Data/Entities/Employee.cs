using Cars_Test.Data.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Cars_Test.Data.Entities
{
    /// <summary>
    /// Модель сотрудника компании
    /// </summary>
    public class Employee : EntityBase
    {
        /// <summary>
        /// Полное имя
        /// </summary>
        [Required]
        public string Fullname { get; set; }    

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Должность
        /// </summary>

        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; }

        /// <summary>
        /// Транспортные средства сотрудника
        /// </summary>
        [Required]
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
