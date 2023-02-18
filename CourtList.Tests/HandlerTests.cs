using CourtList.Models;
using CourtList.Services;
using CourtList.Services.Constants;
using CourtList.Yandex;
using CourtList.Yandex.Constants;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CourtList.Tests;

public class HandlerTests : IClassFixture<TestFixture>
{
    private readonly IConfigurationService _configurationService;
    private readonly IRegionService _regionService;

    public HandlerTests(TestFixture fixture)
    {
        _configurationService =
            fixture?.serviceProvider?.GetRequiredService<IConfigurationService>() ?? new ConfigurationService();

        if (fixture == null)
        {
            throw new ArgumentNullException(nameof(fixture));
        }

        _regionService = fixture.serviceProvider.GetRequiredService<IRegionService>();
    }


    [Fact]
    public void IsCorrectResult_ShowAll()
    {
        var pageNum = -1000;
        var request = GetTestRequest(pageNum);

        var handler = new Handler();
        var functionResult = handler.FunctionHandler(request);
        var testRegionModel = JsonConvert.DeserializeObject<RegionListModel>(functionResult.Body);

        Assert.NotNull(functionResult);
        Assert.Equal(functionResult.StatusCode, StatusCode.Success);
        Assert.IsType<Response<string>>(functionResult);

        Assert.NotNull(testRegionModel?.Pagination);
        Assert.False(testRegionModel?.Pagination.IsPaginationEnabled());
        Assert.Equal(testRegionModel?.Pagination.Page, pageNum);
        Assert.Equal(testRegionModel?.RegionList.Count, testRegionModel?.Pagination.TotalCount);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(null)]
    public void IsCorrectResult_FirstPage(int? paramPageNum)
    {
        var correctRegionModel = GetCorrectModel();
        var correctPageLength = _configurationService.GetInt(Field.PageLength);
        var correctPageCount = correctRegionModel.Pagination.PageCount;

        var correctPageNum = 1;
        if (paramPageNum.HasValue)
        {
            correctPageNum = paramPageNum.Value == 0 ? 1 : paramPageNum.Value;
        }

        var handler = new Handler();
        var request = GetTestRequest(paramPageNum);
        var functionResult = handler.FunctionHandler(request);
        var testRegionModel = JsonConvert.DeserializeObject<RegionListModel>(functionResult.Body);

        //Validate response
        Assert.NotNull(testRegionModel);
        Assert.Equal(functionResult.StatusCode, StatusCode.Success);


        //Validate pagination
        Assert.NotNull(testRegionModel?.Pagination);
        Assert.Equal(testRegionModel.Pagination.Page, correctPageNum);
        Assert.Equal(testRegionModel.Pagination.PageLength, correctPageLength);
        Assert.Equal(testRegionModel.Pagination.PageCount, correctPageCount);
        Assert.True(testRegionModel.Pagination.IsPaginationEnabled());
        var offset = (correctPageNum - 1) * correctPageLength;
        Assert.Equal(testRegionModel.Pagination.GetOffset(), offset);

        //Validate region data
        var index = 0;
        var correctRegionList = correctRegionModel.RegionList.ToList();
        foreach (var testRegion in testRegionModel.RegionList)
        {
            Assert.NotNull(testRegion?.Subject);
            var correctRegion = correctRegionList[index];
            var correctSubj = correctRegion.Subject;
            Assert.Equal(testRegion?.Subject.Name, correctSubj.Name);
            Assert.Equal(testRegion?.Subject.Id, correctSubj.Id);
            Assert.Equal(testRegion?.CourtList.Count, correctRegion.CourtList.Count);

            var subIndex = 0;
            var correctCourtList = correctRegion.CourtList.ToList();
            foreach (var testCourt in testRegion?.CourtList ?? new List<Court>())
            {
                var correctCourt = correctCourtList[subIndex];
                Assert.Equal(testCourt.Name, correctCourt.Name);
                Assert.Equal(testCourt.Code, correctCourt.Code);
                subIndex++;
            }

            index++;
        }
    }

    [Fact]
    public void IsCorrectResult_MiddlePage()
    {
        var correctRegionModel = GetCorrectModel();
        var correctPageLength = _configurationService.GetInt(Field.PageLength);
        var correctPageCount = correctRegionModel.Pagination.PageCount;

        var pageNum = (int)(correctRegionModel.Pagination.PageCount / 2);
        var request = GetTestRequest(pageNum);

        var handler = new Handler();
        var functionResult = handler.FunctionHandler(request);
        var testRegionModel = JsonConvert.DeserializeObject<RegionListModel>(functionResult.Body);

        //Validate response
        Assert.NotNull(functionResult);
        Assert.Equal(functionResult.StatusCode, StatusCode.Success);

        //Validate pagination
        Assert.NotNull(testRegionModel);
        Assert.Equal(testRegionModel.Pagination.Page, pageNum);
        var offset = (pageNum - 1) * correctPageLength;
        Assert.Equal(testRegionModel.Pagination.GetOffset(), offset);

        //Validate region data
        var correctRegionListOffset = correctRegionModel.RegionList
            .Skip(offset)
            .Select(x => x)
            .ToList();

        Assert.True(correctRegionListOffset.Count > testRegionModel.RegionList.Count);

        var index = 0;
        foreach (var testRegion in testRegionModel.RegionList)
        {
            Assert.NotNull(testRegion?.Subject);
            var correctRegion = correctRegionListOffset[index];
            Assert.Equal(testRegion?.Subject.Name, correctRegion.Subject.Name);
            Assert.Equal(testRegion?.Subject.Id, correctRegion.Subject.Id);
            Assert.Equal(testRegion?.CourtList.Count, correctRegion.CourtList.Count);

            var subIndex = 0;
            var correctCourtList = correctRegion.CourtList.ToList();
            foreach (var testCourt in testRegion?.CourtList ?? new List<Court>())
            {
                var correctCourt = correctCourtList[subIndex];
                Assert.Equal(testCourt.Name, correctCourt.Name);
                Assert.Equal(testCourt.Code, correctCourt.Code);
                subIndex++;
            }

            index++;
        }
    }

    [Theory]
    [InlineData("test", "testpass123")]
    [InlineData("test123", "testpass")]
    [InlineData("test123", "testpass123")]
    public void IsCorrectResult_FailValidation(string login, string pass)
    {
        var request = GetTestRequest(1);
        request.queryStringParameters[ParameterName.Login] = login;
        request.queryStringParameters[ParameterName.Password] = pass;

        var handler = new Handler();
        var functionResult = handler.FunctionHandler(request);
        var testErrorModel = JsonConvert.DeserializeObject<ErrorBody>(functionResult.Body);

        Assert.NotNull(functionResult);
        Assert.Equal(functionResult.StatusCode, StatusCode.Forbidden);
        Assert.IsType<Response<string>>(functionResult);

        Assert.NotNull(testErrorModel);
        Assert.Equal(testErrorModel.Error, Message.Forbidden);
    }

    private RegionListModel GetCorrectModel()
    {
        var correctRegionModel = _regionService.GetRegionList(-1000);
        return correctRegionModel;
    }


    private Request GetTestRequest(int? pageNum)
    {
        var request = new Request()
        {
            queryStringParameters = new Dictionary<string, string>()
            {
                { ParameterName.Login, _configurationService.GetString(Field.Login) },
                { ParameterName.Password, _configurationService.GetString(Field.Password) }
            }
        };

        if (pageNum.HasValue)
        {
            request.queryStringParameters.Add(ParameterName.Page, pageNum.Value.ToString());
        }

        return request;
    }
}