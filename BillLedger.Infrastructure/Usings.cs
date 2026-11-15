global using BillLedger.Application.Common.Interfaces.Repositories;
global using BillLedger.Application.Common.Interfaces.Services;
global using BillLedger.Application.Common.Patterns;
global using BillLedger.Application.Customers.Requests;
global using BillLedger.Application.Billing.Requests;

global using BillLedger.Domain.Enums;
global using BillLedger.Domain.Entities;
global using BillLedger.Domain.Patterns;
global using BillLedger.Domain.Patterns.Interfaces;
global using BillLedger.Domain.ValueObjects;

global using BillLedger.Infrastructure.Data;
global using BillLedger.Infrastructure.Data.Converters;
global using BillLedger.Infrastructure.Data.Interceptors;
global using BillLedger.Infrastructure.Repositories;
global using BillLedger.Infrastructure.Services;

global using LiteBus.Events.Abstractions;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

global using System.Linq.Expressions;