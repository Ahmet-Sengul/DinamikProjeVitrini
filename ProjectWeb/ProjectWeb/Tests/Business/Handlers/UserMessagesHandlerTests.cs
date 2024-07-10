
using Business.Handlers.UserMessageses.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.UserMessageses.Queries.GetUserMessagesQuery;
using Entities.Concrete;
using static Business.Handlers.UserMessageses.Queries.GetUserMessagesesQuery;
using static Business.Handlers.UserMessageses.Commands.CreateUserMessagesCommand;
using Business.Handlers.UserMessageses.Commands;
using Business.Constants;
using static Business.Handlers.UserMessageses.Commands.UpdateUserMessagesCommand;
using static Business.Handlers.UserMessageses.Commands.DeleteUserMessagesCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class UserMessagesHandlerTests
    {
        Mock<IUserMessagesRepository> _userMessagesRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _userMessagesRepository = new Mock<IUserMessagesRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task UserMessages_GetQuery_Success()
        {
            //Arrange
            var query = new GetUserMessagesQuery();

            _userMessagesRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserMessages, bool>>>())).ReturnsAsync(new UserMessages()
//propertyler buraya yazılacak
//{																		
//UserMessagesId = 1,
//UserMessagesName = "Test"
//}
);

            var handler = new GetUserMessagesQueryHandler(_userMessagesRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.UserMessagesId.Should().Be(1);

        }

        [Test]
        public async Task UserMessages_GetQueries_Success()
        {
            //Arrange
            var query = new GetUserMessagesesQuery();

            _userMessagesRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<UserMessages, bool>>>()))
                        .ReturnsAsync(new List<UserMessages> { new UserMessages() { /*TODO:propertyler buraya yazılacak UserMessagesId = 1, UserMessagesName = "test"*/ } });

            var handler = new GetUserMessagesesQueryHandler(_userMessagesRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<UserMessages>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task UserMessages_CreateCommand_Success()
        {
            UserMessages rt = null;
            //Arrange
            var command = new CreateUserMessagesCommand();
            //propertyler buraya yazılacak
            //command.UserMessagesName = "deneme";

            _userMessagesRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserMessages, bool>>>()))
                        .ReturnsAsync(rt);

            _userMessagesRepository.Setup(x => x.Add(It.IsAny<UserMessages>())).Returns(new UserMessages());

            var handler = new CreateUserMessagesCommandHandler(_userMessagesRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _userMessagesRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task UserMessages_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateUserMessagesCommand();
            //propertyler buraya yazılacak 
            //command.UserMessagesName = "test";

            _userMessagesRepository.Setup(x => x.Query())
                                           .Returns(new List<UserMessages> { new UserMessages() { /*TODO:propertyler buraya yazılacak UserMessagesId = 1, UserMessagesName = "test"*/ } }.AsQueryable());

            _userMessagesRepository.Setup(x => x.Add(It.IsAny<UserMessages>())).Returns(new UserMessages());

            var handler = new CreateUserMessagesCommandHandler(_userMessagesRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task UserMessages_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateUserMessagesCommand();
            //command.UserMessagesName = "test";

            _userMessagesRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserMessages, bool>>>()))
                        .ReturnsAsync(new UserMessages() { /*TODO:propertyler buraya yazılacak UserMessagesId = 1, UserMessagesName = "deneme"*/ });

            _userMessagesRepository.Setup(x => x.Update(It.IsAny<UserMessages>())).Returns(new UserMessages());

            var handler = new UpdateUserMessagesCommandHandler(_userMessagesRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _userMessagesRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task UserMessages_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteUserMessagesCommand();

            _userMessagesRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserMessages, bool>>>()))
                        .ReturnsAsync(new UserMessages() { /*TODO:propertyler buraya yazılacak UserMessagesId = 1, UserMessagesName = "deneme"*/});

            _userMessagesRepository.Setup(x => x.Delete(It.IsAny<UserMessages>()));

            var handler = new DeleteUserMessagesCommandHandler(_userMessagesRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _userMessagesRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

