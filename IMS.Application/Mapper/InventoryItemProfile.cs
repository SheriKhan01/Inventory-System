using AutoMapper;
using IMS.Application.Dtos;
using IMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Mapper
{
    public class InventoryItemProfile : Profile
    {
        public InventoryItemProfile()
        {
            CreateMap<InventoryItem, InventoryItemDto>().ReverseMap();
        }
    }

}
