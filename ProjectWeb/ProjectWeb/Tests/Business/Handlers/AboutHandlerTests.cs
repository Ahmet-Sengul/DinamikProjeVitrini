
using Business.Handlers.Abouts.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Abouts.Queries.GetAboutQuery;
using Entities.Concrete;
using static Business.Handlers.Abouts.Queries.GetAboutsQuery;
using static Business.Handlers.Abouts.Commands.CreateAboutCommand;
using Business.Handlers.Abouts.Commands;
using Business.Constants;
using static Business.Handlers.Abouts.Commands.UpdateAboutCommand;
using static Business.Handlers.Abouts.Commands.DeleteAboutCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class AboutHandlerTests
    {
        Mock<IAboutRepository> _aboutRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _aboutRepository = new Mock<IAboutRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task About_GetQuery_Success()
        {
            //Arrange
            var query = new GetAboutQuery();

            _aboutRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<About, bool>>>())).ReturnsAsync(new About()
//propertyler buraya yazılacak
//{																		
//AboutId = 1,
//AboutName = "Test"
//}
);

            var handler = new GetAboutQueryHandler(_aboutRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.AboutId.Should().Be(1);

        }

        [Test]
        public async Task About_GetQueries_Success()
        {
            //Arrange
            var query = new GetAboutsQuery();

            _aboutRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<About, bool>>>()))
                        .ReturnsAsync(new List<About> { new About() { /*TODO:propertyler buraya yazılacak AboutId = 1, AboutName = "test"*/ } });

            var handler = new GetAboutsQueryHandler(_aboutRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<About>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task About_CreateCommand_Success()
        {
            About rt = null;
            //Arrange
            var command = new CreateAboutCommand();
            //propertyler buraya yazılacak
            //command.AboutName = "deneme";

            _aboutRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<About, bool>>>()))
                        .ReturnsAsync(rt);

            _aboutRepository.Setup(x => x.Add(It.IsAny<About>())).Returns(new About());

            var handler = new CreateAboutCommandHandler(_aboutRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _aboutRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task About_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateAboutCommand();
            //propertyler buraya yazılacak 
            //command.AboutName = "test";

            _aboutRepository.Setup(x => x.Query())
                                           .Returns(new List<About> { new About() { /*TODO:propertyler buraya yazılacak AboutId = 1, AboutName = "test"*/ } }.AsQueryable());

            _aboutRepository.Setup(x => x.Add(It.IsAny<About>())).Returns(new About());

            var handler = new CreateAboutCommandHandler(_aboutRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task About_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateAboutCommand();
            //command.AboutName = "test";

            _aboutRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<About, bool>>>()))
                        .ReturnsAsync(new About() { /*TODO:propertyler buraya yazılacak AboutId = 1, AboutName = "deneme"*/ });

            _aboutRepository.Setup(x => x.Update(It.IsAny<About>())).Returns(new About());

            var handler = new UpdateAboutCommandHandler(_aboutRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _aboutRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task About_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteAboutCommand();

            _aboutRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<About, bool>>>()))
                        .ReturnsAsync(new About() { /*TODO:propertyler buraya yazılacak AboutId = 1, AboutName = "deneme"*/});

            _aboutRepository.Setup(x => x.Delete(It.IsAny<About>()));

            var handler = new DeleteAboutCommandHandler(_aboutRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _aboutRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

