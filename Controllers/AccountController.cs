﻿using Blog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TokenService _tokenService;
        public AccountController(TokenService tokenService) => _tokenService = tokenService;

        [HttpPost("v1/login")]
        public IActionResult Login() => Ok(_tokenService.GenerateToken(null));

    }
}