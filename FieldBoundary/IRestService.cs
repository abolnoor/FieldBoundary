using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FieldBoundary
{
    internal interface IRestService
    {
        Task<Feature> BoundaryDetectionsAsync(Position position);
    }
}