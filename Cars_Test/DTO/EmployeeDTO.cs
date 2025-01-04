namespace Cars_Test.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public string Phone { get; set; }

        public string Position { get; set; }

        public ICollection<VehicleDTO> Vehicles { get; set; }
    }
}
