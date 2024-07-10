
using Business.Handlers.Contacts.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Contacts.Queries.GetContactQuery;
using Entities.Concrete;
using static Business.Handlers.Contacts.Queries.GetContactsQuery;
using static Business.Handlers.Contacts.Commands.CreateContactCommand;
using Business.Handlers.Contacts.Commands;
using Business.Constants;
using static Business.Handlers.Contacts.Commands.UpdateContactCommand;
using static Business.Handlers.Contacts.Commands.DeleteContactCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class ContactHandlerTests
    {
        Mock<IContactRepository> _contactRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _contactRepository = new Mock<IContactRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Contact_GetQuery_Success()
        {
            //Arrange
            var query = new GetContactQuery();

            _contactRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Contact, bool>>>())).ReturnsAsync(new Contact()
//propertyler buraya yazılacak
//{																		
//ContactId = 1,
//ContactName = "Test"
//}
);

            var handler = new GetContactQueryHandler(_contactRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.ContactId.Should().Be(1);

        }

        [Test]
        public async Task Contact_GetQueries_Success()
        {
            //Arrange
            var query = new GetContactsQuery();

            _contactRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Contact, bool>>>()))
                        .ReturnsAsync(new List<Contact> { new Contact() { /*TODO:propertyler buraya yazılacak ContactId = 1, ContactName = "test"*/ } });

            var handler = new GetContactsQueryHandler(_contactRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Contact>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Contact_CreateCommand_Success()
        {
            Contact rt = null;
            //Arrange
            var command = new CreateContactCommand();
            //propertyler buraya yazılacak
            //command.ContactName = "deneme";

            _contactRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Contact, bool>>>()))
                        .ReturnsAsync(rt);

            _contactRepository.Setup(x => x.Add(It.IsAny<Contact>())).Returns(new Contact());

            var handler = new CreateContactCommandHandler(_contactRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _contactRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Contact_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateContactCommand();
            //propertyler buraya yazılacak 
            //command.ContactName = "test";

            _contactRepository.Setup(x => x.Query())
                                           .Returns(new List<Contact> { new Contact() { /*TODO:propertyler buraya yazılacak ContactId = 1, ContactName = "test"*/ } }.AsQueryable());

            _contactRepository.Setup(x => x.Add(It.IsAny<Contact>())).Returns(new Contact());

            var handler = new CreateContactCommandHandler(_contactRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Contact_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateContactCommand();
            //command.ContactName = "test";

            _contactRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Contact, bool>>>()))
                        .ReturnsAsync(new Contact() { /*TODO:propertyler buraya yazılacak ContactId = 1, ContactName = "deneme"*/ });

            _contactRepository.Setup(x => x.Update(It.IsAny<Contact>())).Returns(new Contact());

            var handler = new UpdateContactCommandHandler(_contactRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _contactRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Contact_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteContactCommand();

            _contactRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Contact, bool>>>()))
                        .ReturnsAsync(new Contact() { /*TODO:propertyler buraya yazılacak ContactId = 1, ContactName = "deneme"*/});

            _contactRepository.Setup(x => x.Delete(It.IsAny<Contact>()));

            var handler = new DeleteContactCommandHandler(_contactRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _contactRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

