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
            var newPhoto = new PhotoModel()
            {
                PhotoId = null,
                UserId = Guid.NewGuid(),
                LocationUrl = "http://www.domain.com/somelocation",
                LongDescription = "Some longer description",
                CreatedDate = null
            };
            var response = RestClientToTest.Post<PhotoModel>(ResponseFormat.Json, "photo", newPhoto, null);
        }

        [Test]
        [Ignore("Used to clear out the database for the rest service")]
        public void _998_we_should_be_able_to_clear_out_the_data_for_the_rest_service()
        {

        }

        [Test]
        [Ignore("Used when running after clearing out the database used by the rest service")]
        public void _999_there_should_be_no_photos_in_the_web_service()
        {

        }
    }
}
