﻿// <auto-generated />
namespace Magicodes.Admin.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations.Infrastructure;

    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class Added_AuthenticationSource_To_User : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201506061210411_Added_AuthenticationSource_To_User"; }
        }

        string IMigrationMetadata.Source
        {
            get { return null; }
        }

        string IMigrationMetadata.Target
        {
            get { return "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"; }
        }
    }
}