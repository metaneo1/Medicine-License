CREATE TABLE [dbo].[AdminUnits] (
    [Id]             [dbo].[Identical] IDENTITY (1, 1) NOT NULL,
    [ParentId]       [dbo].[Identical] NULL,
    [IdTypeadm]      [dbo].[Identical] NOT NULL,
    [Latitude]       DECIMAL (12, 10)  NULL,
    [Longitude]      DECIMAL (12, 10)  NULL,
    [Comment]        NVARCHAR (MAX)    NULL,
    [IsRayonCenter]  [dbo].[Logical]   NULL,
    [Name_ru]        NVARCHAR (MAX)    NULL,
    [Name_kg]        NVARCHAR (MAX)    NULL,
    [Description_kg] NVARCHAR (MAX)    NULL,
    [Description_ru] NVARCHAR (MAX)    NULL,
    [CODE]           NVARCHAR (MAX)    NULL,
    CONSTRAINT [XPKÒåððèòîðèàëüíàÿ_Åäèíèöà] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_11] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[AdminUnits] ([Id]),
    CONSTRAINT [R_12] FOREIGN KEY ([IdTypeadm]) REFERENCES [dbo].[TypeAdminUnits] ([Id])
);



