namespace Cars_Test.DTO.Employee
{
    public class AddEmployeeDTO
    {
        public string Fullname { get; set; }

        public string Phone { get; set; }

        public string Position { get; set; }

        public ICollection<int> VehicleIds { get; set; }
    }
}
