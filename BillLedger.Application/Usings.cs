global using BillLedger.Domain.Entities;
global using BillLedger.Domain.Enums;
global using BillLedger.Domain.Events;
global using BillLedger.Domain.ValueObjects;

global using BillLedger.Application.Common.Patterns;
global using BillLedger.Application.Common.Interfaces.Repositories;
global using BillLedger.Application.Common.Interfaces.Services;

global using BillLedger.Application.Billing.Errors;
global using BillLedger.Application.Billing.Requests;
global using BillLedger.Application.Billing.Responses;
global using BillLedger.Application.Billing.Commands;

global using BillLedger.Application.Customers.Errors;
global using BillLedger.Application.Customers.Requests;
global using BillLedger.Application.Customers.Responses;
global using BillLedger.Application.Customers.Queries;
global using BillLedger.Application.Customers.Commands;
global using BillLedger.Application.Customers.Validators;

global using BillLedger.Application.Notifications.Events;

global using FluentValidation;

global using LiteBus.Commands.Abstractions;
global using LiteBus.Events.Abstractions;
global using LiteBus.Queries.Abstractions;
global using LiteBus.Commands;
global using LiteBus.Events;
global using LiteBus.Extensions.Microsoft.DependencyInjection;
global using LiteBus.Messaging;
global using LiteBus.Queries;

global using Microsoft.Extensions.DependencyInjection;
global using Mapster;

global using Microsoft.Extensions.Logging;

global using System.ComponentModel;
global using System.Linq.Expressions;