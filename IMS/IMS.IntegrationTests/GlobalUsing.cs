//Packages
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.Extensions.DependencyInjection;
global using System.Net.Http.Json;
global using Shouldly;
global using System.Net;
global using IMS.Presentation.DTOs.GetDTO;
global using Newtonsoft.Json;
global using Shared.Pagination;

//DAL
global using IMS.DAL;
global using IMS.DAL.Entities;

//API
global using IMS.Presentation;
global using static IMS.Presentation.Routing.ApiRoutes;
global using IMS.Presentation.DTOs.UpdateDTO;
global using IMS.Presentation.DTOs.CreateDTO;

//Shared
global using Shared.Enums;

//IntegrationTests
global using IMS.IntegrationTests.Helpers;
