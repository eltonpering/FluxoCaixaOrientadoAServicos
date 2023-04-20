using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Application.Interfaces;
using Questao5.Application.ViewModels;
using Questao5.Domain.Repositories;

namespace Questao5.Questao5.Tests.Questao5.Tests.UnitTests;

public class MovimentacaoReadRequestHandlerTests
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IServiceContext _serviceContext;

    public MovimentacaoReadRequestHandlerTests()
    {
        _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
        _movimentoRepository = Substitute.For<IMovimentoRepository>();
        _serviceContext = Substitute.For<IServiceContext>();
    }

    [Fact]
    public async Task Handle_ContaCorrenteNaoExiste_RetornaNull()
    {
        // Arrange
        var handler = new MovimentacaoReadRequestHandler(_contaCorrenteRepository, _movimentoRepository, _serviceContext);
        var request = new GetMovimentacaoRequest(1234);
        _contaCorrenteRepository.ContaCorrenteExisteByNumeroAsync(request.Numero).Returns(false);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _serviceContext.Received(1).AddError("Conta corrente não encontrada.");
    }

    [Fact]
    public async Task Handle_ContaCorrenteExisteSemMovimentos_RetornaListaVazia()
    {
        // Arrange
        var handler = new MovimentacaoReadRequestHandler(_contaCorrenteRepository, _movimentoRepository, _serviceContext);
        var request = new GetMovimentacaoRequest(1234);
        _contaCorrenteRepository.ContaCorrenteExisteByNumeroAsync(request.Numero).Returns(true);

        var movimentos = new List<MovimentoViewModel>();
        _movimentoRepository.GetContaCorrenteByNumeroAsync(request.Numero).Returns(movimentos);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ContaCorrenteExiste_RetornaMovimentos()
    {
        // Arrange
        var handler = new MovimentacaoReadRequestHandler(_contaCorrenteRepository, _movimentoRepository, _serviceContext);
        var request = new GetMovimentacaoRequest(1234);
        _contaCorrenteRepository.ContaCorrenteExisteByNumeroAsync(request.Numero).Returns(true);

        var movimentos = new List<MovimentoViewModel>()
            {
                new MovimentoViewModel { IdMovimento = "1", IdContaCorrente = "1234", DataMovimento = new System.DateTime(2023, 4, 19), TipoMovimento = 'C', Valor = 100 },
                new MovimentoViewModel { IdMovimento = "2", IdContaCorrente = "1234", DataMovimento = new System.DateTime(2023, 4, 18), TipoMovimento = 'D', Valor = 50 }
            };
        _movimentoRepository.GetContaCorrenteByNumeroAsync(request.Numero).Returns(movimentos);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("1", result.ToList()[0].IdMovimento);        
        Assert.Equal("1234", result.ToList()[0].IdContaCorrente);
        Assert.Equal(new System.DateTime(2023, 4, 19), result.ToList()[0].DataMovimento);
        Assert.Equal('C', result.ToList()[0].TipoMovimento);
        Assert.Equal(100, result.ToList()[0].Valor);

        Assert.Equal("2", result.ToList()[1].IdMovimento);
        Assert.Equal("1234", result.ToList()[1].IdContaCorrente);
        Assert.Equal(new System.DateTime(2023, 4, 18), result.ToList()[1].DataMovimento);
        Assert.Equal('D', result.ToList()[1].TipoMovimento);
        Assert.Equal(50, result.ToList()[1].Valor);
    }   
}

