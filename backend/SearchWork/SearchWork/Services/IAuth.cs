﻿using SearchWork.Models.DTO;

namespace SearchWork.Services
{
    public interface IAuth
    {
        public Task<string?> Login(string email, string password);
        public Task<string> Register(RegisterDto model);
    }
}
