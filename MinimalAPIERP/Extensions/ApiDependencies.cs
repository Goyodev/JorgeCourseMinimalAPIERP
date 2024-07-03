using AutoMapper;
using ERP.Data;

namespace MinimalAPIERP.Extensions
{
    public class ApiDependencies
    {
        public ApiDependencies(AppDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public AppDbContext Context { get; }
        public IMapper Mapper { get; }
    }
}
