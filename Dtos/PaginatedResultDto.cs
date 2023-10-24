using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RefactorThis.Dtos
{
    public class PaginatedResultDto<T>
    {
        public ICollection<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PaginatedResultDto(ICollection<T> items, int totalCount, int pageIndex, int pageSize)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

    }
}
