﻿using System.Collections.ObjectModel;

namespace Diploma_Utility
{
    public static class WC
    {
        public const string ImagePath = @"\images\post\";
        public const string SessionCard = "PostSession";

        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public const string EmailAdmin = "artem.verenco@gmail.com";


        public const string SessionInquiryId = "InquirySession";


        public const string CategoryName = "Category";

        public const string Success = "Success";
        public const string Error = "Error";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static readonly IEnumerable<string> listStatus = new ReadOnlyCollection<string>(
            new List<string>
            {
                StatusApproved,StatusCancelled,StatusInProcess,StatusPending,StatusRefunded,StatusShipped
            });
    }
}
