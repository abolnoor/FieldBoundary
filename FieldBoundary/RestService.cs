using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FieldBoundary
{
    class RestService : IRestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            _client.DefaultRequestHeaders.Add("apikey", "30wLjRr5CG1nNCz");
        }

        public async Task<Feature> BoundaryDetectionsAsync(Position position)
        {
            Debug.Write(JsonConvert.SerializeObject(position));
            var uri = new Uri(string.Format("https://spacenus-api.azurewebsites.net/boundary_detections.json", string.Empty));
            Feature feature = new Feature();
            
            try
            {
                //var json = JsonConvert.SerializeObject(item);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");

                //MultipartFormDataContent content = new MultipartFormDataContent
                //{
                //    { new StringContent("3"), "accuracy" },
                //    { new StringContent("49.8737566"), "latitude" },
                //    { new StringContent("8.6909503"), "longitude" },
                //    { new StringContent("false"), "fallback_boundary" }
                //};
                
                MultipartFormDataContent content = new MultipartFormDataContent
                {
                    { new StringContent("3"), "accuracy" },
                    { new StringContent(position.Latitude.ToString("F7")), "latitude" },
                    { new StringContent(position.Longitude.ToString("F7")), "longitude" },
                    { new StringContent("false"), "fallback_boundary" }
                };
                Debug.Write(@"\t position.Latitude.ToString(F7) {0}", position.Latitude.ToString("F7"));
                Debug.Write(@"\t position.Longitude.ToString(F7) {0}", position.Longitude.ToString("F7"));


                Debug.Write(JsonConvert.SerializeObject(content));

                HttpResponseMessage response = await _client.PostAsync(uri, content);


                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tField successfully detected.");
                    var res = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"\t res {0}", res);

                    if (res != "")
                    {
                        Debug.WriteLine("res not empty");
                        BoundaryInfo bInfo = JsonConvert.DeserializeObject<BoundaryInfo>(res);
                        if(bInfo.features.Length > 0)
                        feature = bInfo.features[0];
                        Debug.WriteLine(@"\t feature {0}", JsonConvert.SerializeObject(feature));
                    }
                    //IList<Position> geo = new List<Position>();
                    //Debug.WriteLine(@"\t geo {0}", JsonConvert.SerializeObject(geo));
                    
                   
                   
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\t ERROR {0}", ex.Message);
            }

            return feature;
        }

    }
}
