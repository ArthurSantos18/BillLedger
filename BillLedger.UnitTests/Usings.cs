global using AutoFixture;

global using BillLedger.Domain.Entities;
global using BillLedger.Domain.Enums;
global using BillLedger.Domain.Events;
global using BillLedger.Domain.Exceptions;
global using BillLedger.Domain.ValueObjects;

global using BillLedger.Application.Common.Patterns;
global using BillLedger.Application.Common.Interfaces.Repositories;
global using BillLedger.Application.Common.Interfaces.Services;

global using BillLedger.Application.Notifications.Events;

global using BillLedger.Application.Customers.Commands;
global using BillLedger.Application.Customers.Errors;
global using BillLedger.Application.Customers.Requests;
global using BillLedger.Application.Customers.Queries;
global using BillLedger.Application.Customers.Validators;

global using BillLedger.Application.Billing.Commands;
global using BillLedger.Application.Billing.Errors;
global using BillLedger.Application.Billing.Requests;
global using BillLedger.Application.Billing.Queries;
global using BillLedger.Application.Billing.Validators;

global using BillLedger.UnitTests.Common.Customizations;

global using FluentAssertions;

global using FluentValidation.TestHelper;

global using Microsoft.Extensions.Logging;

global using Moq;