using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            #endregion

            #region Warehouse
            CreateMap<Warehouse, WarehouseDto>().ReverseMap();
            CreateMap<WarehouseCreateDto, Warehouse>();
            #endregion

            #region Warehouse Items
            CreateMap<WarehouseItemCreateDto, WarehouseItem>();
            CreateMap<WarehouseItem, WarehouseItemDto>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name));
            #endregion

            #region Audit log
            CreateMap<AuditLog, AuditLogDto>();
            #endregion
        }
    }
}
