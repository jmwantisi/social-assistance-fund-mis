namespace socialAssistanceFundMIS.Common
{
    public static class Constants
    {
        public static class Validation
        {
            public const int MaxNameLength = 100;
            public const int MaxEmailLength = 255;
            public const int MaxAddressLength = 500;
            public const int MaxPhoneNumberLength = 20;
            public const int MaxIdentityCardLength = 20;
            public const int MinAge = 18;
            public const int MaxAge = 120;
        }

        public static class Pagination
        {
            public const int DefaultPageSize = 20;
            public const int MaxPageSize = 100;
        }

        public static class Status
        {
            public const string Active = "Active";
            public const string Inactive = "Inactive";
            public const string Pending = "Pending";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
        }

        public static class FileTypes
        {
            public const string Image = "image";
            public const string Document = "document";
            public const string Pdf = "pdf";
        }

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Officer = "Officer";
            public const string Viewer = "Viewer";
        }
    }
}

