//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Samples.Application.Members.Commands.CreateMember;
using Samples.Application.Members.Commands.DeleteMember;
using Samples.Application.Members.Commands.UpdateMember;
using Samples.Application.Members.Queries.GetMemberByEmail;
using Samples.Application.Members.Queries.GetMemberById;

namespace Samples.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<GetMemberByEmailQueryHandler>();
        services.AddScoped<GetMemberByIdQueryHandler>();
        services.AddScoped<CreateMemberCommandHandler>();
        services.AddScoped<UpdateMemberCommandHandler>();
        services.AddScoped<DeleteMemberCommandHandler>();

        return services;
    }
}
