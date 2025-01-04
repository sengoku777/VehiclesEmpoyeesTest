namespace Cars_Test.DTO.Employee
{
    public class UpdateEmployeeDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public string Position { get; set; }

        public string Phone { get; set; }

        public ICollection<int> VehicleIds { get; set; }
    }
}
