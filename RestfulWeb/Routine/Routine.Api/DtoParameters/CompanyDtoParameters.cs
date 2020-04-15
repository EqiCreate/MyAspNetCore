using System;

namespace Routine.Api.DtoParameters
{
    public class CompanyDtoParameters
    {
        private const int MaxPageSize = 20;
        public string CompanyName { get; set; }

        public string SearchTerm { get; set; }

        public int PageNumber { get; set; } = 1;

        private int _pageSize=2;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        } 



    }
}
