CREATE TABLE [dbo].[Message] (
    [Id]              [dbo].[IdenticalChild]  IDENTITY (1, 1) NOT NULL,
    [Id_Parent]       [dbo].[IdenticalChild]  NULL,
    [MessageText]     NVARCHAR (MAX)          NULL,
    [MessageDate]     DATETIME                NULL,
    [Id_Request]      [dbo].[IdenticalParent] NOT NULL,
    [Id_QuestionType] [dbo].[IdenticalParent] NULL,
    [Id_AnswerWriter] [dbo].[IdenticalParent] NOT NULL,
    CONSTRAINT [XPKÑîîáùåíèå] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_189] FOREIGN KEY ([Id_Request]) REFERENCES [dbo].[LicenseRequest] ([Id]),
    CONSTRAINT [R_191] FOREIGN KEY ([Id_QuestionType]) REFERENCES [dbo].[QuestionType] ([Id]),
    CONSTRAINT [R_206] FOREIGN KEY ([Id_AnswerWriter]) REFERENCES [dbo].[ApplicationUser] ([Id]),
    CONSTRAINT [R_207] FOREIGN KEY ([Id_Parent]) REFERENCES [dbo].[Message] ([Id])
);

