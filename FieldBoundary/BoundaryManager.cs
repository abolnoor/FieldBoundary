using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FieldBoundary
{
      class BoundaryDetectionsManager
    {
        IRestService restService;

        public BoundaryDetectionsManager(IRestService service)
        {
            restService = service;
        }

        public Task<Feature> DetectionsTaskAsync(Position p)
        {
            return restService.BoundaryDetectionsAsync(p);
        }
    }
}
