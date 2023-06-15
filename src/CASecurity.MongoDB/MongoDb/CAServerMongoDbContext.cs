using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace CASecurity.MongoDB;

[ConnectionStringName("Default")]
public class CASecurityMongoDbContext : AbpMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */
    //public IMongoCollection<Token> Tokens => Collection<Token>();
    //public IMongoCollection<TokenPriceData> TokenPriceData => Collection<TokenPriceData>();


    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        // modelBuilder.Entity<Token>(t =>
        // {
        //     t.CollectionName = CASecurityConsts.DbTablePrefix + "Token" + CASecurityConsts.DbSchema;
        // });
        // modelBuilder.Entity<TokenPriceData>(t =>
        // {
        //     t.CollectionName = CASecurityConsts.DbTablePrefix + "TokenPrice" + CASecurityConsts.DbSchema;
        // });
        
    }
}