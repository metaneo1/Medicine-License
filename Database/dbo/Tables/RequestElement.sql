CREATE TABLE [dbo].[RequestElement] (
    [Id]                  [dbo].[Identical]       IDENTITY (1, 1) NOT NULL,
    [Id_Parent]           [dbo].[Identical]       NULL,
    [IsActive]            [dbo].[Logical]         NULL,
    [OrderNumber]         INT                     NULL,
    [Id_RequestElemType]  [dbo].[IdenticalParent] NOT NULL,
    [Id_TemplateDocument] [dbo].[IdenticalParent] NULL,
    [Id_RequestType]      [dbo].[IdenticalParent] NOT NULL,
    [Id_StructureType]    [dbo].[IdenticalParent] NOT NULL,
    [IsRequired]          [dbo].[Logical]         NULL,
    CONSTRAINT [XPKÝëåìåíò_çàÿâêè] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_200] FOREIGN KEY ([Id_Parent]) REFERENCES [dbo].[RequestElement] ([Id]),
    CONSTRAINT [R_201] FOREIGN KEY ([Id_RequestElemType]) REFERENCES [dbo].[RequestElementType] ([Id]),
    CONSTRAINT [R_202] FOREIGN KEY ([Id_TemplateDocument]) REFERENCES [dbo].[DocTemplForReqElemType] ([Id]),
    CONSTRAINT [R_203] FOREIGN KEY ([Id_RequestType]) REFERENCES [dbo].[RequestType] ([Id]),
    CONSTRAINT [R_210] FOREIGN KEY ([Id_StructureType]) REFERENCES [dbo].[RequestElemStructureType] ([Id])
);

