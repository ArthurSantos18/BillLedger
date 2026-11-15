global using BillLedger.Domain.Entities;
global using BillLedger.Domain.Patterns;
global using BillLedger.Domain.Patterns.Interfaces;
global using BillLedger.Domain.ValueObjects;

global using BillLedger.Infrastructure.Data;
global using BillLedger.Infrastructure.Data.Converters;
global using BillLedger.Infrastructure.Data.Interceptors;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;