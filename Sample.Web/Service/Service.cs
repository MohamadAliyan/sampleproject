using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sample.Web.Infrastructure;
using Util;

namespace Sample.Web.Service
{
    public class Service<TViewModel> : IService<TViewModel>
    {
        public readonly IOptionsSnapshot<AppSettings> _settings;
        public readonly IHttpClient _apiClient;
        private readonly ILogger<Service<TViewModel>> _logger;
        private readonly IHttpContextAccessor _httpContextAccesor;
        public readonly string _remoteServiceBaseUrl;

        public Service(
            IOptionsSnapshot<AppSettings> settings,
            IHttpClient httpClient,
            IHttpContextAccessor httpContextAccesor,
            ILogger<Service<TViewModel>> logger)
        {
            _settings = settings;
            _apiClient = httpClient;
            _logger = logger;
            _httpContextAccesor = httpContextAccesor;
            _remoteServiceBaseUrl = $"{_settings.Value.InvoiceServiceUrl}/api";
        }

        public string Entity { get; set; }
        public string GetAllUrl(string baseUri) => $"{baseUri}/{Entity}/GetAllList";
        public string GetAllUrl(string baseUri, int pageNumber) => $"{baseUri}/{Entity}/GetAll/page/{pageNumber}";
        public string GetUrl(string baseUri, long id) => $"{baseUri}/{Entity}/Get/{id}";
        public string DeleteUrl(string baseUri, long id) => $"{baseUri}/{Entity}/Delete/{id}";
        public string InsertUrl(string baseUri) => $"{baseUri}/{Entity}/Create";
        public string EditUrl(string baseUri) => $"{baseUri}/{Entity}/Edit";
        public string GetAllbySearchUrl(string baseUri, int pageNumber) => $"{baseUri}/{Entity}/GetAllbySearch/page/{pageNumber}";
        public string GetAllbySearchListUrl(string baseUri, bool allIncluded) => $"{baseUri}/{Entity}/GetAllListbySearch?allIncluded={allIncluded}";
        private string InsertAndGetIdUrl(string baseUri) => $"{baseUri}/{Entity}/InsertAndGetId";

        public virtual List<TViewModel> GetAllList()
        {
            var url = GetAllUrl(_remoteServiceBaseUrl);
            var dataString = _apiClient.GetStringAsync(url, authorizationToken: GetUserTokenAsync()).Result;
            var response = JsonConvert.DeserializeObject<List<TViewModel>>(dataString);
            return response;
        }
        public PagingList<TViewModel> GetAll(int pageNumber = 1)
        {
            var url = GetAllUrl(_remoteServiceBaseUrl, pageNumber);
            var dataString = _apiClient.GetStringAsync(url, authorizationToken: GetUserTokenAsync()).Result;
            var response = JsonConvert.DeserializeObject<PagingList<TViewModel>>(dataString);
            return response;
        }
        public TViewModel Get(long id)
        {
            var url = GetUrl(_remoteServiceBaseUrl, id);
            var dataString = _apiClient.GetStringAsync(url, authorizationToken: GetUserTokenAsync()).Result;
            var response = JsonConvert.DeserializeObject<TViewModel>(dataString);
            return response;
        }
        public bool Insert(TViewModel model)
        {
            var url = InsertUrl(_remoteServiceBaseUrl);
            var response = _apiClient.PostAsync(url, model, authorizationToken: GetUserTokenAsync()).Result;
            return response.IsSuccessStatusCode;
        }
        public bool Edit(TViewModel model)
        {
            var url = EditUrl(_remoteServiceBaseUrl);
            var response = _apiClient.PostAsync(url, model, authorizationToken: GetUserTokenAsync()).Result;
            return response.IsSuccessStatusCode;
        }
        public void Delete(long id)
        {
            var url = DeleteUrl(_remoteServiceBaseUrl, id);
            var response = _apiClient.GetStringAsync(url, authorizationToken: GetUserTokenAsync()).Result;
        }
        public string GetUserTokenAsync()
        {
            var context = _httpContextAccesor.HttpContext;
            var claim = context.User.Claims.SingleOrDefault(r => r.Type == "token");
            return claim != null ? claim.Value : "";
        }

        public virtual PagingList<TViewModel> GetAllbySearch(int pageNumber = 1, int pageSize = 10, Dictionary<string, dynamic> filterParams = null)
        {
            var url = GetAllbySearchUrl(_remoteServiceBaseUrl, pageNumber);
            var dataString = _apiClient.PostAsync(url, filterParams, authorizationToken: GetUserTokenAsync()).Result;
            var response =
                JsonConvert.DeserializeObject<PagingList<TViewModel>>(dataString.Content.ReadAsStringAsync().Result);
            return response;
        }

        public virtual IEnumerable<TViewModel> GetAllListbySearch(Dictionary<string, dynamic> filterParams, bool allIncluded = false)
        {
            var url = GetAllbySearchListUrl(_remoteServiceBaseUrl, allIncluded);
            var dataString = _apiClient.PostAsync(url, filterParams, authorizationToken: GetUserTokenAsync()).Result;
            var response =
                JsonConvert.DeserializeObject<IEnumerable<TViewModel>>(dataString.Content.ReadAsStringAsync().Result);
            return response;
        }

        public long InsertAndGetId(TViewModel model)
        {
            var url = InsertAndGetIdUrl(_remoteServiceBaseUrl);
            var dataString = _apiClient.PostAsync(url, model, authorizationToken: GetUserTokenAsync()).Result;
            var response = JsonConvert.DeserializeObject<long>(dataString.Content.ReadAsStringAsync().Result);
            return response;
        }
    }
}
