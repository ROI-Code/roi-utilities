using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Roi.Utilities.Rest;

namespace Roi.Utilities.RestSpecs
{
    [TestFixture]
    public class When_using_the_rest_client_to_against_the_Mobile_API_for_interacting_with_photos
    {

        protected static IRestClient RestClientToTest = 
            new RoiRestClient("http://localhost:4242/api");



        [Test]
        public void _010_we_should_be_able_to_create_a_new_photo()
        {
            var newPhoto = GetNew();
            var response = RestClientToTest.Post<PhotoModel>(ResponseFormat.Json, "photo", newPhoto);

            var newlyCreatedPhoto = response.ReturnedObject;

            Assert.That(newlyCreatedPhoto.PhotoId, Is.Not.Null);
        }

        [Test]
        public void _020_we_should_be_able_to_get_at_least_one_photo()
        {
            var response = RestClientToTest.GetMany<PhotoModel>(ResponseFormat.Json, "photo", "");
            Assert.That(response.ReturnedObject, Is.Not.Null);
        }

        [Test]
        public void _030_we_should_be_able_to_update_a_photo()
        {
            var newPhoto = GetNew();
            var response = RestClientToTest.Post<PhotoModel>(ResponseFormat.Json, "photo", newPhoto);

            var photoToModify = response.ReturnedObject;

            var newLocation = "http://www.url.com/NEWlocation";
            photoToModify.LocationUrl = newLocation;

            response = RestClientToTest.Put<PhotoModel>(ResponseFormat.Json, "photo", photoToModify);

            var modifiedPhoto = response.ReturnedObject;
            Assert.That(modifiedPhoto.LocationUrl, Is.EqualTo(newLocation));
        }



        [Test]
        [Ignore("Used to clear out the database for the rest service")]
        public void _998_we_should_be_able_to_clear_out_the_data_for_the_rest_service()
        {
            var response = RestClientToTest.GetMany<PhotoModel>(ResponseFormat.Json, "photo", "");
            foreach (var photoModel in response.ReturnedObject)
            {
                var deleteResponse = RestClientToTest.Delete(ResponseFormat.Json, $"photo/{photoModel.PhotoId}");
                Assert.That(deleteResponse.Success, Is.True);
            }
            make_sure_there_are_no_photos_returned_from_the_web_service();
        }

        [Test]
        [Ignore("Used to make sure the web service returns no new photos")]
        public void _999_there_should_be_no_photos_in_the_web_service()
        {
            make_sure_there_are_no_photos_returned_from_the_web_service();
        }

        private static void make_sure_there_are_no_photos_returned_from_the_web_service()
        {
            var getAllResponse = RestClientToTest.GetMany<PhotoModel>(ResponseFormat.Json, "photo", "");
            Assert.That(getAllResponse.ReturnedObject.Count, Is.EqualTo(0));
        }

        private static PhotoModel GetNew()
        {
            var newPhoto = new PhotoModel()
            {
                PhotoId = null,
                UserId = Guid.NewGuid(),
                LocationUrl = "http://www.domain.com/somelocation",
                LongDescription = "Some longer description",
                CreatedDate = null
            };
            return newPhoto;
        }
    }
}
