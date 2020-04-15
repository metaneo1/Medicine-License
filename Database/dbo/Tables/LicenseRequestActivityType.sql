CREATE TABLE [dbo].[LicenseRequestActivityType] (
    [Id]         [dbo].[Identical]       IDENTITY (1, 1) NOT NULL,
    [Id_Type]    [dbo].[IdenticalParent] NOT NULL,
    [Id_Request] [dbo].[IdenticalParent] NOT NULL,
    CONSTRAINT [XPKÂèä_äåÿòåëüíîñòè_ïî_çàÿâêå] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_180] FOREIGN KEY ([Id_Type]) REFERENCES [dbo].[ActivityType] ([Id]),
    CONSTRAINT [R_181] FOREIGN KEY ([Id_Request]) REFERENCES [dbo].[LicenseRequest] ([Id])
);

