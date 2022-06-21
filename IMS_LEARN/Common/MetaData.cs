namespace IMS_LEARN.Common
{
    public class MetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasNext
        {

            get { return PageSize * (CurrentPage) < TotalCount; }
        }

        public bool HasPrevious
        {
            get { return PageSize * (CurrentPage - 1) > 0; }
        }
    }
}
