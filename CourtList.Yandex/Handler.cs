using CourtList.Models;
using CourtList.Services;
using CourtList.Yandex.Constants;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CourtList.Yandex
{
    public class Handler
    {
        private readonly IAuthService _authService;
        private readonly IRegionService _regionService;

        public Handler()
        {
            var serviceProvider = ServiceInjector.BuilServiceProvider();

            _authService = serviceProvider.GetRequiredService<IAuthService>();
            _regionService = serviceProvider.GetRequiredService<IRegionService>();
        }

        private AuthModel GetAuthModel(Request request)
        {
            var login = request.GetQueryStringParam(ParameterName.Login).ToLower();
            var pass = request.GetQueryStringParam(ParameterName.Password);
            var model = new AuthModel(login, pass);
            return model;
        }

        public Response<string> FunctionHandler(Request request)
        {
            try
            {
                var authModel = GetAuthModel(request);
                _authService.ValidateLogin(authModel);

                var page = request.GetPage();
                var regionList = _regionService?.GetRegionList(page) ?? new RegionListModel(page);
                return new Response<string>(StatusCode.Success, JsonConvert.SerializeObject(regionList));
            }
            catch (UnauthorizedAccessException)
            {
                var body = new ErrorBody(Message.Forbidden);
                return new Response<string>(StatusCode.Forbidden, JsonConvert.SerializeObject(body));
            }
            catch (Exception e)
            {
                var message = $"{e.Message}; {e.InnerException}";
                var body = new ErrorBody(message);
                return new Response<string>(StatusCode.Error, JsonConvert.SerializeObject(body));
            }
        }
    }
}