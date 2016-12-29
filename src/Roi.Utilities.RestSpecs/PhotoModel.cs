using System;

namespace Roi.Utilities.RestSpecs
{
    public class PhotoModel
    {
        public Guid? PhotoId { get; set; }

        public Guid UserId { get; set; }

        public string LocationUrl { get; set; }

        public string LongDescription { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
