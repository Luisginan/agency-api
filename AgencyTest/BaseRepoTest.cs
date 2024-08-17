using Core.Config;
using Core.Utils.DB;
using Core.Utils.Security;
using Microsoft.Extensions.Options;
using Moq;

namespace AgencyTest;

public abstract class BaseRepoTest
{
    protected readonly IQueryBuilderRepository QueryBuilder;
    protected readonly INawaDaoRepository NawaDao;

    protected BaseRepoTest()
    {
        var vaultMock = new Mock<IVault>();
        var optionMock = new Mock<IOptions<DatabaseConfig>>();
        vaultMock.Setup(x => x.RevealSecret(It.IsAny<DatabaseConfig>())).Returns(new DatabaseConfig
        {
            Database = "blueprint",
            Password = "VCt/m8/zEfD5MN61wPTfrQ==",
            Port = "5432",
            Provider = "postgres",
            Server = "localhost",
            Type = "PostgreSQL",
            User = "postgres",
            CommandTimeout = "30",
            ConnectTimeout = "30",
            PoolSize = "100"
        });
        IConnection connection = new Connection(vaultMock.Object, optionMock.Object);
        QueryBuilder = new QueryBuilderRepository();
        NawaDao = new NawaDaoRepository(connection:connection);
    }
}