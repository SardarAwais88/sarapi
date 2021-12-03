using AutoMapper;
using sarapi.Data;
using sarapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sarapi.Configurations
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            // create map of user and user dto
       
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
