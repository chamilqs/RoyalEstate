﻿using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.User;
using RoyalState.Core.Application.ViewModels.Users;

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

            CreateMap<UserViewModel, UserDTO>()
                .ReverseMap()
                .ForMember(src => src.Identification, option => option.Ignore());
            #endregion
        }
    }
}
