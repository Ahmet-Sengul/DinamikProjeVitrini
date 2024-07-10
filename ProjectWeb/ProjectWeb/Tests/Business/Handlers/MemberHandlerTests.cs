
using Business.Handlers.Members.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Members.Queries.GetMemberQuery;
using Entities.Concrete;
using static Business.Handlers.Members.Queries.GetMembersQuery;
using static Business.Handlers.Members.Commands.CreateMemberCommand;
using Business.Handlers.Members.Commands;
using Business.Constants;
using static Business.Handlers.Members.Commands.UpdateMemberCommand;
using static Business.Handlers.Members.Commands.DeleteMemberCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class MemberHandlerTests
    {
        Mock<IMemberRepository> _memberRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _memberRepository = new Mock<IMemberRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Member_GetQuery_Success()
        {
            //Arrange
            var query = new GetMemberQuery();

            _memberRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Member, bool>>>())).ReturnsAsync(new Member()
//propertyler buraya yazılacak
//{																		
//MemberId = 1,
//MemberName = "Test"
//}
);

            var handler = new GetMemberQueryHandler(_memberRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.MemberId.Should().Be(1);

        }

        [Test]
        public async Task Member_GetQueries_Success()
        {
            //Arrange
            var query = new GetMembersQuery();

            _memberRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Member, bool>>>()))
                        .ReturnsAsync(new List<Member> { new Member() { /*TODO:propertyler buraya yazılacak MemberId = 1, MemberName = "test"*/ } });

            var handler = new GetMembersQueryHandler(_memberRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Member>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Member_CreateCommand_Success()
        {
            Member rt = null;
            //Arrange
            var command = new CreateMemberCommand();
            //propertyler buraya yazılacak
            //command.MemberName = "deneme";

            _memberRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Member, bool>>>()))
                        .ReturnsAsync(rt);

            _memberRepository.Setup(x => x.Add(It.IsAny<Member>())).Returns(new Member());

            var handler = new CreateMemberCommandHandler(_memberRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _memberRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Member_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateMemberCommand();
            //propertyler buraya yazılacak 
            //command.MemberName = "test";

            _memberRepository.Setup(x => x.Query())
                                           .Returns(new List<Member> { new Member() { /*TODO:propertyler buraya yazılacak MemberId = 1, MemberName = "test"*/ } }.AsQueryable());

            _memberRepository.Setup(x => x.Add(It.IsAny<Member>())).Returns(new Member());

            var handler = new CreateMemberCommandHandler(_memberRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Member_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateMemberCommand();
            //command.MemberName = "test";

            _memberRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Member, bool>>>()))
                        .ReturnsAsync(new Member() { /*TODO:propertyler buraya yazılacak MemberId = 1, MemberName = "deneme"*/ });

            _memberRepository.Setup(x => x.Update(It.IsAny<Member>())).Returns(new Member());

            var handler = new UpdateMemberCommandHandler(_memberRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _memberRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Member_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteMemberCommand();

            _memberRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Member, bool>>>()))
                        .ReturnsAsync(new Member() { /*TODO:propertyler buraya yazılacak MemberId = 1, MemberName = "deneme"*/});

            _memberRepository.Setup(x => x.Delete(It.IsAny<Member>()));

            var handler = new DeleteMemberCommandHandler(_memberRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _memberRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

