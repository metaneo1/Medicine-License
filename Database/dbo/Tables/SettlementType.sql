CREATE TABLE [dbo].[SettlementType] (
    [Id]          [dbo].[IdenticalChild] IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX)         NULL,
    [Description] NVARCHAR (MAX)         NULL,
    [Level]       INT                    NULL,
    CONSTRAINT [XPKТип_населенного_пункта] PRIMARY KEY CLUSTERED ([Id] ASC)
);

