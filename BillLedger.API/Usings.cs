global using BillLedger.API.Extensions;
global using BillLedger.API.Metadata;
global using BillLedger.API.Middleware;
global using BillLedger.API.Transformers;

global using BillLedger.Domain.Exceptions;

global using BillLedger.Infrastructure.Extensions;

global using BillLedger.Application.Customers.Commands;
global using BillLedger.Application.Customers.Queries;
global using BillLedger.Application.Customers.Requests;
global using BillLedger.Application.Customers.Responses;

global using BillLedger.Application.Billing.Commands;
global using BillLedger.Application.Billing.Queries;
global using BillLedger.Application.Billing.Requests;
global using BillLedger.Application.Billing.Responses;

global using BillLedger.Application.Common.Patterns;

global using FluentValidation;

global using LiteBus.Commands.Abstractions;
global using LiteBus.Queries.Abstractions;

global using Markdig;
global using Markdig.Syntax;

global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.HttpOverrides;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ApiExplorer;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.OpenApi;
global using Microsoft.AspNetCore.RateLimiting;

global using Microsoft.Extensions.Diagnostics.HealthChecks;

global using Microsoft.OpenApi;

global using Scalar.AspNetCore;

global using Serilog;
global using Serilog.Context;

global using System.Diagnostics.CodeAnalysis;
global using System.Threading.RateLimiting;