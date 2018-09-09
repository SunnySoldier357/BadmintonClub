using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server.Tables;
using badmintonclubService.DataObjects;

namespace badmintonclubService.Models
{
    public class badmintonclubContext : DbContext
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<SeasonData> SeasonDatas { get; set; }
        public DbSet<User> Users { get; set; }

        private const string connectionStringName = "Name=MS_TableConnectionString";

        public badmintonclubContext() : base(connectionStringName)
        {

        } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }
    }
}