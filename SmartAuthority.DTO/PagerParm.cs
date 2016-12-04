/*技术支持QQ群：226090960*/
namespace SmartAuthority.DTO
{
    public class PagerParm
    {
        public int CurrentCount
        {
            get
            {
                return (PageIndex - 1) * PageSize;
            }
        }

        public int PageIndex { get; set; }

        public int PageSize
        {
            get
            {
                return 20;
            }
        }
    }
}
