using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cars_Test.Data.Entities.Base;
using Cars_Test.Validators;

namespace Cars_Test.Data.Entities
{
    /// <summary>
    /// Модель транспорта
    /// </summary>
    public class Vehicle : EntityBase
    {
        /// <summary>
        /// Бренд 
        /// </summary>
        [Required(ErrorMessage = "Model car is required")]
        public string Brand { get; set; }

        /// <summary>
        /// Модель
        /// </summary>
        [Required(ErrorMessage = "Model car is required")]
        public string Model { get; set; }

        /// <summary>
        /// Цвет кузова
        /// </summary>
        [Required(ErrorMessage = "Color car is required")]
        public string Color { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        [Required(ErrorMessage = "Number plate is required")]
        [ValidateNumberPlate]
        public string NumberPlate { get; set; }

        /// <summary>
        /// Внешний ключ владельца транспорта
        /// </summary>
        [Required(ErrorMessage = "Employee id is required")]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Владелец
        /// </summary>
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
    }
}
