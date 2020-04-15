CREATE TABLE [dbo].[LocalGoverment2] (
    [Id]                [dbo].[Identical] IDENTITY (1, 1) NOT NULL,
    [Id_Parent]         [dbo].[Identical] NULL,
    [Name]              NVARCHAR (MAX)    NULL,
    [SOATE]             NVARCHAR (MAX)    NULL,
    [Id_SettlementType] [dbo].[Identical] NULL,
    [Latitude]          FLOAT (53)        NULL,
    [Longitude]         FLOAT (53)        NULL,
    CONSTRAINT [PK_LocalGoverment2] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LocalGoverment2_SettlementType] FOREIGN KEY ([Id_SettlementType]) REFERENCES [dbo].[SettlementType] ([Id]),
    CONSTRAINT [FK_Parent_Child] FOREIGN KEY ([Id_Parent]) REFERENCES [dbo].[LocalGoverment2] ([Id])
);

