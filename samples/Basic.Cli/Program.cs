//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Samples.Application.Members.Commands.CreateMember;
using Samples.Application.Members.Commands.DeleteMember;
using Samples.Application.Members.Commands.UpdateMember;
using Samples.Application.Members.Queries.GetMemberByEmail;
using Samples.Core.Abstractions;
using Samples.Infrastructure.Persistence;

Console.WriteLine("Result Sample - Basic Cli");
Console.WriteLine("-------------------------");
IMemberRepository memberRepository = new MemberRepository();

// creates non-existing member, should succeed.
var memberId = await CreateNewMember(memberRepository);

// tries creating existing member, should fail with conflict error message.
await CreateNewMember(memberRepository);

// gets the existing member by email.
await GetMemberByEmail(memberRepository);

// updates the existing member.
await UpdateMember(memberRepository, memberId);

// deletes the existing member
await DeleteMember(memberRepository, memberId);

// tries to get a non-existing member, should faile with not found error.
await GetMemberByEmail(memberRepository);

static async Task<Guid> CreateNewMember(IMemberRepository memberRepository)
{
    Console.Write("Creating new member... ");
    var createHandler = new CreateMemberCommandHandler(memberRepository);
    var createdResult = await createHandler.Handle(new CreateMemberCommand("Foo", "Bar", "foo@bar.com"));

    return createdResult.IfOrElse(
        value =>
        {
            Console.WriteLine("member created successfully!");
            return value.Id;
        },
        errors =>
        {
            Console.WriteLine("member creation failed.");
            Console.WriteLine(errors.First().ToString());
            return Guid.Empty;
        });
}

static async Task GetMemberByEmail(IMemberRepository memberRepository)
{
    Console.Write("Retrieving member foo@bar.com... ");
    var getHandler = new GetMemberByEmailQueryHandler(memberRepository);
    var getResult = await getHandler.Handle(new GetMemberByEmailQuery("foo@bar.com"));

    getResult.IfOrElse(
        value =>
        {
            Console.WriteLine("member found!");
        },
        errors =>
        {
            Console.WriteLine("member not found.");
            Console.WriteLine(errors.First().ToString());
        });
}

static async Task UpdateMember(IMemberRepository memberRepository, Guid memberId)
{
    Console.Write("Updating member... ");
    var updateHandler = new UpdateMemberCommandHandler(memberRepository);
    var updateResult = await updateHandler.Handle(new UpdateMemberCommand(memberId, "Foo2", "Bar2", "foo@bar.com"));

    updateResult.IfOrElse(
        value =>
        {
            Console.WriteLine("member updated successfully!");
        },
        errors =>
        {
            Console.WriteLine("member update failed.");
            Console.WriteLine(errors.First().ToString());
        });
}

static async Task DeleteMember(IMemberRepository memberRepository, Guid memberId)
{
    Console.Write("Deleting member... ");
    var deleteHandler = new DeleteMemberCommandHandler(memberRepository);
    var deleteResult = await deleteHandler.Handle(new DeleteMemberCommand(memberId));

    deleteResult.IfOrElse(
        value =>
        {
            Console.WriteLine("member deleted successfully!");
        },
        errors =>
        {
            Console.WriteLine("member delete failed.");
            Console.WriteLine(errors.First().ToString());
        });
}
