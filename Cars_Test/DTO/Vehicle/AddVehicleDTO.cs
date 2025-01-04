using Cars_Test.Validators;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cars_Test.DTO.Vehicle
{
    public class AddVehicleDTO
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string NumberPlate { get; set; }

        public int EmployeeId { get; set; }
    }
}
