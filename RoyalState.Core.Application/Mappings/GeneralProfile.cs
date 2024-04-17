using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RoyalState.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RoyalState.Core.Application.Features.Properties.Queries.GetPropertyByCode;
using RoyalState.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType;
using RoyalState.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RoyalState.Core.Application.Features.SaleTypes.Commands.CreateSaleType;
using RoyalState.Core.Application.Features.SaleTypes.Commands.UpdateSaleType;
using RoyalState.Core.Application.ViewModels.User;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region User
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(dest => dest.HasError, option => option.Ignore())
                .ForMember(dest => dest.Error, option => option.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(dest => dest.HasError, option => option.Ignore())
                .ForMember(dest => dest.Error, option => option.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, RegisterDTO>()
             .ReverseMap()
             .ForMember(dest => dest.Role, option => option.Ignore())
             .ForMember(dest => dest.Phone, option => option.Ignore())
             .ForMember(dest => dest.ImageUrl, option => option.Ignore());

            CreateMap<UserViewModel, UserDTO>()
                .ReverseMap()
                .ForMember(src => src.Identification, option => option.Ignore());

            CreateMap<UpdateUserRequest, SaveUserViewModel>()
                .ForMember(dest => dest.File, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.HasError, opt => opt.Ignore())
                .ForMember(dest => dest.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterDTO, SaveUserViewModel>()
            .ForMember(dest => dest.File, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.HasError, opt => opt.Ignore())
            .ForMember(dest => dest.Error, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Phone, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();
            #endregion

            #region CQRS
            CreateMap<GetPropertyByCodeQuery, GetPropertyByCodeParameter>()
                .ReverseMap();

            #region PropertyType
            CreateMap<CreatePropertyTypeCommand, PropertyType>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpdatePropertyTypeCommand, PropertyType>()
               .ForMember(x => x.Properties, opt => opt.Ignore())

               .ReverseMap();
            CreateMap<PropertyTypeUpdateResponse, PropertyType>()
          .ForMember(x => x.Properties, opt => opt.Ignore())
          .ReverseMap();
            #endregion

            #region SaleType
            CreateMap<CreateSaleTypeCommand, SaleType>()
               .ForMember(x => x.Properties, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<UpdateSaleTypeCommand, SaleType>()
               .ForMember(x => x.Properties, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<SaleTypeUpdateResponse, SaleType>()
            .ForMember(x => x.Properties, opt => opt.Ignore())
            .ReverseMap();
            #endregion

            #region TypeDTO
            CreateMap<TypeDTO, SaleType>()
             .ForMember(x => x.Properties, opt => opt.Ignore())
             .ReverseMap();
            CreateMap<TypeDTO, PropertyType>()
             .ForMember(x => x.Properties, opt => opt.Ignore())
             .ReverseMap();

            CreateMap<TypeDTO, Improvement>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
               .ForMember(x => x.PropertyImprovements, opt => opt.Ignore())
              .ReverseMap();
            #endregion

            #region Improvement

            CreateMap<CreateImprovementCommand, Improvement>()
              .ForMember(x => x.Properties, opt => opt.Ignore())
               .ForMember(x => x.PropertyImprovements, opt => opt.Ignore())
              .ReverseMap();

            CreateMap<UpdateImprovementCommand, Improvement>()
            .ForMember(x => x.Properties, opt => opt.Ignore())
             .ForMember(x => x.PropertyImprovements, opt => opt.Ignore())
            .ReverseMap();
            CreateMap<ImprovementUpdateResponse, Improvement>()
            .ForMember(x => x.Properties, opt => opt.Ignore())
             .ForMember(x => x.PropertyImprovements, opt => opt.Ignore())
            .ReverseMap();
            #endregion

            #endregion
        }
    }
}
