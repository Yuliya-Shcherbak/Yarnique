using FluentMigrator.Builders.Create.Table;

namespace Yarnique.Database.Migrations.Extensions
{
    internal static class MigrationExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax AsMaxString(this ICreateTableColumnAsTypeSyntax createTableColumnAsTypeSyntax)
        {
            return createTableColumnAsTypeSyntax.AsString(int.MaxValue);
        }
    }
}
