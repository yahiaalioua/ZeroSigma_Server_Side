﻿using AngularAuthApi.Authentication.DTOS;
using AngularAuthApi.Entities;

namespace AngularAuthApi.Authentication.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateToken(LoginDto user);
    }
}
