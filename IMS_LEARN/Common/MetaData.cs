namespace IMS_LEARN.Common
{
    public class MetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
        public bool HasNext
        {
            get { return PageSize * (CurrentPage - 1) < TotalPages; }
        }

        public bool HasPrevious
        {
            get { return PageSize * (CurrentPage - 1) > 0; }
        }
    }
}
