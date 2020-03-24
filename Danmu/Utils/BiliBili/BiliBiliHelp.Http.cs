using System;
using System.Threading.Tasks;
using Danmu.Utils.Common;
using static Danmu.Utils.Global.VariableDictionary;

namespace Danmu.Utils.BiliBili
{
    public partial class BiliBiliHelp
    {
        /// <summary>
        ///     获取视频的Page原始数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<byte[]> GetBiliBiliPageRawAsync(string url)
        {
            var key = Md5.GetMd5(url);
            return await _cache.GetOrCreateHttpCacheAsync(key, TimeSpan.FromMinutes(_setting.CidCacheTime), async () =>
            {
                var httpClient = _httpClientFactory.CreateClient(Deflate);
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode) return await response.Content.ReadAsByteArrayAsync();
                return new byte[0];
            });
        }

        /// <summary>
        ///     获取BiliBili弹幕原始数据
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        private async Task<byte[]> GetDanmuRawAsync(string url)
        {
            var key = Md5.GetMd5(url);
            return await _cache.GetOrCreateHttpCacheAsync(key, TimeSpan.FromMinutes(_setting.CidCacheTime), async () =>
            {
                var deflateClient = _httpClientFactory.CreateClient(Deflate);
                if (!string.IsNullOrEmpty(_setting.Cookie))
                    deflateClient.DefaultRequestHeaders.Add("Cookie", _setting.Cookie);
                var response = await deflateClient.GetAsync(url);
                if (response.IsSuccessStatusCode) return await response.Content.ReadAsByteArrayAsync();
                return new byte[0];
            });
        }

        /// <summary>
        ///     获取BiliBili页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetBiliBiliHtmlAsync(string url)
        {
            var httpClient = _httpClientFactory.CreateClient(Gzip);
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
            return string.Empty;
        }
    }
}
