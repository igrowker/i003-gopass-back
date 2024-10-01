using GoPass.Domain.DTOs.Request.PaginationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Domain.IQueryableExtensions
{
    public static class IQueriableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            return queryable.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
                .Take(paginationDto.PageSize);  
        }
    }
}
