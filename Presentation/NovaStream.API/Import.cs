global using System.Net;
global using System.Text;
global using System.Globalization;

global using NovaStream.API.Middlewares;
global using NovaStream.Application.Services;
global using NovaStream.Persistence.Services;
global using NovaStream.Infrastructure.Services;
global using NovaStream.Domain.Entities.Concrete;
global using NovaStream.Persistence.Data.Contexts;
global using NovaStream.Application.Dtos.Abstract;
global using NovaStream.Application.Dtos.Concrete;
global using NovaStream.Domain.Entities.Concrete.Adapters;

global using Mapster;
global using Newtonsoft.Json;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
