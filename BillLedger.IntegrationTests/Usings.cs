global using BillLedger.API;

global using BillLedger.Application.Customers.Requests;
global using BillLedger.Application.Customers.Responses;

global using BillLedger.Application.Billing.Requests;
global using BillLedger.Application.Billing.Responses;

global using BillLedger.Application.Common.Patterns;
global using BillLedger.Application.Common;

global using BillLedger.Domain.Enums;

global using BillLedger.IntegrationTests.Base;

global using BillLedger.Infrastructure.Data;
global using BillLedger.Infrastructure.Data.Interceptors;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.RateLimiting;
global using Microsoft.AspNetCore.Mvc.Testing;

global using Microsoft.EntityFrameworkCore;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Options;

global using FluentAssertions;

global using Microsoft.AspNetCore.Mvc;

global using System.Net;
global using System.Net.Http.Json;

global using System.Threading.RateLimiting;

global using Testcontainers.PostgreSql;