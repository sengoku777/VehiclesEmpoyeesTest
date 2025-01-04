using AutoMapper;
using Cars_Test.DTO.Employee;
using Cars_Test.DTO.Vehicle;

namespace Cars_Test.DTO.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Data.Entities.Employee, EmployeeDTO>()
                .ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => src.Vehicles));

            CreateMap<Data.Entities.Vehicle, VehicleDTO>();

            CreateMap<AddEmployeeDTO, Data.Entities.Employee>();

            CreateMap<AddVehicleAnEmployeeDTO, Data.Entities.Employee>();

            CreateMap<AddVehicleDTO, Data.Entities.Vehicle>();

            CreateMap<UpdateEmployeeDTO, Data.Entities.Employee>();

            CreateMap<UpdateVehicleDTO, Data.Entities.Vehicle>();
        }
    }
}
