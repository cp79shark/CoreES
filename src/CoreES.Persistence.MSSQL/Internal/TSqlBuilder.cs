using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL.Internal
{
    internal class TSqlBuilder
    {
        public static string CreateDatabaseStatement(string DatabaseName)
        {
            return $"IF NOT EXISTS(select * from sys.databases where name='{DatabaseName}') CREATE DATABASE [{DatabaseName}]";
        }

        public static string CreateSchemaStatementVersion1_0_0()
        {
            string script = @"
IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='DbVersionHistory' AND xtype='U')
BEGIN TRY
	BEGIN TRAN
		CREATE TABLE [DbVersionHistory]
		(
			[Id] INT NOT NULL IDENTITY(1,1),
			[Version] NVARCHAR(50) NOT NULL,
			[UpdatedOn] DATETIME2 NOT NULL,
			[Reason] NVARCHAR(1000) NOT NULL,
			CONSTRAINT [PK_Versions] PRIMARY KEY CLUSTERED (Id)
		)

		CREATE TABLE [Events]
		(
			[StreamPosition] BIGINT NOT NULL IDENTITY(1,1),
            [StreamIdHash] CHAR(40) NOT NULL,
			[StreamId] NVARCHAR(1000) NOT NULL,
			[EventId] UNIQUEIDENTIFIER NOT NULL,
			[EventNumber] INT NOT NULL,
			[EventType] NVARCHAR(1000),
			[Data] VARBINARY(MAX),
			[Metadata] VARBINARY(MAX),
			[Created] DATETIME2 NOT NULL,
			CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED(StreamPosition)
		)
		--CREATE UNIQUE NONCLUSTERED INDEX [IX_Events_Stream] ON [Events](StreamIdHash, EventNumber)

		CREATE TABLE [Aggregates]
		(
			[StreamId] CHAR(40) NOT NULL,
			[NumberOfEvents] INT NOT NULL,
			[Metadata] VARBINARY(MAX),
			CONSTRAINT [PK_Aggregates] PRIMARY KEY CLUSTERED(StreamId)
		)

		INSERT INTO [DbVersionHistory]
		(
			[Version]
			, [UpdatedOn]
			, [Reason]
		)
        VALUES
		(
			'1.0.0'
			, getdate()
			, 'Initial version'
		)
	COMMIT TRAN
	PRINT 'Database has been created successfully'
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
            BEGIN
                  ROLLBACK TRANSACTION
            END
      SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage;

    DECLARE @ErrorMessage NVARCHAR(max),
              @ErrorSeverity INT,
              @ErrorState INT;

    SET @ErrorMessage = ERROR_MESSAGE();
    SET @ErrorSeverity = ERROR_SEVERITY();
    SET @ErrorState = ERROR_STATE();

    RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
    RETURN;
END CATCH
ELSE
    RETURN
";

            return script;
        }

        public static string DropDatabaseStatement(string DatabaseName)
        {
            return $"IF EXISTS(select * from sys.databases where name='{DatabaseName}') DROP DATABASE [{DatabaseName}]";
        }

        public static string AppendEventStatement()
        {
            return @"
INSERT INTO [Events]
(
    [StreamIdHash]
    , [StreamId]
	, [EventId]
	, [EventNumber]
	, [EventType]
	, [Data]
	, [Metadata]
	, [Created]
)
VALUES
(
    @StreamIdHash
    , @StreamId
    , @EventId
    , @EventNumber
    , @EventType
    , @Data
    , @Metadata
    , @Created
)
";
        }
    }
}
