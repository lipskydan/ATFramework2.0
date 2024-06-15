namespace DemoUtilities;

[TestFixture]
public class ApiWorkerVerifications
{
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpClient _httpClient;
    private ApiWorker _apiWorker;

    [SetUp]
    public void SetUp()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
        };
        _apiWorker = new ApiWorker(_httpClient);
    }

    [TearDown]
    public void TearDown()
    {
        _apiWorker.Dispose();
    }

    [Test]
    public async Task GetAsync_ReturnsResponseContent()
    {
        // Arrange
        var expectedResponse = "{\"userId\": 1, \"id\": 1, \"title\": \"title\", \"body\": \"body\"}";
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _apiWorker.GetAsync("posts/1");

        // Assert
        Assert.AreEqual(expectedResponse, result);
    }

    [Test]
    public async Task PostAsync_ReturnsResponseContent()
    {
        // Arrange
        var expectedResponse = "{\"id\": 101}";
        var postData = "{\"title\":\"foo\",\"body\":\"bar\",\"userId\":1}";
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _apiWorker.PostAsync("posts", postData);

        // Assert
        Assert.AreEqual(expectedResponse, result);
    }

    [Test]
    public async Task PutAsync_ReturnsResponseContent()
    {
        // Arrange
        var expectedResponse = "{\"id\": 1, \"title\": \"foo\", \"body\": \"bar\", \"userId\": 1}";
        var putData = "{\"id\":1,\"title\":\"foo\",\"body\":\"bar\",\"userId\":1}";
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _apiWorker.PutAsync("posts/1", putData);

        // Assert
        Assert.AreEqual(expectedResponse, result);
    }

    [Test]
    public async Task DeleteAsync_ReturnsResponseContent()
    {
        // Arrange
        var expectedResponse = "{}";
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _apiWorker.DeleteAsync("posts/1");

        // Assert
        Assert.AreEqual(expectedResponse, result);
    }
}

