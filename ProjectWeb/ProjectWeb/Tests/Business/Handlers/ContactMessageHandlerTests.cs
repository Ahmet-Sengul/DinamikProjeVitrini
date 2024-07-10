
using Business.Handlers.ContactMessages.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.ContactMessages.Queries.GetContactMessageQuery;
using Entities.Concrete;
using static Business.Handlers.ContactMessages.Queries.GetContactMessagesQuery;
using static Business.Handlers.ContactMessages.Commands.CreateContactMessageCommand;
using Business.Handlers.ContactMessages.Commands;
using Business.Constants;
using static Business.Handlers.ContactMessages.Commands.UpdateContactMessageCommand;
using static Business.Handlers.ContactMessages.Commands.DeleteContactMessageCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class ContactMessageHandlerTests
    {
        Mock<IContactMessageRepository> _contactMessageRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _contactMessageRepository = new Mock<IContactMessageRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task ContactMessage_GetQuery_Success()
        {
            //Arrange
            var query = new GetContactMessageQuery();

            _contactMessageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ContactMessage, bool>>>())).ReturnsAsync(new ContactMessage()
//propertyler buraya yazılacak
//{																		
//ContactMessageId = 1,
//ContactMessageName = "Test"
//}
);

            var handler = new GetContactMessageQueryHandler(_contactMessageRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.ContactMessageId.Should().Be(1);

        }

        [Test]
        public async Task ContactMessage_GetQueries_Success()
        {
            //Arrange
            var query = new GetContactMessagesQuery();

            _contactMessageRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<ContactMessage, bool>>>()))
                        .ReturnsAsync(new List<ContactMessage> { new ContactMessage() { /*TODO:propertyler buraya yazılacak ContactMessageId = 1, ContactMessageName = "test"*/ } });

            var handler = new GetContactMessagesQueryHandler(_contactMessageRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<ContactMessage>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task ContactMessage_CreateCommand_Success()
        {
            ContactMessage rt = null;
            //Arrange
            var command = new CreateContactMessageCommand();
            //propertyler buraya yazılacak
            //command.ContactMessageName = "deneme";

            _contactMessageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ContactMessage, bool>>>()))
                        .ReturnsAsync(rt);

            _contactMessageRepository.Setup(x => x.Add(It.IsAny<ContactMessage>())).Returns(new ContactMessage());

            var handler = new CreateContactMessageCommandHandler(_contactMessageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _contactMessageRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task ContactMessage_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateContactMessageCommand();
            //propertyler buraya yazılacak 
            //command.ContactMessageName = "test";

            _contactMessageRepository.Setup(x => x.Query())
                                           .Returns(new List<ContactMessage> { new ContactMessage() { /*TODO:propertyler buraya yazılacak ContactMessageId = 1, ContactMessageName = "test"*/ } }.AsQueryable());

            _contactMessageRepository.Setup(x => x.Add(It.IsAny<ContactMessage>())).Returns(new ContactMessage());

            var handler = new CreateContactMessageCommandHandler(_contactMessageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task ContactMessage_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateContactMessageCommand();
            //command.ContactMessageName = "test";

            _contactMessageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ContactMessage, bool>>>()))
                        .ReturnsAsync(new ContactMessage() { /*TODO:propertyler buraya yazılacak ContactMessageId = 1, ContactMessageName = "deneme"*/ });

            _contactMessageRepository.Setup(x => x.Update(It.IsAny<ContactMessage>())).Returns(new ContactMessage());

            var handler = new UpdateContactMessageCommandHandler(_contactMessageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _contactMessageRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task ContactMessage_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteContactMessageCommand();

            _contactMessageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ContactMessage, bool>>>()))
                        .ReturnsAsync(new ContactMessage() { /*TODO:propertyler buraya yazılacak ContactMessageId = 1, ContactMessageName = "deneme"*/});

            _contactMessageRepository.Setup(x => x.Delete(It.IsAny<ContactMessage>()));

            var handler = new DeleteContactMessageCommandHandler(_contactMessageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _contactMessageRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

