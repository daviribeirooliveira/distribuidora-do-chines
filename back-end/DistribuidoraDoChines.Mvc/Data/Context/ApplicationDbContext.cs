using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraDoChines.Mvc.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}