//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Samples.Core.Abstractions;
using Samples.Infrastructure.Persistence;

namespace Samples.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IMemberRepository, MemberRepository>();

        return services;
    }
}
