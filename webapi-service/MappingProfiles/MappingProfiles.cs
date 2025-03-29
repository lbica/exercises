using AutoMapper;
using webapi.Dtos;
using webapi.Models;


namespace webapi.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Customer, CustomerReadDto>();

            CreateMap<CustomerCreateDto, Customer>();

            CreateMap<CustomerUpdateDto, Customer>();
        }
    }
}
