using System;
using System.Collections.Generic;
using System.Text;

namespace EFCrudRepository
{
    public class PagingDetail
    {
        public PagingDetail(int pageSize, int pageNumber)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
