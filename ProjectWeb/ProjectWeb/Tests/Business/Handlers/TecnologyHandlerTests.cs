
using Business.Handlers.Tecnologies.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Tecnologies.Queries.GetTecnologyQuery;
using Entities.Concrete;
using static Business.Handlers.Tecnologies.Queries.GetTecnologiesQuery;
using static Business.Handlers.Tecnologies.Commands.CreateTecnologyCommand;
using Business.Handlers.Tecnologies.Commands;
using Business.Constants;
using static Business.Handlers.Tecnologies.Commands.UpdateTecnologyCommand;
using static Business.Handlers.Tecnologies.Commands.DeleteTecnologyCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class TecnologyHandlerTests
    {
        Mock<ITecnologyRepository> _tecnologyRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _tecnologyRepository = new Mock<ITecnologyRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Tecnology_GetQuery_Success()
        {
            //Arrange
            var query = new GetTecnologyQuery();

            _tecnologyRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Tecnology, bool>>>())).ReturnsAsync(new Tecnology()
//propertyler buraya yazılacak
//{																		
//TecnologyId = 1,
//TecnologyName = "Test"
//}
);

            var handler = new GetTecnologyQueryHandler(_tecnologyRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.TecnologyId.Should().Be(1);

        }

        [Test]
        public async Task Tecnology_GetQueries_Success()
        {
            //Arrange
            var query = new GetTecnologiesQuery();

            _tecnologyRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Tecnology, bool>>>()))
                        .ReturnsAsync(new List<Tecnology> { new Tecnology() { /*TODO:propertyler buraya yazılacak TecnologyId = 1, TecnologyName = "test"*/ } });

            var handler = new GetTecnologiesQueryHandler(_tecnologyRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Tecnology>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Tecnology_CreateCommand_Success()
        {
            Tecnology rt = null;
            //Arrange
            var command = new CreateTecnologyCommand();
            //propertyler buraya yazılacak
            //command.TecnologyName = "deneme";

            _tecnologyRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Tecnology, bool>>>()))
                        .ReturnsAsync(rt);

            _tecnologyRepository.Setup(x => x.Add(It.IsAny<Tecnology>())).Returns(new Tecnology());

            var handler = new CreateTecnologyCommandHandler(_tecnologyRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _tecnologyRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Tecnology_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateTecnologyCommand();
            //propertyler buraya yazılacak 
            //command.TecnologyName = "test";

            _tecnologyRepository.Setup(x => x.Query())
                                           .Returns(new List<Tecnology> { new Tecnology() { /*TODO:propertyler buraya yazılacak TecnologyId = 1, TecnologyName = "test"*/ } }.AsQueryable());

            _tecnologyRepository.Setup(x => x.Add(It.IsAny<Tecnology>())).Returns(new Tecnology());

            var handler = new CreateTecnologyCommandHandler(_tecnologyRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Tecnology_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateTecnologyCommand();
            //command.TecnologyName = "test";

            _tecnologyRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Tecnology, bool>>>()))
                        .ReturnsAsync(new Tecnology() { /*TODO:propertyler buraya yazılacak TecnologyId = 1, TecnologyName = "deneme"*/ });

            _tecnologyRepository.Setup(x => x.Update(It.IsAny<Tecnology>())).Returns(new Tecnology());

            var handler = new UpdateTecnologyCommandHandler(_tecnologyRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _tecnologyRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Tecnology_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteTecnologyCommand();

            _tecnologyRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Tecnology, bool>>>()))
                        .ReturnsAsync(new Tecnology() { /*TODO:propertyler buraya yazılacak TecnologyId = 1, TecnologyName = "deneme"*/});

            _tecnologyRepository.Setup(x => x.Delete(It.IsAny<Tecnology>()));

            var handler = new DeleteTecnologyCommandHandler(_tecnologyRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _tecnologyRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

