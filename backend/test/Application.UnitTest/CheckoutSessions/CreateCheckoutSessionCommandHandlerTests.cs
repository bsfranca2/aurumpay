using Ardalis.Result;

using AurumPay.Application.CheckoutSessions.Create;
using AurumPay.Domain.Catalog;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Services;
using AurumPay.Domain.Stores;

namespace AurumPay.Application.UnitTest.CheckoutSessions;

public class CreateCheckoutSessionCommandHandlerTests
{
    private readonly Mock<ICheckoutContext> _mockCheckoutContext;
    private readonly Mock<IStoreContext> _mockStoreContext;
    private readonly Mock<ICartService> _mockCartService;
    private readonly Mock<IDeviceIdentityProvider> _mockDeviceIdentity;
    private readonly Mock<ICheckoutSessionManager> _mockSessionManager;
    private readonly Mock<ICheckoutSessionRepository> _mockSessionRepository;
    private readonly Mock<IStoreProductService> _mockStoreProductService;
    private readonly CreateCheckoutSessionCommandHandler _handler;

    public CreateCheckoutSessionCommandHandlerTests()
    {
        _mockStoreContext = new Mock<IStoreContext>();
        _mockCartService = new Mock<ICartService>();
        _mockDeviceIdentity = new Mock<IDeviceIdentityProvider>();
        _mockSessionManager = new Mock<ICheckoutSessionManager>();

        _mockCheckoutContext = new Mock<ICheckoutContext>();
        _mockCheckoutContext.Setup(c => c.Store).Returns(_mockStoreContext.Object);
        _mockCheckoutContext.Setup(c => c.CartService).Returns(_mockCartService.Object);
        _mockCheckoutContext.Setup(c => c.DeviceIdentity).Returns(_mockDeviceIdentity.Object);
        _mockCheckoutContext.Setup(c => c.SessionManager).Returns(_mockSessionManager.Object);

        _mockSessionRepository = new Mock<ICheckoutSessionRepository>();
        _mockStoreProductService = new Mock<IStoreProductService>();

        _handler = new CreateCheckoutSessionCommandHandler(
            _mockCheckoutContext.Object,
            _mockSessionRepository.Object,
            _mockStoreProductService.Object
        );
    }

    [Fact]
    public async Task Handle_WithNewCartItems_ShouldValidateAndCreateSession()
    {
        // Arrange
        StoreId storeId = new(1000);
        string fingerprint = "test-device-fingerprint";
        string product1PublicId = PublicIdGenerator.GeneratePublicId();
        string product2PublicId = PublicIdGenerator.GeneratePublicId();
        CreateCheckoutSessionCommand command = new(
            new Dictionary<string, int> { { product1PublicId, 2 }, { product2PublicId, 1 } });

        Dictionary<string, ProductId> productMapping = new()
        {
            { product1PublicId, new ProductId(1000) },
            { product2PublicId, new ProductId(1001) }
        };

        _mockStoreContext.Setup(s => s.GetCurrentStoreId()).Returns(storeId);
        _mockDeviceIdentity.Setup(d => d.GetFingerprint()).Returns(fingerprint);
        _mockStoreProductService
            .Setup(s => s.MapPublicIdsToProductIdsAsync(
                It.IsAny<HashSet<string>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(productMapping);

        _mockCartService
            .Setup(s => s.SetCartItemsAsync(It.IsAny<HashSet<CartItem>>()))
            .Returns(Task.CompletedTask);

        _mockSessionRepository
            .Setup(r => r.CreateAsync(It.IsAny<CheckoutSession>()))
            .ReturnsAsync((CheckoutSession session) => session);

        _mockSessionManager
            .Setup(m => m.EstablishSessionAsync(It.IsAny<CheckoutSession>()))
            .Returns(Task.CompletedTask);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify cart items were set in the cart service
        _mockCartService.Verify(s => s.SetCartItemsAsync(
                It.Is<HashSet<CartItem>>(items =>
                    items.Count == 2 &&
                    items.Any(i => i.ProductId.Equals(productMapping[product1PublicId])))),
            Times.Once);

        // Verify session was created with correct parameters
        _mockSessionRepository.Verify(r => r.CreateAsync(
                It.Is<CheckoutSession>(s =>
                    s.StoreId.Equals(storeId) &&
                    s.Fingerprint == fingerprint)),
            Times.Once);

        // Verify session was established
        _mockSessionManager.Verify(m => m.EstablishSessionAsync(
                It.IsAny<CheckoutSession>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingCart_ShouldLoadCartAndCreateSession()
    {
        // Arrange
        StoreId storeId = new(1000);
        string fingerprint = "test-device-fingerprint";
        CreateCheckoutSessionCommand command = new([]);

        HashSet<CartItem> existingCartItems = new()
        {
            new CartItem(new ProductId(1000), 1), new CartItem(new ProductId(1001), 3)
        };

        _mockStoreContext.Setup(s => s.GetCurrentStoreId()).Returns(storeId);
        _mockDeviceIdentity.Setup(d => d.GetFingerprint()).Returns(fingerprint);

        _mockCartService
            .Setup(s => s.GetCartItemsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(existingCartItems));

        _mockSessionRepository
            .Setup(r => r.CreateAsync(It.IsAny<CheckoutSession>()))
            .ReturnsAsync((CheckoutSession session) => session);

        _mockSessionManager
            .Setup(m => m.EstablishSessionAsync(It.IsAny<CheckoutSession>()))
            .Returns(Task.CompletedTask);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify existing cart items were retrieved
        _mockCartService.Verify(s => s.GetCartItemsAsync(It.IsAny<CancellationToken>()), Times.Once);

        // Verify SetCartItemsAsync was NOT called (since we're using existing cart)
        _mockCartService.Verify(s => s.SetCartItemsAsync(It.IsAny<HashSet<CartItem>>()), Times.Never);

        // Verify session was created with existing cart items
        _mockSessionRepository.Verify(r => r.CreateAsync(
                It.Is<CheckoutSession>(s =>
                    s.StoreId.Equals(storeId) &&
                    s.Fingerprint == fingerprint)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyCart_ShouldReturnValidationError()
    {
        // Arrange
        CreateCheckoutSessionCommand command = new([]);

        _mockCartService
            .Setup(s => s.GetCartItemsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new HashSet<CartItem>()));

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().NotBeNullOrEmpty();
        result.ValidationErrors.Should().ContainSingle();
        result.ValidationErrors.First().ErrorMessage.Should().Contain("empty");

        // Verify that no session was created
        _mockSessionRepository.Verify(r => r.CreateAsync(It.IsAny<CheckoutSession>()), Times.Never);
        _mockSessionManager.Verify(m => m.EstablishSessionAsync(It.IsAny<CheckoutSession>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidProducts_ShouldReturnValidationError()
    {
        // Arrange
        CreateCheckoutSessionCommand command = new(new Dictionary<string, int> { { "invalid-product", 2 } });

        // Setup product service to return empty mapping (no valid products)
        _mockStoreProductService
            .Setup(s => s.MapPublicIdsToProductIdsAsync(
                It.IsAny<HashSet<string>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, ProductId>());

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().NotBeNullOrEmpty();
        result.ValidationErrors.Should().ContainSingle();
        result.ValidationErrors.First().ErrorMessage.Should().Contain("No valid products found");

        // Verify that no cart items were set and no session was created
        _mockCartService.Verify(s => s.SetCartItemsAsync(It.IsAny<HashSet<CartItem>>()), Times.Never);
        _mockSessionRepository.Verify(r => r.CreateAsync(It.IsAny<CheckoutSession>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCartServiceReturnsError_ShouldReturnThatError()
    {
        // Arrange
        CreateCheckoutSessionCommand command = new(new Dictionary<string, int>());

        ValidationError expectedError = new("Custom cart error");

        _mockCartService
            .Setup(s => s.GetCartItemsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Invalid(expectedError));

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().NotBeNullOrEmpty();
        result.ValidationErrors.Should().ContainSingle();
        result.ValidationErrors.First().Should().Be(expectedError);

        // Verify no session was created
        _mockSessionRepository.Verify(r => r.CreateAsync(It.IsAny<CheckoutSession>()), Times.Never);
    }
}